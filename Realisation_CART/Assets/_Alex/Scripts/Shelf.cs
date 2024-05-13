using UnityEngine;

namespace BoxSystem
{
    public class Shelf : MonoBehaviour
    {

        [field: Header("Put the item scriptable object here")]
        [field: SerializeField] public ItemData ItemData { get; private set; }

        [Header("item max quantity to take per trip")]
        [SerializeField] private int m_itemQuantity;

        [Header("respawn shelf parameters")]
        [SerializeField] private float m_timeBeforeRespawnForOneItem;
        [SerializeField] private bool m_activateItemRespawn = true;

        public GameObject ItemPrefab { get; private set; }
        private MeshRenderer m_renderer;
        private ShelfSetup m_shelfSetup;

        private int m_remainingItems;
        private Color m_initialColor;
        private float m_currentTimer = 0;
        private bool m_itemIsRespawning = false;

        private void Awake()
        {
            ItemPrefab = Resources.Load<GameObject>("Prefabs/Item");

            m_remainingItems = m_itemQuantity;
            m_renderer = GetComponent<MeshRenderer>();
            m_shelfSetup = GetComponent<ShelfSetup>();
        }

        private void Start()
        {
            m_initialColor = m_renderer.material.color;
        }


        private void Update()
        {
            ItemAutoRespawn();
        }

        private void ItemAutoRespawn()
        {
            if (!m_itemIsRespawning || !m_activateItemRespawn)
                return;

            m_currentTimer += Time.deltaTime;

            if (m_currentTimer > m_timeBeforeRespawnForOneItem * m_itemQuantity)
            {
                ResetShelf();
                ResetTimer();
            }

        }

        /// <summary> Check if the item limit is not broken on this shelf </summary>
        public bool CanTakeItem()
        {
            return m_remainingItems > 0;
        }

        /// <summary> Take an item from shelf </summary>
        public GameObject GetItemFromShelf()
        {
            m_remainingItems--;                                                              // remove an available item count.
                                                                                             
            GameObject objectToSpawn = ItemPrefab;                                           // take the item prefab
            Item itemScript = objectToSpawn.GetComponent<Item>();                            // access his 'Item' script
            itemScript.m_data = ItemData;                                                    // Determine what kind of item it is           
            objectToSpawn.transform.position = m_shelfSetup.GetRandomSpawnPoints();          // Set start position to the shelf
            GameObject instant = Instantiate(objectToSpawn);                                 // Spawn Item Prefab
            GameObject model = Instantiate(itemScript.m_data.m_object);                      // Spawn Model
            model.transform.SetParent(instant.transform);
            Vector3 boxScale = BoxManager.GetInstance().GetLocalScale();
            instant.transform.localScale = new Vector3(boxScale.x * m_shelfSetup.ScaleMultiplier.x, 
                                                       boxScale.y * m_shelfSetup.ScaleMultiplier.y, 
                                                       boxScale.z * m_shelfSetup.ScaleMultiplier.z);// Item prefab become parent of Model
            model.transform.localPosition = Vector3.zero;                                    // Reset Model position

            BoxCollider modelCollider = model.GetComponent<BoxCollider>();

            if (modelCollider != null) 
            {
                modelCollider.enabled = false;
            }
            
            if (m_remainingItems < 1)
                DisabledShelf();

            return instant;
        }

        /// <summary> Change the color of unavailable shelf </summary>
        private void DisabledShelf()
        {
            if (m_activateItemRespawn)
                m_itemIsRespawning = true;

            m_renderer.material.color = Color.black;

        }

        /// <summary> Reset item available on shelf and reset all colors </summary>
        private void ResetShelf()
        {
            m_itemIsRespawning = false;

            m_remainingItems = m_itemQuantity;
            m_renderer.material.color = m_initialColor;
        }

        private void ResetTimer()
        {
            m_currentTimer = 0;
        }

        public int GetItemRemaining()
        {
            return m_remainingItems;
        }
    }
}
