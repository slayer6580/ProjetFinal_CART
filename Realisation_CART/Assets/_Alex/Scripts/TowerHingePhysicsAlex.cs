using System.Collections.Generic;
using UnityEngine;

namespace BoxSystem
{
    public class TowerHingePhysicsAlex : MonoBehaviour
    {
        [field: SerializeField] public TowerBoxSystem TowerBoxSystem { get; private set; }
        [SerializeField] private Transform m_towerFake;
        [SerializeField] private int m_nbOfFixedBoxes;
        private int m_boxCount = 0;

        private List<GameObject> m_allBoxes = new List<GameObject>();
        private List<GameObject> m_boxesWithHinge = new List<GameObject>();
        private List<Vector3> m_boxesWithHingesInitialPosition = new List<Vector3>();
        private List<HingeJoint> m_hinges = new List<HingeJoint>();
      

        [Header("Mettre les Prefabs de la boite")]
        [SerializeField] private GameObject m_boxNoHingePrefab;
        [SerializeField] private GameObject m_boxWithHingePrefab;

        [Header("Hinge Settings")]
        [SerializeField][Range(0.0f, 1000.0f)] private float m_springForce = 0;
        [SerializeField] private float m_springForceReduction = 0;
        [SerializeField] private float m_springForceMinimum = 0;
        [SerializeField] private float m_minimumForceAppliedToMove;

        [Header("Lose items parameters")]
        [SerializeField] private List<Vector2> m_loseItemStats = new List<Vector2>();
        private float m_currentTimer = 0;
        private float m_timeBeforeLost = 0;

        private Eside m_side;

        [Header("ReadOnly")]
        [SerializeField] private float m_firstHingeAngleRead = 0;
        [SerializeField] private float m_allHingeAngleRead = 0;



        enum Eside
        {
            left,
            right
        }

        private void Awake()
        {
            m_side = Eside.left;
        }

        private void Update()
        {

            if (m_hinges.Count == 0) return;

            HingesBalance();
            CopyFakeTowerToRealTower();
            ItemLosing();
        }

        private void ItemLosing()
        {
            // For Futur Test
            m_firstHingeAngleRead = m_hinges[0].angle;
            int hingeCount = m_hinges.Count;
            float allHingeAngle = 0;
            for (int i = 0; i < hingeCount; i++)
            {
                allHingeAngle += m_hinges[i].angle;
            }

            m_allHingeAngleRead = allHingeAngle;

            float difference = Mathf.Infinity;
            float timeBeforeLost = 0;

            // regarde selon les angles des boites totale le nombre d'item a perdre par secondes
            foreach (Vector2 stats in m_loseItemStats)
            {
                float closeAngleToStats = Mathf.Abs(stats.x - Mathf.Abs(m_allHingeAngleRead));
                if (closeAngleToStats < difference)
                {
                    difference = closeAngleToStats;
                    timeBeforeLost = stats.y;
                }
            }

            if (timeBeforeLost == 0)
                return;

            Debug.Log("seconde avant de perdre un item: " + timeBeforeLost);

            m_currentTimer += Time.deltaTime;

            if (m_currentTimer > timeBeforeLost)
            {
                m_currentTimer = 0;
                TowerBoxSystem.RemoveItemImpulse();
            }
        }

        private void HingesBalance()
        {
            float topBox_XPosition = GetTopBox().transform.localPosition.x;
            Rigidbody topBoxRb = GetTopBox().GetComponent<Rigidbody>();
            float forceMagnitude = topBoxRb.velocity.magnitude;

            if (topBox_XPosition < 0 && m_side == Eside.right)
            {
                ReplaceAllBoxWithHingeToOrigin();
                ChangeAllAnchors(Eside.left);

                if (forceMagnitude < m_minimumForceAppliedToMove)
                {
                    foreach (GameObject boxes in m_boxesWithHinge)
                    {
                        boxes.GetComponent<Rigidbody>().velocity = Vector3.zero;
                    }
                }
            }
            else if (topBox_XPosition > 0 && m_side == Eside.left)
            {
                ReplaceAllBoxWithHingeToOrigin();
                ChangeAllAnchors(Eside.right);

                if (forceMagnitude < m_minimumForceAppliedToMove)
                {
                    foreach (GameObject boxes in m_boxesWithHinge)
                    {
                        boxes.GetComponent<Rigidbody>().velocity = Vector3.zero;
                    }
                }
            }
        }

