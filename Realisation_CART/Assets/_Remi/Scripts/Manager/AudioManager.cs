using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Manager
{
    public class AudioManager : MonoBehaviour
    {
        [field: Header("Put all audio sounds here, read the Tooltip for the sound order")]
        [Tooltip("CartCollision\nCartRolling\nStep01\nStep02\nStep03\nStep04\nGrabItem\nDriftBegin\nDriftLoop\nCashRegister")]
        [SerializeField] private AudioClip[] m_soundsPool = new AudioClip[2];

        [SerializeField] private GameObject m_audioBoxPrefab;

        [Header("Put all AudioBox here")]
        [SerializeField] private List<AudioBox> m_audioBox;

        private AudioSource AudioManagerSource { get; set;}

        private Dictionary<ESoundType, AudioClip[]> m_soundsByType = new Dictionary<ESoundType, AudioClip[]>();

        public static AudioManager _AudioManager { get; private set; }

        [SerializeField] private int m_numberOfAudioBox = 10;

        /// <summary> This enum is used to store all the sounds in the game </summary>
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

        /// <summary> This enum is used to store all the sound modifications in the game </summary>
        public enum ESoundModification
        {
            Pitch,
            Volume,
            Count
        }

        /// <summary> This enum is used to store all the sound types in the game </summary>
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
        }

        /// <summary> Modify the pitch or volume of a sound </summary>
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

        /// <summary> Play a sound effect one shot </summary>
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

        /// <summary> Play a sound one shot on a transform </summary>
        public int PlaySoundEffectsLoopOnTransform(ESound sound, Transform parent)
        {

            if (parent == null)
            {
                parent = transform;
            }

            AudioBox audiobox = FindAValidAudioBox();

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

        /// <summary> Play a sound one shot at a position </summary>
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

        /// <summary> Stop a sound effect </summary>
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

        /// <summary> Find an audioBox in the audiobox pool </summary>
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

        /// <summary> Reactivate an audioBox for the duration of the Audiolip </summary>
        private IEnumerator ReActivateAudioBox(AudioBox audiobox, AudioClip clip)
        {
            yield return new WaitForSeconds(clip.length);
            audiobox.m_isPlaying = false;
        }

        /// <summary> Move an audioBox at a position </summary>
        private void MoveAudioBox(AudioBox audioBox, Vector3 newPosition)
        {
            audioBox.transform.position = newPosition;
        }

        /// <summary> Make an audioBox play an audioclip one shot</summary>
        private void PlayClipOneShot(AudioBox audiobox, ESound sound)
        {
            AudioSource audioSource = audiobox._AudioSource;
            audioSource.loop = false;
            audioSource.clip = m_soundsPool[(int)sound];
            audioSource.Play();
        }

        /// <summary> Make an audiobox play tan audioclip in loop </summary>
        private void PlayClipLoop(AudioBox audiobox, ESound sound)
        {
            AudioSource audioSource = audiobox._AudioSource;
            audioSource.loop = true;
            audioSource.clip = m_soundsPool[(int)sound];
            audioSource.Play();
        }

        /// <summary> To deactivate the music </summary>
        private void DesactivateMusic()
        {
            GetComponent<AudioSource>().Stop();
        }
    }
}
