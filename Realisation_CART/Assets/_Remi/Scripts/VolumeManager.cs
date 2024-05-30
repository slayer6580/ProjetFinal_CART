using UnityEngine;
using UnityEngine.Audio;

namespace AudioControl
{
    public class VolumeManager : MonoBehaviour
    {
        [SerializeField] private AudioMixer m_audioMixer;
        [SerializeField] private float m_minVolume = -40;
        [SerializeField] private float m_maxVolume = 20;

        private void Awake()
        {
            float master = Mathf.Lerp(m_minVolume, m_maxVolume, PlayerPrefs.GetFloat("MasterVolume", 1));
            float music = Mathf.Lerp(m_minVolume, m_maxVolume, PlayerPrefs.GetFloat("MusicVolume", 1));
            float sfx = Mathf.Lerp(m_minVolume, m_maxVolume, PlayerPrefs.GetFloat("SoundFXVolume", 1));

            m_audioMixer.SetFloat("MasterVolume", master);
            m_audioMixer.SetFloat("MusicVolume", music);
            m_audioMixer.SetFloat("SoundFXVolume", sfx);
        }

        public void SetMasterVolume(float volume)
        {
            PlayerPrefs.SetFloat("MasterVolume", volume);
            float master = Mathf.Lerp(m_minVolume, m_maxVolume, volume);
            m_audioMixer.SetFloat("MasterVolume", master);
        }

        public void SetMusicVolume(float volume)
        {
            PlayerPrefs.SetFloat("MusicVolume", volume);
            float music = Mathf.Lerp(m_minVolume, m_maxVolume, volume);
            m_audioMixer.SetFloat("MusicVolume", music);
        }
    }
}
