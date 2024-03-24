using DiscountDelirium;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

namespace BoxSystem
{

    public class Tower : MonoBehaviour
    {
        [field: SerializeField] public GameObject Cart { get; private set; }
        [SerializeField] private GameObject m_boxPrefab;       
        [SerializeField] private float m_boxHeight;
        private int m_boxCount = 0;
        private Stack<Box> m_boxesInCart = new Stack<Box>();

        void Start()
        {
            AddBoxToTower();
        }

        public void AddBoxToTower()
        {
            m_boxCount++;
            GameObject instant = Instantiate(m_boxPrefab);
            instant.transform.rotation = Cart.transform.rotation;
            instant.transform.SetParent(transform);
            instant.name = "Boxe " + m_boxCount;
            Box instantBox = instant.GetComponent<Box>();
            instantBox.SetTower(this);
            float height = (m_boxCount - 1) * m_boxHeight;
            instant.transform.position = new Vector3(transform.position.x, height + transform.position.y, transform.position.z);
            m_boxesInCart.Push(instantBox);
        }

        public void AddSpringToBox()
        {
            Debug.Log("Add spring to box");
            if (m_boxCount > 1)
            {
                Box previousBoxe = m_boxesInCart.ToArray()[m_boxesInCart.Count - 1];
                Debug.Log("previousBoxe: " + previousBoxe.name);
                Debug.Log("newBoxe: " + m_boxesInCart.ToArray()[0].transform.name);
                SpringJoint springJoint = m_boxesInCart.ToArray()[0].gameObject.AddComponent<SpringJoint>();
                Rigidbody newBoxeRB = previousBoxe.GetComponent<Rigidbody>();
                if (newBoxeRB == null)
                {
                    newBoxeRB = previousBoxe.AddComponent<Rigidbody>();
                }
                springJoint.connectedBody = newBoxeRB;
                springJoint.spring = 5;
                springJoint.damper = 0;
                springJoint.minDistance = 0;
                springJoint.maxDistance = 0.2f;
                springJoint.tolerance = 0.06f;
                springJoint.enableCollision = true;
            }

            if (m_boxCount > 2)
            {
                Debug.Log("m_boxCount > 2: " + m_boxesInCart.Count);
                Box previousBox = m_boxesInCart.ToArray()[0];
                Debug.Log("previousBox: " + previousBox.name);
                SpringJoint previousSpringJoint = previousBox.GetComponent<SpringJoint>();
                previousSpringJoint.spring = 5;
            }
        }

        public void RemoveBoxToTower()
        {
            if (m_boxCount == 1)
            {
                Debug.LogWarning("¨On peut pas enlever toute les boites du panier");
                return;
            }

            m_boxCount--;
            Box boxToRemove = m_boxesInCart.Pop();
            Destroy(boxToRemove.gameObject);

            if (m_boxCount > 2)
            {
                Debug.Log("m_boxCount > 2: " + (m_boxesInCart.Count - 1));
                Box currentTopBox = m_boxesInCart.ToArray()[0];
                Debug.Log("previousBox: " + currentTopBox.name);
                SpringJoint previousSpringJoint = currentTopBox.GetComponent<SpringJoint>();
                previousSpringJoint.spring = 5;
            }

            // Debug.Log("boxes in tower: " + m_boxesInCart.Count);
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

        private Box GetTopBox()
        {
            return m_boxesInCart.Peek();
        }
    }
}
