using DiscountDelirium;
using System;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

namespace Spawner
{
    public class NPCSpawner : MonoBehaviour
    {
        [field: SerializeField] private GameObject NPCPrefab { get; set; } = null;
        [field: SerializeField] private GameObject SpawningZones { get; set; } = null;

        [field: Header("Spawning Settings")]

        [field: Header("Entrance of the level :                       Rate = Spawn per minutes")]
        [SerializeField] private bool m_isSpawningFromEntrance = false;
        [SerializeField] private bool m_isOneShot = true;
        [SerializeField] private int m_numberOfNPCFromEntrance = 1;
        [SerializeField] private bool m_isRateRandom = false;
        [SerializeField] private float m_spawningEntranceRandomness = 0.0f;
        [SerializeField] private float m_spawningEntranceRate = 0.0f;
        [SerializeField] private float m_secondsUntilNextSpawn = 0.0f;

        private List<GameObject> AllSpawningSpots { get; set; } = new List<GameObject>();
        private List<GameObject> EntranceSpawningSpots { get; set; } = new List<GameObject>();

        public const float SIXTY_SECONDES = 60.0f;

        [SerializeField] private Quaternion m_npcsOrientation = Quaternion.identity;


        private void Awake()
        {
            GetSpotGameObjects();
            if (m_isRateRandom)
                GenerateRandomRates();
            AssignRates();
        }

        private void GetSpotGameObjects()
        {
            foreach (Transform child in SpawningZones.transform)
            {
                if (child.name == "Entrance")
                {
                    foreach (Transform grandChild in child)
                    {
                        EntranceSpawningSpots.Add(grandChild.gameObject);
                        AllSpawningSpots.Add(grandChild.gameObject);
                    }
                }
            }
        }

        private void GenerateRandomRates()
        {
            m_secondsUntilNextSpawn = GenerateRandomRate(m_isSpawningFromEntrance, m_spawningEntranceRate, m_spawningEntranceRandomness);
            Debug.Log("set m_currentSpawningEntranceRate: " + m_secondsUntilNextSpawn);
        }


        private void AssignRates()
        {
            if (m_spawningEntranceRate == 0)
            { 
                m_secondsUntilNextSpawn = 0;
            }
            else
            { 
                m_secondsUntilNextSpawn = (m_spawningEntranceRate * SIXTY_SECONDES) / Mathf.Pow(m_spawningEntranceRate, 2);
            }
        }

        private void Update()
        {
            UpdateSpawns();
        }

        private float GenerateRandomRate(bool isSpawningFromZone, float spawningRate, float spawningRandomness)
        {
            if (!isSpawningFromZone || spawningRandomness <= 0) return spawningRate;

            return math.abs(UnityEngine.Random.Range(spawningRate - spawningRandomness, spawningRate + spawningRandomness));
        }

        private void UpdateSpawns()
        {
            SpawnNPCs(m_isSpawningFromEntrance, ref m_numberOfNPCFromEntrance, m_spawningEntranceRandomness, ref m_secondsUntilNextSpawn, m_spawningEntranceRate);
        }

        private void SpawnNPCs(bool isSpawningFromZone, ref int numberOfNPCs, float spawningEntranceRandomness, ref float currentSpawningRate, float originalSpawningRate)
        {
            if (!isSpawningFromZone) return;

            if (m_isOneShot)
            {
                if (numberOfNPCs <= 0) return;
                Spawn();
                numberOfNPCs--;
                if (numberOfNPCs == 0)
                {
                    gameObject.SetActive(false);
                }
            }
            else
            {
                int currentSpawningRateInt = Mathf.CeilToInt((SIXTY_SECONDES / currentSpawningRate));
                int originalSpawningRateInt = (int)originalSpawningRate;
                currentSpawningRate -= Time.deltaTime;

                if (currentSpawningRate <= 0)
                {
                    ResetRate(originalSpawningRate);
                    Spawn();
                }
            }
        }

        private void Spawn()
        {
            if (EntranceSpawningSpots.Count == 0) return;

            var randomSpot = UnityEngine.Random.Range(0, GetZoneGOList().Count);

            for (int i = 0; i < GetZoneGOList().Count; i++)
            {
                if (EntranceSpawningSpots.Contains(GetZoneGOList()[randomSpot]))
                {
                    break;
                }

                randomSpot = UnityEngine.Random.Range(0, GetZoneGOList().Count);
            }

            if (EntranceSpawningSpots.Contains(GetZoneGOList()[randomSpot]))
            {
                Instantiate(NPCPrefab, GetZoneGOList()[randomSpot].transform.position, m_npcsOrientation);
                EntranceSpawningSpots.Remove(GetZoneGOList()[randomSpot]);

                GameStateMachine.Instance.ClientsList.Add(NPCPrefab);
            }
        }

        private List<GameObject> GetZoneGOList()
        {
            return EntranceSpawningSpots;
        }

        private void ResetRate(float spawningRate)
        {
            if (m_isRateRandom)
            {
                GenerateRandomRates();
                return;
            }
            else
                AssignRates();
        }
    }
}