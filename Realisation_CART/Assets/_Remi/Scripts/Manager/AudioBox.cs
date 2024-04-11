using UnityEngine;

namespace Manager
{
    /// <summary> This class is used to store the AudioSource component of an AudioBox object </summary>
    public class AudioBox : MonoBehaviour
    {
        [HideInInspector] public bool m_isPlaying = false;

        public AudioSource _AudioSource { get; private set; }

        private void Awake()
        {
            _AudioSource = GetComponent<AudioSource>();
        }
    }
}