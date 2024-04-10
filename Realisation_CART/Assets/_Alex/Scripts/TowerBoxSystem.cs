using System.Collections.Generic;
using UnityEngine;
using DiscountDelirium;
using Unity.VisualScripting;

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
        [field: Header("La hauteur de placement de la boite")]
        [field: SerializeField] public float BoxGapHeight { get; private set; } = 0.001f;
        [Header("La force d'expulsion des boites a la caisse")]
        [SerializeField] private float m_itemExpulsionForce;
        [SerializeField] private float m_boxExpulsionForce;

        private int m_boxCount = 0;
        private List<Box> m_boxesInCart = new List<Box>();

        [SerializeField] private TowerHingePhysicsAlex m_towerPhysics;
        [SerializeField] private int m_cartokenValueMultiplier = 1;


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
                RemoveBoxImpulse(m_boxExpulsionForce);
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
                    RemoveBoxImpulse(m_boxExpulsionForce);
                }

                if (GetTopBox() != null)
                {
                    RemoveItemImpulse(m_itemExpulsionForce);
                }
                else
                {
                    Debug.Log("Le cart vient de se vider, peut pas enlever d'objet");
                }

            }
            else if (Input.GetKeyDown(KeyCode.Y))
            {
                Vector3 data = EmptyCartAndGetScore();
                Debug.Log("totalScore: " + data.x + ",  nbOfItems: " + data.y + ", nbOfCartoken: " + data.z);
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
            float height = (m_boxCount - 1) * (instantBox.gameObject.GetComponent<BoxSetup>().GetBoxHeightDifference() + BoxGapHeight);
            Vector3 localPosition = new Vector3(0, height, 0);
            instant.transform.localPosition = localPosition;
            instantBox.SetInitialLocationInBox(localPosition);

            // ajout a la liste
            m_boxesInCart.Add(instantBox);

            m_towerPhysics.AddBoxToFakeTower();

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
        public Vector3 EmptyCartAndGetScore()
        {
            int totalScore = 0;
            int nbOfItems = 0;
            int nbOfCartokens = 0;

            while (m_boxesInCart.Count > 0)
            {
                Box topBox = GetTopBox();
                List<Box.ItemInBox> itemsInBox = topBox.GetItemsInBox();

                for (int i = 0; i < itemsInBox.Count; i++)
                {
                    nbOfItems++;                 
                    totalScore += itemsInBox[i].m_item.GetComponent<Item>().m_data.m_cost;
                    nbOfCartokens = nbOfItems * m_cartokenValueMultiplier;
                }
                RemoveBoxImpulse(m_boxExpulsionForce);
            }

            return new Vector3(nbOfItems, totalScore, nbOfCartokens);
        }

        /// <summary> Place toute les boites a leur origine pour placement des angrages des hinges </summary>
        public void ReplaceAllBoxToOrigin()
        {
            for (int i = 0; i < m_boxesInCart.Count; i++)
            {
                m_boxesInCart.ToArray()[i].ReplaceBoxToOrigin();
            }
        }

        private void RemoveBoxImpulse(float force)
        {
            Box topBox = GetTopBox();

            if (topBox == null)
            {
                Debug.LogWarning("No box to remove");
                return;
            }

            Vector3 totalImpulse = topBox.transform.up * force;

            Rigidbody rb = topBox.AddComponent<Rigidbody>();
            rb.AddForce(totalImpulse, ForceMode.Impulse);
            AutoDestruction destruct = topBox.GetComponent<AutoDestruction>();
            destruct.enabled = true;
            destruct.DestroyItem();

            RemoveLastBoxFromTower();
            m_towerPhysics.RemoveBoxFromFakeTower();

        }

        public void RemoveItemImpulse(float force)
        {
            Box topBox = GetTopBox();

            Box.ItemInBox lastItemInBox = topBox.GetLastItem();

            Vector3 totalImpulse = lastItemInBox.m_item.transform.up * force;

            Rigidbody rb = lastItemInBox.m_item.GetComponent<Rigidbody>();
            if (!rb)
            {
                rb = lastItemInBox.m_item.gameObject.AddComponent<Rigidbody>();
            }

            rb.AddForce(totalImpulse, ForceMode.Impulse);

            AutoDestruction destruct = lastItemInBox.m_item.GetComponent<AutoDestruction>();
            destruct.enabled = true;
            destruct.DestroyItem();

            topBox.ResetSlots(lastItemInBox);
            topBox.GetItemsInBox().Remove(lastItemInBox);

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
            int total = m_boxesInCart.Count;
            return m_boxesInCart[total - 1];
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
            int total = m_boxesInCart.Count;
            m_boxesInCart.RemoveAt(total - 1);
        }

        /// <summary> Donne la boite en dessous de la boite du dessus </summary>
        public Box GetPreviousTopBox()
        {
            if (m_boxesInCart.Count < 2)
                return null;

            return m_boxesInCart.ToArray()[1];
        }

        /// <summary> Donne le stack complet des boites </summary>
        public List<Box> GetAllBoxes()
        {
            return m_boxesInCart;
        }
        #endregion

    }

}
