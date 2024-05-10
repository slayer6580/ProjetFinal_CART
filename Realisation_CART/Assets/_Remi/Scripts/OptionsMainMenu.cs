using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

namespace DiscountDelirium
{
    public class OptionsMainMenu : MonoBehaviour
    {
        [SerializeField] private AudioMixer m_audioMixer;

        [SerializeField] private Slider m_masterVolumeSlider;
        [SerializeField] private Slider m_musicVolumeSlider;
        [SerializeField] private Slider m_sfxVolumeSlider;

        [SerializeField] private float m_minVolume = -40;
        [SerializeField] private float m_maxVolume = 20;

        private void Awake()
        {
            m_masterVolumeSlider.value = PlayerPrefs.GetFloat("MasterVolume", 1);
            m_musicVolumeSlider.value = PlayerPrefs.GetFloat("MusicVolume", 1);
            m_sfxVolumeSlider.value = PlayerPrefs.GetFloat("SoundFXVolume", 1);
        }

        public void SetMasterVolume()
        {
            PlayerPrefs.SetFloat("MasterVolume", m_masterVolumeSlider.value);
            float master = Mathf.Lerp(m_minVolume, m_maxVolume, m_masterVolumeSlider.value);
            m_audioMixer.SetFloat("MasterVolume", master);
        }

        public void SetMusicVolume()
        {
            PlayerPrefs.SetFloat("MusicVolume", m_musicVolumeSlider.value);
            float music = Mathf.Lerp(m_minVolume, m_maxVolume, m_musicVolumeSlider.value);
            m_audioMixer.SetFloat("MusicVolume", music);
        }

        public void SetSFXVolume()
        {
            PlayerPrefs.SetFloat("SoundFXVolume", m_sfxVolumeSlider.value);
            float sfx = Mathf.Lerp(m_minVolume, m_maxVolume, m_sfxVolumeSlider.value);
            m_audioMixer.SetFloat("SoundFXVolume", sfx);
        }

    }
}
