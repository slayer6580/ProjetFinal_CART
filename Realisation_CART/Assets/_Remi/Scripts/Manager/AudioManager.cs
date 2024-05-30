using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.Rendering;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;

namespace Manager
{
    public class AudioManager : MonoBehaviour
    {
        [field: Header("Put all audio sounds here, read the Tooltip for the sound order")]
        [Tooltip("CartCollision\nCartRolling\nStep01\nStep02\nStep03\nStep04\nGrabItem\nDriftBegin\nDriftLoop\nCashRegister\nMeleeSwoosh\nCannonSound\nHit\nUIHover\nUIClick\nUIBack\nUIScroll")]
        [SerializeField] private AudioClip[] m_soundsPool = new AudioClip[23];
        [Tooltip("ThemeMusic\nCart_Song_01\nCart_Song_02\nWaitingRoomMusic")]
        [SerializeField] private AudioClip[] m_musicPool = new AudioClip[4];

        [SerializeField] private GameObject m_audioBoxPrefab;

        [Header("Put all AudioBox here")]
        [SerializeField] private List<AudioBox> m_audioBox;

        private AudioSource MusicAudioSource { get; set; }
        private AudioSource UIAudioSource { get; set; }

        [SerializeField] private AudioMixerGroup m_masterGroup;
        [SerializeField] private AudioMixerGroup m_sfxGroup;
        [SerializeField] private AudioMixerGroup m_musicGroup;
        [SerializeField] private AudioMixerGroup m_uiGroup;

        public static AudioManager _AudioManager { get; private set; }

        [field: SerializeField] public EMusic MainMenuMusic { get; private set; } = EMusic.ThemeMusic;
        [field: SerializeField] public EMusic LevelOneMusic { get; private set; } = EMusic.Cart_Song_01;
        [field: SerializeField] public EMusic LevelTwoMusic { get; private set; } = EMusic.Cart_Song_02;
        [field: SerializeField] public EMusic LevelThreeMusic { get; private set; } = EMusic.ThemeMusic;
        [field: SerializeField] public EMusic TutorialMusic { get; private set; } = EMusic.WaitingRoomMusic;

        [SerializeField] private int m_numberOfAudioBox = 10;
        [SerializeField] private bool m_isClientSoundEnabled = true;
        private int m_scrollAudioBox;
        private bool m_isLevelMusicPlaying = false;
        private int m_sceneMusicIndex = -1;

        /// <summary> This enum is used to store all the sounds in the game </summary>
        public enum ESound
        {
            CartCollision, CartRolling,
            Step01, Step02, Step03, Step04,
            GrabItem,
            DriftBegin, DriftLoop,
            CashRegister, BoxDropOnCounter, BoxDropSpecial,
            MeleeSwoosh, CannonSound, Hit, Splat,
            UIHover, UIClick, UIBack, UIScroll,
            FireStart, FireLoop, Sprinklers,
            Countdown,
            Count
        }

        /// <summary> This enum is used to store all the music in the game </summary>
        public enum EMusic
        {
            ThemeMusic,
            Cart_Song_01,
            Cart_Song_02,
            WaitingRoomMusic,
            Count
        }

        /// <summary> This enum is used to store all the sound modifications in the game </summary>
        public enum EAudioModification
        {
            SoundPitch,
            SoundVolume,
            MusicVolume,
            UIVolume,
            MasterVolume,
            Count
        }

        private void Awake()
        {
            //Debug.Log("AudioManager Awake");
            if (_AudioManager != null)
            {
                Debug.LogWarning("AudioManager already exists.");
                Destroy(gameObject);
                return;
            }

            _AudioManager = this;

            for (int i = 0; i < m_numberOfAudioBox; i++)
            {
                GameObject audioBox = Instantiate(m_audioBoxPrefab, transform);
                m_audioBox.Add(audioBox.GetComponent<AudioBox>());
                audioBox.transform.SetParent(transform);
            }

            MusicAudioSource = GetComponent<AudioSource>();
            if (MusicAudioSource == null) Debug.LogError("No AudioSource on AudioManager");

            GameObject child = transform.GetChild(0).gameObject;
            if (child.name != "UIAudioSource") Debug.LogWarning("UIAudioSource renamed, moved or not found! Current game object: " + child.name);
            UIAudioSource = child.GetComponent<AudioSource>();
            if (UIAudioSource == null) Debug.LogError("No UI AudioSource on AudioManager");
        }

