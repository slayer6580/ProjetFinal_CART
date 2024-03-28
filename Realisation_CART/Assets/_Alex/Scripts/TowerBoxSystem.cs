using DiscountDelirium;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

namespace BoxSystem
{
    public class TowerBoxSystem : MonoBehaviour
    {
        [field: Header("Mettre le Player ici")]
        [field: SerializeField] public GameObject Player { get; private set; }

        [field: Header("La distance ou un item devrait snap a la boite pendant le slerp")]
        [field: SerializeField] public float ItemSnapDistance { get; private set; }

        [Header("Mettre le Prefab de la boite")]
        [SerializeField] private GameObject m_boxPrefab;
        [Header("La hauteur de placement de la boite")]
        [SerializeField] private float m_boxHeight;

        private int m_boxCount = 0;
        private Stack<Box> m_boxesInCart = new Stack<Box>();

        void Start()
        {
            AddBoxToTower();
        }

        /// <summary> Ajoute une boite a la tour </summary>
        public void AddBoxToTower()
        {
            m_boxCount++;

            // setup de la boite
            GameObject instant = Instantiate(m_boxPrefab);
            instant.transform.rotation = Player.transform.rotation;
            instant.transform.SetParent(transform);
            instant.name = "Boxe " + m_boxCount;
            Box instantBox = instant.GetComponent<Box>();
            instantBox.SetTower(this);

            // hauteur de la boite dans la tour
            float height = (m_boxCount - 1) * m_boxHeight;
            instant.transform.localPosition = new Vector3(0, height, 0);

            // ajout a la liste
            m_boxesInCart.Push(instantBox);

        }

        /// <summary> Enleve une boite a la tour </summary>
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
            else if (Input.GetKeyDown(KeyCode.O))
            {
                if (GetTopBox() == null)
                    return;

                if (GetTopBox().BoxIsEmpty())
                    RemoveBoxImpulse();

                if (GetTopBox() != null)
                    GetTopBox().RemoveItemImpulse();

            }
            else if (Input.GetKeyDown(KeyCode.P))
            {
                RemoveBoxImpulse();
            }
            else if (Input.GetKeyDown(KeyCode.Y))
            {
                int[] data = EmptyCartAndGetScore();
                Debug.Log(" , totalScore: " + data[0] + "nbOfItems: " + data[1]);
            }
        }

        /// <summary> Regarde si la boite du dessus pourrait prendre un objet d'une certaine taille </summary>
        public bool CanTakeObjectInTheActualBox(ItemData.ESize size)
        {
            if (GetTopBox() == null)
                return false;

            return GetTopBox().CanPutItemInsideBox(size);
        }

        /// <summary> Pour donner un item a la boite du dessus </summary>
        public void PutObjectInTopBox(GameObject item)
        {
            GetTopBox().PutItemInBox(item);
        }

        /// <summary> Donne la boite du dessus </summary>
        private Box GetTopBox()
        {
            if (m_boxesInCart.Count == 0)
                return null;

            return m_boxesInCart.Peek();
        }

        /// <summary> Retire une boite avec force </summary>
        public void RemoveBoxImpulse()
        {
            // get the top item
            if (m_boxesInCart.Count <= 0)
            {
                Debug.LogWarning("No box on the stack");
                return;
            }

            Debug.Log("Item to remove: " + m_boxesInCart.ToArray()[0].name);

            Box topBox = GetTopBox();

            Rigidbody boxRB = topBox.GetComponent<Rigidbody>();
            if (boxRB == null)
                boxRB = topBox.AddComponent<Rigidbody>();

            boxRB.AddForce(Vector3.left + Vector3.up * 10, ForceMode.Impulse);
            topBox.gameObject.GetComponent<AutoDestruction>().enabled = true;

            m_boxCount--;
            m_boxesInCart.Pop();
        }

        /// <summary> Vide le panier et rend le nombre d'items de la tour et le score total </summary>
        public int[] EmptyCartAndGetScore()
        {
            int totalScore = 0;
            int nbOfItems = 0;
            int[] data = { totalScore, nbOfItems };

            while (m_boxesInCart.Count > 0)
            {
                Box topBox = GetTopBox();
                List<Box.ItemInBox> itemsInBox = topBox.GetItemsList();

                for (int i = 0; i < itemsInBox.Count; i++)
                {
                    data[1]++;
                    data[0] += itemsInBox[i].m_item.GetComponent<Item>().m_data.m_cost;
                }
                RemoveBoxImpulse();
            }

            return data;
        }
    }

}
