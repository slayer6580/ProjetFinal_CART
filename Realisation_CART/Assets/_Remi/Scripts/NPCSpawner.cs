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

        private enum e_spawningZone
        {
            FromAllZones,
            FromEntrance,
            FromFrontLeft,
            FromFrontRight,
            FromBackLeft,
            FromBackRight,
            count
        }

        [field: Header("Spawning Settings")]

        //[field: Header("All zones of the level")]
        //[SerializeField] private bool m_isSpawningFromAllZones = false;
        //[SerializeField] private int m_numberOfNPCsFromAllZones = 1;
        //[SerializeField] private float m_spawningRandomness = 0.0f;
        //[SerializeField] private float m_spawningRate = 0.0f; // 0 for one shot spawning
        //[SerializeField] private float m_currentSpawningRate = 0.0f;

        [field: Header("Entrance of the level :                       Rate = Spawn per minutes")]
        [SerializeField] private bool m_isSpawningFromEntrance = false;
        [SerializeField] private bool m_isOneShot = true;
        [SerializeField] private int m_numberOfNPCFromEntrance = 1;
        [SerializeField] private bool m_isRateRandom = false;
        [SerializeField] private float m_spawningEntranceRandomness = 0.0f;
        [SerializeField] private float m_spawningEntranceRate = 0.0f;
        [SerializeField] private float m_secondsUntilNextSpawn = 0.0f;

        //[field: Header("Front-Left of the level")]
        //[SerializeField] private bool m_isSpawningFromFrontLeft = false;
        //[SerializeField] private int m_numberOfNPCsFromFrontLeft = 1;
        //[SerializeField] private float m_spawningFrontLeftRandomness = 0.0f;
        //[SerializeField] private float m_spawningFrontLeftRate = 0.0f; // 0 for one shot spawning
        //[SerializeField] private float m_currentSpawningFrontLeftRate = 0.0f;

        //[field: Header("Front-Right of the level")]
        //[SerializeField] private bool m_isSpawningFromFrontRight = false;
        //[SerializeField] private int m_numberOfNPCsFromFrontRight = 1;
        //[SerializeField] private float m_spawningFrontRightRandomness = 0.0f;
        //[SerializeField] private float m_spawningFrontRightRate = 0.0f; // 0 for one shot spawning
        //[SerializeField] private float m_currentSpawningFrontRightRate = 0.0f;

        //[field: Header("Back-Left of the level")]
        //[SerializeField] private bool m_isSpawningFromBackLeft = false;
        //[SerializeField] private int m_numberOfNPCsFromBackLeft = 1;
        //[SerializeField] private float m_spawningBackLeftRandomness = 0.0f;
        //[SerializeField] private float m_spawningBackLeftRate = 0.0f; // 0 for one shot spawning
        //[SerializeField] private float m_currentSpawningBackLeftRate = 0.0f;

        //[field: Header("Back-Right of the level")]
        //[SerializeField] private bool m_isSpawningFromBackRight = false;
        //[SerializeField] private int m_numberOfNPCsFromBackRight = 1;
        //[SerializeField] private float m_spawningBackRightRandomness = 0.0f;
        //[SerializeField] private float m_spawningBackRightRate = 0.0f; // 0 for one shot spawning
        //[SerializeField] private float m_currentSpawningBackRightRate = 0.0f;

        private List<GameObject> AllSpawningSpots { get; set; } = new List<GameObject>();
        private List<GameObject> EntranceSpawningSpots { get; set; } = new List<GameObject>();
        private List<GameObject> FrontLeftSpawningSpots { get; set; } = new List<GameObject>();
        private List<GameObject> FrontRightSpawningSpots { get; set; } = new List<GameObject>();
        private List<GameObject> BackLeftSpawningSpots { get; set; } = new List<GameObject>();
        private List<GameObject> BackRightSpawningSpots { get; set; } = new List<GameObject>();
        public const float SIXTY_SECONDES = 60.0f;

        //private const float ONE_SHOT_DONE = -10000;

        private void Awake()
        {
            Debug.Log("Awake");
            GetSpotGameObjects();
            if (m_isRateRandom)
                GenerateRandomRates(e_spawningZone.FromEntrance); // TODO Remi: Focus on EntranceZone only
            AssignRates(e_spawningZone.FromEntrance);// TODO Remi: Focus on EntranceZone only
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
                else if (child.name == "Front-Left")
                {
                    foreach (Transform grandChild in child)
                    {
                        FrontLeftSpawningSpots.Add(grandChild.gameObject);
                        AllSpawningSpots.Add(grandChild.gameObject);
                    }
                }
                else if (child.name == "Front-Right")
                {
                    foreach (Transform grandChild in child)
                    {
                        FrontRightSpawningSpots.Add(grandChild.gameObject);
                        AllSpawningSpots.Add(grandChild.gameObject);
                    }
                }
                else if (child.name == "Back-Left")
                {
                    foreach (Transform grandChild in child)
                    {
                        BackLeftSpawningSpots.Add(grandChild.gameObject);
                        AllSpawningSpots.Add(grandChild.gameObject);
                    }
                }
                else if (child.name == "Back-Right")
                {
                    foreach (Transform grandChild in child)
                    {
                        BackRightSpawningSpots.Add(grandChild.gameObject);
                        AllSpawningSpots.Add(grandChild.gameObject);
                    }
                }
            }
        }

        private void GenerateRandomRates(e_spawningZone spawningZone)
        {
            Debug.Log("GenerateRandomRates");

            switch (spawningZone)
            {
                case e_spawningZone.FromAllZones:

                    //m_currentSpawningRate = GenerateRandomRate(m_isSpawningFromAllZones, m_spawningRate, m_spawningRandomness);
                    break;
                case e_spawningZone.FromEntrance:
                    m_secondsUntilNextSpawn = GenerateRandomRate(m_isSpawningFromEntrance, m_spawningEntranceRate, m_spawningEntranceRandomness);
                    Debug.Log("set m_currentSpawningEntranceRate: " + m_secondsUntilNextSpawn);
                    break;
                case e_spawningZone.FromFrontLeft:
                    //m_currentSpawningFrontLeftRate = GenerateRandomRate(m_isSpawningFromFrontLeft, m_spawningFrontLeftRate, m_spawningFrontLeftRandomness);
                    break;
                case e_spawningZone.FromFrontRight:
                    //m_currentSpawningFrontRightRate = GenerateRandomRate(m_isSpawningFromFrontRight, m_spawningFrontRightRate, m_spawningFrontRightRandomness);
                    break;
                case e_spawningZone.FromBackLeft:
                    //m_currentSpawningBackLeftRate = GenerateRandomRate(m_isSpawningFromBackLeft, m_spawningBackLeftRate, m_spawningBackLeftRandomness);
                    break;
                case e_spawningZone.FromBackRight:
                    //m_currentSpawningBackRightRate = GenerateRandomRate(m_isSpawningFromBackRight, m_spawningBackRightRate, m_spawningBackRightRandomness);
                    break;
                case e_spawningZone.count:
                    GenerateRateForEveryZone();
                    break;
                default:
                    Debug.LogError("Invalid SpawningType");
                    break;
            }
        }

        private void GenerateRateForEveryZone()
        {
            Debug.Log("GenerateRateForEveryZone");
            for (int j = 0; j < (int)e_spawningZone.count; j++)
            {
                GenerateRandomRates((e_spawningZone)j);
            }
        }

        private void AssignRateForEveryZone()
        {
            Debug.Log("AssignRateForEveryZone");
            for (int i = 0; i < (int)e_spawningZone.count; i++)
            {
                AssignRates((e_spawningZone)i);
            }
        }

        private void AssignRates(e_spawningZone spawningZone)
        {
            Debug.Log("AssignRates");
            if (m_spawningEntranceRandomness != 0) return;

            switch (spawningZone)
            {
                case e_spawningZone.FromAllZones:

                    //m_currentSpawningRate = GenerateRandomRate(m_isSpawningFromAllZones, m_spawningRate, m_spawningRandomness);
                    break;
                case e_spawningZone.FromEntrance:
                    //Debug.Log("m_spawningEntranceRate: " + m_spawningEntranceRate);
                    //Debug.Log("m_secondsUntilNextSpawn: " + m_secondsUntilNextSpawn);
                    if (m_spawningEntranceRate == 0)
                        {m_secondsUntilNextSpawn = 0; Debug.Log("m_secondsUntilNextSpawn: " + m_secondsUntilNextSpawn); }
                    else
                        {m_secondsUntilNextSpawn = (m_spawningEntranceRate * SIXTY_SECONDES) / Mathf.Pow(m_spawningEntranceRate, 2); Debug.Log("m_secondsUntilNextSpawn: " + m_secondsUntilNextSpawn); }

                    //Debug.Log("(m_spawningEntranceRate * SIXTY_SECONDES): " + (m_spawningEntranceRate * SIXTY_SECONDES) + " / " + "math.exp2(m_spawningEntranceRate): " + Mathf.Pow(m_spawningEntranceRate, 2));
                    //Debug.Log("m_currentSpawningEntranceRate: " + m_currentSpawningEntranceRate);

                    break;
                case e_spawningZone.FromFrontLeft:
                    //m_currentSpawningFrontLeftRate = GenerateRandomRate(m_isSpawningFromFrontLeft, m_spawningFrontLeftRate, m_spawningFrontLeftRandomness);
                    break;
                case e_spawningZone.FromFrontRight:
                    //m_currentSpawningFrontRightRate = GenerateRandomRate(m_isSpawningFromFrontRight, m_spawningFrontRightRate, m_spawningFrontRightRandomness);
                    break;
                case e_spawningZone.FromBackLeft:
                    //m_currentSpawningBackLeftRate = GenerateRandomRate(m_isSpawningFromBackLeft, m_spawningBackLeftRate, m_spawningBackLeftRandomness);
                    break;
                case e_spawningZone.FromBackRight:
                    //m_currentSpawningBackRightRate = GenerateRandomRate(m_isSpawningFromBackRight, m_spawningBackRightRate, m_spawningBackRightRandomness);
                    break;
                case e_spawningZone.count:
                    AssignRateForEveryZone();
                    break;
                default:
                    Debug.LogError("Invalid SpawningType");
                    break;
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
            for (int i = 0; i < (int)e_spawningZone.count; i++)
            {
                switch ((e_spawningZone)i)
                {
                    case e_spawningZone.FromAllZones:
                        //SpawnNPCs((SpawningType)i, m_isSpawningFromAllZones, m_numberOfNPCsFromAllZones, m_spawningRate, ref m_currentSpawningRate);
                        break;
                    case e_spawningZone.FromEntrance:
                        //Debug.Log("m_currentSpawningEntranceRate: " + m_currentSpawningEntranceRate);
                        SpawnNPCs(e_spawningZone.FromEntrance, m_isSpawningFromEntrance, ref m_numberOfNPCFromEntrance, m_spawningEntranceRandomness, ref m_secondsUntilNextSpawn, m_spawningEntranceRate);
                        break;
                    case e_spawningZone.FromFrontLeft:
                        //SpawnNPCs((SpawningType)i, m_isSpawningFromFrontLeft, m_numberOfNPCsFromFrontLeft, m_spawningFrontLeftRate, ref m_currentSpawningFrontLeftRate);
                        break;
                    case e_spawningZone.FromFrontRight:
                        //SpawnNPCs((SpawningType)i, m_isSpawningFromFrontRight, m_numberOfNPCsFromFrontRight, m_spawningFrontRightRate, ref m_currentSpawningFrontRightRate);
                        break;
                    case e_spawningZone.FromBackLeft:
                        //SpawnNPCs((SpawningType)i, m_isSpawningFromBackLeft, m_numberOfNPCsFromBackLeft, m_spawningBackLeftRate, ref m_currentSpawningBackLeftRate);
                        break;
                    case e_spawningZone.FromBackRight:
                        //SpawnNPCs((SpawningType)i, m_isSpawningFromBackRight, m_numberOfNPCsFromBackRight, m_spawningBackRightRate, ref m_currentSpawningBackRightRate);
                        break;
                    default:
                        Debug.LogError("Invalid SpawningType");
                        break;
                }
            }
        }

        private void SpawnNPCs(e_spawningZone spawnType, bool isSpawningFromZone, ref int numberOfNPCs, float spawningEntranceRandomness, ref float currentSpawningRate, float originalSpawningRate)
        {
            if (!isSpawningFromZone /*|| numberOfNPCs <= 0*/) return;

            if (m_isOneShot)
            {
                if (numberOfNPCs <= 0) return;
                //for (int i = 0; i < numberOfNPCs; i++)
                //{
                Debug.Log("spawningRate 1: " + currentSpawningRate);
                Spawn(spawnType);
                Debug.Log("NPC spawned 1");
                numberOfNPCs--;
                //}
            }
            else
            {
                // m_spawningEntranceRate = 1 then 1 npc every 1 minute
                // m_spawningEntranceRate = 0.5 then 1 npc every 2 minutes
                // m_spawningEntranceRate = 10 then 1 npc every 6 seconds
                

                //if (currentSpawningRate <= 0)
                //{
                //    Debug.Log("spawningRate 2: " + currentSpawningRate);
                //    //Spawn(spawnType);
                //    Debug.Log("NPC spawned 2");
                //    ResetRate(spawnType, originalSpawningRate);
                //    return;
                //}

                //Debug.Log("spawningRate 3: " + currentSpawningRate);
                // Spawn the first NPC and start the spawning countdown
                Debug.Log("currentSpawningRate: " + currentSpawningRate);
                int currentSpawningRateInt = Mathf.CeilToInt((SIXTY_SECONDES / currentSpawningRate));
                int originalSpawningRateInt = (int)originalSpawningRate;
                //(m_spawningEntranceRate * SIXTY_SECONDES) / Mathf.Pow(m_spawningEntranceRate, 2)
                //if (currentSpawningRateInt == originalSpawningRateInt)
                //{
                //    Spawn(spawnType);
                //    Debug.Log("NPC spawned 3");
                //    //if (currentSpawningRate - 1 > 0)
                //    //    currentSpawningRate -= 1;
                //}
                //else
                //{
                    Debug.Log("ScurrentSpawningRateInt: " + currentSpawningRateInt + "== originalSpawningRateInt: " + originalSpawningRateInt + " = " + (currentSpawningRateInt == originalSpawningRateInt));
                    currentSpawningRate -= Time.deltaTime;
                //}
            }

            if (currentSpawningRate <= 0)
            {
                Debug.Log("ResetRate currentSpawningRate: " + currentSpawningRate);
                ResetRate(spawnType, originalSpawningRate);
                Spawn(spawnType);
            }

        }

        private void Spawn(e_spawningZone spawnType)
        {
            var randomSpot = UnityEngine.Random.Range(0, GetZoneGOList(spawnType).Count);
            GameObject newNPC = Instantiate(NPCPrefab, GetZoneGOList(spawnType)[randomSpot].transform.position, Quaternion.identity);
            Transform[] bodyparts = newNPC.GetComponentsInChildren<Transform>();
        }

        private List<GameObject> GetZoneGOList(e_spawningZone spawnType)
        {
            switch (spawnType)
            {
                case e_spawningZone.FromAllZones:
                    return AllSpawningSpots;
                case e_spawningZone.FromEntrance:
                    return EntranceSpawningSpots;
                case e_spawningZone.FromFrontLeft:
                    return FrontLeftSpawningSpots;
                case e_spawningZone.FromFrontRight:
                    return FrontRightSpawningSpots;
                case e_spawningZone.FromBackLeft:
                    return BackLeftSpawningSpots;
                case e_spawningZone.FromBackRight:
                    return BackRightSpawningSpots;
                default:
                    Debug.LogError("Invalid SpawningType");
                    return null;
            }
        }

        private void ResetRate(e_spawningZone zoneToReset, float spawningRate)
        {
            Debug.Log("ResetRate");
            if (m_isRateRandom)
            {
                GenerateRandomRates(zoneToReset);
                return;
            }
            else/* if (m_isOneShot)*/
                AssignRates(zoneToReset);
        }
    }
}