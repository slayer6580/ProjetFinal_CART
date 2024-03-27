using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BoxSystem
{
    public class BoxPhysics : MonoBehaviour
    {
        private Rigidbody rb;
        private bool lastIsKinematicState;
        public Vector3 m_incomingVelocity = Vector3.zero;

        void Start()
        {
            rb = GetComponent<Rigidbody>();
            if (rb != null)
            {
                lastIsKinematicState = rb.isKinematic;
            }
        }
        void Update()
        {
            if (rb != null && rb.isKinematic != lastIsKinematicState)
            {
                lastIsKinematicState = rb.isKinematic;

                OnKinematicStateChanged(rb.isKinematic);
            }
        }

        private void OnKinematicStateChanged(bool isKinematic)
        {
            Debug.Log($"Rigidbody kinematic state changed to: {isKinematic}");

            if (isKinematic) return;

            ApplyIncomingVelocity();
        }

        private void ApplyIncomingVelocity()
        {
            Debug.Log("ApplyIncomingVelocity() vel: " + m_incomingVelocity.magnitude);
            if (m_incomingVelocity == Vector3.zero) return;

            rb.AddForce(m_incomingVelocity * 10, ForceMode.Impulse);
            //m_incomingVelocity = Vector3.zero;
            Debug.Log("Incoming velocity applied: " + rb.velocity.magnitude);
        }
    }
}
