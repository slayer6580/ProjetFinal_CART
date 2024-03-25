using DiscountDelirium;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using static BoxSystem.Box;

namespace BoxSystem
{

    public class Tower1 : MonoBehaviour
    {
        [field: SerializeField] public GameObject Cart { get; private set; }
        [SerializeField] private GameObject m_boxPrefab;       
        [SerializeField] private float m_boxHeight;
        private int m_boxCount = 0;
        private Stack<Box1> m_boxesInCart = new Stack<Box1>();

        void Start()
        {
            AddBoxToTower();
            AddSpringToBox();
        }

        //public void AddBoxToTower()
        //{
        //    Debug.Log("AddBoxToTower()");
        //    m_boxCount++;
        //    float height = (m_boxCount - 1) * m_boxHeight;
        //    GameObject instant = Instantiate(m_boxPrefab, transform.position, Cart.transform.rotation);
        //    instant.transform.rotation = Cart.transform.rotation;
        //    instant.transform.SetParent(transform);
        //    instant.name = "Box " + m_boxCount;
        //    Box1 instantBox = instant.GetComponent<Box1>();
        //    instantBox.SetTower(this);
        //    instant.transform.position = new Vector3(transform.position.x, height + transform.position.y, transform.position.z);
        //    m_boxesInCart.Push(instantBox);
        //}

        public void AddBoxToTower()
        {
            Debug.Log("AddBoxToTower()");
            m_boxCount++;
            float height = (m_boxCount - 1) * m_boxHeight;
            Debug.Log("(m_boxCount - 1)" + (m_boxCount - 1));
            Debug.Log("m_boxHeight: " + m_boxHeight);
            Debug.Log("height: " + height);
            Vector3 desiredPos = new Vector3(transform.position.x, height + transform.position.y, transform.position.z);
            Debug.Log("desiredPos: " + desiredPos);
            GameObject instant = Instantiate(m_boxPrefab, desiredPos, Cart.transform.rotation);
            instant.transform.rotation = Cart.transform.rotation;
            instant.transform.SetParent(transform);
            instant.name = "Boxe " + m_boxCount;
            Box1 instantBox = instant.GetComponent<Box1>();
            instantBox.SetTower(this);
            instant.transform.position = desiredPos;
            m_boxesInCart.Push(instantBox);
        }

        public void AddSpringToBox()
        {
            if (m_boxCount == 1) // Pour ajouter un spring entre la première boite et le panier
            {
                //Debug.Log("Add spring to box : m_boxCount == 0");
                //Rigidbody cartRB = GetComponentInParent<Rigidbody>();
                //if (cartRB == null) Debug.LogError("Cart n'a pas de rigidbody");
                //SpringJoint springJoint = m_boxesInCart.ToArray()[0].gameObject.AddComponent<SpringJoint>();
                //SetSprintJointValues(springJoint, cartRB);

                Rigidbody boxRB = m_boxesInCart.ToArray()[0].GetComponent<Rigidbody>();
                if (boxRB == null) Debug.LogWarning("Box n'a pas de rigidbody");
                boxRB.isKinematic = true;
                return;
            }

            if (m_boxCount > 1) // Pour ajouter un spring entre les boites
            {
                Box1 previousBoxe = m_boxesInCart.ToArray()[m_boxesInCart.Count - 1];
                //Debug.Log("previousBoxe: " + previousBoxe.name);
                //Debug.Log("newBoxe: " + m_boxesInCart.ToArray()[0].transform.name);
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
                Debug.Log("m_boxCount > 2: " + m_boxesInCart.Count);
                Box1 previousBox = m_boxesInCart.ToArray()[0];
                Debug.Log("previousBox: " + previousBox.name);
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

        public void RemoveBoxImpulse()
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
