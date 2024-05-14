using System.Collections.Generic;
using UnityEngine;
using DiscountDelirium;
using Unity.VisualScripting;
using static Manager.ScoreManager;
using UnityEngine.SceneManagement;
using CartControl;

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

        [Header("Customers Contact variables")]
        [SerializeField] private float m_contactExpulsionForce;
        [SerializeField] private float m_scaleMultiplier;

        [Header("Destruction Time")]
        [SerializeField] private float m_itemDestructionTime;
        [SerializeField] private float m_boxDestructionTime;

        [Header("boxes movement forward and backward")]
        [SerializeField] private float m_maxGapDifference;
        [SerializeField] private float m_forceMultiplier;
        [SerializeField] private float m_minForceToMove;
        [SerializeField] private float m_collisionMultiplier;
        [SerializeField] private float m_collisionAnimationTime;
        private float m_currentAnimationTime = 0;
        private float m_collisionForce = 0;
        private Rigidbody m_playerRb;
        private bool m_collisionAnimation = false;


        [Header("Cart token")]
        [SerializeField] private int m_cartokenValueMultiplier = 1;

        private int m_boxCount = 0;
        private List<Box> m_boxesInCart = new List<Box>();

        private TowerHingePhysicsAlex m_towerPhysics;


        private void Awake()
        {
            InitializeTowerPhysicsVariables();
            m_playerRb = Player.GetComponent<Rigidbody>();
        }

        void Start()
        {
            AddBoxToTower();
        }

        private void Update()
        {
           // KeyboardDebug();
            BoxMovement();
            CollisionAnimation();
        }



        private void KeyboardDebug()
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
            }
        }

        private void InitializeTowerPhysicsVariables()
        {
            Scene scene = gameObject.scene;
            GameObject[] gameObjects = scene.GetRootGameObjects();

            foreach (GameObject gameObject in gameObjects)
            {
                if (gameObject.name != "TowerPhysics") continue;

                GameObject TowerPhysicsGO = gameObject.GetComponent<TowerPhysicsGenerator>().GenerateTowerPhysics();
                m_towerPhysics = TowerPhysicsGO.GetComponent<TowerHingePhysicsAlex>();
                m_towerPhysics.TowerBoxSystem = this;

                CartStateMachine cartStateMachine = GetComponentInParent<CartStateMachine>();
                cartStateMachine.BoxForce = TowerPhysicsGO.GetComponent<AddForceToBox>();
            }

            if (m_towerPhysics == null) Debug.LogError("TowerPhysics not found");
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

        }


        public void AddItemToTower(GameObject item)
        {

            Rigidbody rb = item.GetComponent<Rigidbody>();
            if (rb)
            {
                Destroy(rb);
            }

            Item itemScript = item.GetComponent<Item>();
            ItemData.ESize size = itemScript.m_data.m_size;

            if (!CanTakeObjectInTheActualBox(size))
            {
                AddBoxToTower();
            }

            PutObjectInTopBox(item);
            //Debug.LogError("item to put in box: " + item.name);

        }

        /// <summary> Look if top box can take item </summary>
        public bool CanTakeObjectInTheActualBox(ItemData.ESize size)
        {
            if (GetTopBox() == null)
                return false;

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
            Destroy(topBox.gameObject, m_boxDestructionTime);

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

            BoxCollider modelCollider = lastItemInBox.m_item.transform.GetChild(0).GetComponent<BoxCollider>();

            if (modelCollider != false)
            {
                modelCollider.enabled = true;
            }

            rb.AddForce(totalImpulse, ForceMode.Impulse);

            Destroy(lastItemInBox.m_item.gameObject, m_itemDestructionTime);

            topBox.ResetSlots(lastItemInBox);
            topBox.GetItemsInBox().Remove(lastItemInBox);
        }

        private void ItemStolenAnimation(GameObject item)
        {
            Vector3 totalImpulse = Vector3.up * m_contactExpulsionForce;
            Rigidbody rb = item.GetComponent<Rigidbody>();
            if (!rb)
            {
                rb = item.gameObject.AddComponent<Rigidbody>();
            }

            BoxCollider modelCollider = item.transform.GetChild(0).GetComponent<BoxCollider>();

            if (modelCollider != false)
            {
                modelCollider.enabled = true;
            }

            rb.AddForce(totalImpulse, ForceMode.Impulse);
            item.transform.localScale *= m_scaleMultiplier;
        }

        public GameObject GetStolenItem()
        {
            if (GetTopBox().IsEmpty())
            {
                RemoveBoxImpulse();
                return null;
            }

            Box topBox = GetTopBox();

            Box.ItemInBox lastItemInBox = topBox.GetLastItem();

            lastItemInBox.m_item.transform.SetParent(null);
            ItemStolenAnimation(lastItemInBox.m_item); // 88888888888888888888888888888888888
            topBox.ResetSlots(lastItemInBox);
            topBox.GetItemsInBox().Remove(lastItemInBox);

            return lastItemInBox.m_item;
        }

        public void ActivateCollisionAnimation(float force)
        {
            if (m_collisionAnimation)
                return;

            m_collisionForce = force * m_collisionMultiplier;
            m_currentAnimationTime = 0;
            m_collisionAnimation = true;
        }

        public void CollisionAnimation()
        {
            if (!m_collisionAnimation)
                return;

            int nbOfFixedBoxes = m_towerPhysics.GetNbOfFixedBoxes();
            int boxesToMove = m_boxCount - nbOfFixedBoxes;


            if (boxesToMove == 0)
            {
                return;
            }

            int lastIndex = m_boxesInCart.Count - 1;

            if ((Mathf.Abs(m_collisionForce) > m_maxGapDifference))
            {
                if (m_collisionForce > 0)
                {
                    m_collisionForce = m_maxGapDifference;
                }
                else
                {
                    m_collisionForce = -m_maxGapDifference;
                }
            }
            float HalfAnimationTime = m_collisionAnimationTime / 2;

            m_currentAnimationTime += Time.deltaTime;
            bool comeBack = m_currentAnimationTime > HalfAnimationTime;


            float lerpingTime;
            if (comeBack)
                lerpingTime = 1 - ((m_currentAnimationTime - HalfAnimationTime) / HalfAnimationTime);
            else
                lerpingTime = m_currentAnimationTime / HalfAnimationTime;


            for (int i = lastIndex; i > (nbOfFixedBoxes - 1); i--)
            {
                Transform boxGraphics = m_boxesInCart[i].transform.GetChild(1);


                boxGraphics.localPosition = Vector3.Lerp(Vector3.zero, new Vector3(0, 0, m_collisionForce * boxesToMove), lerpingTime);


                boxesToMove--;
            }         

            if (m_currentAnimationTime > m_collisionAnimationTime)
            {
                m_collisionAnimation = false;
            }


        }

        public void BoxMovement()
        {

            if (m_collisionAnimation)
                return;

            int nbOfFixedBoxes = m_towerPhysics.GetNbOfFixedBoxes();
            int boxesToMove = m_boxCount - nbOfFixedBoxes;
            float force = Vector3.Dot(-Player.transform.forward, m_playerRb.velocity.normalized) * m_playerRb.velocity.magnitude;

            if (boxesToMove == 0)
            {
                return;
            }

            if (Mathf.Abs(force) < m_minForceToMove)
            {
                return;
            }

            int lastIndex = m_boxesInCart.Count - 1;
            float distanceZ = force * m_forceMultiplier;

            if ((Mathf.Abs(distanceZ) > m_maxGapDifference))
            {
                if (distanceZ > 0)
                {
                    distanceZ = m_maxGapDifference;
                }
                else
                {
                    distanceZ = -m_maxGapDifference;
                }
            }

            for (int i = lastIndex; i > (nbOfFixedBoxes - 1); i--)
            {
                Transform boxGraphics = m_boxesInCart[i].transform.GetChild(1);
                boxGraphics.localPosition = new Vector3(0,
                                                        0,
                                                        distanceZ * boxesToMove);
                boxesToMove--;
            }


        }

        public void SetBoxesTransformInCart(int index, Vector3 localPosition, Vector3 localEulerAngles)
        {
            m_boxesInCart[index].transform.localPosition = localPosition;
            m_boxesInCart[index].transform.localEulerAngles = localEulerAngles;
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
