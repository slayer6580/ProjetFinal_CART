using UnityEngine;

namespace Manager
{
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