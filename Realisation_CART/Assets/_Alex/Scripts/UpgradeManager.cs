using CartControl;
using Cinemachine;
using DiscountDelirium;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;


namespace StatsSystem
{
    public class UpgradeManager : MonoBehaviour
    {
        [System.Serializable]
        public enum EStats
        {
            acceleration,
            maxSpeed,
            handling,
            balance,
            ranged,
            melee
        }

        [field: Header("Put main character here")]
        [field: SerializeField] public CartStateMachine CartMachine { get; private set; }

        [Header("Next Scene")]
        [SerializeField] private string m_nextSceneName;

        [Header("Put main virtual camera here")]
        [SerializeField] private CinemachineVirtualCamera m_virtualCamera;

        [Header("Put character scene position here")]
        [SerializeField] private Transform m_scenePosition;

        [Header("Rotation of player")]
        [SerializeField] private Vector3 m_playerRotationEuler;

        [Header("starting cart token to give at player")]
        [SerializeField] private int m_startingCartToken;

        [Header("nb of token cost depending on stat level")]
        [SerializeField] private int[] m_upgradeCostByLevel;

        [Header("Melee Weapons")]
        [SerializeField] private Melee m_melee_1;
        [SerializeField] private Melee m_melee_2;

        [Header("All Canvas Items")]
        [SerializeField] private TextMeshProUGUI m_nbOfCartTokenText;
        [Space]
        [SerializeField] private List<TextMeshProUGUI> m_allCostText = new List<TextMeshProUGUI>();
        [SerializeField] private List<TextMeshProUGUI> m_allBuyingText = new List<TextMeshProUGUI>();
        [SerializeField] private List<Button> m_allBuyButton = new List<Button>();
        [SerializeField] private List<Button> m_allSellButton = new List<Button>();
        [SerializeField] private List<Image> m_allFillBars = new List<Image>();

        private List<int> m_allStats = new List<int>();
        private List<int> m_allBuyingCost = new List<int>();
        private List<int> m_allUpgrade = new List<int>();

        private const int MAX_LEVEL = 4;
        private const int NB_OF_STATS = 6;


        private void SetUpList()
        {
            for (int i = 0; i < NB_OF_STATS; i++)
            {
                m_allStats.Add(0);
                m_allBuyingCost.Add(0);
                m_allUpgrade.Add(0);
            }
        }

        private void Awake()
        {
            AddMoney(m_startingCartToken);
            SetUpList();
        }

        private void Start()
        {
            GetSavedUpgrade();
            UpdateAll();
        }


        private void UpdateAll()
        {
            UpdateMoney();
            UpdateCost();
            UpdateBuyingCost();
            UpdateFillBars();
            UpdateBuyButton();
            UpdateSellButton();
            VisualManager.GetInstance().UpdateAll();
            m_melee_1.UpdateMeleeWeapon();
            m_melee_2.UpdateMeleeWeapon();
            DeselectAllButtons();
        }

        /// <summary> Load upgrades with PlayerPrefs </summary>
        private void GetSavedUpgrade()
        {
            m_allStats[0] = PlayerPrefs.GetInt("Acceleration", 0);
            m_allStats[1] = PlayerPrefs.GetInt("MaxSpeed", 0); ;
            m_allStats[2] = PlayerPrefs.GetInt("Handling", 0);
            m_allStats[3] = PlayerPrefs.GetInt("Balance", 0);
            m_allStats[4] = PlayerPrefs.GetInt("Ranged", 0);
            m_allStats[5] = PlayerPrefs.GetInt("Melee", 0);
        }

        /// <summary> Update all cost of upgrade </summary>
        private void UpdateCost()
        {

            for (int i = 0; i < NB_OF_STATS; i++)
            {
                if (m_allStats[i] < MAX_LEVEL)
                {
                    m_allCostText[i].text = m_upgradeCostByLevel[m_allStats[i]].ToString();
                    continue;
                }

                m_allCostText[i].text = " ";
            }
        }

        /// <summary> Update all fill bars </summary>
        private void UpdateFillBars()
        {
            for (int i = 0; i < NB_OF_STATS; i++)
                m_allFillBars[i].fillAmount = (float)m_allStats[i] / MAX_LEVEL;
        }

        /// <summary> Update all buying cost </summary>
        private void UpdateBuyingCost()
        {
            for (int i = 0; i < NB_OF_STATS; i++)
            {
                if (m_allUpgrade[i] == 0)
                {
                    m_allBuyingText[i].text = " ";
                    continue;
                }

                m_allBuyingText[i].text = "- " + m_allBuyingCost[i].ToString();
            }
        }

