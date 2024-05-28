using BoxSystem;
using SavingSystem;
using StatsSystem;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace DiscountDelirium
{
    public class ScoreUI : MonoBehaviour
    {
        [SerializeField] private GameObject m_panelUI;
		[SerializeField] private GameObject m_panelUpgrade;
		[SerializeField] private TextMeshProUGUI m_textTotalScore;
		[SerializeField] private TextMeshProUGUI m_textScore;
        [SerializeField] private TextMeshProUGUI m_textNbItems;
        [SerializeField] private TextMeshProUGUI m_textNbCartoken;
        [SerializeField] private UpgradeManager m_upgradeManager;

        [Header("For Last Level Only")]
        [SerializeField] private LeaderboardManager m_leaderboardManager;

        [Header("Items scroll system")]
        [SerializeField] private TowerBoxSystem m_towerBoxSystem;
        [SerializeField] private TextMeshProUGUI m_textPrefab;
        [SerializeField] private Transform m_content;

        private List<string> m_allNames = new List<string>();
        private List<int> m_allQuantities = new List<int>();
        private List<int> m_allCost = new List<int>();

        public void EnableUI(bool show)
        {
            m_panelUI.SetActive(show);
        }

        public void ShowScore(int totalScore, int nbItem, int nbCartoken, int score)
        {
            m_textScore.text = "Your score: " + score.ToString();
            m_textTotalScore.text = "Your total score: " + totalScore.ToString();  
            m_textNbItems.text = "Number of items: " + nbItem.ToString();
			m_textNbCartoken.text = "Number of cartokens: " + nbCartoken.ToString();
        }

        public void NextToUpgrade()
        {
            m_panelUI.SetActive(false);
            m_panelUpgrade.SetActive(true);
            m_upgradeManager.TeleportPlayerToUpgradeScene();
		}

        public void GoToLeaderboard()
        {
            m_leaderboardManager.OpenLeaderboardEndGame();
        }

        public void AddItemsInformationsInGameOverPanel()
        {
            int allchildsInContext = m_content.childCount;

            // Clear content
            for (int i = allchildsInContext; i > 0; i--)
            {
                Destroy(m_content.GetChild(i - 1).gameObject);
            }

            List<Box> boxList = m_towerBoxSystem.GetAllBoxes();

            // Search for item names and quantity
            foreach (Box box in boxList)
            {
                List<Box.ItemInBox> allItems = box.GetItemsInBox();

                foreach (Box.ItemInBox item in allItems)
                {
                    ItemData itemData = item.m_item.GetComponent<Item>().m_data;
                    string name = itemData.m_name;

                    if (m_allNames.Contains(name))
                    {
                        int index = m_allNames.IndexOf(name);
                        m_allQuantities[index]++;
                    }
                    else
                    {
                        m_allNames.Add(name);
                        m_allQuantities.Add(1);
                        m_allCost.Add(itemData.m_cost);
                    }
                }
            }

            // Add infos in strings
            for (int i = 0; i < m_allNames.Count; i++)
            {
                int totalCost = m_allQuantities[i] * m_allCost[i];
                TextMeshProUGUI infoText = Instantiate(m_textPrefab);
                infoText.text = m_allNames[i] + " x " + m_allQuantities[i].ToString() + ":  " + totalCost;
                infoText.transform.SetParent(m_content);
            }

            
        }
    }
}
