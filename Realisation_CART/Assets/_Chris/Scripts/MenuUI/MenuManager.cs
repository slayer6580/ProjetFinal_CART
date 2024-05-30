using Manager;
using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using static Manager.AudioManager;

namespace DiscountDelirium
{
    public class MenuManager : MonoBehaviour
    {
        [Header("Menus")]
        [SerializeField] private GameObject m_mainMenu;
        [SerializeField] private GameObject m_optionsMenu;
        [SerializeField] private GameObject m_creditsPanel;

        [Header("Virtual Cursor")]
        [SerializeField] private GameObject m_cursor;

        [Header("Volume Sliders")]
        [SerializeField] private Slider m_masterSlider;
        [SerializeField] private Slider m_musicSlider;
        [SerializeField] private Slider m_soundFXSlider;

        [field: Header("Main Menu Scene")]
        [field: SerializeField] public EMusic MainMenuMusic { get; private set; } = EMusic.ThemeMusic;
        //private int m_audioSourceIndex;

        private void Awake()
        {
            PauseState.OnPause += OpenMainMenu;
            PauseState.OnResume += CloseMainMenu;
            EndGameState.OnEndGame += ShowCursor;
        }

        private void Start()
        {
            m_masterSlider.value = PlayerPrefs.GetFloat("MasterVolume", 1.0f);
            m_musicSlider.value = PlayerPrefs.GetFloat("MusicVolume", 1.0f);
            if (m_soundFXSlider == null) Debug.LogError("No sound slider found!");
            m_soundFXSlider.value = PlayerPrefs.GetFloat("SoundFXVolume", 1.0f);

            _AudioManager.SetMainMenuMusic(MainMenuMusic);
            //m_audioSourceIndex = _AudioManager.StartCurrentSceneMusic();
        }

        public void StartGame()
        {
            PlayerPrefs.SetInt("Score", 0);
            SceneManager.LoadScene("Tutorial");
        }

        public void OpenMainMenu() 
        {
            _AudioManager.PlayUIClickSound();

            if (m_mainMenu != null) 
            {
                m_mainMenu.SetActive(true);
            }
            if (m_cursor != null)
            {
                m_cursor.SetActive(true);
                return;
            }
        }

        public void CloseMainMenu()
        {
            if (m_mainMenu != null)
            {
                m_mainMenu.SetActive(false);
                m_optionsMenu.SetActive(false);
                m_cursor.SetActive(false);
                _AudioManager.StopUIScrollSound();
            }
        }

        public void OptionsMenu(bool active) 
        {
            m_mainMenu.SetActive(!active);
            m_optionsMenu.SetActive(active);
        }

        public void QuitGame() 
        {
            Debug.Log("QuitGame");
            Application.Quit();
        }

        public void BackToMainMenu() 
        {
            Time.timeScale = 1.0f;
            SceneManager.LoadScene("MainMenu");
        }

        public void ShowCursor() 
        {
            if (m_cursor != null) 
            {
                m_cursor.SetActive(true);
                return;
            }
            Debug.LogWarning("No cursor");
            
        }

        public void SetMasterVolume()
        {
            //Debug.Log("Master Volume: " + m_masterSlider.value);
            PlayerPrefs.SetFloat("MasterVolume", m_masterSlider.value);
            _AudioManager.ModifyAudio(-1, EAudioModification.MasterVolume, m_masterSlider.value);
            _AudioManager.ModifyAudio(-1, EAudioModification.UIVolume, m_masterSlider.value);
        }

        public void SetMusicVolume()
        {
            //Debug.Log("Music Volume: " + m_musicSlider.value);
            PlayerPrefs.SetFloat("MusicVolume", m_musicSlider.value);
            _AudioManager.ModifyAudio(-1, EAudioModification.MusicVolume, m_musicSlider.value);
        }

        public void SetSoundVolume()
        {
            if (m_soundFXSlider == null) Debug.LogError("No sound slider found!");
            //Debug.Log("Sound slide value: " + m_soundFXSlider.value);
            PlayerPrefs.SetFloat("SoundFXVolume", m_soundFXSlider.value);
            for (int i = 0; i < _AudioManager.GetAudioBox().Count; i++)
            {
                _AudioManager.ModifyAudio(i, EAudioModification.SoundVolume, m_soundFXSlider.value);
            }

            _AudioManager.ModifyAudio(-1, EAudioModification.UIVolume, m_soundFXSlider.value);
        }

        public void ShowCredits(bool active) 
        {
            m_mainMenu.SetActive(!active);
            m_creditsPanel.SetActive(active);
        }
    }
}
