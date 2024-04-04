using System.Collections.Generic;
using UnityEngine;

namespace Manager
{
    public class AudioManager : MonoBehaviour
    {
        [field: SerializeField] private AudioSource CartAudioSource { get; set;}

        [field: Header("Put all audio sounds here, read the Tooltip for the sound order")]
        [Tooltip("Footstep\nCartRolling\nCartBanging")]
        [SerializeField] private AudioClip[] m_soundsPool = new AudioClip[2];

        [Header("Put all AudioBox here")]
        [SerializeField] private List<AudioBox> m_audioBox;

        private AudioSource AudioManagerSource { get; set;}

        private Dictionary<ESoundType, AudioClip[]> m_soundsByType = new Dictionary<ESoundType, AudioClip[]>();

        public static AudioManager _AudioManager { get; private set; }
        public enum ESoundName
        {
            Footstep,
            CartRolling,
            CartBanging,
        }

        public enum ESoundSetting
        {
            Play,
            Stop,
            Count
        }

        public enum ESoundModification
        {
            Pitch,
            Volume,
            Count
        }

        public enum ESoundType
        {
            Client,
            Environment,
            GameEvent,
            UI,
            Count
        }

        public enum EInGameAudioSource
        {
            AudioManager,
            Cart,
            Count
        }

        private void Awake()
        {
            if (_AudioManager != null)
            {
                Destroy(gameObject);
                return;
            }

            _AudioManager = this;
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

            m_soundsByType[ESoundType.Client] = playerClips;

            for (int i = 0; i < m_soundsPool.Length && i < playerClips.Length; i++)
            {
                m_soundsPool[i] = playerClips[i];
                Debug.Log("Player sound loaded: " + playerClips[i].name);
            }
        }

        public void PlaySoundByType(ESoundType soundType)
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

        public Dictionary<ESoundType, AudioClip[]> GetDictionary()
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

        public void SetSoundByTypeToSource(ESoundSetting setSound, ESoundType soundType, ESoundName soundName, EInGameAudioSource audioSource)
        {
            if (setSound == ESoundSetting.Stop)
            {
                GetAudioSource(audioSource).Stop();
                return;
            }

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

        public void ModifySoundBySource(EInGameAudioSource audioSource, ESoundModification modif, float value)
        {
            if (modif == ESoundModification.Pitch)
            {
                GetAudioSource(audioSource).pitch = value;
            }
            else if (modif == ESoundModification.Volume)
            {
                GetAudioSource(audioSource).volume = value;
            }
        }

        private AudioSource GetAudioSource(EInGameAudioSource audioSource)
        {
            if (audioSource == EInGameAudioSource.AudioManager)
            {
                return AudioManagerSource;
            }
            if (audioSource == EInGameAudioSource.Cart)
            {
                return CartAudioSource;
            }
            else
            {
                return null;
            }
        }
    }
}
