using TMPro;
using UnityEngine;

namespace BoxSystem
{
    public class Shelf : MonoBehaviour
    {
        [Header("Put the item prefab here")]
        [SerializeField] private GameObject m_itemPrefab;
        [Header("Put the item scriptable object here")]
        [SerializeField] private ItemData m_itemData;
        [Header("item max quantity to take per trip")]
        [SerializeField] private int m_itemQuantityPerTrip;

        [Header("DEBUG ONLY, Put Debug TMP Here")]
        [SerializeField] private TextMeshPro m_quantityText;

        private MeshRenderer m_renderer;

        private int m_remainingItems;
        private Color m_initialColor;

        private void Awake()
        {
            m_remainingItems = m_itemQuantityPerTrip;
            m_quantityText.text = m_remainingItems.ToString();
            m_renderer = GetComponent<MeshRenderer>();
        }

        private void Start()
        {
            ColorShelf();
            m_initialColor = m_renderer.material.color;
        }

        /// <summary> Check if the item limit is not broken on this shelf </summary>
        public bool CanTakeItem()
        {
            return m_remainingItems > 0;
        }

        /// <summary> Take an item from shelf </summary>
        public GameObject GetItemFromShelf()
        {

            m_remainingItems--;                                             // remove an available item count.

            GameObject objectToSpawn = m_itemPrefab;                        // take the item prefab
            Item itemScript = objectToSpawn.GetComponent<Item>();           // access his 'Item' script
            itemScript.m_data = m_itemData;                                 // Determine what kind of item it is           
            objectToSpawn.transform.position = transform.position;          // Set start position to the shelf
            GameObject instant = Instantiate(objectToSpawn);                // Spawn Item Prefab
            GameObject model = Instantiate(itemScript.m_data.m_object);     // Spawn Model
            model.transform.SetParent(instant.transform);                   // Item prefab become parent of Model
            model.transform.localPosition = Vector3.zero;                   // Reset Model position

            m_quantityText.text = m_remainingItems.ToString();

            if (m_remainingItems < 1)
                DisabledShelf();

            return instant;
        }


        private void OnValidate()
        {
            m_quantityText.text = m_itemQuantityPerTrip.ToString();
        }


        /// <summary> Change Color of shelf depending of item size </summary>
        private void ColorShelf()
        {
            switch (m_itemData.m_size)
            {
                case ItemData.ESize.small:
                    m_renderer.material.color = Color.green;
                    break;
                case ItemData.ESize.medium:
                    m_renderer.material.color = Color.yellow;
                    break;
                case ItemData.ESize.large:
                    m_renderer.material.color = Color.red;
                    break;
                default:
                    m_renderer.material.color = Color.white;
                    break;
            }
        }

        /// <summary> Get number of items available to take  </summary>
        public int GetRemainingItems()
        {
            return m_remainingItems;
        }


        /// <summary> Change the color of unavailable shelf </summary>
        private void DisabledShelf()
        {
            m_renderer.material.color = Color.gray;
        }

        /// <summary> Reset item available on shelf and reset all colors </summary>
        public void ResetShelf()
        {
            m_remainingItems = m_itemQuantityPerTrip;
            m_quantityText.text = m_remainingItems.ToString();
            m_renderer.material.color = m_initialColor;
        }


    }



}
