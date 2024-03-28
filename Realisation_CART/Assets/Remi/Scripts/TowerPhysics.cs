using UnityEngine;
using DiscountDelirium;
using CartControl;
using System;
using static UnityEditor.PlayerSettings;

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
        [SerializeField] public static float m_springStrenght = 100.0f;
        [SerializeField] public static float m_springDamper = 0;
        [SerializeField] public static float m_springMinDistance = 0;
        [SerializeField] public static float m_springMaxDistance = 0.2f;
        [SerializeField] public static float m_springTolerance = 0.06f;
        [SerializeField] public static bool m_springEnableCollision = true;


        private TowerBoxSystem _Tower { get; set; } = null;

        public enum JointMode
        {
            Spring,
            Hinge,
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
        }

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

        public void ModifyTopBoxSpringIntesity()
        {
            //if (m_boxCount > 2)
            //{
            //    Debug.Log("m_boxCount > 2: " + (m_boxesInCart.Count - 1));
            //    Box1 currentTopBox = m_boxesInCart.ToArray()[0];
            //    Debug.Log("previousBox: " + currentTopBox.name);
            //    SpringJoint previousSpringJoint = currentTopBox.GetComponent<SpringJoint>();
            //    previousSpringJoint.spring = 5;
            //}
        }

        public void AddJointToBox() // p-e rajouter instantBox en parametre    // Rémi
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

        private void AddHingeJoint()
        {
            // TODO: Should be done in the rigidbody in the prefab
            //if (_Tower.GetBoxCount() == 1) // Pour ajouter un spring entre la première boite et le panier
            //{
            //    Box1 box = _Tower.GetTopBox();
            //    box.GetComponent<Rigidbody>().isKinematic = true;
            //    return;
            //}

            //if (_Tower.GetBoxCount() > 1) // Pour ajouter un spring entre les boites
            //{
            //    Box1 box = _Tower.GetTopBox();
            //    box.GetComponent<Rigidbody>().isKinematic = true;
            //    return;
            //}

            //if (_Tower.GetBoxCount() > NB_UNDROPPABLE_BOXES) 
            //{
            //    Box1 box = _Tower.GetTopBox();
            //}
        }

        private void AddSpringJoint()
        {
            if (_Tower.GetBoxCount() <= m_nbOfUndroppableBoxes) return;

            Debug.Log("AddSpringJoint() _Tower.GetBoxCount(): " + _Tower.GetBoxCount());
            Box previousTopBox = null; // GetPreviousTopBox();
            if (previousTopBox == null) Debug.LogWarning("Previous Top Box est null");
            else Debug.Log("Previous Top Box: " + previousTopBox.name);

            Box topBox = _Tower.GetTopBox();
            topBox.GetComponent<Rigidbody>().isKinematic = false;

            Rigidbody previousTopBoxRB = previousTopBox.GetComponent<Rigidbody>();
            if (previousTopBoxRB == null) Debug.LogError("Previous Top Box n'a pas de rigidbody");
            SpringJoint springJoint = topBox.gameObject.AddComponent<SpringJoint>();
            SetSprintJointValues(springJoint, previousTopBoxRB);

            SpringJoint previousSpringJoint = previousTopBoxRB.GetComponent<SpringJoint>();

            if (previousSpringJoint != null)
            {
                Debug.Log("Spring found in previous box: " + previousTopBoxRB.gameObject.name);
                previousTopBoxRB.transform.localPosition = GetBoxDesiredPosition(previousTopBox);
                previousSpringJoint.spring = 0;
                previousSpringJoint.connectedBody = null;
            }
            else
            {
                Debug.Log("No srping in previous box: " + previousTopBoxRB.gameObject.name);

            }

            previousTopBoxRB.isKinematic = true;
        }

        //public Box GetPreviousTopBox()
        //{
        //    if (_Tower.m_boxesInCart.Count < 2)
        //        return null;

        //    return _Tower.m_boxesInCart.ToArray()[1];
        //}

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
            //Debug.Log("Incoming Impulse: " + impulse.magnitude);

            Rigidbody rb = lastItemInBox.m_item.GetComponent<Rigidbody>();
            if (rb == null)
            {
                rb = lastItemInBox.m_item.gameObject.AddComponent<Rigidbody>();
                rb.AddForce(totalImpulse, ForceMode.Impulse);
                rb.mass = 0.1f; // TODO Remi: Get the mass depending on the item info mass
            }
            else
                rb.AddForce(totalImpulse, ForceMode.Impulse);

            lastItemInBox.m_item.GetComponent<AutoDestruction>().enabled = true;
            Debug.Log("Item in autodestruction mode: " + lastItemInBox.m_item.name);
            topBox.GetItemsInBox().Remove(lastItemInBox);

            topBox.ResetSlots(lastItemInBox);
        }

        /// <summary> Retire une boite avec une force provenant de l'exterieur </summary>
        public void RemoveBoxImpulse(Vector3 velocity)
        {
            //Debug.Log("RemoveBoxImpulse() Velocity: " + velocity.magnitude);
            Box topBox = _Tower.GetTopBox();
            if (topBox == null)
            {
                Debug.LogWarning("No box to remove");
                return;
            }

            // Detach joint from box
            DetachJoint(topBox);

            Vector3 vectorUp = topBox.transform.up * m_boxVectorUpImpulseIntesity;
            Vector3 incomingImpulse = velocity * m_boxRemoveImpulseIntesity;
            Vector3 totalImpulse = vectorUp + incomingImpulse;
            Debug.Log("Incoming Impulse: " + totalImpulse.magnitude);
            topBox.GetComponent<Rigidbody>().isKinematic = false;
            topBox.GetComponent<Rigidbody>().AddForce(totalImpulse, ForceMode.Impulse);
            //topBox.GetComponent<Rigidbody>().AddForce(velocity, ForceMode.Impulse);

            //Debug.Log("AutoDestruction enabled");
            topBox.GetComponent<AutoDestruction>().enabled = true;
            _Tower.RemoveLastBoxFromTower();
        }

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
        private static void SetSprintJointValues(SpringJoint springJoint, Rigidbody newBoxeRB)
        {
            springJoint.connectedBody = newBoxeRB;
            springJoint.spring = m_springStrenght;
            springJoint.damper = m_springDamper;
            springJoint.minDistance = m_springMinDistance;
            springJoint.maxDistance = m_springMaxDistance;
            springJoint.tolerance = m_springTolerance;
            springJoint.enableCollision = m_springEnableCollision;
        }

        /// <summary> Vérifie si on peut retirer le contenu de la boite </summary>
        public void CheckIfCanDropContent(Vector3 velocity)
        {
            Debug.Log("CheckIfCanDropContent");

            Box box = _Tower.GetTopBox();
            if (box == null)
            {
                Debug.LogWarning("No box to check");
                return;
            }

            if (box.IsEmpty() && _Tower.GetBoxesCount() > m_nbOfUndroppableBoxes)
                RemoveBoxImpulse(velocity);
            else if (!box.IsEmpty())
                RemoveItemImpulse(velocity);
        }

        public Vector3 GetBoxDesiredPosition(Box box)
        {
            return new Vector3(0, box.gameObject.GetComponent<BoxSetup>().SlotHeight * _Tower.GetBoxesCount(), 0);
        }
    }
}