        public void PlayUIHoverSound()
        {
            PlayUISoundOneShot(ESound.UIHover);
        }

        public void PlayUIClickSound()
        {
            //Debug.Log("PlayUIClickSound");
            PlayUISoundOneShot(ESound.UIClick);
        }

        public void PlayUIBackSound()
        {
            //Debug.Log("PlayUIBackSound");
            PlayUISoundOneShot(ESound.UIBack);
        }

        public void PlayUIScrollSound(int value)
        {
            //Debug.Log("PlayUIScrollSound");
            //m_scrollAudioBox = MusicOptionSliderEffect(ESound.UIScroll, Vector3.zero);
            PlayUISoundLoop(value);
        }

        public void StopUIScrollSound()
        {
            //Debug.Log("StopUIScrollSound");
            StopUISoundLoop();
        }

        private void PlayUISoundOneShot(ESound uiSound)
        {
            float volume = 1;
            float pitch = 1;

            AudioClip clip = m_soundsPool[(int)uiSound];

            UIAudioSource.volume = volume;
            UIAudioSource.pitch = pitch;

            UIAudioSource.loop = false;
            UIAudioSource.clip = m_soundsPool[(int)uiSound];
            UIAudioSource.Play();
        }

        private void PlayUISoundLoop(int value)
        {
            AudioMixerGroup mixerGroup;
            switch (value)
            {
                case 0:
                    mixerGroup = m_masterGroup;
                    break;
                case 1:
                    mixerGroup = m_musicGroup;
                    break;
                case 2:
                    mixerGroup = m_sfxGroup;
                    break;
                case 3:
                    mixerGroup = m_uiGroup;
                    break;
                default:
                    mixerGroup = m_uiGroup;
                    break;
            }

            UIAudioSource.outputAudioMixerGroup = mixerGroup;

            UIAudioSource.loop = true;
            UIAudioSource.clip = m_soundsPool[19];
            UIAudioSource.Play();
        }

        private void StopUISoundLoop()
        {
            Debug.Log("StopUISoundLoop");
           // UIAudioSource.Pause();
            UIAudioSource.Stop();
            UIAudioSource.loop = false;
            UIAudioSource.pitch = 1;
            UIAudioSource.volume = 1;
        }

        /// <summary> Modify the pitch or volume of a sound of a music </summary>
        public void ModifyAudio(int index, EAudioModification modif, float value = 0)
        {
            //Debug.Log("ModifyAudio: " + modif + " Value: " + value + " Index: " + index);
            if (index > 0 && index <= m_audioBox.Count)
            {
                //Debug.LogWarning("No more audio box available");
                AudioSource audioSource = m_audioBox[index]._AudioSource;

                if (modif == EAudioModification.SoundPitch)
                {
                    audioSource.pitch = value;
                }
                else if (modif == EAudioModification.SoundVolume)
                {
                    float currentVolume = PlayerPrefs.GetFloat("SoundFXVolume", 1);
                    audioSource.volume = Mathf.Min(value, currentVolume);
                }
                return;
            }
            else if (modif == EAudioModification.MusicVolume)
            {
                float currentVolume = PlayerPrefs.GetFloat("MusicVolume", 1);
                MusicAudioSource.volume = Mathf.Min(value, currentVolume);
            }
            else if (modif == EAudioModification.UIVolume)
            {
                float currentVolume = PlayerPrefs.GetFloat("SoundFXVolume", 1);
                UIAudioSource.volume = Mathf.Min(value, currentVolume);
            }
            else if (modif == EAudioModification.MasterVolume)
            {
                float currentVolume = PlayerPrefs.GetFloat("MasterVolume", 1);
                MusicAudioSource.volume = Mathf.Min(value, currentVolume);

                for (int i = 0; i < m_audioBox.Count; i++)
                {
                    m_audioBox[i]._AudioSource.volume = Mathf.Min(value, currentVolume);
                }
            }
        }

