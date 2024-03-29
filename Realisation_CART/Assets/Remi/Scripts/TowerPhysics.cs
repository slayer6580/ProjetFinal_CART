using UnityEngine;
using DiscountDelirium;

namespace BoxSystem
{
    public class TowerPhysics : MonoBehaviour
    {
        [field: SerializeField] private GameObject DebugCartPrefab { get; set; } = null;
        [field: SerializeField] private GameObject Player { get; set; } = null;

        [Header("Items and boxes physics settings")]
        [SerializeField] public float m_itemRemoveImpulseIntesity = 0.2f;
        [SerializeField] public float m_itemVectorUpImpulseIntesity = 10.0f;
        [SerializeField] public float m_boxRemoveImpulseIntesity = 0.2f;
        [SerializeField] public float m_boxVectorUpImpulseIntesity = 10.0f;
        [SerializeField] public int m_nbOfUndroppableBoxes = 4;

        [Header("Debug Cart Settings")]
        [SerializeField] public float m_debugCartDistance = 10.0f;
        [SerializeField] public float m_debugCartSpeed = 3.0f;

        [Header("Joint Settings")]
        [SerializeField] public JointMode m_currentJointMode = JointMode.Spring;

        [Header("Spring Settings")]
        [SerializeField] public float m_springStrenght = 100.0f;
        [SerializeField] public float m_springDamper = 0;
        [SerializeField] public float m_springMinDistance = 0;
        [SerializeField] public float m_springMaxDistance = 0.2f;
        [SerializeField] public float m_springTolerance = 0.06f;
        [SerializeField] public bool m_springEnableCollision = true;
        [SerializeField] public float m_springBreakForce = 100.0f;
        [SerializeField] public float m_springBreakTorque = 100.0f;

        private TowerBoxSystem _Tower { get; set; } = null;

        public enum JointMode
        {
            Spring,
            Hinge,
            None,
            Count
        }

