using DiscountDelirium;
using System.Collections.Generic;
using UnityEngine;

namespace BoxSystem
{

    public class Tower : MonoBehaviour
    {
        [SerializeField] private GameObject boxPrefab;
        [SerializeField] private float boxHeight;
        private int m_boxCount = 0;
        private Stack<Box> m_boxesInCart = new Stack<Box>();

        void Start()
        {
            AddBoxToTower();
            Box firstBox = m_boxesInCart.Peek();
            GetComponent<DebugControls>().RB = firstBox.GetComponent<Rigidbody>();
        }

        public void AddBoxToTower()
        {
            m_boxCount++;
            Vector3 newBoxPos = transform.position;
            Quaternion newBoxRot = Quaternion.identity;

            if (m_boxCount > 1)
            {
                newBoxPos = new Vector3(
                    m_boxesInCart.ToArray()[0].transform.position.x,
                    m_boxesInCart.ToArray()[0].transform.position.y,
                    m_boxesInCart.ToArray()[0].transform.position.z);
            }

            GameObject instant = Instantiate(boxPrefab, newBoxPos, newBoxRot);
            instant.transform.SetParent(transform);
            instant.name = "Boxe " + m_boxCount;
            Box instantBox = instant.GetComponent<Box>();
            instantBox.SetTower(this);
            float _x = transform.position.x;
            float _z = transform.position.z;
            float height = (m_boxCount - 1) * boxHeight;
            Debug.Log("m_boxCount: " + m_boxCount);
            if (m_boxCount > 1)
            {
                _x = m_boxesInCart.ToArray()[0].transform.position.x;
                _z = m_boxesInCart.ToArray()[0].transform.position.z;
                Box newBoxe = m_boxesInCart.ToArray()[m_boxesInCart.Count - 1];
                SpringJoint springJoint = instant.AddComponent<SpringJoint>();
                springJoint.connectedBody = newBoxe.GetComponent<Rigidbody>();
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
                previousSpringJoint.spring = 100;
            }

            instant.transform.position = new Vector3(_x, height, _z);
            m_boxesInCart.Push(instantBox);
        }

        public void RemoveBoxToTower()
        {
            if (m_boxCount == 1)
            {
                Debug.LogError("¨On peut pas enlever toute les boites du panier");
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

        public bool CanTakeObjectInTheActualBox(ItemData.ESize size)
        {
            return GetTopBox().CanPutItemInsideBox(size);
        }

        public void PutObjectInTopBox(GameObject item, ItemData.ESize size)
        {
            GetTopBox().PutItemInBox(item, size);
        }

        private Box GetTopBox()
        {
            return m_boxesInCart.Peek();
        }
    }
}
