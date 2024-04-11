using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace DiscountDelirium
{
    public class MenuManager : MonoBehaviour
    {
        [Header("Menus")]
        [SerializeField] private GameObject m_mainMenu;
        [SerializeField] private GameObject m_optionsMenu;

        [Header("Volume Sliders")]
        [SerializeField] private Slider m_musicSlider;
        [SerializeField] private Slider m_soundSlider;

        [SerializeField] private float m_musicVolume;
        [SerializeField] private float m_soundVolume;

        [Header("Virtual Cursor")]
        [SerializeField] private GameObject m_cursor;

        private void Awake()
        {
            PauseState.OnPause += OpenMainMenu;
            PauseState.OnResume += CloseMainMenu;
        }

        public void StartGame()
        {
            SceneManager.LoadScene("Main");
        }

        public void OpenMainMenu() 
        {
            m_mainMenu.SetActive(true);
            m_cursor.SetActive(true);
        }

        public void CloseMainMenu()
        {
            m_mainMenu.SetActive(false);
            m_optionsMenu.SetActive(false);
            m_cursor.SetActive(false);
        }

        public void OptionsMenu(bool active) 
        {
            m_mainMenu.SetActive(!active);
            m_optionsMenu.SetActive(active);
        }

        public void SetVolume() 
        {
            m_musicVolume = m_musicSlider.value;
            m_soundVolume = m_soundSlider.value;
        }

        public void QuitGame() 
        {
            Debug.Log("QuitGame");
            Application.Quit();
        }

        public void BackToMainMenu() 
        {
            SceneManager.LoadScene("MainMenu");
        }
    }
}
