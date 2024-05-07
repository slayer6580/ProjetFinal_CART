using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace DiscountDelirium
{
    public class TutorialManager : MonoBehaviour
    {
        [SerializeField] private List<GameObject> m_sections = new List<GameObject>();
        [SerializeField] private List<GameObject> m_pages = new List<GameObject>();
        [SerializeField] private GameObject m_loadingScreen;
        [SerializeField] private Color32 m_sectionSelectedColor;
        [SerializeField] private Color32 m_sectionNotSelectedColor;
        [SerializeField] private TextMeshProUGUI m_AButtonText;
        [SerializeField] private GameObject m_backPanel;
        private int m_currentPage = 0;
        private int m_nbOfPages = 0;

        private void Awake()
        {
            m_nbOfPages = m_pages.Count;    
        }

        private void Start()
        {
            UpdatePage();
        }

        private void UpdatePage()
        {
            if ( (m_currentPage == 0))            
                m_backPanel.SetActive(false);            
            else
                m_backPanel.SetActive(true);

            if (m_currentPage == m_pages.Count - 1)
                m_AButtonText.text = "Start Game";
            else
                m_AButtonText.text = "Next Page";

            for (int i = 0; i < m_nbOfPages; i++)
            {
                if (i == m_currentPage)
                {
                    m_pages[i].SetActive(true);
                    m_sections[i].GetComponent<Image>().color = m_sectionSelectedColor;
                    continue;
                }

                m_pages[i].SetActive(false);
                m_sections[i].GetComponent<Image>().color = m_sectionNotSelectedColor;
            }
        }

        private void TutorialFinish()
        {
            m_loadingScreen.SetActive(true);
            SceneManager.LoadScene("Main");
        }

        public void OnAccept()
        {
            m_currentPage++;

            if (m_currentPage == m_pages.Count)
            {
                TutorialFinish();
                return;
            }

            UpdatePage();
        }

        public void OnDecline()
        {
            if (m_currentPage == 0)
                return;

            m_currentPage--;

            UpdatePage();
        }
    }
}
