using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CartControl
{
    public class CartPhysicsPresets : MonoBehaviour
    {
        private bool m_playerIsWaterSliding = false;
        public float SlipperyTurningDrag { get; set; } = 0.0f;
        public float SlipperyDriftingDrag { get; set; } = 0.0f;

        public float NormalTurningDrag { get; set; } = 2.0f;
        public float NormalDriftingDrag { get; set; } = 1.0f;

        [SerializeField] private float m_slipperyTime = 3.0f;
        private float m_currentSlipperyTime = 0.0f;
        private object _CartStateMachine { get; set; }

        private void Awake()
        {
            _CartStateMachine = GetComponent<CartStateMachine>(); // Cant use _CartStateMachine : weird bug
            NormalTurningDrag = GetComponent<CartStateMachine>().TurningDrag;
            NormalDriftingDrag = GetComponent<CartStateMachine>().DriftingDrag;
            m_currentSlipperyTime = m_slipperyTime;
        }

        private void Update()
        {
            if (m_playerIsWaterSliding == false) return;

            GetComponent<CartStateMachine>().TurningDrag = SlipperyTurningDrag;
            GetComponent<CartStateMachine>().DriftingDrag = SlipperyDriftingDrag;
            m_currentSlipperyTime -= Time.deltaTime;

            if (m_currentSlipperyTime <= 0.0f)
            {
                m_playerIsWaterSliding = false;
                m_currentSlipperyTime = m_slipperyTime;
                GetComponent<CartStateMachine>().TurningDrag = NormalTurningDrag;
                GetComponent<CartStateMachine>().DriftingDrag = NormalDriftingDrag;
            }
        }

        internal void ActivateSlipperyPhysicPreset()
        {
            m_playerIsWaterSliding = true;
        }
    }
}
