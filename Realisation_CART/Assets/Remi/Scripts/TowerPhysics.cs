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
        [SerializeField] private float m_removeImpulseIntesity = 1.0f;
        [SerializeField] private float m_vectorUpImpulseIntesity = 1.0f;
        [Header("Debug Cart Settings")]
        [SerializeField] private float m_debugCartDistance = 10.0f;
        [SerializeField] private float m_debugCartSpeed = 3.0f;

        [SerializeField] private const int NB_UNDROPPABLE_BOXES = 4;

        private TowerBoxSystem _Tower { get; set; } = null;
        private bool m_isInHingeMode = false;

        private void Awake()
        {
            _Tower = GetComponent<TowerBoxSystem>();
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.K))
            {
                Vector3 eulerOfCart = Player.transform.rotation.eulerAngles;
                Vector3 localEulerOfTower = transform.localRotation.eulerAngles;
                Vector3 rotation = new Vector3(0, 90, 0);
                Vector3 rightSideOfPlayer = -Player.transform.right;
                Vector3 rightStartPosition = Player.transform.position + (rightSideOfPlayer * m_debugCartDistance);

                Vector3 desiredSpawnPostion = Vector3.zero;
                desiredSpawnPostion += eulerOfCart;
                desiredSpawnPostion += localEulerOfTower;
                desiredSpawnPostion += rotation;

                Transform debugCart = Instantiate(DebugCartPrefab).transform;
                debugCart.eulerAngles = desiredSpawnPostion;
                debugCart.position = rightStartPosition;
                debugCart.gameObject.GetComponent<DebugCart>().SetSpeed(m_debugCartSpeed);
            }
            else if (Input.GetKeyDown(KeyCode.L))
            {
                Vector3 eulerOfCart = Player.transform.rotation.eulerAngles;
                Vector3 localEulerOfTower = transform.localRotation.eulerAngles;
                Vector3 rotation = new Vector3(0, -90, 0);
                Vector3 leftSideOfPlayer = Player.transform.right;
                Vector3 leftStartPosition = Player.transform.position + (leftSideOfPlayer * m_debugCartDistance);

                Vector3 desiredSpawnPostion = Vector3.zero;
                desiredSpawnPostion += eulerOfCart;
                desiredSpawnPostion += localEulerOfTower;
                desiredSpawnPostion += rotation;

                Transform debugCart = Instantiate(DebugCartPrefab).transform;
                debugCart.eulerAngles = desiredSpawnPostion;
                debugCart.position = leftStartPosition;
                debugCart.gameObject.GetComponent<DebugCart>().SetSpeed(m_debugCartSpeed);
            }
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
            if (m_isInHingeMode)
            {
                AddHingeJoint();
                return;
            }

            //Debug.LogError("AddJointToBox()");
            AddSpringJoint();
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
            //Box box = _Tower.GetTopBox();

            //box.GetComponent<Rigidbody>().isKinematic = true;

            if (_Tower.GetBoxCount() <= NB_UNDROPPABLE_BOXES) // Pour ajouter un spring entre la première boite et le panier
            {
                //Box box = _Tower.GetTopBox();
                //box.GetComponent<Rigidbody>().isKinematic = true;
                return;
            }

            if (_Tower.GetBoxCount() > NB_UNDROPPABLE_BOXES) // Pour ajouter un spring entre la première boite et le panier
            {
                //Box box = _Tower.GetTopBox();
                //box.GetComponent<Rigidbody>().isKinematic = false;
                //Rigidbody cartRB = GetComponentInParent<Rigidbody>();
                //if (cartRB == null) Debug.LogError("Cart n'a pas de rigidbody");
                //SpringJoint springJoint = box.gameObject.AddComponent<SpringJoint>();
                //SetSprintJointValues(springJoint, cartRB);
            }

            if (_Tower.GetBoxCount() > 1) // Pour ajouter un spring entre les boites
            {
                //Box1 previousBoxe = m_boxesInCart.ToArray()[m_boxesInCart.Count - 1];
                //SpringJoint springJoint = m_boxesInCart.ToArray()[0].gameObject.AddComponent<SpringJoint>();
                //Rigidbody newBoxeRB = previousBoxe.GetComponent<Rigidbody>();
                //if (newBoxeRB == null)
                //{
                //    newBoxeRB = previousBoxe.AddComponent<Rigidbody>();
                //}
                //SetSprintJointValues(springJoint, newBoxeRB);
                //Box1 box1 = GetTopBox();
                ////box1.GetComponent<Rigidbody>().isKinematic = true;
                //Box box = _Tower.GetTopBox();
                //box.GetComponent<Rigidbody>().isKinematic = true;
                return;
            }

            if (_Tower.GetBoxCount() > 2) // Pour changer la force du spring du top box
            {
                //Box1 previousBox = m_boxesInCart.ToArray()[0];
                //SpringJoint previousSpringJoint = previousBox.GetComponent<SpringJoint>();
                //previousSpringJoint.spring = 10;
            }
        }

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

            Rigidbody rb = lastItemInBox.m_item.GetComponent<Rigidbody>();

            if (rb == null)
                lastItemInBox.m_item.gameObject.AddComponent<Rigidbody>().AddForce(velocity, ForceMode.Impulse);
            else
                rb.AddForce(velocity, ForceMode.Impulse);

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

            Vector3 vectorUp = topBox.transform.up * m_vectorUpImpulseIntesity;
            Vector3 incomingImpulse = velocity * m_removeImpulseIntesity;
            Vector3 impulse = vectorUp + incomingImpulse;
            Debug.Log("Incoming Impulse: " + impulse.magnitude);
            topBox.GetComponent<Rigidbody>().isKinematic = false;
            topBox.GetComponent<Rigidbody>().AddForce(vectorUp + incomingImpulse, ForceMode.Impulse);
            //topBox.GetComponent<Rigidbody>().AddForce(velocity, ForceMode.Impulse);

            //Debug.Log("AutoDestruction enabled");
            topBox.GetComponent<AutoDestruction>().enabled = true;
            _Tower.RemoveLastBoxFromTower();
        }

        /// <summary> Configure les valeurs du join Spring  </summary>
        private static void SetSprintJointValues(SpringJoint springJoint, Rigidbody newBoxeRB)
        {
            springJoint.connectedBody = newBoxeRB;
            springJoint.spring = 100;
            springJoint.damper = 0;
            springJoint.minDistance = 0;
            springJoint.maxDistance = 0.2f;
            springJoint.tolerance = 0.06f;
            springJoint.enableCollision = true;
        }

        /// <summary> Vérifie si on peut retirer le contenu de la boite </summary>
        public void CheckIfCanDropContent(Vector3 velocity)
        {
            Debug.Log("CheckIfCanDropContent");

            Box box = _Tower.GetTopBox();
            if(box == null)
            {
                Debug.LogWarning("No box to check");
                return;
            }

            if (box.IsEmpty() && _Tower.GetBoxesCount() > NB_UNDROPPABLE_BOXES)
                RemoveBoxImpulse(velocity);
            else if (!box.IsEmpty())
                RemoveItemImpulse(velocity);
        }
    }
}