        /// <summary> Modify the pitch or volume of a sound of a one shot sound based on an object speed </summary>
        public float ModifyAudioBasedOnBodySpeed(float speed, EAudioModification modif, float valueToModify)
        {
            if (modif == EAudioModification.SoundPitch)
            {
                return valueToModify * (speed / 2);
            }
            else if (modif == EAudioModification.SoundVolume)
            {
                return valueToModify * (speed / 2);
            }
            else if (modif == EAudioModification.MusicVolume)
            {
                Debug.LogWarning("MusicVolume is not a valid modification for this function");
                return 0;
            }
            else if (modif == EAudioModification.MasterVolume)
            {
                Debug.LogWarning("MasterVolume is not a valid modification for this function");
                return 0;
            }

            return 0;
        }

        /// <summary> Play a sound effect one shot </summary>
        public void PlaySoundEffectsOneShot(ESound sound, Vector3 newPosition, float volume = 1, float pitch = 1)
        {
            AudioBox audiobox = FindAValidAudioBox();

            if (audiobox == null)
                return;

            //Debug.Log("Playing sound: " + sound + " at element : " + (int)sound + " max elements: " + m_soundsPool.Length);
            AudioClip clip = m_soundsPool[(int)sound];
            audiobox.m_isPlaying = true;
            audiobox._AudioSource.volume = volume;
            audiobox._AudioSource.pitch = pitch;

            MoveAudioBoxLocal(audiobox, newPosition);
            PlayClipOneShot(audiobox, sound);
            StartCoroutine(ReActivateAudioBox(audiobox, clip));
        }

        /// <summary> Play a sound one shot on a transform </summary>
        public int PlaySoundEffectsLoopOnTransform(ESound sound, Transform parent)
        {
            if (parent == null)
            {
                Debug.LogWarning("Parent is null, playing sound on AudioManager");
                parent = transform;
            }

            AudioBox audiobox = FindAValidAudioBox();

            int index = m_audioBox.IndexOf(audiobox);

            if (audiobox == null)
                return -1;

            audiobox._AudioSource.pitch = 1;
            audiobox._AudioSource.volume = 1;

            audiobox.transform.SetParent(parent, false);
            audiobox.transform.localPosition = Vector3.zero;

            AudioClip clip = m_soundsPool[(int)sound];
            audiobox.m_isPlaying = true;
            //MoveAudioBoxLocal(audiobox, Vector3.zero);
            MoveAudioBoxWorld(audiobox, parent.position);
            PlayClipLoop(audiobox, sound);
            return index;
        }

        /// <summary> Play UI on loop </summary>
        public int MusicOptionSliderEffect(ESound sound, Vector3 newPosition)
        {
            AudioBox audiobox = FindAValidAudioBox();
            int index = m_audioBox.IndexOf(audiobox);

            if (audiobox == null)
                return -1;

            AudioClip clip = m_soundsPool[(int)sound];
            audiobox.m_isPlaying = true;
            MoveAudioBoxWorld(audiobox, newPosition);
            PlayClipLoop(audiobox, sound);
            return index;
        }

