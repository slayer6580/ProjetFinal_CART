using DiscountDelirium;
using System;
using UnityEngine;

namespace BoxSystem
{
    public class TowerPhysics : MonoBehaviour
    {
        [field: SerializeField] private GameObject DebugCartPrefab { get; set; } = null;
        [field: SerializeField] private GameObject Player { get; set; } = null;

        [Header("Items and boxes physics settings")]
        [SerializeField] private float m_itemRemoveImpulseIntesity = 0.2f;
        [SerializeField] private float m_itemVectorUpImpulseIntesity = 10.0f;
        [SerializeField] private float m_boxRemoveImpulseIntesity = 0.2f;
        [SerializeField] private float m_boxVectorUpImpulseIntesity = 10.0f;
        [SerializeField] private int m_nbOfUndroppableBoxes = 4;

        [Header("Debug Cart Settings")]
        [SerializeField] private float m_debugCartDistance = 10.0f;
        [SerializeField] private float m_debugCartSpeed = 3.0f;

        [Header("Joint Settings")]
        [SerializeField] private JointMode m_currentJointMode = JointMode.Spring;

        [Header("Hinge Settings")]
        [SerializeField] private Vector3 m_hingeAnchor = Vector3.zero;
        [SerializeField] private Vector3 m_hingeAxis = Vector3.zero;
        [SerializeField] private bool m_hingeAutoConfigConnAnchor = true;
        [SerializeField] private Vector3 m_hingeConnAnchor = Vector3.zero;
        [SerializeField] private bool m_hingeUseSpring = true;
        [SerializeField] private float m_hingeSpringStrenght = 100.0f;
        [SerializeField] private float m_hingeSpringDamper = 0;
        [SerializeField] private float m_hingeTargetPos = 0;
        [SerializeField] private bool m_hingeUseMotor = true;
        [SerializeField] private float m_hingeMotorTargetVelocity = 0;
        [SerializeField] private float m_hingeMotorForce = 100.0f;
        [SerializeField] private bool m_hingeMotorFreespin = true;
        [SerializeField] private bool m_hingUseLimits = true;
        [SerializeField] private float m_hingeLimitsMin = 0;
        [SerializeField] private float m_hingeLimitsMax = 0;
        [SerializeField] private float m_hingeLimitsBounciness = 0;
        [SerializeField] private float m_hingeLimitsContactDistance = 0;
        [SerializeField] private bool m_hingeExtendedLimits = true;
        [SerializeField] private bool m_hingeUseAcceleration = true;
        [SerializeField] private float m_hingeBreakForce = 100.0f;
        [SerializeField] private float m_hingeBreakTorque = 100.0f;
        [SerializeField] private bool m_hingeEnableCollision = true;
        [SerializeField] private bool m_hingeEnablePreprocess = true;
        [SerializeField] private float m_hingeMassScale = 1;
        [SerializeField] private float m_hingeConnectedMassScale = 1;

        [Header("Spring Settings")]
        [SerializeField] private float m_springStrenght = 100.0f;
        [SerializeField] private float m_springDamper = 0;
        [SerializeField] private float m_springMinDistance = 0;
        [SerializeField] private float m_springMaxDistance = 0.2f;
        [SerializeField] private float m_springTolerance = 0.06f;
        [SerializeField] private bool m_springEnableCollision = true;
        [SerializeField] private float m_springBreakForce = 100.0f;
        [SerializeField] private float m_springBreakTorque = 100.0f;
        [SerializeField] private bool m_springAutoConfigConnAnchor = true;
        [SerializeField] private bool m_springEnablePreprocess = true;
        [SerializeField] private float m_springMassScale = 1;
        [SerializeField] private float m_springConnectedMassScale = 1;


        private TowerBoxSystem Tower { get; set; } = null;

        public enum JointMode
        {
            Spring,
            Hinge,
            None,
            Count
        }

        private void Awake()
        {
            Tower = GetComponent<TowerBoxSystem>();
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
            if (Tower.GetBoxCount() <= m_nbOfUndroppableBoxes) return;

            Box topBox = Tower.GetTopBox();
            if (topBox == null) { Debug.LogWarning("Top Box est null"); return; }
            Rigidbody topBoxRB = topBox.GetComponent<Rigidbody>();
            if (topBoxRB == null) { Debug.LogWarning("Top Box Rigidbody est null"); return; }
            Rigidbody previousTopBoxRB = Tower.GetPreviousTopBox().GetComponent<Rigidbody>();
            if (previousTopBoxRB == null) { Debug.LogWarning("Previous Top Box Rigidbody est null"); return; }

            DisablePhysicsOnRB(previousTopBoxRB);

            RemoveJointFromBoxRB(previousTopBoxRB);
            AddJointBetween(previousTopBoxRB, topBoxRB);

            EnablePhysicOnRB(topBoxRB);
        }

        private static void EnablePhysicOnRB(Rigidbody _rigidBody)
        {
            _rigidBody.GetComponent<CollisionDetector>().enabled = true;
            _rigidBody.isKinematic = false;
        }

        private static void DisablePhysicsOnRB(Rigidbody _rigidBody)
        {
            _rigidBody.GetComponent<CollisionDetector>().enabled = false;
        }

