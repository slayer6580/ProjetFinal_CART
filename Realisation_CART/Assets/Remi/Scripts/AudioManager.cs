using System.Collections.Generic;
using UnityEngine;

namespace Manager
{
    public class AudioManager : MonoBehaviour
    {
        private AudioSource AudioManagerSource { get; set;}

        [SerializeField] private AudioClip[] m_soundsPool = new AudioClip[2];

        private Dictionary<SoundType, AudioClip[]> m_soundsByType = new Dictionary<SoundType, AudioClip[]>();

        public static AudioManager _Instance { get; private set; }

        public enum SoundSetting
        {
            Play,
            Stop,
            Count
        }

        public enum SoundType
        {
            Client,
            Environment,
            GameEvent,
            UI,
            Count
        }

        public enum SoundName
        {
            Footstep,
            CartRolling,
            CartBanging,
            Count
        }

        public enum InGameAudioSource
        {
            Cart,
            Count
        }

        private void Awake()
        {
            if (_Instance != null)
            {
                Destroy(gameObject);
                return;
            }

            _Instance = this;
            DontDestroyOnLoad(gameObject);
        }

        private void Start()
        {
            AudioManagerSource = GetComponent<AudioSource>();
            RefreshSoundPool();
        }

        public void RefreshSoundPool()
        {
            for (int i = 0; i < m_soundsPool.Length; i++)
            {
                m_soundsPool[i] = null;
            }

            LoadClientSoundsToThePool();
        }

        /// <summary> Loads the sounds from Sounds/Client into the array, the sounds include Player and clients footsteps, cart rolling and banging.</summary>
        private void LoadClientSoundsToThePool()
        {
            var playerClips = Resources.LoadAll<AudioClip>("Sounds/Client");
            Debug.Log("Player sounds loaded: " + playerClips.Length);

            m_soundsByType[SoundType.Client] = playerClips;

            for (int i = 0; i < m_soundsPool.Length && i < playerClips.Length; i++)
            {
                m_soundsPool[i] = playerClips[i];
                Debug.Log("Player sound loaded: " + playerClips[i].name);
            }
        }

        public void PlaySoundByType(SoundType soundType)
        {
            if (m_soundsByType.ContainsKey(soundType))
            {
                var clips = m_soundsByType[soundType];
                var clip = clips[Random.Range(0, clips.Length)];
                Debug.Log("Playing sound: " + clip.name);
            }
            else
            {
                Debug.LogError("No sounds of type: " + soundType);
            }
        }

        public Dictionary<SoundType, AudioClip[]> GetDictionary()
        {
            return m_soundsByType;
        }

        public void PlaySoundInAudioSystemSource(AudioClip selectedClip)
        {
            // Keep this line to play sounds outside of play mode
            if (AudioManagerSource == null) AudioManagerSource = GetComponent<AudioSource>(); 

            AudioManagerSource.clip = selectedClip;
            AudioManagerSource.Play();
        }

        public void SetSoundByTypeToSource(SoundSetting setSound, SoundType soundType, SoundName soundName, InGameAudioSource audioSource)
        {
            if (m_soundsByType.ContainsKey(soundType))
            {
                var clips = m_soundsByType[soundType];
                var clip = clips[(int)soundName];
                Debug.Log("Playing sound: " + clip.name);
                GetAudioSource(audioSource).clip = clip;
                GetAudioSource(audioSource).Play();
            }
            else
            {
                Debug.LogError("No sounds of type: " + soundType);
            }
        }

        private AudioSource GetAudioSource(InGameAudioSource audioSource)
        {
            if (audioSource == InGameAudioSource.Cart)
            {
                return AudioManagerSource;
            }
            else
            {
                return null;
            }
        }
    }
}
