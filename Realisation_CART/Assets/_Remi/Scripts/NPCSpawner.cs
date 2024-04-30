using System.Collections.Generic;
using UnityEngine;

namespace Spawner
{
    public class NPCSpawner : MonoBehaviour
    {
        [field: SerializeField] private GameObject NPCPrefab { get;  set; } = null;

        [field: SerializeField] private GameObject SpawningZones { get;  set; } = null;

        private List<GameObject> EntranceSpawningSpots { get; set; } = new List<GameObject>();

        private void Awake()
        {
            foreach (Transform child in SpawningZones.transform)
            {
                foreach (Transform grandChild in child)
                {
                    EntranceSpawningSpots.Add(grandChild.gameObject);
                }
            }

            foreach (var spot in EntranceSpawningSpots)
            {
                Instantiate(NPCPrefab, spot.transform.position, Quaternion.identity);
            }
        }
    }
}
