using CartControl;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

namespace BoxSystem
{

    public class DebugTower : MonoBehaviour
    {
        [field: SerializeField] public GameObject Cart { get; private set; }
        [field: SerializeField] private GameObject DebugCartPrefab { get; set; } = null;
        [field: SerializeField] private GameObject Character { get; set; } = null;
        [SerializeField] private GameObject m_boxPrefab;       
        [SerializeField] private float m_boxHeight;
        private int m_boxCount = 0;
        private Stack<Box1> m_boxesInCart = new Stack<Box1>();

        void Start()
        {
            AddBoxToTower();
            AddSpringToBox();
        }

        public void AddBoxToTower()
        {
            //Debug.Log("AddBoxToTower()");
            m_boxCount++;
            float height = (m_boxCount - 1) * m_boxHeight;
            Vector3 desiredPos = new Vector3(transform.position.x, height + transform.position.y, transform.position.z);

            GameObject instant = Instantiate(m_boxPrefab, desiredPos, Cart.transform.rotation);
            instant.transform.rotation = Cart.transform.rotation;
            instant.transform.SetParent(transform);
            instant.name = "Box " + m_boxCount;
            Box1 instantBox = instant.GetComponent<Box1>();
            //instantBox.SetTower(this);
            instant.transform.position = desiredPos;
            m_boxesInCart.Push(instantBox);
        }

        public void AddSpringToBox()
        {
            if (m_boxCount == 1) // Pour ajouter un spring entre la première boite et le panier
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
                Box1 previousBoxe = m_boxesInCart.ToArray()[m_boxesInCart.Count - 1];
                SpringJoint springJoint = m_boxesInCart.ToArray()[0].gameObject.AddComponent<SpringJoint>();
                Rigidbody newBoxeRB = previousBoxe.GetComponent<Rigidbody>();
                if (newBoxeRB == null)
                {
                    newBoxeRB = previousBoxe.AddComponent<Rigidbody>();
                }
                SetSprintJointValues(springJoint, newBoxeRB);
            }

            if (m_boxCount > 2) // Pour changer la force du spring du top box
            {
                Box1 previousBox = m_boxesInCart.ToArray()[0];
                SpringJoint previousSpringJoint = previousBox.GetComponent<SpringJoint>();
                previousSpringJoint.spring = 10;
            }
        }

        private static void SetSprintJointValues(SpringJoint springJoint, Rigidbody newBoxeRB)
        {
            springJoint.connectedBody = newBoxeRB;
            springJoint.spring = 1;
            springJoint.damper = 0;
            springJoint.minDistance = 0;
            springJoint.maxDistance = 0.2f;
            springJoint.tolerance = 0.06f;
            springJoint.enableCollision = true;
        }

        public void RemoveBoxToTower()
        {
            if (m_boxCount == 1)
            {
                Debug.LogWarning("¨On peut pas enlever toute les boites du panier");
                return;
            }

            m_boxCount--;
            Box1 boxToRemove = m_boxesInCart.Pop();
            Destroy(boxToRemove.gameObject);
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
            topBox.GetComponent<Rigidbody>().AddForce(velocity, ForceMode.Impulse);
            m_boxesInCart.Pop();
            Destroy(topBox.gameObject);
        }

        private void ModifyTopBoxSpringIntesity()
        {
            if (m_boxCount > 2)
            {
                Debug.Log("m_boxCount > 2: " + (m_boxesInCart.Count - 1));
                Box1 currentTopBox = m_boxesInCart.ToArray()[0];
                Debug.Log("previousBox: " + currentTopBox.name);
                SpringJoint previousSpringJoint = currentTopBox.GetComponent<SpringJoint>();
                previousSpringJoint.spring = 5;
            }

        }

        // TEST
        private void Update()
        {
            //if (Input.GetKeyDown(KeyCode.KeypadPlus))
            //{
            //    AddBoxToTower();
            //    AddSpringToBox();
            //}
            //else if (Input.GetKeyDown(KeyCode.KeypadMinus))
            //{
            //    RemoveBoxToTower();
            //    ModifyTopBoxSpringIntesity();
            //}
            //else if (Input.GetKeyDown(KeyCode.K))
            //{
            //    Vector3 pos = Character.transform.localPosition + Cart.transform.localPosition;
            //    Quaternion rot = Character.transform.localRotation * Cart.transform.localRotation;
            //    GameObject instant = Instantiate(DebugCartPrefab, pos + new Vector3(-3, 0, 0), rot * Quaternion.Euler(0, 90, 0));
            //}
            //else if (Input.GetKeyDown(KeyCode.L))
            //{
            //    Vector3 pos = Character.transform.localPosition + Cart.transform.localPosition;
            //    Quaternion rot = Character.transform.localRotation * Cart.transform.localRotation;
            //    GameObject instant = Instantiate(DebugCartPrefab, pos + new Vector3(3, 0, 0), rot * Quaternion.Euler(0, -90, 0));
            //}
        }

        public bool CanTakeObjectInTheActualBox(ItemData.ESize size)
        {
            return GetTopBox().CanPutItemInsideBox(size);
        }

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
            if (box.IsEmpty()) RemoveBoxImpulse(velocity);
            else box.RemoveItemImpulse(velocity);
        }
    }
}
