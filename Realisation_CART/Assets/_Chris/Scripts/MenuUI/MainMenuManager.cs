using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace DiscountDelirium
{
    public class MainMenuManager : MonoBehaviour
    {
        [Header("Menus")]
        [SerializeField] private GameObject m_mainMenu;
        [SerializeField] private GameObject m_optionsMenu;

        [Header("Volume Sliders")]
        [SerializeField] private Slider m_musicSlider;
        [SerializeField] private Slider m_soundSlider;

        [SerializeField] private float m_musicVolume;
        [SerializeField] private float m_soundVolume;

        public void StartGame()
        {
            Debug.Log("StartGame");
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
            Debug.Log("MainMenu");
        }
    }
}
