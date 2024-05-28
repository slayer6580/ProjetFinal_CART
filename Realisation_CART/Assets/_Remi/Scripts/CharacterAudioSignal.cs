using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Manager.AudioManager;

namespace Manager
{
    public class CharacterAudioSignal : MonoBehaviour
    {
        private float m_footstepVolume = 0.025f;
        [SerializeField] private float m_timeBetweenFootsteps = 0.5f;
        private float m_currentTimeBetweenFootsteps = 0.5f;
        [SerializeField] private bool m_isFootstepPlaying = false;
        private Rigidbody m_playerRB = null;

        private void Start()
        {
            m_playerRB = transform.parent.GetComponentInParent<Rigidbody>();
            if (m_playerRB == null) Debug.LogError("Player Rigidbody not found");
        }

        public void PlayFootstep()
        {
            //Debug.Log("PlayFootstep");

            if (m_isFootstepPlaying) return;

            ESound randomFootstep = (ESound)Random.Range((int)ESound.Step01, (int)ESound.Step03 + 1);

            float playerSpeed = m_playerRB.velocity.magnitude;
            float footstepVolume = m_footstepVolume * playerSpeed;
            float footstepPitch = playerSpeed / 10;


            _AudioManager.PlaySoundEffectsOneShot(randomFootstep, transform.position, footstepVolume, footstepPitch);
            m_isFootstepPlaying = true;
        }

        public void PlayAttack()
        {

        }

        public void PlayHurt()
        {

        }

        public void PlayDeath()
        {

        }

        private void Update()
        {
            if(m_isFootstepPlaying)
            {
                m_currentTimeBetweenFootsteps -= Time.deltaTime;

                if (m_currentTimeBetweenFootsteps <= 0)
                {
                    m_isFootstepPlaying = false;
                    m_currentTimeBetweenFootsteps = m_timeBetweenFootsteps;
                }
            }
        }
    }
}
