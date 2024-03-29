using TMPro;
using UnityEngine;

namespace DiscountDelirium
{
    public class ScoreUI : MonoBehaviour
    {
        [SerializeField] private GameObject m_panelUI;
        [SerializeField] private TextMeshProUGUI m_textScore;
        [SerializeField] private TextMeshProUGUI m_textNbItems;

        public void EnableUI(bool show)
        {
            m_panelUI.SetActive(show);
        }

        public void ShowScore(int score, int nbItem)
        {
            m_textScore.text = "Your score: " + score.ToString();
            m_textNbItems.text = "Number of items: " + nbItem.ToString();
        }
    }
}