        /// <summary> Update money left </summary>
        private void UpdateMoney()
        {
            m_nbOfCartTokenText.text = PlayerPrefs.GetInt("Money", 0).ToString();
        }

        /// <summary> Update all buy button </summary>
        public void UpdateBuyButton()
        {
            int money = PlayerPrefs.GetInt("Money", 0);

            for (int i = 0; i < NB_OF_STATS; i++)
            {
                if (m_allStats[i] < MAX_LEVEL)
                {
                    m_allBuyButton[i].interactable = money >= m_upgradeCostByLevel[m_allStats[i]];
                    continue;
                }

                m_allBuyButton[i].interactable = false;

            }

        }

        /// <summary> Update all sell button </summary>
        public void UpdateSellButton()
        {
            for (int i = 0; i < NB_OF_STATS; i++)
                m_allSellButton[i].interactable = m_allUpgrade[i] > 0;
        }

        /// <summary> Buy upgrade </summary>
        public void BuyUpgrade(int statsIndex)
        {
            int cost;

            cost = m_upgradeCostByLevel[m_allStats[statsIndex]];
            m_allBuyingCost[statsIndex] += cost;
            m_allUpgrade[statsIndex]++;
            m_allStats[statsIndex]++;

            int money = PlayerPrefs.GetInt("Money", 0);
            money -= cost;
            PlayerPrefs.SetInt("Money", money);

            SaveUpgrades();
            UpdateAll();
        }

        /// <summary> Sell upgrade </summary>
        public void SellUpgrade(int statsIndex)
        {
            int refund;

            refund = m_upgradeCostByLevel[m_allStats[statsIndex] - 1];
            m_allBuyingCost[statsIndex] -= refund;
            m_allUpgrade[statsIndex]--;
            m_allStats[statsIndex]--;

            int money = PlayerPrefs.GetInt("Money", 0);
            money += refund;
            PlayerPrefs.SetInt("Money", money);

            SaveUpgrades();
            UpdateAll();
        }

        /// <summary> Deselect all button for better UI button interaction </summary>
        private void DeselectAllButtons()
        {
            GameObject myEventSystem = GameObject.Find("EventSystem");
            myEventSystem.GetComponent<UnityEngine.EventSystems.EventSystem>().SetSelectedGameObject(null);
        }

        /// <summary> Accept All upgrade and change scene </summary>
        public void AcceptUpgrade()
        {
            for (int i = 0; i < NB_OF_STATS; i++)
                m_allBuyingCost[i] = 0;


            for (int i = 0; i < NB_OF_STATS; i++)
                m_allUpgrade[i] = 0;

            SceneManager.LoadScene(m_nextSceneName);
        }


        /// <summary> Save upgrades with PlayerPrefs </summary>
        private void SaveUpgrades()
        {
            PlayerPrefs.SetInt("Acceleration", m_allStats[0]);
            PlayerPrefs.SetInt("MaxSpeed", m_allStats[1]);
            PlayerPrefs.SetInt("Handling", m_allStats[2]);
            PlayerPrefs.SetInt("Balance", m_allStats[3]);
            PlayerPrefs.SetInt("Ranged", m_allStats[4]);
            PlayerPrefs.SetInt("Melee", m_allStats[5]);
        }

        /// <summary> Teleport player at a scene for the upgrade UI scene and change caméra </summary>
        public void TeleportPlayerToUpgradeScene()
        {
            UpdateAll();
            m_virtualCamera.Priority = 8;
            Rigidbody rb = CartMachine.gameObject.GetComponent<Rigidbody>();

            if (rb != null)
                rb.useGravity = false;

            CartMachine.gameObject.transform.position = m_scenePosition.position;
            CartMachine.gameObject.transform.localEulerAngles = m_playerRotationEuler;
        }


        /// <summary> Restart all upgrade choices </summary>
        public void Restart()
        {

            for (int i = 0; i < NB_OF_STATS; i++)
            {
                int nbOfUpgrade = m_allUpgrade[i];

                for (int j = 0; j < nbOfUpgrade; j++)
                    SellUpgrade(i);
            }

            UpdateAll();
        }

        /// <summary> Add money to spend </summary>
        public void AddMoney(int amount)
        {
            int money = PlayerPrefs.GetInt("Money", 0);
            money += amount;
            PlayerPrefs.SetInt("Money", money);
            UpdateMoney();
        }

    }

}

