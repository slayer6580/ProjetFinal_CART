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

        private TowerBoxSystem Tower { get; set; } = null;

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

        public enum JointMode
        {
            Spring,
            Hinge,
            None,
            Count
        }

        enum Eside
        {
            left,
            right
        }

        private void Awake()
        {
            Tower = GetComponent<TowerBoxSystem>();
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

            //foreach (HingeJoint hingeJoint in m_hinges)
            //{
            //    JointSpring spring = hingeJoint.spring;
            //    spring.targetPosition = m_angle;
            //    spring.spring = m_springForce;
            //    hingeJoint.spring = spring;
            //}

            if (m_hinges.Count <= 0) return;

            // DOIT REGARDER ANGLE DE LA BOITE AVEC LE PREMIER HINGE
            if (m_hinges[0].angle > m_angleOffset && m_side == Eside.right)
            {
                Tower.ReplaceAllBoxToOrigin();
                EnabledColliderOnBoxes(true);
                ChangeAllAnchors(Eside.left);

            }
            else if (m_hinges[0].angle < -m_angleOffset && m_side == Eside.left)
            {
                Tower.ReplaceAllBoxToOrigin();
                EnabledColliderOnBoxes(true);
                ChangeAllAnchors(Eside.right);
            }
            else if (m_hinges[0].angle > -m_angleOffset && m_hinges[0].angle < m_angleOffset)
            {
                EnabledColliderOnBoxes(false);
            }

            UpdateSprings();
        }

        private void EnabledColliderOnBoxes(bool value)
        {
            Stack<Box> allBoxes = Tower.GetAllBoxes();
            foreach (Box box in allBoxes)
            {
                box.EnabledAllColliders(value);
            }
        }

        private void ChangeAllAnchors(Eside wantedSide)
        {
            m_side = wantedSide;
            // BOX_HALF_WIDTH = GetTopBox.GetAnchorWidth()new Vector3(-Tower.GetTopBox().GetAnchorWidth(), -Tower.GetTopBox().GetAnchorHeight(), 0)
            // BOX_HALF_HEIGHT = GetTopBox.GetAnchorHeight()
            Vector3 anchorPosition = m_side == Eside.left ? new Vector3(-Tower.GetTopBox().GetAnchorWidth(), -Tower.GetTopBox().GetAnchorHeight(), 0) : new Vector3(Tower.GetTopBox().GetAnchorWidth(), -Tower.GetTopBox().GetAnchorHeight(), 0);

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
                spring.spring = m_springForce; // POUR DIMINUER LA FORCE DU SPRING DES BOITES AU DESSUS
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

            //DisablePhysicsOnRB(previousTopBoxRB);

            RemoveJointFromBoxRB(previousTopBoxRB);
            AddJointBetween(previousTopBoxRB, topBoxRB);

            EnablePhysicOnRB(topBoxRB);
        }

        private static void DisablePhysicsOnRB(Rigidbody _rigidBody)
        {
            if (_rigidBody.GetComponent<CollisionDetector>().enabled)
                _rigidBody.GetComponent<CollisionDetector>().enabled = false;
        }

        private static void EnablePhysicOnRB(Rigidbody _rigidBody)
        {
            _rigidBody.GetComponent<CollisionDetector>().enabled = true;
            _rigidBody.isKinematic = false;
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

                //previousJoint.connectedBody = null;
            }
            else if (m_currentJointMode == JointMode.Spring)
            {
                SpringJoint springJoint = boxRB.GetComponent<SpringJoint>();
                if (springJoint == null)
                {
                    Debug.LogWarning("Spring joint is null");
                    return;
                }

                springJoint.connectedBody = null;
                //springJoint.spring = m_undroppableBoxesSrpingStrenght;
                //springJoint.damper = m_undroppableBoxesSpringDamper;
                //springJoint.minDistance = m_undroppableBoxesSpringMinDistance;
                //springJoint.maxDistance = m_undroppableBoxesSpringMaxDistance;
                //springJoint.tolerance = m_undroppableBoxesSpringTolerance;
                //springJoint.breakForce = m_undroppableBoxesSpringBreakForce;
                //springJoint.breakTorque = m_undroppableBoxesSpringBreakTorque;
                //springJoint.enableCollision = m_undroppableBoxesSpringEnableCollision;
                //springJoint.enablePreprocessing = m_undroppableBoxesSpringEnablePreprocess;
                //springJoint.massScale = m_undroppableBoxesSpringMassScale;
                //springJoint.connectedMassScale = m_undroppableBoxesSpringConnectedMassScale;

                previousJoint = springJoint;
            }

            //boxRB.isKinematic = true;
            //Destroy(previousJoint); // If we destroy the joint we need to add the new top box spring everytime a box is removed from the tower
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

            //MoveTopJointToNewTopBox();

            Box topBox = Tower.GetTopBox();
            if (topBox == null)
            {
                Debug.LogWarning("No box to remove");
                return;
            }

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

            if (m_currentJointMode == JointMode.Hinge)
                m_hinges.Remove(topBox.GetComponent<HingeJoint>());

            // TODO Remi: Do not forget to uncomment
            //if (topBox.GetComponent<AutoDestruction>().enabled) return;

            //Tower.RemoveLastBoxFromTower();
            //topBox.GetComponent<AutoDestruction>().enabled = true;
        }

        /// <summary> Retire le top joint de la top box et donne les valeurs top joint au joint de la nouvelle top box </summary>
        private void MoveTopJointToNewTopBox()
        {
            if (Tower.GetBoxCount() <= m_nbOfUndroppableBoxes) return;

            Box oldTopBox = Tower.GetTopBox();

            if (m_currentJointMode == JointMode.Spring)
            {
                if (oldTopBox == null) { Debug.LogWarning("No box to remove"); return; }
                SpringJoint oldTopSpringJoint = oldTopBox.GetComponent<SpringJoint>();
                if (oldTopSpringJoint == null) { Debug.LogWarning("Spring joint is null"); return; }
                if (oldTopSpringJoint != null)
                    Destroy(oldTopSpringJoint);

                Box newTopBox = Tower.GetPreviousTopBox();
                if (newTopBox == null) { Debug.LogWarning("No new top box"); return; }
                SpringJoint newTopSpringJoint = newTopBox.GetComponent<SpringJoint>();
                if (newTopSpringJoint == null) { Debug.LogWarning("New Spring joint is null"); return; }
                Rigidbody newLowerBoxRB = GetBoxUnderneath(newTopBox).GetComponent<Rigidbody>();
                if (newLowerBoxRB == null) { Debug.LogWarning("New lower box RB is null"); return; }

                newTopSpringJoint.connectedBody = newLowerBoxRB;
                //newTopSpringJoint.autoConfigureConnectedAnchor = m_springAutoConfigConnAnchor;
                newTopSpringJoint.spring = m_springStrenght;
                newTopSpringJoint.damper = m_springDamper;
                newTopSpringJoint.minDistance = m_springMinDistance;
                newTopSpringJoint.maxDistance = m_springMaxDistance;
                newTopSpringJoint.tolerance = m_springTolerance;
                newTopSpringJoint.breakForce = m_springBreakForce;
                newTopSpringJoint.breakTorque = m_springBreakTorque;
                newTopSpringJoint.enableCollision = m_springEnableCollision;
                //newTopSpringJoint.enablePreprocessing = m_springEnablePreprocess;
                //newTopSpringJoint.massScale = m_springMassScale;
                //newTopSpringJoint.connectedMassScale = m_springConnectedMassScale;
            }
            else if (m_currentJointMode == JointMode.Hinge)
            {
                if (oldTopBox == null) { Debug.LogWarning("No box to remove"); return; }
                HingeJoint oldTopSpringJoint = oldTopBox.GetComponent<HingeJoint>();
                if (oldTopSpringJoint == null) { Debug.LogWarning("Spring joint is null"); return; }
                if (oldTopSpringJoint != null)
                    Destroy(oldTopSpringJoint);

                Box newTopBox = Tower.GetPreviousTopBox();
                if (newTopBox == null) { Debug.LogWarning("No new top box"); return; }
                HingeJoint newTopHingeJoint = newTopBox.GetComponent<HingeJoint>();
                if (newTopHingeJoint == null) { Debug.LogWarning("New Spring joint is null"); return; }
                Rigidbody newLowerBoxRB = GetBoxUnderneath(newTopBox).GetComponent<Rigidbody>();
                if (newLowerBoxRB == null) { Debug.LogWarning("New lower box RB is null"); return; }

                newTopHingeJoint.connectedBody = newLowerBoxRB;
                //newTopHingeJoint.autoConfigureConnectedAnchor = m_hingeAutoConfigConnAnchor;
                //newTopHingeJoint.breakForce = m_hingeBreakForce;
                //newTopHingeJoint.breakTorque = m_hingeBreakTorque;
                newTopHingeJoint.enableCollision = true;
                //newTopHingeJoint.enablePreprocessing = m_hingeEnablePreprocess;
                //newTopHingeJoint.massScale = m_hingeMassScale;
                //newTopHingeJoint.connectedMassScale = m_hingeConnectedMassScale;
            }
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


                // Vector3 anchorPosition = wantedSide == Eside.left ? new Vector3(-BOX_HALF_WIDTH, -BOX_HALF_HEIGHT, 0) : new Vector3(0.5f, -0.5f, 0);
                // Get the top bordures of the box to set the anchor
                BoxSetup boxSetup = sourceBody.GetComponent<BoxSetup>();
                float boxWidth = boxSetup.BoxWidth;
                float boxHeight = boxSetup.BoxHeight;
                //float boxLength = boxSetup.BoxLength;
                Vector3 anchorPosLeft = new Vector3(-Tower.GetTopBox().GetAnchorWidth(), -Tower.GetTopBox().GetAnchorHeight(), 0);
                //Vector3 anchorPosRight = new Vector3(Tower.GetTopBox().GetAnchorWidth(), -Tower.GetTopBox().GetAnchorHeight(), 0);
                //anchorPosLeft += Vector3.up * boxHeight / 2;

                // Hinge on the right side of the box
                hingeJoint.anchor = anchorPosLeft;

                //hingeJoint.axis = m_hingeAxis;
                hingeJoint.axis = new Vector3(0, 0, 1);

                hingeJoint.useSpring = true;
                JointSpring spring = hingeJoint.spring;
                {
                    spring.targetPosition = m_angle;
                    spring.spring = m_springForce;
                }
                hingeJoint.spring = spring;

                //hingeJoint.autoConfigureConnectedAnchor = m_hingeAutoConfigConnAnchor;
                //hingeJoint.connectedAnchor = m_hingeConnAnchor;


                //hingeJoint.spring = new JointSpring();
                //JointSpring spring = hingeJoint.spring;
                //{
                //    spring.spring = m_hingeSpringStrenght;
                //    spring.damper = m_hingeSpringDamper;
                //    spring.targetPosition = m_hingeTargetPos;
                //}
                //hingeJoint.spring = spring;


                //hingeJoint.useMotor = m_hingeUseMotor;
                //hingeJoint.motor = new JointMotor();
                //JointMotor motor = hingeJoint.motor;
                //{
                //    motor.targetVelocity = m_hingeMotorTargetVelocity;
                //    motor.force = m_hingeMotorForce;
                //    motor.freeSpin = m_hingeMotorFreespin;
                //}
                //hingeJoint.motor = motor;

                hingeJoint.useLimits = false;
                JointLimits limits = hingeJoint.limits;
                {
                    limits.min = 0f;
                    limits.max = 90f;
                    limits.bounciness = 0;
                    limits.bounceMinVelocity = 0;
                    //limits.contactDistance = 180;
                }
                hingeJoint.limits = limits;

                m_hinges.Add(hingeJoint);

                //hingeJoint.extendedLimits = m_hingeExtendedLimits;
                //hingeJoint.useAcceleration = m_hingeUseAcceleration;
                hingeJoint.breakForce = float.PositiveInfinity;
                hingeJoint.breakTorque = float.PositiveInfinity;
                hingeJoint.enableCollision = true;
                //hingeJoint.enablePreprocessing = m_hingeEnablePreprocess;
                //hingeJoint.massScale = m_hingeMassScale;
                //hingeJoint.connectedMassScale = m_hingeConnectedMassScale;
            }
            else if (m_currentJointMode == JointMode.Spring)
            {
                SpringJoint springJoint = sourceBody.gameObject.AddComponent<SpringJoint>();

                springJoint.connectedBody = attachedBody;
                //springJoint.autoConfigureConnectedAnchor = m_springAutoConfigConnAnchor;
                springJoint.spring = m_springStrenght;
                springJoint.damper = m_springDamper;
                springJoint.minDistance = m_springMinDistance;
                springJoint.maxDistance = m_springMaxDistance;
                springJoint.tolerance = m_springTolerance;
                springJoint.breakForce = m_springBreakForce;
                springJoint.breakTorque = m_springBreakTorque;
                springJoint.enableCollision = m_springEnableCollision;
                //springJoint.enablePreprocessing = m_springEnablePreprocess;
                //springJoint.massScale = m_springMassScale;
                //springJoint.connectedMassScale = m_springConnectedMassScale;
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

        public Box GetBoxUnderneath(Box upperBox)
        {
            if (Tower.GetBoxCount() < 2)
                return null;

            return Tower.GetAllBoxes().ToArray()[GetBoxIndex(upperBox) - 1];
        }

        private int GetBoxIndex(Box box)
        {
            Box[] boxes = Tower.GetAllBoxes().ToArray();
            for (int i = 0; i < boxes.Length; i++)
            {
                if (boxes[i] == box)
                    return i;
            }

            return -1;
        }
    }
}
