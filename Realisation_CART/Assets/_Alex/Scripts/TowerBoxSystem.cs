using System.Collections.Generic;
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
        [SerializeField] private float m_boxGapHeight;
        [Header("La force d'expulsion des boites a la caisse")]
        [SerializeField] private float m_expulsionForce;

        private int m_boxCount = 0;
        private Stack<Box> m_boxesInCart = new Stack<Box>();
        private TowerPhysics m_towerPhysics;

        private void Awake()
        {
            m_towerPhysics = GetComponent<TowerPhysics>();
        }

        void Start()
        {
            AddBoxToTower();
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.KeypadPlus))
            {
                AddBoxToTower();
            }
            else if (Input.GetKeyDown(KeyCode.KeypadMinus))
            {
                m_towerPhysics.RemoveBoxImpulse(Vector3.up * m_expulsionForce, true); // appel par TowerPhysics
            }
            else if (Input.GetKeyDown(KeyCode.O)) // Remove item
            {
                if (GetTopBox() == null)
                {
                    Debug.Log("Il n'y a pas de boite dans le cart");
                    return;
                }
                if (GetTopBox().IsEmpty())
                {
                    Debug.Log("La boite est vide, on enleve la boite");
                     m_towerPhysics.RemoveBoxImpulse(Vector3.up * m_expulsionForce, true); // appel par TowerPhysics
                }

                if (GetTopBox() != null)
                {
                    m_towerPhysics.RemoveItemImpulse(Vector3.up * 10);
                }
                else
                {
                    Debug.Log("Le cart vient de se vider, peut pas enlever d'objet");
                }

            }
            else if (Input.GetKeyDown(KeyCode.Y))
            {
                int[] data = EmptyCartAndGetScore();
                Debug.Log("totalScore: " + data[0] + ",  nbOfItems: " + data[1]);
            }
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
            float height = (m_boxCount - 1) * (instantBox.gameObject.GetComponent<BoxSetup>().SlotHeight + m_boxGapHeight);
            Vector3 localPosition = new Vector3(0, height, 0);
            instant.transform.localPosition = localPosition;
            instantBox.SetInitialLocationInBox(localPosition);

            // ajout a la liste
            m_boxesInCart.Push(instantBox);

            m_towerPhysics.AddJointToBox();

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

        /// <summary> Vide le panier et rend le nombre d'items de la tour et le score total </summary>
        public int[] EmptyCartAndGetScore()
        {
            int totalScore = 0;
            int nbOfItems = 0;
            int[] data = { totalScore, nbOfItems };

            while (m_boxesInCart.Count > 0)
            {
                Box topBox = GetTopBox();
                List<Box.ItemInBox> itemsInBox = topBox.GetItemsInBox();

                for (int i = 0; i < itemsInBox.Count; i++)
                {
                    data[1]++;
                    data[0] += itemsInBox[i].m_item.GetComponent<Item>().m_data.m_cost;
                }
                 m_towerPhysics.RemoveBoxImpulse(Vector3.up * m_expulsionForce, true); // appel par TowerPhysics
            }

            return data;
        }

        #region (--- Getter ---)
        /// <summary> Donne le nombre de boites dans le panier </summary>
        public int GetBoxCount()
        {
            return m_boxCount;
        }

        /// <summary> Donne la boite du dessus </summary>
        public Box GetTopBox()
        {
            if (m_boxesInCart.Count == 0)
                return null;

            return m_boxesInCart.Peek();
        }

        /// <summary> Reduit le nombre de boites dans le panier </summary>
        public void RemoveLastBoxFromTower()
        {
            if (m_boxesInCart.Count == 0)
            {
                Debug.Log("No box to remove");
                return;
            }

            m_boxCount--;
            m_boxesInCart.Pop();       
        }

        /// <summary> Donne la boite en dessous de la boite du dessus </summary>
        public Box GetPreviousTopBox()
        {
            if (m_boxesInCart.Count < 2)
                return null;

            return m_boxesInCart.ToArray()[1];
        }

        public Box GetBoxUnderneath(Box upperBox)
        {
            if (m_boxesInCart.Count < 2)
                return null;

            return m_boxesInCart.ToArray()[GetBoxIndex(upperBox)-1];
        }

        private int GetBoxIndex(Box box)
        {
            Box[] boxes = m_boxesInCart.ToArray();
            for (int i = 0; i < boxes.Length; i++)
            {
                if (boxes[i] == box)
                    return i;
            }

            return -1;
        }
        #endregion
    }

}
