using System.Collections.Generic;
using UnityEngine;

namespace Manager
{
    public class AudioSystem : MonoBehaviour
    {
        private AudioSource AudioSystemSource { get; set;}
        [field: SerializeField] private AudioSource CartAudioSource { get; set; } = null;

        [SerializeField] private AudioClip[] m_soundsPool = new AudioClip[2];

        private Dictionary<SoundType, AudioClip[]> m_soundsByType = new Dictionary<SoundType, AudioClip[]>();

        public static AudioSystem _Instance { get; private set; }

        public enum SoundType
        {
            Client,
            Environment,
            GameEvent,
            UI,
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
            AudioSystemSource = GetComponent<AudioSource>();
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
            if (AudioSystemSource == null) AudioSystemSource = GetComponent<AudioSource>(); 

            AudioSystemSource.clip = selectedClip;
            AudioSystemSource.Play();
        }

        public void PlaySoundOnGameObject(AudioClip selectedClip, GameObject gameObject)
        {
            AudioSource audioSource = gameObject.GetComponent<AudioSource>();
            if (audioSource == null)
            {
                audioSource = gameObject.AddComponent<AudioSource>();
            }

            audioSource.clip = selectedClip;
            audioSource.Play();
        }
    }
}
