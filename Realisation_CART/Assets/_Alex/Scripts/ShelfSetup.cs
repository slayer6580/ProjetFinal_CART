using System.Collections.Generic;
using UnityEngine;

namespace BoxSystem
{
    [RequireComponent(typeof(Shelf))]
    public class ShelfSetup : MonoBehaviour
    {

        [Header("number of items")]
        [SerializeField] private bool m_haveMaxItem;
        [ShowIf("m_haveMaxItem", false)][SerializeField] private float m_nbOfItems;

        [Header("item size multiplier")]
        [SerializeField] private Vector3 m_scaleMultiplier;

        [Header("put surfaces here")]
        [SerializeField] private Transform m_surfaces;

        [Header("put spawnPoints here")]
        [SerializeField] private Transform m_spawnPoints;

        [Header("space between slots")]
        [SerializeField][Range(0.05f, 1f)] private float m_lenghtGap;
        [SerializeField][Range(0.05f, 1f)] private float m_depthGap;

        private List<Transform> m_surfacesList = new List<Transform>();
        private List<Vector3> m_spawnPointsList = new List<Vector3>();
        private List<List<Vector3>> m_slotsLocalPositionsPerSurface = new List<List<Vector3>>();
        private List<Vector3> m_allSlotsLocalPositions = new List<Vector3>();
        private List<Vector3> m_surfaceLocalEulers = new List<Vector3>();

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
            GetAllSpawnPoints();
            CreateAllSlots();

            if (m_nbOfItems != 0 || m_haveMaxItem)
                PlaceItemsOnShelf();
        }


        private GameObject SpawnItem()
        {
            GameObject objectToSpawn = m_shelf.ItemPrefab;                  // take the item prefab
            Item itemScript = objectToSpawn.GetComponent<Item>();           // access his 'Item' script
            itemScript.m_data = m_shelf.ItemData;                           // Determine what kind of item it is  
            GameObject model = Instantiate(itemScript.m_data.m_object);     // Spawn Model

            BoxCollider modelCollider = model.GetComponent<BoxCollider>();
            if (modelCollider != null)
                Destroy(modelCollider);

            return model;
        }


        private void PlaceItemsOnShelf()
        {
            BoxManager boxInstance = BoxManager.GetInstance();

            if (m_allSlotsLocalPositions.Count <= m_nbOfItems || m_haveMaxItem)
            {
                AllSlotsHaveAnItem(boxInstance);
            }
            else
            {
                PlaceItemRandomlyAmountSlots(boxInstance);
            }

        }

        private void AllSlotsHaveAnItem(BoxManager boxInstance)
        {
            for (int i = 0; i < m_slotsLocalPositionsPerSurface.Count; i++)
            {
                for (int j = 0; j < m_slotsLocalPositionsPerSurface[i].Count; j++)
                {
                    GameObject item = SpawnItem();
                    item.transform.SetParent(gameObject.transform);
                    Vector3 boxScale = boxInstance.GetLocalScale();
                    item.transform.localScale = new Vector3(boxScale.x * m_scaleMultiplier.x, boxScale.y * m_scaleMultiplier.y, boxScale.z * m_scaleMultiplier.z);
                    item.transform.localPosition = m_slotsLocalPositionsPerSurface[i][j] + new Vector3(0, (boxInstance.SlotHeight / 2) * m_scaleMultiplier.y, 0);
                    item.transform.localEulerAngles = m_surfaceLocalEulers[i];
                }
            }
        }

