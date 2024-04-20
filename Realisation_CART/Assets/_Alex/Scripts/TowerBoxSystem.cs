using System.Collections.Generic;
using UnityEngine;
using DiscountDelirium;
using Unity.VisualScripting;
using static Manager.ScoreManager;

namespace BoxSystem
{
    public class TowerBoxSystem : MonoBehaviour
    {

        [field: Header("Put main player GO here")]
        [field: SerializeField] public GameObject Player { get; private set; }

        [field: Header("The distance of an item max before snapping inside a box")]
        [field: SerializeField] public float ItemSnapDistance { get; private set; }

        [Header("Put box prefab here")]
        [SerializeField] private GameObject m_boxPrefab;

        [field: Header("height gap box placement")]
        [field: SerializeField] public float BoxGapHeight { get; private set; } = 0.001f;

        [Header("Expulsion of items and boxes")]
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
                RemoveBoxImpulse();
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
                    RemoveBoxImpulse();
                }

                if (GetTopBox() != null)
                {
                    RemoveItemImpulse();
                }
                else
                {
                    Debug.Log("Le cart vient de se vider, peut pas enlever d'objet");
                }

            }
            else if (Input.GetKeyDown(KeyCode.Y))
            {
                Vector3 data = _ScoreManager.EmptyCartAndGetScore();
                Debug.Log("totalScore: " + data.x + ",  nbOfItems: " + data.y + ", nbOfCartoken: " + data.z);
            }
        }

        /// <summary> Add a box to the tower </summary>
        public void AddBoxToTower()
        {
            m_boxCount++;

            // box setup
            GameObject instant = Instantiate(m_boxPrefab);
            instant.transform.rotation = Player.transform.rotation;
            instant.transform.SetParent(transform);
            instant.name = "Boxe " + m_boxCount;
            Box instantBox = instant.GetComponent<Box>();
            instantBox.SetTower(this);

            // height of box
            float height = (m_boxCount - 1) * (BoxManager.GetInstance().GetBoxHeightDifference() + BoxGapHeight);
            Vector3 localPosition = new Vector3(0, height, 0);
            instant.transform.localPosition = localPosition;
            instantBox.SetBoxInitialLocalPosition(localPosition);

            // add in lists
            m_boxesInCart.Add(instantBox);
            m_towerPhysics.AddBoxToPhysicsTower();

            Debug.Log("box added: " + instantBox.name);

        }

        /// <summary> Look if top box can take item </summary>
        public bool CanTakeObjectInTheActualBox(ItemData.ESize size)
        {
            if (GetTopBox() == null)
                return false;

            Debug.Log("Can take item: " + GetTopBox().CanPutItemInsideBox(size).ToString());
            return GetTopBox().CanPutItemInsideBox(size);
        }

        /// <summary> Give item to top box </summary>
        public void PutObjectInTopBox(GameObject item)
        {
            GetTopBox().PutItemInBox(item);
        }

        /// <summary> Remove the top box with an impulse force </summary>
        public void RemoveBoxImpulse()
        {
            Box topBox = GetTopBox();

            if (topBox == null)
            {
                Debug.LogWarning("No box to remove");
                return;
            }

            topBox.transform.SetParent(null);

            Vector3 totalImpulse = topBox.transform.up * m_boxExpulsionForce;
            Rigidbody rb = topBox.AddComponent<Rigidbody>();
            rb.AddForce(totalImpulse, ForceMode.Impulse);
            AutoDestruction destruct = topBox.GetComponent<AutoDestruction>();
            destruct.enabled = true;
            destruct.DestroyItem();

            RemoveLastBoxFromTower();
            m_towerPhysics.RemoveBoxFromPhysicsTower();

        }

        /// <summary> Remove the last item inside the top box with an impulse force </summary>
        public void RemoveItemImpulse()
        {
        
            if (GetTopBox().IsEmpty())
            {
                RemoveBoxImpulse();
                return;
            }

            Box topBox = GetTopBox();

            Box.ItemInBox lastItemInBox = topBox.GetLastItem();

            lastItemInBox.m_item.transform.SetParent(null);

            Vector3 totalImpulse = lastItemInBox.m_item.transform.up * m_itemExpulsionForce;
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

        /// <summary> Give number of boxes in cart </summary>
        public int GetBoxCount()
        {
            return m_boxCount;
        }

        /// <summary> Get the top box of the tower </summary>
        public Box GetTopBox()
        {
            if (m_boxesInCart.Count == 0)
                return null;
            int total = m_boxesInCart.Count;
            return m_boxesInCart[total - 1];
        }

        /// <summary> Remove the box from the tower </summary>
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

        /// <summary> Give box under top box </summary>
        public Box GetPreviousTopBox()
        {
            if (m_boxesInCart.Count < 2)
                return null;

            return m_boxesInCart.ToArray()[1];
        }

        /// <summary> Give list of all the boxes </summary>
        public List<Box> GetAllBoxes()
        {
            return m_boxesInCart;
        }
        #endregion

    }

}
