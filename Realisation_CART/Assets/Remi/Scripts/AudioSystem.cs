using System.Collections.Generic;
using UnityEngine;

namespace Manager
{
    public class AudioSystem : MonoBehaviour
    {
        [SerializeField] private AudioClip[] m_soundsPool = new AudioClip[2];

        private Dictionary<SoundType, AudioClip[]> m_soundsByType = new Dictionary<SoundType, AudioClip[]>();

        private AudioSource _AudioSource { get; set;}

        public static AudioSystem _Instance { get; private set; }

        public enum SoundType
        {
            Player,
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
            _AudioSource = GetComponent<AudioSource>();
            RefreshSoundPool();
        }

        public void RefreshSoundPool()
        {
            for (int i = 0; i < m_soundsPool.Length; i++)
            {
                m_soundsPool[i] = null;
            }

            LoadPlayerSoundToThePool();
        }

        private void LoadPlayerSoundToThePool()
        {
            var playerClips = Resources.LoadAll<AudioClip>("Sounds/Cart");
            Debug.Log("Player sounds loaded: " + playerClips.Length);

            m_soundsByType[SoundType.Player] = playerClips;

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

        internal void PlaySound(AudioClip selectedClip)
        {
            Debug.Log("Playing sound: " + selectedClip.name);

            // Keep this line to play sounds outside of play mode
            if (_AudioSource == null) _AudioSource = GetComponent<AudioSource>(); 

            _AudioSource.clip = selectedClip;
            _AudioSource.Play();

        }
    }
}
