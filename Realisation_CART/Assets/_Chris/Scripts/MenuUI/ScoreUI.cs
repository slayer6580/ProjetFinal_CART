using StatsSystem;
using TMPro;
using UnityEngine;

namespace DiscountDelirium
{
    public class ScoreUI : MonoBehaviour
    {
        [SerializeField] private GameObject m_panelUI;
		[SerializeField] private GameObject m_panelUpgrade;
		[SerializeField] private TextMeshProUGUI m_textScore;
        [SerializeField] private TextMeshProUGUI m_textNbItems;
        [SerializeField] private TextMeshProUGUI m_textNbCartoken;
        [SerializeField] private UpgradeManager m_upgradeManager;
     

        public void EnableUI(bool show)
        {
            m_panelUI.SetActive(show);
        }

        public void ShowScore(int score, int nbItem, int nbCartoken)
        {
            m_textScore.text = "Your score: " + score.ToString();
            m_textNbItems.text = "Number of items: " + nbItem.ToString();
			m_textNbCartoken.text = "Number of cartokens: " + nbCartoken.ToString();
        }

        public void NextToUpgrade()
        {
            m_panelUI.SetActive(false);
            m_panelUpgrade.SetActive(true);
            m_upgradeManager.TeleportPlayerToUpgradeScene();

		}
    }
}
