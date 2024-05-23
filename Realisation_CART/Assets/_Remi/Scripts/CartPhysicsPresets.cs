using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CartControl
{
    public class CartPhysicsPresets : MonoBehaviour
    {

        private void Awake()
        {
            NormalTurningDrag = GetComponent<CartStateMachine>().TurningDrag;
            NormalDriftingDrag = GetComponent<CartStateMachine>().DriftingDrag;
        }

        public float SlipperyTurningDrag { get; set; } = 0.0f;
        public float SlipperyDriftingDrag { get; set; } = 0.0f;

        public float NormalTurningDrag { get; set; } = 2.0f;
        public float NormalDriftingDrag { get; set; } = 1.0f;
    }
}
