using System;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

namespace BoxSystem
{

    public class Tower1 : MonoBehaviour
    {
        [field: Header("Mettre le Cart GO ici")]
        [field: SerializeField] public GameObject Cart { get; private set; }
        [field: SerializeField] private GameObject DebugCartPrefab { get; set; } = null;
        [field: SerializeField] public GameObject Player { get; private set; }

        [field: Header("La distance ou un item devrait snap a la boite pendant le slerp")]
        [field: SerializeField] public float ItemSnapDistance { get; private set; }

        [Header("Mettre le Prefab de la boite")]
        [SerializeField] private GameObject m_boxPrefab;
        [Header("La hauteur de placement de la boite")]
        [SerializeField] private float m_boxHeight;

        private int m_boxCount = 0;
        private Stack<Box1> m_boxesInCart = new Stack<Box1>();

        private const int MAX_BOXES_BEFORE_DROP = 4;

        void Start()
        {
            AddBoxToTower();
            AddSpringToBox();
        }

        /// <summary> Ajoute une boite a la tour </summary>
        public void AddBoxToTower()
        {
            m_boxCount++;

            // setup de la boite
            GameObject instant = Instantiate(m_boxPrefab);
            instant.transform.rotation = Player.transform.rotation;
            instant.transform.SetParent(transform);
            instant.name = "Box " + m_boxCount;
            Box1 instantBox = instant.GetComponent<Box1>();
            instantBox.SetTower(this);

            // hauteur de la boite dans la tour
            float height = (m_boxCount - 1) * m_boxHeight;
            instant.transform.localPosition = new Vector3(0, height, 0);

            // ajout a la liste
            m_boxesInCart.Push(instantBox);

            //  AddSpringToBox();  // Ta fonction ici, p-e rajouter instantBox en parametre
        }


        //public void AddBoxToTower()
        //{
        //    //Debug.Log("AddBoxToTower()");
        //    m_boxCount++;
        //    float height = (m_boxCount - 1) * m_boxHeight;
        //    Vector3 desiredPos = new Vector3(transform.position.x, height + transform.position.y, transform.position.z);

        //    GameObject instant = Instantiate(m_boxPrefab, desiredPos, Cart.transform.rotation);
        //    instant.transform.rotation = Cart.transform.rotation;
        //    instant.transform.SetParent(transform);
        //    instant.name = "Box " + m_boxCount;
        //    Box1 instantBox = instant.GetComponent<Box1>();
        //    instantBox.SetTower(this);
        //    instant.transform.position = desiredPos;
        //    m_boxesInCart.Push(instantBox);
        //}


        public void AddSpringToBox() // p-e rajouter instantBox en parametre    // R�mi
        {
            if (m_boxCount == 1) // Pour ajouter un spring entre la premi�re boite et le panier
            {
                //Rigidbody cartRB = GetComponentInParent<Rigidbody>();
                //if (cartRB == null) Debug.LogError("Cart n'a pas de rigidbody");
                //SpringJoint springJoint = m_boxesInCart.ToArray()[0].gameObject.AddComponent<SpringJoint>();
                //SetSprintJointValues(springJoint, cartRB);
                Box1 box1 = GetTopBox();
                box1.GetComponent<Rigidbody>().isKinematic = true;
                return;
            }

            if (m_boxCount > 1) // Pour ajouter un spring entre les boites
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
                //box1.GetComponent<Rigidbody>().isKinematic = true;
                Box1 box1 = GetTopBox();
                box1.GetComponent<Rigidbody>().isKinematic = true;
                return;
            }

            if (m_boxCount > 2) // Pour changer la force du spring du top box
            {
                //Box1 previousBox = m_boxesInCart.ToArray()[0];
                //SpringJoint previousSpringJoint = previousBox.GetComponent<SpringJoint>();
                //previousSpringJoint.spring = 10;
            }
        }

        private static void SetSprintJointValues(SpringJoint springJoint, Rigidbody newBoxeRB) // R�mi
        {
            springJoint.connectedBody = newBoxeRB;
            springJoint.spring = 1;
            springJoint.damper = 0;
            springJoint.minDistance = 0;
            springJoint.maxDistance = 0.2f;
            springJoint.tolerance = 0.06f;
            springJoint.enableCollision = true;
        }

