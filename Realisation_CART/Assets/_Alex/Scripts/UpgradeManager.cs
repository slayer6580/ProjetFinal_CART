using TMPro;

using UnityEngine;
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
            defense
        }

        [Header("starting cart token to give at player")]
        [SerializeField] private int m_startingCartToken;

        [Header("nb of token cost depending on stat level")]
        [SerializeField] private int[] m_upgradeCostByLevel;

        [Header("Mettre les TextMeshProUGUI du panel upgrade ci dessous")]
        [SerializeField] private bool m_ShowTMP = false;
        [ShowIf("m_ShowTMP", true)][SerializeField] private TextMeshProUGUI m_nbOfCartTokenText;
        [ShowIf("m_ShowTMP", true)][SerializeField] private TextMeshProUGUI m_accelerationCostText;
        [ShowIf("m_ShowTMP", true)][SerializeField] private TextMeshProUGUI m_accelerationBuyingText;
        [ShowIf("m_ShowTMP", true)][SerializeField] private Button m_accelerationBuyButton;
        [ShowIf("m_ShowTMP", true)][SerializeField] private Button m_accelerationSellButton;
        [ShowIf("m_ShowTMP", true)][SerializeField] private Image m_accelerationFillBar;
        [ShowIf("m_ShowTMP", true)][SerializeField] private TextMeshProUGUI m_maxSpeedCostText;
        [ShowIf("m_ShowTMP", true)][SerializeField] private TextMeshProUGUI m_maxSpeedBuyingText;
        [ShowIf("m_ShowTMP", true)][SerializeField] private Button m_maxSpeedBuyButton;
        [ShowIf("m_ShowTMP", true)][SerializeField] private Button m_maxSpeedSellButton;
        [ShowIf("m_ShowTMP", true)][SerializeField] private Image m_maxSpeedFillBar;
        [ShowIf("m_ShowTMP", true)][SerializeField] private TextMeshProUGUI m_handlingCostText;
        [ShowIf("m_ShowTMP", true)][SerializeField] private TextMeshProUGUI m_handlingBuyingText;
        [ShowIf("m_ShowTMP", true)][SerializeField] private Button m_handlingBuyButton;
        [ShowIf("m_ShowTMP", true)][SerializeField] private Button m_handlingSellButton;
        [ShowIf("m_ShowTMP", true)][SerializeField] private Image m_handlingFillBar;
        [ShowIf("m_ShowTMP", true)][SerializeField] private TextMeshProUGUI m_defenseCostText;
        [ShowIf("m_ShowTMP", true)][SerializeField] private TextMeshProUGUI m_defenseBuyingText;
        [ShowIf("m_ShowTMP", true)][SerializeField] private Button m_defenseBuyButton;
        [ShowIf("m_ShowTMP", true)][SerializeField] private Button m_defenseSellButton;
        [ShowIf("m_ShowTMP", true)][SerializeField] private Image m_defenseFillBar;


        private int m_nbCartToken;
        public int AccelerationStat { get; private set; } = 0;
        public int MaxSpeedStat { get; private set; } = 0;
        public int HandlingStat { get; private set; } = 0;
        public int DefenseStat { get; private set; } = 0;

        private int m_accelerationBuyingCost = 0;
        private int m_maxSpeedBuyingCost = 0;
        private int m_handlingBuyingCost = 0;
        private int m_defenseBuyingCost = 0;

        private int m_accelerationUpgrade = 0;
        private int m_maxSpeedUpgrade = 0;
        private int m_handlingUpgrade = 0;
        private int m_defenseUpgrade = 0;

        private const int MAX_LEVEL = 4;

        private void Awake()
        {
            m_nbCartToken = m_startingCartToken;
        }

        private void Start() // TEST ONLY
        {
            UpdateAll();
        }

        private void OnEnable()
        {
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

            DeselectAllButtons();
        }

        private void UpdateCost()
        {
            if (AccelerationStat < MAX_LEVEL)
                m_accelerationCostText.text = m_upgradeCostByLevel[AccelerationStat].ToString();
            else
                m_accelerationCostText.text = " ";

            if (MaxSpeedStat < MAX_LEVEL)
                m_maxSpeedCostText.text = m_upgradeCostByLevel[MaxSpeedStat].ToString();
            else
                m_maxSpeedCostText.text = " ";

            if (HandlingStat < MAX_LEVEL)
                m_handlingCostText.text = m_upgradeCostByLevel[HandlingStat].ToString();
            else
                m_handlingCostText.text = " ";

            if (DefenseStat < MAX_LEVEL)
                m_defenseCostText.text = m_upgradeCostByLevel[DefenseStat].ToString();
            else
                m_defenseCostText.text = " ";
        }

        private void UpdateFillBars()
        {
            m_accelerationFillBar.fillAmount = (float)AccelerationStat / MAX_LEVEL;
            m_maxSpeedFillBar.fillAmount = (float)MaxSpeedStat / MAX_LEVEL;
            m_handlingFillBar.fillAmount = (float)HandlingStat / MAX_LEVEL;
            m_defenseFillBar.fillAmount = (float)DefenseStat / MAX_LEVEL;
        }

        private void UpdateBuyingCost()
        {
            if (m_accelerationUpgrade == 0)
                m_accelerationBuyingText.text = " ";
            else
                m_accelerationBuyingText.text = "- " + m_accelerationBuyingCost.ToString();

            if (m_maxSpeedUpgrade == 0)
                m_maxSpeedBuyingText.text = " ";
            else
                m_maxSpeedBuyingText.text = "- " + m_maxSpeedBuyingCost.ToString();

            if (m_handlingUpgrade == 0)
                m_handlingBuyingText.text = " ";
            else
                m_handlingBuyingText.text = "- " + m_handlingBuyingCost.ToString();

            if (m_defenseUpgrade == 0)
                m_defenseBuyingText.text = " ";
            else
                m_defenseBuyingText.text = "- " + m_defenseBuyingCost.ToString();

        }

        private void UpdateMoney()
        {
            m_nbOfCartTokenText.text = m_nbCartToken.ToString();
        }

        public void UpdateBuyButton()
        {
            if (AccelerationStat < MAX_LEVEL)
                m_accelerationBuyButton.interactable = m_nbCartToken >= m_upgradeCostByLevel[AccelerationStat];
            else
                m_accelerationBuyButton.interactable = false;

            if (MaxSpeedStat < MAX_LEVEL)
                m_maxSpeedBuyButton.interactable = m_nbCartToken >= m_upgradeCostByLevel[MaxSpeedStat];
            else
                m_maxSpeedBuyButton.interactable = false;

            if (HandlingStat < MAX_LEVEL)
                m_handlingBuyButton.interactable = m_nbCartToken >= m_upgradeCostByLevel[HandlingStat];
            else
                m_handlingBuyButton.interactable = false;

            if (DefenseStat < MAX_LEVEL)
                m_defenseBuyButton.interactable = m_nbCartToken >= m_upgradeCostByLevel[DefenseStat];
            else
                m_defenseBuyButton.interactable = false;

        }

        public void UpdateSellButton()
        {
            m_accelerationSellButton.interactable = m_accelerationUpgrade > 0;
            m_maxSpeedSellButton.interactable = m_maxSpeedUpgrade > 0;
            m_handlingSellButton.interactable = m_handlingUpgrade > 0;
            m_defenseSellButton.interactable = m_defenseUpgrade > 0;
        }

        public void BuyUpgrade(int statsIndex)
        {
            EStats stats = (EStats)statsIndex;

            int cost;
            switch (stats)
            {
                case EStats.acceleration:
                    cost = m_upgradeCostByLevel[AccelerationStat];
                    m_accelerationBuyingCost += cost;
                    m_accelerationUpgrade++;
                    AccelerationStat++;
                    break;
                case EStats.maxSpeed:
                    cost = m_upgradeCostByLevel[MaxSpeedStat];
                    m_maxSpeedBuyingCost += cost;
                    m_maxSpeedUpgrade++;
                    MaxSpeedStat++;
                    break;
                case EStats.handling:
                    cost = m_upgradeCostByLevel[HandlingStat];
                    m_handlingBuyingCost += cost;
                    m_handlingUpgrade++;
                    HandlingStat++;
                    break;
                case EStats.defense:
                    cost = m_upgradeCostByLevel[DefenseStat];
                    m_defenseBuyingCost += cost;
                    m_defenseUpgrade++;
                    DefenseStat++;
                    break;
                default:
                    cost = int.MaxValue;
                    break;
            }

            m_nbCartToken -= cost;
            UpdateAll();
        }

        public void SellUpgrade(int statsIndex)
        {
            EStats stats = (EStats)statsIndex;

            int refund;
            switch (stats)
            {
                case EStats.acceleration:
                    refund = m_upgradeCostByLevel[AccelerationStat - 1];
                    m_accelerationBuyingCost -= refund;
                    m_accelerationUpgrade--;
                    AccelerationStat--;
                    break;
                case EStats.maxSpeed:
                    refund = m_upgradeCostByLevel[MaxSpeedStat - 1];
                    m_maxSpeedBuyingCost -= refund;
                    m_maxSpeedUpgrade--;
                    MaxSpeedStat--;
                    break;
                case EStats.handling:
                    refund = m_upgradeCostByLevel[HandlingStat - 1];
                    m_handlingBuyingCost -= refund;
                    m_handlingUpgrade--;
                    HandlingStat--;
                    break;
                case EStats.defense:
                    refund = m_upgradeCostByLevel[DefenseStat - 1];
                    m_defenseBuyingCost -= refund;
                    m_defenseUpgrade--;
                    DefenseStat--;
                    break;
                default:
                    refund = 0;
                    break;

            }      

            m_nbCartToken += refund;
            UpdateAll();
        }

        private void DeselectAllButtons()
        {
            GameObject myEventSystem = GameObject.Find("EventSystem");
            myEventSystem.GetComponent<UnityEngine.EventSystems.EventSystem>().SetSelectedGameObject(null);
        }

        public void AcceptUpgrade()
        {
            m_accelerationBuyingCost = 0;
            m_maxSpeedBuyingCost = 0;
            m_handlingBuyingCost = 0;
            m_defenseBuyingCost = 0;

            m_accelerationUpgrade = 0;
            m_maxSpeedUpgrade = 0;
            m_handlingUpgrade = 0;
            m_defenseUpgrade = 0;
            UpdateAll();
        }

        public void Restart()
        {
            int accelerationUpgrade = m_accelerationUpgrade;
            for (int i = 0; i < accelerationUpgrade; i++)
            {
                SellUpgrade((int)EStats.acceleration);
            }
            int maxSpeedUpgrade = m_maxSpeedUpgrade;
            for (int i = 0; i < maxSpeedUpgrade; i++)
            {
                SellUpgrade((int)EStats.maxSpeed);
            }
            int handlingUpgrade = m_handlingUpgrade;
            for (int i = 0; i < handlingUpgrade; i++)
            {
                SellUpgrade((int)EStats.handling);
            }
            int defenseUpgrade = m_defenseUpgrade;
            for (int i = 0; i < defenseUpgrade; i++)
            {
                SellUpgrade((int)EStats.defense);
            }

            UpdateAll();
        }

        public void AddMoney(int amount)
        {
            m_nbCartToken += amount;
        }
    }

}