        /// <summary> Stop a sound effect </summary>
        public void StopSoundEffectsLoop(int index)
        {
            if (index < 0 || index >= m_audioBox.Count)
            {
                Debug.LogWarning("No more audio box available");
                return;
            }

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

        /// <summary> Start the music of the current scene </summary>
        public int StartCurrentSceneMusic()
        {
            if (m_isLevelMusicPlaying) return m_sceneMusicIndex;
          
            //Debug.Log("StartCurrentSceneMusic");
            Scene currentScene = SceneManager.GetActiveScene();
            string sceneName = currentScene.name;
            int index = -1;

            if (sceneName == "MainMenu")
            {
                Debug.Log("Playing MainMenuMusic");
                index = PlayMusic(_AudioManager.MainMenuMusic);
             //   ModifyAudio(index, EAudioModification.MusicVolume, 1.0f);
            }
            else if (sceneName == "Tutorial")
            { 
                Debug.Log("Playing WaitingRoomMusic");
                index = PlayMusic(_AudioManager.TutorialMusic);
               // ModifyAudio(index, EAudioModification.MusicVolume, 1.0f);
            }
            else if (sceneName == "Level01")
            {
                Debug.Log("Playing LevelOneMusic");
                index = PlayMusic(_AudioManager.LevelOneMusic);
               // ModifyAudio(index, EAudioModification.MusicVolume, 1.0f);
            }
            else if (sceneName == "Level02")
            { 
                Debug.Log("Playing LevelTwoMusic");
                index = PlayMusic(_AudioManager.LevelTwoMusic);
               // ModifyAudio(index, EAudioModification.MusicVolume, 1.0f);
            }
            else if (sceneName == "Level03")
            {
                Debug.Log("Playing LevelThreeMusic");
                index = PlayMusic(_AudioManager.LevelThreeMusic);
               // ModifyAudio(index, EAudioModification.MusicVolume, 1.0f);
            }

            if (index != -1)
            { 
                m_isLevelMusicPlaying = true; 
                m_sceneMusicIndex = index;
            }

            return index;
        }

        /// <summary> Play a music </summary>
        public int PlayMusic(EMusic music)
        {
            AudioBox audiobox = FindAValidAudioBox();
            int index = m_audioBox.IndexOf(audiobox);

            if (audiobox == null)
                return -1;

            audiobox.m_isPlaying = true;
            MusicAudioSource.clip = m_musicPool[(int)music];
            MusicAudioSource.volume = PlayerPrefs.GetFloat("MusicVolume", 1);
            MusicAudioSource.Play();
            return index;
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
        private void MoveAudioBoxLocal(AudioBox audioBox, Vector3 newPosition)
        {
            audioBox.transform.localPosition = newPosition;
        }

        /// <summary> Move an audioBox at a position </summary>
        private void MoveAudioBoxWorld(AudioBox audioBox, Vector3 newPosition)
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

        public void SetTutorialMusic(EMusic newSong)
        {
            TutorialMusic = newSong;
        }

        public void SetMainMenuMusic(EMusic newSong)
        {
            MainMenuMusic = newSong;
        }

        public List<AudioBox> GetAudioBox()
        {
            return m_audioBox;
        }

        internal void PlayCollisionAudio(Vector3 position, float speed, float volume, float pitch)
        {
            float collisionVolume = volume;
            float collisionPitch = pitch;
            collisionVolume = ModifyAudioBasedOnBodySpeed(speed, EAudioModification.SoundVolume, collisionVolume);
            collisionPitch = ModifyAudioBasedOnBodySpeed(speed, EAudioModification.SoundPitch, collisionPitch);
            //Debug.Log("CollisionVolume: " + collisionVolume + " CollisionPitch: " + collisionPitch);
            _AudioManager.PlaySoundEffectsOneShot(ESound.CartCollision, position, collisionVolume, collisionPitch);
        }

        public void MuteAllAudioBoxes(bool isMuted)
        {
            //Debug.Log("MuteAllAudioBoxes");
            foreach (AudioBox audioBox in m_audioBox)
            {
                //if (audioBox.m_isPlaying == false) return;

                if (isMuted)
                {
                    //Debug.Log("MuteAllAudioBoxes: Mute");
                    audioBox._AudioSource.mute = true;
                    audioBox._AudioSource.Pause();
                }
                else
                {
                    audioBox._AudioSource.mute = false;
                    audioBox._AudioSource.UnPause();
                }
            }
        }
    }
}
