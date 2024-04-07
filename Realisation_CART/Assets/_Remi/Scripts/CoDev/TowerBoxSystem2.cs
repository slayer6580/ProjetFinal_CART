using System.Collections.Generic;
using UnityEngine;
using DiscountDelirium;

namespace BoxSystem
{
    public class TowerBoxSystem2 : MonoBehaviour
    {

        [field: Header("Mettre le Player ici")]
        [field: SerializeField] public GameObject Player { get; private set; }
        private TowerPhysics _TowerPhysics { get; set; }

        [field: Header("La distance ou un item devrait snap a la boite pendant le slerp")]
        [field: SerializeField] public float ItemSnapDistance { get; private set; }

        [Header("La hauteur de placement de la boite")]
        [SerializeField] private float m_boxGapHeight;
        [Header("La force d'expulsion des boites a la caisse")]
        [SerializeField] private float m_itemExpulsionForce;
        [SerializeField] private float m_boxExpulsionForce;

        private int m_boxCount = 0;
        private List<Box2> m_boxesInCart = new List<Box2>();


        void Start()
        {
            _TowerPhysics = GetComponent<TowerPhysics>();
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
                RemoveBoxImpulse(Vector3.up * m_boxExpulsionForce);
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
                    RemoveBoxImpulse(Vector3.up * m_boxExpulsionForce);
                }

                if (GetTopBox() != null)
                {
                    RemoveItemImpulse(Vector3.up * m_itemExpulsionForce);
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
            GameObject instant = Instantiate(_TowerPhysics.GetBoxPrefabFromDropableOrder());
            instant.transform.rotation = Player.transform.rotation;
            instant.transform.SetParent(transform);
            instant.name = "Boxe " + m_boxCount;
            Box2 instantBox = instant.GetComponent<Box2>();
            instantBox.SetTower(this);


            // hauteur de la boite dans la tour
            float height = (m_boxCount - 1) * (instantBox.gameObject.GetComponent<BoxSetup2>().SlotHeight + m_boxGapHeight);
            Vector3 localPosition = new Vector3(0, height, 0);
            instant.transform.localPosition = localPosition;
            instantBox.SetInitialLocationInBox(localPosition);

            // ajout a la liste
            m_boxesInCart.Add(instantBox);

            _TowerPhysics.ModifyJoint();
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
                Box2 topBox = GetTopBox();
                List<Box2.ItemInBox> itemsInBox = topBox.GetItemsInBox();

                for (int i = 0; i < itemsInBox.Count; i++)
                {
                    data[1]++;
                    data[0] += itemsInBox[i].m_item.GetComponent<Item>().m_data.m_cost;
                }
                RemoveBoxImpulse(Vector3.up * m_boxExpulsionForce);
            }

            return data;
        }


        /// <summary> Place toute les boites a leur origine pour placement des angrages des hinges </summary>
        public void ReplaceAllBoxToOrigin()
        {
            for (int i = 0; i < m_boxesInCart.Count; i++)
            {
                m_boxesInCart.ToArray()[i].ReplaceBoxToOrigin();
            }
        }

        private void RemoveBoxImpulse(Vector3 velocity)
        {
            Box2 topBox = GetTopBox();

            if (topBox == null)
            {
                Debug.LogWarning("No box to remove");
                return;
            }

            Vector3 totalImpulse = velocity;

            Rigidbody rb = topBox.GetComponent<Rigidbody>();
            rb.isKinematic = false;
            rb.AddForce(totalImpulse, ForceMode.Impulse);
            AutoDestruction destruct = topBox.GetComponent<AutoDestruction>();
            destruct.enabled = true;
            destruct.DestroyItem();

            RemoveLastBoxFromTower();

        }

        public void RemoveItemImpulse(Vector3 velocity)
        {
            Box2 topBox = GetTopBox();

            Box2.ItemInBox lastItemInBox = topBox.GetLastItem();

            Vector3 totalImpulse = velocity;

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

        /// 
        public Box2 GetFirstBox()
        {
            return m_boxesInCart[0];
        }

        /// <summary> Donne la boite du dessus </summary>
        public Box2 GetTopBox()
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
        public Box2 GetPreviousTopBox()
        {
            if (m_boxesInCart.Count < 2)
                return null;

            Debug.Log("m_boxesInCart.Count: " + m_boxesInCart.Count);
            return m_boxesInCart[m_boxesInCart.Count - 2];
        }

        /// <summary> Donne le stack complet des boites </summary>
        public List<Box2> GetAllBoxes()
        {
            return m_boxesInCart;
        }
        #endregion

        public void EnabledColliderOnBoxes(bool value)
        {
            foreach (Box2 box in m_boxesInCart)
            {
                box.EnabledCollider(value);
            }
        }
    }

}
