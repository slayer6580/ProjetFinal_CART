using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Manager
{
    public class AudioManager : MonoBehaviour
    {
        [field: Header("Put all audio sounds here, read the Tooltip for the sound order")]
        [Tooltip("Footstep\nCartRolling\nCartBanging")]
        [SerializeField] private AudioClip[] m_soundsPool = new AudioClip[2];

        [SerializeField] private GameObject m_audioBoxPrefab;

        [Header("Put all AudioBox here")]
        [SerializeField] private List<AudioBox> m_audioBox;

        private AudioSource AudioManagerSource { get; set;}

        private Dictionary<ESoundType, AudioClip[]> m_soundsByType = new Dictionary<ESoundType, AudioClip[]>();

        public static AudioManager _AudioManager { get; private set; }

        [SerializeField] private int m_numberOfAudioBox = 10;

        public enum ESound
        {
            CartCollision,
            CartRolling,
            Step01,
            Step02,
            Step03,
            Step04,
            GrabItem,
            DriftBegin,
            DriftLoop,
            CashRegister,
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

        private void Awake()
        {
            if (_AudioManager != null)
            {
                Debug.LogWarning("AudioManager already exists.");
                Destroy(gameObject);
                return;
            }

            _AudioManager = this;
            DontDestroyOnLoad(gameObject);

            for (int i = 0; i < m_numberOfAudioBox; i++)
            {
                GameObject audioBox = Instantiate(m_audioBoxPrefab, transform);
                m_audioBox.Add(audioBox.GetComponent<AudioBox>());
                audioBox.transform.SetParent(transform);
            }
        }

        private void Start()
        {
            AudioManagerSource = GetComponent<AudioSource>();
            //RefreshSoundPool();
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


        public void ModifySound(int index, ESoundModification modif, float value)
        {
            AudioSource audioSource = m_audioBox[index]._AudioSource;

            if (modif == ESoundModification.Pitch)
            {
                audioSource.pitch = value;
            }
            else if (modif == ESoundModification.Volume)
            {
                audioSource.volume = value;
            }
        }

        /// <summary> Tout les clients va jouer un son une fois </summary>
        public void PlaySoundEffectsOneShot(ESound sound, Vector3 newPosition, float volume = 1)
        {
            AudioBox audiobox = FindAValidAudioBox();
            
            if (audiobox == null)
                return;

            AudioClip clip = m_soundsPool[(int)sound];
            audiobox.m_isPlaying = true;
            audiobox.GetComponent<AudioSource>().volume = volume;

			MoveAudioBox(audiobox, newPosition);
            PlayClipOneShot(audiobox, sound);
            StartCoroutine(ReActivateAudioBox(audiobox, clip));
        }

        /// <summary> To play a sound one shot on a transform </summary>
        public int PlaySoundEffectsLoopOnTransform(ESound sound, Transform parent)
        {

            if (parent == null)
            {
                parent = transform;
            }

            AudioBox audiobox = FindAValidAudioBox();
            print("Pitch: " + audiobox.GetComponent<AudioSource>().pitch);

			int index = m_audioBox.IndexOf(audiobox);

            if (audiobox == null)
                return -1;


            audiobox.transform.SetParent(parent);
            audiobox.transform.localPosition = Vector3.zero;
            AudioClip clip = m_soundsPool[(int)sound];
            audiobox.m_isPlaying = true;
            MoveAudioBox(audiobox, Vector3.zero);
            PlayClipLoop(audiobox, sound);
            return index;
        }

        /// <summary> To play a sound one shot at a position </summary>
        public int PlaySoundEffectsLoop(ESound sound,  Vector3 newPosition)
        {
            AudioBox audiobox = FindAValidAudioBox();
            int index = m_audioBox.IndexOf(audiobox);

            if (audiobox == null)
                return -1;

            AudioClip clip = m_soundsPool[(int)sound];
            audiobox.m_isPlaying = true;
            MoveAudioBox(audiobox, newPosition);
            PlayClipLoop(audiobox, sound);
            return index;
        }

        /// <summary> To stop a sound effect </summary>
        public void StopSoundEffectsLoop(int index)
        {
            AudioBox audiobox = m_audioBox[index];
            AudioSource audioSource = audiobox.GetComponent<AudioSource>();
            audioSource.pitch = 1;
            audioSource.volume = 1;
			audioSource.Stop();
            audiobox.m_isPlaying = false;

            if (audiobox.transform.parent != transform)
            {
                audiobox.transform.SetParent(transform);
            }
        }

        /// <summary> To find an audioBox in the audiobox pool </summary>
        private AudioBox FindAValidAudioBox()
        {
            foreach (AudioBox audioBox in m_audioBox)
            {
                if (!audioBox.m_isPlaying)
                {
                    return audioBox;
                }
            }

            return null;
        }

        /// <summary> To reactivate an audioBox for the duration of the Audiolip </summary>
        private IEnumerator ReActivateAudioBox(AudioBox audiobox, AudioClip clip)
        {
            yield return new WaitForSeconds(clip.length);
            audiobox.m_isPlaying = false;
        }

        /// <summary> To move an audioBox at a position </summary>
        private void MoveAudioBox(AudioBox audioBox, Vector3 newPosition)
        {
            audioBox.transform.position = newPosition;
        }

        /// <summary> To make an audioBox play an audioclip one shot</summary>
        private void PlayClipOneShot(AudioBox audiobox, ESound sound)
        {
            AudioSource audioSource = audiobox._AudioSource;
            audioSource.loop = false;
            audioSource.clip = m_soundsPool[(int)sound];
            audioSource.Play();
        }

        /// <summary> To make an audiobox play tan audioclip in loop </summary>
        private void PlayClipLoop(AudioBox audiobox, ESound sound)
        {
            AudioSource audioSource = audiobox._AudioSource;
            audioSource.loop = true;
            audioSource.clip = m_soundsPool[(int)sound];
            audioSource.Play();
        }

        /// <summary> Pour désactiver la musique du jeu </summary>
        private void DesactivateMusic()
        {
            GetComponent<AudioSource>().Stop();
        }
    }
}
