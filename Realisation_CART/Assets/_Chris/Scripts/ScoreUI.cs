using TMPro;
using UnityEngine;

namespace DiscountDelirium
{
    public class ScoreUI : MonoBehaviour
    {
        [SerializeField] private GameObject m_panelUI;
        [SerializeField] private TextMeshProUGUI m_textScore;

        public void EnableUI(bool show)
        {
            m_panelUI.SetActive(show);
        }

        public void ShowScore(float score) 
        {
            m_textScore.text = "Your score: " + score.ToString();
        }
    }
}