        private void RemoveJointFromBoxRB(Rigidbody boxRB) 
        {
            ReplaceBoxToOrigin();
            Joint previousJoint = null;

            if (m_currentJointMode == JointMode.Hinge)
            {
                previousJoint = boxRB.GetComponent<HingeJoint>();
                if (previousJoint == null)
                {
                    Debug.LogWarning("Hinge joint is null");
                    return;
                }
                
                previousJoint.connectedBody = null;
            }
            else if (m_currentJointMode == JointMode.Spring)
            {
                previousJoint = boxRB.GetComponent<SpringJoint>();
                if (previousJoint == null)
                {
                    Debug.LogWarning("Spring joint is null");
                    return;
                }

                previousJoint.connectedBody = null;
            }

            boxRB.isKinematic = true;
            Destroy(previousJoint);
        }

        private void ReplaceBoxToOrigin()
        {
            Box previousTopBox = Tower.GetPreviousTopBox();
            previousTopBox.ReplaceBoxToOrigin();
            previousTopBox.transform.eulerAngles = Player.transform.eulerAngles;
        }

        /// <summary> Retire un item avec une force provenant de l'exterieur </summary>
        public void RemoveItemImpulse(Vector3 velocity)
        {
            Box topBox = Tower.GetTopBox();

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
            Box topBox = Tower.GetTopBox();
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

            topBox.GetComponent<Rigidbody>().isKinematic = false;
            topBox.GetComponent<Rigidbody>().AddForce(totalImpulse, ForceMode.Impulse);

            if (topBox.GetComponent<AutoDestruction>().enabled) return;

            Tower.RemoveLastBoxFromTower();
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

        /// <summary> Ajoute et configure les valeurs du join entre deux rigidbody </summary>
        private void AddJointBetween(Rigidbody attachedBody, Rigidbody sourceBody)
        {
            if (m_currentJointMode == JointMode.Hinge)
            {
                HingeJoint hingeJoint = sourceBody.gameObject.AddComponent<HingeJoint>();
                if (hingeJoint == null)
                {
                    Debug.LogWarning("Spring joint is null");
                    return;
                }

                hingeJoint.connectedBody = attachedBody;
                hingeJoint.anchor = m_hingeAnchor;
                hingeJoint.axis = m_hingeAxis;
                hingeJoint.autoConfigureConnectedAnchor = m_hingeAutoConfigConnAnchor;
                hingeJoint.connectedAnchor = m_hingeConnAnchor;

                hingeJoint.useSpring = m_hingeUseSpring;
                hingeJoint.spring = new JointSpring();
                JointSpring spring = hingeJoint.spring;
                {
                    spring.spring = m_hingeSpringStrenght;
                    spring.damper = m_hingeSpringDamper;
                    spring.targetPosition = m_hingeTargetPos;
                }

                hingeJoint.useMotor = m_hingeUseMotor;
                hingeJoint.motor = new JointMotor();
                JointMotor motor = hingeJoint.motor;
                {
                    motor.targetVelocity = m_hingeMotorTargetVelocity;
                    motor.force = m_hingeMotorForce;
                    motor.freeSpin = m_hingeMotorFreespin;
                }

                hingeJoint.useLimits = m_hingUseLimits;
                JointLimits limits = hingeJoint.limits;
                {
                    limits.min = m_hingeLimitsMin;
                    limits.max = m_hingeLimitsMax;
                    limits.bounciness = m_hingeLimitsBounciness;
                    limits.contactDistance = m_hingeLimitsContactDistance;
                }

                hingeJoint.limits = limits;
                hingeJoint.extendedLimits = m_hingeExtendedLimits;
                hingeJoint.useAcceleration = m_hingeUseAcceleration;
                hingeJoint.breakForce = m_hingeBreakForce;
                hingeJoint.breakTorque = m_hingeBreakTorque;
                hingeJoint.enableCollision = m_hingeEnableCollision;
                hingeJoint.enablePreprocessing = m_hingeEnablePreprocess;
                hingeJoint.massScale = m_hingeMassScale;
                hingeJoint.connectedMassScale = m_hingeConnectedMassScale;
            }
            else if (m_currentJointMode == JointMode.Spring)
            {
                SpringJoint springJoint = sourceBody.gameObject.AddComponent<SpringJoint>();

                springJoint.connectedBody = attachedBody;
                springJoint.autoConfigureConnectedAnchor = m_springAutoConfigConnAnchor;
                springJoint.spring = m_springStrenght;
                springJoint.damper = m_springDamper;
                springJoint.minDistance = m_springMinDistance;
                springJoint.maxDistance = m_springMaxDistance;
                springJoint.tolerance = m_springTolerance;
                springJoint.breakForce = m_springBreakForce;
                springJoint.breakTorque = m_springBreakTorque;
                springJoint.enableCollision = m_springEnableCollision;
                springJoint.enablePreprocessing = m_springEnablePreprocess;
                springJoint.massScale = m_springMassScale;
                springJoint.connectedMassScale = m_springConnectedMassScale;
            }
        }

        /// <summary> Vérifie si le contenu de la tour (boite ou item) peut tomber </summary>
        public void CheckIfCanDropContent(Vector3 velocity)
        {
            Box box = Tower.GetTopBox();
            if (box == null)
            {
                Debug.LogWarning("No box to check");
                return;
            }

            if (box.IsEmpty() && Tower.GetBoxCount() > m_nbOfUndroppableBoxes)
                RemoveBoxImpulse(velocity);
            else if (!box.IsEmpty())
                RemoveItemImpulse(velocity);
        }
    }
}