        public void AddBoxToFakeTower()
        {
            m_boxCount++;
            bool HaveHinge = false;

            GameObject instant;

            if (m_boxCount <= m_nbOfFixedBoxes)
            {
                instant = Instantiate(m_boxNoHingePrefab);
            }
            else
            {
                instant = Instantiate(m_boxWithHingePrefab);
                HaveHinge = true;
            }

            m_allBoxes.Add(instant);

            // setup de la boite
            instant.transform.rotation = m_towerFake.rotation;
            instant.transform.SetParent(m_towerFake);
            instant.name = "Boxe " + m_boxCount;
            Box instantBox = instant.GetComponent<Box>();


            // hauteur de la boite dans la tour initial
            float heightDifference = instantBox.gameObject.GetComponent<BoxSetup>().GetBoxHeightDifference();
            float localHeight = (m_boxCount - 1) * (heightDifference + TowerBoxSystem.BoxGapHeight);
            Vector3 initialLocalPosition = new Vector3(0, localHeight, 0);

            // TODO changer ca pour selon transform.up de la boite en dessous avec eulerAngle identique a la boite du dessous aussi pour les hinges
            if (!HaveHinge)
            {
                instant.transform.localPosition = initialLocalPosition;
            }

            else
            {
                // Box placement based on box underneath
                Transform boxUnder = GetBoxUnderLast().transform;
                instant.transform.localPosition = (boxUnder.up * heightDifference) + boxUnder.localPosition;
                instant.transform.eulerAngles = boxUnder.transform.eulerAngles;

                // Add connection to hinge box
                Rigidbody previousTopBoxRB = GetBoxUnderLast().GetComponent<Rigidbody>();
                Rigidbody boxRB = instant.GetComponent<Rigidbody>();
                SetUpJoint(previousTopBoxRB, boxRB);
                m_boxesWithHingesInitialPosition.Add(initialLocalPosition);
            }


        }

        private void CopyFakeTowerToRealTower()
        {
            List<Box> realBoxes = TowerBoxSystem.GetAllBoxes();
            int realBoxesCount = realBoxes.Count;

            for (int i = 0; i < realBoxesCount; i++)
            {
                realBoxes[i].transform.localPosition = m_allBoxes[i].transform.localPosition;
                realBoxes[i].transform.localEulerAngles = m_allBoxes[i].transform.localEulerAngles;
            }
        }

        public void RemoveBoxFromFakeTower()
        {
            GameObject lastBox = GetTopBox();
            m_allBoxes.Remove(lastBox);

            if (m_boxesWithHinge.Contains(lastBox))
            {
                int index = m_boxesWithHinge.IndexOf(lastBox);
                m_boxesWithHinge.Remove(lastBox);
                m_hinges.RemoveAt(index);                
                m_boxesWithHingesInitialPosition.RemoveAt(index);
            }

            Destroy(lastBox);
            m_boxCount--;
        }


        private void EnabledColliderOnBoxes(bool value)
        {
            foreach (GameObject box in m_allBoxes)
            {
                box.GetComponent<BoxCollider>().enabled = value;
            }
        }

        private void ReplaceAllBoxWithHingeToOrigin()
        {
            for (int i = 0; i < m_boxesWithHinge.Count; i++)
            {
                Debug.Log("Origin Boxes Places");
                m_boxesWithHinge[i].transform.localPosition = m_boxesWithHingesInitialPosition[i];
                m_boxesWithHinge[i].transform.localEulerAngles = Vector3.zero;
            }
        }

        private void ChangeAllAnchors(Eside wantedSide)
        {
            m_side = wantedSide;
            Vector3 anchorPosition = m_side == Eside.left ? new Vector3(-TowerBoxSystem.GetTopBox().GetAnchorWidth(), -TowerBoxSystem.GetTopBox().GetAnchorHeight(), 0) : new Vector3(TowerBoxSystem.GetTopBox().GetAnchorWidth(), -TowerBoxSystem.GetTopBox().GetAnchorHeight(), 0);

            foreach (HingeJoint hingeJoint in m_hinges)
            {
                hingeJoint.anchor = anchorPosition;
            }

        }


        private void SetUpJoint(Rigidbody attachedBody, Rigidbody sourceBody)
        {

            HingeJoint hingeJoint = sourceBody.gameObject.GetComponent<HingeJoint>();

            // Hinge connection
            hingeJoint.connectedBody = attachedBody;
            m_hinges.Add(hingeJoint);
            m_boxesWithHinge.Add(sourceBody.gameObject);

            // Joint set up
            JointSpring spring = hingeJoint.spring;
            spring.targetPosition = 0;
            float springForce = m_springForce - (m_springForceReduction * (m_hinges.Count - 1));
            if (springForce < m_springForceMinimum)
                springForce = m_springForceMinimum;

            spring.spring = springForce;
            hingeJoint.spring = spring;

            // Auto Set up of anchor side
            Vector3 anchorPosition = m_side == Eside.left ? new Vector3(-TowerBoxSystem.GetTopBox().GetAnchorWidth(), -TowerBoxSystem.GetTopBox().GetAnchorHeight(), 0) : new Vector3(TowerBoxSystem.GetTopBox().GetAnchorWidth(), -TowerBoxSystem.GetTopBox().GetAnchorHeight(), 0);
            hingeJoint.anchor = anchorPosition;

        }


        public GameObject GetTopBox()
        {
            if (m_boxCount == 0)
                return null;

            return m_allBoxes[m_boxCount - 1];
        }

        private GameObject GetBoxUnderLast()
        {
            return m_allBoxes[m_boxCount - 2];
        }
    }
}
