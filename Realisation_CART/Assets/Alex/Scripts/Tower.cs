using System.Collections.Generic;
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
            instant.transform.position = new Vector3(transform.position.x, height, transform.position.z);
            m_boxesInCart.Push(instantBox);
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
        }

        // TEST
        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.KeypadPlus))
            {
                AddBoxToTower();
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
