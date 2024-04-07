using DiscountDelirium;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace BoxSystem
{
    public class TowerPhysics : MonoBehaviour
    {
        [field: SerializeField] private GameObject DebugCartPrefab { get; set; } = null;
        [field: SerializeField] private GameObject Player { get; set; } = null;
        private TowerBoxSystem2 _TowerBoxSystem { get; set; } = null;

        private List<GameObject> m_boxesWithHinge = new List<GameObject>();
        private List<Vector3> m_boxesInitialPosition = new List<Vector3>();

        [Header("Mettre les Prefabs de la boite")]
        [SerializeField] private GameObject m_boxNoHingePrefab;
        [SerializeField] private GameObject m_boxWithHingePrefab;

        [Header("Items and boxes physics settings")]
        [SerializeField] private float m_itemRemoveImpulseIntesity = 0.2f;
        [SerializeField] private float m_itemVectorUpImpulseIntesity = 10.0f;
        [SerializeField] private float m_boxRemoveImpulseIntesity = 0.2f;
        [SerializeField] private float m_boxVectorUpImpulseIntesity = 10.0f;
        private int m_nbOfUndroppableBoxes = 1;

        [Header("Debug Cart Settings")]
        [SerializeField] private float m_debugCartDistance = 10.0f;
        [SerializeField] private float m_debugCartSpeed = 3.0f;

        //[Header("Joint Settings")]
        //[SerializeField] private JointMode m_currentJointMode = JointMode.Spring;

        [Header("Hinge Settings")]

        [SerializeField] private List<HingeJoint> m_hinges;
        [SerializeField] private float m_angle = 0;
        [SerializeField][Range(0.0f, 1000.0f)] private float m_springForce = 0;
        [SerializeField] private float m_angleOffset = 0.1f;
        //[SerializeField] private Vector3 m_hingeAnchor = Vector3.zero;
        //[SerializeField] private Vector3 m_hingeAxis = new Vector3(0, 0, 1);
        //[SerializeField] private bool m_hingeAutoConfigConnAnchor = true;
        //[SerializeField] private Vector3 m_hingeConnAnchor = Vector3.zero;
        //[SerializeField] private bool m_hingeUseSpring = true;
        //[SerializeField] private float m_hingeSpringStrenght = 100.0f;
        //[SerializeField] private float m_hingeSpringDamper = 0;
        //[SerializeField] private float m_hingeTargetPos = 0;

        //[SerializeField] private bool m_hingeUseMotor = true;
        //[SerializeField] private float m_hingeMotorTargetVelocity = 0;
        //[SerializeField] private float m_hingeMotorForce = 100.0f;
        //[SerializeField] private bool m_hingeMotorFreespin = true;

        //[SerializeField] private bool m_hingUseLimits = true;
        //[SerializeField] private float m_hingeLimitsMin = 0;
        //[SerializeField] private float m_hingeLimitsMax = 0;

        //[SerializeField] private float m_hingeLimitsBounciness = 0;
        //[SerializeField] private float m_hingeLimitsContactDistance = 0;
        //[SerializeField] private bool m_hingeExtendedLimits = true;
        //[SerializeField] private bool m_hingeUseAcceleration = true;

        //[SerializeField] private float m_hingeBreakForce = float.PositiveInfinity;
        //[SerializeField] private float m_hingeBreakTorque = float.PositiveInfinity;
        //[SerializeField] private bool m_hingeEnableCollision = true;

        //[SerializeField] private bool m_hingeEnablePreprocess = true;
        //[SerializeField] private float m_hingeMassScale = 1;
        //[SerializeField] private float m_hingeConnectedMassScale = 1;

        [Header("Top Box Spring Settings")]
        [SerializeField] private float m_springStrenght = 100.0f;
        [SerializeField] private float m_springDamper = 0;
        [SerializeField] private float m_springMinDistance = 0;
        [SerializeField] private float m_springMaxDistance = 0.2f;
        [SerializeField] private float m_springTolerance = 0.06f;
        [SerializeField] private bool m_springEnableCollision = true;
        [SerializeField] private float m_springBreakForce = 100.0f;
        [SerializeField] private float m_springBreakTorque = 100.0f;

        //[SerializeField] private bool m_springAutoConfigConnAnchor = true;
        //[SerializeField] private bool m_springEnablePreprocess = true;
        //[SerializeField] private float m_springMassScale = 1;
        //[SerializeField] private float m_springConnectedMassScale = 1;

        //[Header("Undroppable Boxes Spring Settings")]
        //[SerializeField] private float m_undroppableBoxesSrpingStrenght = 0.0f;
        //[SerializeField] private float m_undroppableBoxesSpringDamper = 0;
        //[SerializeField] private float m_undroppableBoxesSpringMinDistance = 0;
        //[SerializeField] private float m_undroppableBoxesSpringMaxDistance = 0.2f;
        //[SerializeField] private float m_undroppableBoxesSpringTolerance = 0.00f;
        //[SerializeField] private bool m_undroppableBoxesSpringEnableCollision = false;
        //[SerializeField] private float m_undroppableBoxesSpringBreakForce = 100000000.0f;
        //[SerializeField] private float m_undroppableBoxesSpringBreakTorque = 10000000.0f;
        //[SerializeField] private bool m_undroppableBoxesSpringAutoConfigConnAnchor = true;
        //[SerializeField] private bool m_undroppableBoxesSpringEnablePreprocess = true;
        //[SerializeField] private float m_undroppableBoxesSpringMassScale = 1;
        //[SerializeField] private float m_undroppableBoxesSpringConnectedMassScale = 1;

        private Eside m_side;

        [Header("ReadOnly")]
        [SerializeField] private float m_angleRead = 0;

        //public enum JointMode
        //{
        //    Spring,
        //    Hinge,
        //    None,
        //    Count
        //}

        enum Eside
        {
            left,
            right
        }

        private void Awake()
        {
            foreach (GameObject go in m_boxesWithHinge)
            {
                //m_hinges.Add(go.GetComponent<HingeJoint>());
                //PAS BESOIN DE PRENDRE LIGNE EN BAS CAR TU AS DEJA CETTE DONN� UTILIS� DANS GetTopBox.ReplaceBoxToOrigin()
                m_boxesInitialPosition.Add(go.transform.localPosition);
            }

            _TowerBoxSystem = GetComponent<TowerBoxSystem2>();
            m_side = Eside.left;
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

            if (m_hinges.Count <= 0) return;
            //Debug.Log("Angle: " + m_hinges[0].angle);
            // DOIT REGARDER ANGLE DE LA BOITE AVEC LE PREMIER HINGE
            if (m_hinges[0].angle > m_angleOffset && m_side == Eside.right)
            {

                _TowerBoxSystem.ReplaceAllBoxToOrigin();
                EnabledColliderOnBoxes(true);
                ChangeAllAnchors(Eside.left);

            }
            else if (m_hinges[0].angle < -m_angleOffset && m_side == Eside.left)
            {
                _TowerBoxSystem.ReplaceAllBoxToOrigin();
                EnabledColliderOnBoxes(true);
                ChangeAllAnchors(Eside.right);
            }
            else if (m_hinges[0].angle > -m_angleOffset && m_hinges[0].angle < m_angleOffset) 
            {
                //Debug.Log("Angle is between -0.1 and 0.1");
                EnabledColliderOnBoxes(false);
            }

            UpdateSprings();

            m_angleRead = m_hinges[0].angle;
        }

        private void LateUpdate()
        {

            foreach (GameObject box in m_boxesWithHinge)
            {
                Vector3 lockedPosition = new Vector3(box.transform.localPosition.x, box.transform.localPosition.y, _TowerBoxSystem.GetFirstBox().transform.localPosition.z);
                box.transform.localPosition = lockedPosition;
            }

        }

        private void EnabledColliderOnBoxes(bool value)
        {
            foreach (GameObject box in m_boxesWithHinge)
            {
                box.GetComponent<BoxCollider>().enabled = value;
            }
        }

        private void ReplaceAllBoxesToOrigin()
        {
            Debug.Log("ReplaceAllBoxesToOrigin: " + m_boxesWithHinge.Count);
            for (int i = 0; i < m_boxesWithHinge.Count; i++)
            {
                Debug.Log("i: " + i);
                Debug.Log("ReplaceAllBoxesToOrigin: " + m_boxesWithHinge[i].name);
                m_boxesWithHinge[i].transform.localPosition = m_boxesInitialPosition[i];
            }
        }

        private void ChangeAllAnchors(Eside wantedSide)
        {
            m_side = wantedSide;
            Vector3 anchorPosition = m_side == Eside.left ? new Vector3(-_TowerBoxSystem.GetTopBox().GetAnchorWidth(), -_TowerBoxSystem.GetTopBox().GetAnchorHeight(), 0) : new Vector3(_TowerBoxSystem.GetTopBox().GetAnchorWidth(), -_TowerBoxSystem.GetTopBox().GetAnchorHeight(), 0);

            foreach (HingeJoint hingeJoint in m_hinges)
            {
                hingeJoint.anchor = anchorPosition;
            }

        }

        private void UpdateSprings()
        {
            for (int i = 0; i < m_hinges.Count; i++)
            {
                JointSpring spring = m_hinges[i].spring;
                spring.targetPosition = m_angle;
                spring.spring = m_springForce / (i + 1); // POUR DIMINUER LA FORCE DU SPRING DES BOITES AU DESSUS
                m_hinges[i].spring = spring;
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

        /// <summary> Ajoute un joint à la boite</summary>
        public void ModifyJoint()
        {
            if (_TowerBoxSystem.GetBoxCount() <= m_nbOfUndroppableBoxes) return;

            Box2 topBox = _TowerBoxSystem.GetTopBox();
            if (topBox == null) { Debug.LogWarning("Top Box est null"); return; }
            Rigidbody topBoxRB = topBox.GetComponent<Rigidbody>();
            if (topBoxRB == null) { Debug.LogWarning("Top Box Rigidbody est null"); return; }
            Rigidbody previousTopBoxRB = _TowerBoxSystem.GetPreviousTopBox().GetComponent<Rigidbody>();
            if (previousTopBoxRB == null) { Debug.LogWarning("Previous Top Box Rigidbody est null"); return; }

            if (previousTopBoxRB.GetComponent<CollisionDetector>().enabled)
                previousTopBoxRB.GetComponent<CollisionDetector>().enabled = false;
            if (topBoxRB.GetComponent<CollisionDetector>().enabled == false)
                topBoxRB.GetComponent<CollisionDetector>().enabled = true;

            //DisablePhysicsOnRB(previousTopBoxRB);

            //RemoveJointFromBoxRB(previousTopBoxRB);
            //AddJointBetween(previousTopBoxRB, topBoxRB);
            AttachJoint(previousTopBoxRB, topBoxRB);

            //EnablePhysicOnRB(topBoxRB);
        }

        private void AttachJoint(Rigidbody attachedBody, Rigidbody sourceBody)
        {
            Debug.Log("attachedBody: " + attachedBody.name + " sourceBody: " + sourceBody.name);
            HingeJoint hingeJoint = sourceBody.gameObject.GetComponent<HingeJoint>();
            if (hingeJoint == null)
            {
                Debug.LogWarning("Hinge joint is null");
                return;
            }

            hingeJoint.connectedBody = attachedBody;
            m_hinges.Add(hingeJoint);
            m_boxesWithHinge.Add(sourceBody.gameObject);
        }

        //private static void DisablePhysicsOnRB(Rigidbody _rigidBody)
        //{
        //    if (_rigidBody.GetComponent<CollisionDetector>().enabled)
        //        _rigidBody.GetComponent<CollisionDetector>().enabled = false;
        //}

        //private static void EnablePhysicOnRB(Rigidbody _rigidBody)
        //{
        //    if (_rigidBody.GetComponent<CollisionDetector>().enabled == false)
        //        _rigidBody.GetComponent<CollisionDetector>().enabled = true;
        //    //_rigidBody.isKinematic = false;
        //}


        //private void RemoveJointFromBoxRB(Rigidbody boxRB)
        //{
        //    ReplaceBoxToOrigin();
        //    Joint previousJoint = null;

        //    if (m_currentJointMode == JointMode.Hinge)
        //    {
        //        previousJoint = boxRB.GetComponent<HingeJoint>();
        //        if (previousJoint == null)
        //        {
        //            Debug.LogWarning("Hinge joint is null");
        //            return;
        //        }

        //        //previousJoint.connectedBody = null;
        //    }
        //    else if (m_currentJointMode == JointMode.Spring)
        //    {
        //        SpringJoint springJoint = boxRB.GetComponent<SpringJoint>();
        //        if (springJoint == null)
        //        {
        //            Debug.LogWarning("Spring joint is null");
        //            return;
        //        }

        //        springJoint.connectedBody = null;
        //        previousJoint = springJoint;
        //    }

        //    //boxRB.isKinematic = true;
        //    //Destroy(previousJoint); // If we destroy the joint we need to add the new top box spring everytime a box is removed from the tower
        //}

        private void ReplaceBoxToOrigin()
        {
            Box2 previousTopBox = _TowerBoxSystem.GetPreviousTopBox();
            previousTopBox.ReplaceBoxToOrigin();
            previousTopBox.transform.eulerAngles = Player.transform.eulerAngles;
        }

        /// <summary> Retire un item avec une force provenant de l'exterieur </summary>
        public void RemoveItemImpulse(Vector3 velocity)
        {
            Box2 topBox = _TowerBoxSystem.GetTopBox();

            if (topBox.GetItemsInBox().Count <= 0)
            {
                Debug.LogWarning("No item in the box");
                return;
            }

            Box2.ItemInBox lastItemInBox = topBox.GetLastItem();
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
            m_boxesWithHinge.Remove(lastItemInBox.m_item);
            topBox.GetItemsInBox().Remove(lastItemInBox);

            topBox.ResetSlots(lastItemInBox);
        }

        /// <summary> Retire une boite avec une force provenant de l'exterieur </summary>
        public void RemoveBoxImpulse(Vector3 velocity, bool single = false)
        {

            //MoveTopJointToNewTopBox();

            Box2 topBox = _TowerBoxSystem.GetTopBox();
            if (topBox == null)
            {
                Debug.LogWarning("No box to remove");
                return;
            }

            HingeJoint hingeJoint = topBox.GetComponent<HingeJoint>();
            if (hingeJoint == null)
            {
                Debug.LogWarning("Hinge joint is null");
                return;
            }

            hingeJoint.connectedBody = null;
            m_hinges.Remove(hingeJoint);

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

            topBox.GetComponent<Rigidbody>().isKinematic = false;
            topBox.GetComponent<Rigidbody>().AddForce(totalImpulse, ForceMode.Impulse);

            // TODO Remi: Do not forget to uncomment
            //if (topBox.GetComponent<AutoDestruction>().enabled) return;

            //Tower.RemoveLastBoxFromTower();
            //topBox.GetComponent<AutoDestruction>().enabled = true;
        }

        ///// <summary> Ajoute et configure les valeurs du join entre deux rigidbody </summary>
        //private void AddJointBetween(Rigidbody attachedBody, Rigidbody sourceBody)
        //{
        //    if (m_currentJointMode == JointMode.Hinge)
        //    {
        //        HingeJoint hingeJoint = sourceBody.gameObject.AddComponent<HingeJoint>();
        //        if (hingeJoint == null)
        //        {
        //            Debug.LogWarning("Spring joint is null");
        //            return;
        //        }

        //        hingeJoint.connectedBody = attachedBody;

        //        BoxSetup boxSetup = sourceBody.GetComponent<BoxSetup>();
        //        float boxWidth = boxSetup.BoxWidth;
        //        float boxHeight = boxSetup.BoxHeight;

        //        hingeJoint.axis = new Vector3(0, 0, 1);

        //        hingeJoint.useSpring = true;
        //        JointSpring spring = hingeJoint.spring;
        //        {
        //            spring.targetPosition = m_angle;
        //            spring.spring = m_springForce;
        //        }
        //        hingeJoint.spring = spring;


        //        hingeJoint.useLimits = false;
        //        JointLimits limits = hingeJoint.limits;
        //        {
        //            limits.min = 0f;
        //            limits.max = 90f;
        //            limits.bounciness = 0;
        //            limits.bounceMinVelocity = 0;
        //            //limits.contactDistance = 180;
        //        }
        //        hingeJoint.limits = limits;

        //        m_hinges.Add(hingeJoint);

        //        hingeJoint.breakForce = float.PositiveInfinity;
        //        hingeJoint.breakTorque = float.PositiveInfinity;
        //        hingeJoint.enableCollision = true;

        //    }
        //    else if (m_currentJointMode == JointMode.Spring)
        //    {
        //        SpringJoint springJoint = sourceBody.gameObject.AddComponent<SpringJoint>();

        //        springJoint.connectedBody = attachedBody;

        //        springJoint.spring = m_springStrenght;
        //        springJoint.damper = m_springDamper;
        //        springJoint.minDistance = m_springMinDistance;
        //        springJoint.maxDistance = m_springMaxDistance;
        //        springJoint.tolerance = m_springTolerance;
        //        springJoint.breakForce = m_springBreakForce;
        //        springJoint.breakTorque = m_springBreakTorque;
        //        springJoint.enableCollision = m_springEnableCollision;

        //    }
        //}

        /// <summary> Vérifie si le contenu de la tour (boite ou item) peut tomber </summary>
        public void CheckIfCanDropContent(Vector3 velocity)
        {
            Box2 box = _TowerBoxSystem.GetTopBox();
            if (box == null)
            {
                Debug.LogWarning("No box to check");
                return;
            }

            if (box.IsEmpty() && _TowerBoxSystem.GetBoxCount() > m_nbOfUndroppableBoxes)
                RemoveBoxImpulse(velocity);
            else if (!box.IsEmpty())
                RemoveItemImpulse(velocity);
        }

        public Box2 GetBoxUnderneath(Box2 upperBox)
        {
            if (_TowerBoxSystem.GetBoxCount() < 2)
                return null;

            return _TowerBoxSystem.GetAllBoxes().ToArray()[GetBoxIndex(upperBox) - 1];
        }

        private int GetBoxIndex(Box2 box)
        {
            Box2[] boxes = _TowerBoxSystem.GetAllBoxes().ToArray();
            for (int i = 0; i < boxes.Length; i++)
            {
                if (boxes[i] == box)
                    return i;
            }

            return -1;
        }

        internal GameObject GetBoxPrefabFromDropableOrder()
        {
            Debug.Log("Box count: " + _TowerBoxSystem.GetBoxCount() + " <= " + m_nbOfUndroppableBoxes + " ? " + (_TowerBoxSystem.GetBoxCount() <= m_nbOfUndroppableBoxes));
            if (_TowerBoxSystem.GetBoxCount() <= m_nbOfUndroppableBoxes)
            {
                Debug.Log("Box is undroppable");
                return m_boxNoHingePrefab;
            }
            else
            {
                Debug.Log("Box is droppable with hinge");
                return m_boxWithHingePrefab;
            }
        }
    }
}
