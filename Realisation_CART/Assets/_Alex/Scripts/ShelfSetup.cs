using System.Collections.Generic;
using UnityEngine;

namespace BoxSystem
{
    [RequireComponent(typeof(Shelf))]
    public class ShelfSetup : MonoBehaviour
    {

        [Tooltip("limited based on transform list size")]
        [SerializeField] private float m_nbOfItems;

        [Tooltip("item multiplier size")]
        [SerializeField] private float m_sizeMultiplier;

        [Header("Items transforms, Don't touch")]
        [SerializeField] private List<Transform> m_itemTransform;

        [Header("Transforms Gizmos")]
        [SerializeField] private bool m_canSeeTransformsGizmos;
        [SerializeField] private float m_gizmosSize;

        private Shelf m_shelf;

        private void Awake()
        {
            m_shelf = GetComponent<Shelf>();
        }

        void Start()
        {
            if (m_nbOfItems != 0)
            PlaceItemsOnShelf();
        }


        private void OnDrawGizmos()
        {
            if (m_canSeeTransformsGizmos)
            {
                foreach (Transform t in m_itemTransform)
                {
                    Gizmos.DrawSphere(t.position, m_gizmosSize);
                }                 
            }
        }

        private GameObject SpawnItem()
        {
            GameObject objectToSpawn = m_shelf.ItemPrefab;                  // take the item prefab
            Item itemScript = objectToSpawn.GetComponent<Item>();           // access his 'Item' script
            itemScript.m_data = m_shelf.ItemData;                           // Determine what kind of item it is                                                                                               // objectToSpawn.transform.position = transform.position;          // Set start position to the shelf
            GameObject instant = Instantiate(objectToSpawn);                // Spawn Item Prefab
            GameObject model = Instantiate(itemScript.m_data.m_object);     // Spawn Model
            model.transform.SetParent(instant.transform);                   // Item prefab become parent of Model
            model.transform.localPosition = Vector3.zero;                   // Reset Model position
            model.transform.localScale = Vector3.one;

            return instant;
        }


        private void PlaceItemsOnShelf()
        {
            ItemData.ESize size = m_shelf.ItemData.m_size;

            BoxManager boxInstance = BoxManager.GetInstance();

            if (m_itemTransform.Count <= m_nbOfItems)
            {
                foreach (Transform _transform in m_itemTransform)
                {
                    GameObject item = SpawnItem();
                    item.transform.SetParent(gameObject.transform);
                    item.transform.localScale = boxInstance.GetLocalScale() * m_sizeMultiplier;
                    item.transform.position = _transform.position + new Vector3(0, (boxInstance.SlotHeight / 2) * m_sizeMultiplier, 0);
                    item.transform.localEulerAngles = _transform.localEulerAngles;
                }
            }
            else
            {
                Debug.LogWarning("Need to create random locations");
                // TODO Find random locations
            }

        }


    }
}