        private void PlaceItemRandomlyAmountSlots(BoxManager boxInstance)
        {
            int nbOfSlots = m_allSlotsLocalPositions.Count;
            int currentIndexPosition;
            int randomIndex;
            List<int> randomSlotsIndex = new List<int>();

            // create a list of indexes for all slot possibility
            for (int i = 0; i < nbOfSlots; i++)
            {
                randomSlotsIndex.Add(i);
            }

            // Shuffle those indexes
            randomSlotsIndex = ShuffleList(randomSlotsIndex); 

            // For each item to add
            for (int i = 0; i < m_nbOfItems; i++)
            {
                randomIndex = randomSlotsIndex[i]; // get a random index
                currentIndexPosition = 0; // set currentIndexPosition at 0

                // for each surfaces
                for (int j = 0; j < m_slotsLocalPositionsPerSurface.Count; j++)
                {                    
                    // Get nb of slots possibilities in the surface          
                    currentIndexPosition += m_slotsLocalPositionsPerSurface[j].Count;

                    // if index is not on this surface
                    if (randomIndex >= currentIndexPosition)
                        continue;
      
                    // Add an item on this surface
                    int oldCurrent = currentIndexPosition - m_slotsLocalPositionsPerSurface[j].Count;
                    int indexOfLocalPosition = randomIndex - oldCurrent;

                    GameObject item = SpawnItem();
                    item.transform.SetParent(gameObject.transform);
                    Vector3 boxScale = boxInstance.GetLocalScale();
                    item.transform.localScale = new Vector3(boxScale.x * m_scaleMultiplier.x, boxScale.y * m_scaleMultiplier.y, boxScale.z * m_scaleMultiplier.z);
                    item.transform.localPosition = m_slotsLocalPositionsPerSurface[j][indexOfLocalPosition] + new Vector3(0, (boxInstance.SlotHeight / 2) * m_scaleMultiplier.y, 0);
                    item.transform.localEulerAngles = m_surfaceLocalEulers[j];
                    break;
                }

            }
        }

        private void GetAllSurfaces()
        {
            if (m_surfaces == null)
                return;

            int nbOfSurfaces = m_surfaces.childCount;

            for (int i = 0; i < nbOfSurfaces; i++)
            {
                Transform surface = m_surfaces.GetChild(i);
                m_surfacesList.Add(surface);
                m_surfaceLocalEulers.Add(surface.transform.localEulerAngles);
            }
        }

        private void GetAllSpawnPoints()
        {
            if (m_surfaces == null)
                return;

            int nbOfSpawns = m_spawnPoints.childCount;

            for (int i = 0; i < nbOfSpawns; i++)
            {
                Vector3 spawnPoint = m_spawnPoints.GetChild(i).transform.position;
                m_spawnPointsList.Add(spawnPoint);
            }
        }

        private void CreateAllSlots()
        {
            List<float> xPositions = new List<float>();
            List<Vector2> zAndYPositions = new List<Vector2>();
            float halfLenghtGap = m_lenghtGap / 2;
            float halfDepthGap = m_depthGap / 2;

            int surfaceCount = m_surfacesList.Count;
            for (int i = 0; i < surfaceCount; i++)
            {
                Transform surface = m_surfacesList[i];
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

                List<Vector3> localPositionsList = new List<Vector3>();

                for (int k = 0; k < xPositions.Count; k++)
                {
                    for (int j = 0; j < zAndYPositions.Count; j++)
                    {
                        Vector3 localPosition = new Vector3(xPositions[k] + surface.localPosition.x,
                                                       zAndYPositions[j].y + surface.localPosition.y,
                                                       zAndYPositions[j].x + surface.localPosition.z);

                        localPositionsList.Add(localPosition);
                        m_allSlotsLocalPositions.Add(localPosition);
                    }
                }

                m_slotsLocalPositionsPerSurface.Add(localPositionsList);
                xPositions.Clear();
                zAndYPositions.Clear();

                DisableSurfaceRenderers(surface);
            }
        }

        private void DisableSurfaceRenderers(Transform surface)
        {
            surface.gameObject.GetComponent<MeshRenderer>().enabled = false;
            surface.GetChild(2).GetComponent<MeshRenderer>().enabled = false;

        }

        public List<int> ShuffleList(List<int> list)
        {
            for (int i = list.Count - 1; i > 0; i--)
            {
                int j = Random.Range(0, i + 1);
                int temp = list[i];
                list[i] = list[j];
                list[j] = temp;
            }
            return list;
        }

        public Vector3 GetRandomSpawnPoints()
        {
            int nbOfSpawn = m_spawnPointsList.Count;

            return m_spawnPointsList[Random.Range(0,nbOfSpawn)];
        }

    }
}
