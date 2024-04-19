using System.Collections.Generic;
using UnityEngine;

namespace BoxSystem
{
    [RequireComponent(typeof(Shelf))]
    public class ShelfSetup : MonoBehaviour
    {

        [Tooltip("Number of items")]
        [SerializeField] private bool m_haveMaxItem;
        [ShowIf("m_haveMaxItem", false)][SerializeField] private float m_nbOfItems;

        [Tooltip("item size multiplier")]
        [SerializeField] private float m_sizeMultiplier;

        [Header("Put surfaces here")]
        [SerializeField] private Transform m_surfaces;

        [Header("Gap between slots")]
        [SerializeField][Range(0.05f, 1f)] private float m_lenghtGap;
        [SerializeField][Range(0.05f, 1f)] private float m_depthGap;

        private List<Transform> m_surfacesList = new List<Transform>();
        private List<Vector3> m_localPositions = new List<Vector3>();
        private Vector3 m_surfaceLocalEuler;

        private Shelf m_shelf;

        private void Awake()
        {
            m_shelf = GetComponent<Shelf>();
        }

        void Start()
        {
            if (m_surfaces == null)
                return;

            GetAllSurfaces();
            CreateAllSlots();

            if (m_nbOfItems != 0 || m_haveMaxItem)
                PlaceItemsOnShelf();
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

            if (m_localPositions.Count <= m_nbOfItems || m_haveMaxItem)
            {
                AllSlotsHaveAnItem(boxInstance);
            }
            else
            {
                Debug.LogWarning("Need to create random locations");
                // TODO Find random locations
            }

        }

        private void AllSlotsHaveAnItem(BoxManager boxInstance)
        {
            foreach (Vector3 localPosition in m_localPositions)
            {
                GameObject item = SpawnItem();
                item.transform.SetParent(gameObject.transform);
                item.transform.localScale = boxInstance.GetLocalScale() * m_sizeMultiplier;
                item.transform.localPosition = localPosition + new Vector3(0, (boxInstance.SlotHeight / 2) * m_sizeMultiplier, 0);
                item.transform.localEulerAngles = m_surfaceLocalEuler;
            }
        }

        private void GetAllSurfaces()
        {
            if (m_surfaces == null)
                return;

            int nbOfSurfaces = m_surfaces.childCount;

            for (int i = 0; i < nbOfSurfaces; i++)
            {
                if (i == 0)
                    m_surfaceLocalEuler = m_surfaces.GetChild(i).localEulerAngles;

                Transform surface = m_surfaces.GetChild(i);
                m_surfacesList.Add(surface);
            }
        }

        private void CreateAllSlots()
        {
            m_localPositions.Clear();

            List<float> xPositions = new List<float>();
            List<Vector2> zAndYPositions = new List<Vector2>();
            float halfLenghtGap = m_lenghtGap / 2;
            float halfDepthGap = m_depthGap / 2;

            foreach (Transform surface in m_surfacesList)
            {
                Transform topZ = surface.GetChild(0);
                Transform bottomZ = surface.GetChild(1);

                // Take measurements of surface
                float maxLenght = surface.transform.localScale.x / 2;
                float maxDepth = surface.transform.localScale.z / 2;

                //Start positions
                float xPosition = maxLenght - halfLenghtGap;
                float zPosition = maxDepth - halfDepthGap;

                //Find all X local positions
                while (xPosition > (-maxLenght + halfLenghtGap))
                {
                    xPositions.Add(xPosition);
                    xPosition -= m_lenghtGap;
                }

                //Find all Z local positions
                while (zPosition > (-maxDepth + halfDepthGap))
                {
                    float topY = topZ.position.y;
                    float bottomY = bottomZ.position.y;
                    float difference = topY - bottomY;
                    float halfDifference = difference / 2;

                    float depthPosition = Mathf.InverseLerp(-maxDepth, maxDepth, zPosition);
                    float yPosition = Mathf.Lerp(halfDifference, -halfDifference, depthPosition);

                    zAndYPositions.Add(new Vector2(zPosition, yPosition));
                    zPosition -= m_depthGap;
                }

                // Place all LocalPositions here
                for (int i = 0; i < xPositions.Count; i++)
                {
                    for (int j = 0; j < zAndYPositions.Count; j++)
                    {
                        m_localPositions.Add(new Vector3(xPositions[i] + surface.localPosition.x, surface.localPosition.y + zAndYPositions[j].y, zAndYPositions[j].x + surface.localPosition.z));
                    }
                }

                xPositions.Clear();
                zAndYPositions.Clear();

                DisableSurfaceRenderer(surface);
            }
        }

        private void DisableSurfaceRenderer(Transform surface)
        {
            surface.gameObject.GetComponent<MeshRenderer>().enabled = false;
        }
    }
}