        /// <summary> Enleve une boite a la tour </summary>
        public void RemoveBoxToTower()
        {
            if (m_boxCount == 1)
            {
                Debug.LogWarning("�On peut pas enlever toute les boites du panier");
                return;
            }

            m_boxCount--;
            Box1 boxToRemove = m_boxesInCart.Pop();
            Destroy(boxToRemove.gameObject);
            // ModifyTopBoxSpringIntesity(); // D�PLASSER ICI
        }

        public void RemoveBoxImpulse(Vector3 velocity)
        {
            Box1 topBox = GetTopBox();
            if (topBox == null)
            {
                Debug.LogWarning("No box to remove");
                return;
            }

            Debug.Log("Box to remove: " + topBox.name);
            topBox.GetComponent<Rigidbody>().isKinematic = false;
            topBox.GetComponent<Rigidbody>().AddForce(velocity, ForceMode.Impulse);
            topBox.MarkForDelete();
            //m_boxesInCart.Pop();
            //Destroy(topBox.gameObject);
        }

        private void ModifyTopBoxSpringIntesity()
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

        // TEST
        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.KeypadPlus))
            {
                AddBoxToTower();
                AddSpringToBox();
            }
            else if (Input.GetKeyDown(KeyCode.KeypadMinus))
            {
                RemoveBoxToTower();
                ModifyTopBoxSpringIntesity();
            }
            else if (Input.GetKeyDown(KeyCode.O))
            {
                Box1 currentTopBox = m_boxesInCart.ToArray()[0];
                currentTopBox.RemoveItemImpulse();
            }
            else if (Input.GetKeyDown(KeyCode.P))
            {
                RemoveBoxImpulse();
            }
            else if (Input.GetKeyDown(KeyCode.K))
            {
                Vector3 pos = Player.transform.localPosition + Cart.transform.localPosition;
                Quaternion rot = Player.transform.localRotation * Cart.transform.localRotation;
                Instantiate(DebugCartPrefab, pos + new Vector3(-3, 0, 0), rot * Quaternion.Euler(0, 90, 0));
            }
            else if (Input.GetKeyDown(KeyCode.L))
            {
                Vector3 pos = Player.transform.localPosition + Cart.transform.localPosition;
                Quaternion rot = Player.transform.localRotation * Cart.transform.localRotation;
                Instantiate(DebugCartPrefab, pos + new Vector3(3, 0, 0), rot * Quaternion.Euler(0, -90, 0));
            }
        }

        /// <summary> Regarde si la boite du dessus pourrait prendre un objet d'une certaine taille </summary>
        public bool CanTakeObjectInTheActualBox(ItemData.ESize size)
        {
            return GetTopBox().CanPutItemInsideBox(size);
        }

        /// <summary> Pour donner un item a la boite du dessus </summary>
        public void PutObjectInTopBox(GameObject item)
        {
            GetTopBox().PutItemInBox(item);
        }

        private Box1 GetTopBox()
        {
            return m_boxesInCart.Peek();
        }

        public void DropContent(Vector3 velocity)
        {
            if (m_boxesInCart.Count == 0) return;
            Debug.Log("Dropping content");
            Box1 box = GetTopBox();
            if (box.IsEmpty() && !box.IsLast())
                RemoveBoxImpulse(velocity);
            else if (!box.IsEmpty() && GetBoxesCount() < MAX_BOXES_BEFORE_DROP)
                box.RemoveItemImpulse(velocity);
        }

        internal int GetBoxesCount()
        {
            return m_boxCount;
        }

        public void RemoveBoxImpulse() // R�mi
        {
            // get the top item
            if (m_boxesInCart.Count <= 0)
            {
                Debug.LogWarning("No box on the stack");
                return;
            }

            Debug.Log("Item to remove: " + m_boxesInCart.ToArray()[0].name);

            Rigidbody boxRB = m_boxesInCart.ToArray()[0].GetComponent<Rigidbody>();
            SpringJoint springJoint = m_boxesInCart.ToArray()[0].GetComponent<SpringJoint>();
            if (springJoint != null)
                Destroy(springJoint);

            if (boxRB == null)
                boxRB = m_boxesInCart.ToArray()[0].AddComponent<Rigidbody>();

            boxRB.AddForce(Vector3.left + Vector3.up * 10, ForceMode.Impulse);
            m_boxesInCart.Pop();
        }
    }
}
