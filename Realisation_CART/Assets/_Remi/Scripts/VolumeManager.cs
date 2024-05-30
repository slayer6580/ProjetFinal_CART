using UnityEngine;
using UnityEngine.Audio;

namespace AudioControl
{
    public class VolumeManager : MonoBehaviour
    {
        [SerializeField] private AudioMixer m_audioMixer;
        private const float MIN_VOLUME = -60;
        private const float MAX_VOLUME = 0;

       

        private void Awake()
        {
            SetVolume();
        }

        public void SetVolume()
        {
            float master = Mathf.Lerp(MIN_VOLUME, MAX_VOLUME, PlayerPrefs.GetFloat("MasterVolume", 0));
            float music = Mathf.Lerp(MIN_VOLUME, MAX_VOLUME, PlayerPrefs.GetFloat("MusicVolume", 0));
            float sfx = Mathf.Lerp(MIN_VOLUME, MAX_VOLUME, PlayerPrefs.GetFloat("SoundFXVolume", 0));
            float ui = Mathf.Lerp(MIN_VOLUME, MAX_VOLUME, PlayerPrefs.GetFloat("UIFXVolume", 0));

            m_audioMixer.SetFloat("MasterVolume", master);
            m_audioMixer.SetFloat("MusicVolume", music);
            m_audioMixer.SetFloat("SoundFXVolume", sfx);
            m_audioMixer.SetFloat("UIFXVolume", ui);
        }

    }
}