        private void Awake()
        {
            _Tower = GetComponent<TowerBoxSystem>();
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.K))
            {
                Vector3 rotation = new Vector3(0, 90, 0);
                Vector3 rightSideOfPlayer = -Player.transform.right;
                Vector3 rightStartPosition = Player.transform.position + (rightSideOfPlayer * m_debugCartDistance);

                SpawnDebugCartAtOffsetPosition(rotation, rightStartPosition);
            }
            else if (Input.GetKeyDown(KeyCode.L))
            {
                Vector3 rotation = new Vector3(0, -90, 0);
                Vector3 leftSideOfPlayer = Player.transform.right;
                Vector3 leftStartPosition = Player.transform.position + (leftSideOfPlayer * m_debugCartDistance);

                SpawnDebugCartAtOffsetPosition(rotation, leftStartPosition);
            }
            else if (Input.GetKeyDown(KeyCode.N))
            {
                if (m_currentJointMode - 1 < 0)
                {
                    m_currentJointMode = JointMode.None;
                    Debug.Log("Current Joint Mode: " + m_currentJointMode); // Do not erase: Necessary for the selection of the joint mode
                    return;
                }

                m_currentJointMode = m_currentJointMode - 1;
                Debug.Log("Current Joint Mode: " + m_currentJointMode); // Do not erase: Necessary for the selection of the joint mode
            }
            else if (Input.GetKeyDown(KeyCode.M))
            {
                if (m_currentJointMode + 1 > JointMode.None)
                {
                    m_currentJointMode = (JointMode)0;
                    Debug.Log("Current Joint Mode: " + m_currentJointMode); // Do not erase: Necessary for the selection of the joint mode
                    return;
                }

                m_currentJointMode = m_currentJointMode + 1;
                Debug.Log("Current Joint Mode: " + m_currentJointMode); // Do not erase: Necessary for the selection of the joint mode
            }

        }

        /// <summary> Spawn un objet de débug CartDebug à une position offset </summary>
        private void SpawnDebugCartAtOffsetPosition(Vector3 rotation, Vector3 rightStartPosition)
        {
            Vector3 eulerOfCart = Player.transform.rotation.eulerAngles;
            Vector3 localEulerOfTower = transform.localRotation.eulerAngles;

            Vector3 desiredSpawnPostion = Vector3.zero;
            desiredSpawnPostion += eulerOfCart;
            desiredSpawnPostion += localEulerOfTower;
            desiredSpawnPostion += rotation;

            Transform debugCart = Instantiate(DebugCartPrefab).transform;
            debugCart.eulerAngles = desiredSpawnPostion;
            debugCart.position = rightStartPosition;
            debugCart.gameObject.GetComponent<DebugCart>().SetSpeed(m_debugCartSpeed);
        }

        /// <summary> Ajoute un joint à la boite </summary>
        public void AddJointToBox()
        {
            if (m_currentJointMode == JointMode.Hinge)
            {
                AddHingeJoint();
                return;
            }
            else if (m_currentJointMode == JointMode.Spring)
            {
                AddSpringJoint();
                return;
            }
        }

        /// <summary> Ajoute un joint de type Hinge à la boite </summary>
        private void AddHingeJoint()
        {
            // TODO: Remi: Implement hinge joint pour le Sprint 2
        }

        /// <summary> Ajoute un joint de type Spring à la boite </summary>
        private void AddSpringJoint()
        {
            if (_Tower.GetBoxCount() <= m_nbOfUndroppableBoxes) return;

            Box previousTopBox = _Tower.GetPreviousTopBox();
            if (previousTopBox == null) Debug.LogWarning("Previous Top Box est null");
            else
            {
                previousTopBox.GetComponent<CollisionDetector>().enabled = false;

                Box topBox = _Tower.GetTopBox();
                topBox.GetComponent<CollisionDetector>().enabled = true;
                topBox.GetComponent<Rigidbody>().isKinematic = false;

                Rigidbody previousTopBoxRB = previousTopBox.GetComponent<Rigidbody>();
                if (previousTopBoxRB == null) Debug.LogError("Previous Top Box n'a pas de rigidbody");
                SpringJoint springJoint = topBox.gameObject.AddComponent<SpringJoint>();
                SetSprintJointValues(springJoint, previousTopBoxRB);

                SpringJoint previousSpringJoint = previousTopBoxRB.GetComponent<SpringJoint>();

                if (previousSpringJoint != null)
                {
                    previousTopBox.ReplaceBoxToOrigin();
                    previousTopBox.transform.eulerAngles = Player.transform.eulerAngles;
                    previousSpringJoint.spring = 0;
                    previousSpringJoint.connectedBody = null;
                }

                previousTopBoxRB.isKinematic = true;
            }

        }

        /// <summary> Retire un item avec une force provenant de l'exterieur </summary>
        public void RemoveItemImpulse(Vector3 velocity)
        {
            Box topBox = _Tower.GetTopBox();

            if (topBox.GetItemsInBox().Count <= 0)
            {
                Debug.LogWarning("No item in the box");
                return;
            }

            Box.ItemInBox lastItemInBox = topBox.GetLastItem();
            if (lastItemInBox.m_item == null)
            {
                Debug.LogWarning("Item is null");
                return;
            }

            Vector3 vectorUp = topBox.transform.up * m_itemVectorUpImpulseIntesity;
            Vector3 incomingImpulse = velocity * m_itemRemoveImpulseIntesity;
            Vector3 totalImpulse = vectorUp + incomingImpulse;

            Rigidbody rb = lastItemInBox.m_item.GetComponent<Rigidbody>();
            if (rb == null)
            {
                rb = lastItemInBox.m_item.gameObject.AddComponent<Rigidbody>();
                rb.AddForce(totalImpulse, ForceMode.Impulse);
                rb.mass = 0.1f; // TODO Remi: Get the mass depending on the item info mass
            }
            else
                rb.AddForce(totalImpulse, ForceMode.Impulse);

            if (lastItemInBox.m_item.GetComponent<AutoDestruction>().enabled) return;

            lastItemInBox.m_item.GetComponent<AutoDestruction>().enabled = true;
            Debug.Log("Item in autodestruction mode: " + lastItemInBox.m_item.name);
            topBox.GetItemsInBox().Remove(lastItemInBox);

            topBox.ResetSlots(lastItemInBox);
        }

        /// <summary> Retire une boite avec une force provenant de l'exterieur </summary>
        public void RemoveBoxImpulse(Vector3 velocity, bool single = false)
        {
            Box topBox = _Tower.GetTopBox();
            if (topBox == null)
            {
                Debug.LogWarning("No box to remove");
                return;
            }

            DetachJoint(topBox);

            Vector3 totalImpulse = Vector3.zero;
            if (!single)
            {
                Vector3 vectorUp = topBox.transform.up * m_boxVectorUpImpulseIntesity;
                Vector3 incomingImpulse = velocity * m_boxRemoveImpulseIntesity;
                totalImpulse = vectorUp + incomingImpulse;
            }
            else 
            {
                totalImpulse = velocity;
            }

            Debug.Log("Incoming Impulse: " + totalImpulse.magnitude);
            topBox.GetComponent<Rigidbody>().isKinematic = false;
            topBox.GetComponent<Rigidbody>().AddForce(totalImpulse, ForceMode.Impulse);

            if (topBox.GetComponent<AutoDestruction>().enabled) return;

            _Tower.RemoveLastBoxFromTower();
            topBox.GetComponent<AutoDestruction>().enabled = true;
        }

        /// <summary> Détache le joint de la boite </summary>
        private static void DetachJoint(Box topBox)
        {
            SpringJoint springJoint = topBox.GetComponent<SpringJoint>();
            if (springJoint != null)
                Destroy(springJoint);

            HingeJoint hingeJoint = topBox.GetComponent<HingeJoint>();
            if (hingeJoint != null)
                Destroy(hingeJoint);
        }

        /// <summary> Configure les valeurs du join Spring  </summary>
        private void SetSprintJointValues(SpringJoint springJoint, Rigidbody newBoxeRB)
        {
            springJoint.connectedBody = newBoxeRB;
            springJoint.spring = m_springStrenght;
            springJoint.damper = m_springDamper;
            springJoint.minDistance = m_springMinDistance;
            springJoint.maxDistance = m_springMaxDistance;
            springJoint.tolerance = m_springTolerance;
            springJoint.enableCollision = m_springEnableCollision;
            springJoint.breakForce = m_springBreakForce;
            springJoint.breakTorque = m_springBreakTorque;
        }

        /// <summary> Vérifie si on peut retirer le contenu de la boite </summary>
        public void CheckIfCanDropContent(Vector3 velocity)
        {
            Box box = _Tower.GetTopBox();
            if (box == null)
            {
                Debug.LogWarning("No box to check");
                return;
            }

            if (box.IsEmpty() && _Tower.GetBoxCount() > m_nbOfUndroppableBoxes)
                RemoveBoxImpulse(velocity);
            else if (!box.IsEmpty())
                RemoveItemImpulse(velocity);
        }
    }
}
