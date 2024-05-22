using CartControl;
using System.Collections.Generic;
using UnityEngine;

namespace DynamicEnvironment
{
    /// <summary> Zone that changes the characters movement to simulate a slippery floor </summary>
    public class SlipperyFloorPhysic : MonoBehaviour
    {

        [field: SerializeField] private float SlipperyTurningDrag { get; set; } = 0.5f;
        [field: SerializeField] private float SlipperyDriftingDrag { get; set; } = 0.5f;

        private List<CartStateMachine> m_currentCarts = new List<CartStateMachine>();

        private float m_initialTurningDrag = 0.0f;
        private float m_initialDriftingDrag = 0.0f;

        private bool m_isSlippery = false;


        private void OnTriggerEnter(Collider other)
        {
            if (m_isSlippery) return;

            if (other.gameObject.layer != GameConstants.PLAYER_BODY
                && other.gameObject.layer != GameConstants.CLIENT_COLLIDER)
                return;
            //Debug.Log("Character Enter Slippery Floor");
            CartStateMachine cartStateMachine = other.gameObject.GetComponentInParent<CartStateMachine>();
            if (cartStateMachine == null) Debug.LogError("CartStateMachine not found in parent");

            m_currentCarts.Add(cartStateMachine);
            CartStateMachine lastAddedCart = m_currentCarts.ToArray()[m_currentCarts.Count - 1];
            m_initialTurningDrag = lastAddedCart.TurningDrag;
            m_initialDriftingDrag = lastAddedCart.DriftingDrag;

            lastAddedCart.TurningDrag = SlipperyTurningDrag;
            lastAddedCart.DriftingDrag = SlipperyDriftingDrag;
            m_isSlippery = true;
        }

        private void OnTriggerExit(Collider other)
        {
            if (!m_isSlippery) return;
            if (other.gameObject.layer != GameConstants.PLAYER_BODY
                && other.gameObject.layer != GameConstants.CLIENT_COLLIDER)
                return;

            //Debug.Log("Character Exit Slippery Floor");
            CartStateMachine cartStateMachine = other.gameObject.GetComponentInParent<CartStateMachine>();

            if (cartStateMachine == null) Debug.LogError("CartStateMachine not found in parent");

            if (!m_currentCarts.Contains(cartStateMachine))
                return;

            CartStateMachine currentCart = other.gameObject.GetComponentInParent<CartStateMachine>();

            currentCart.TurningDrag = m_initialTurningDrag;
            currentCart.DriftingDrag = m_initialDriftingDrag;
            m_currentCarts.Remove(currentCart);

            m_isSlippery = false;

            // TODO Remi: Ask Tommy if clients and player have the same Turning and Drifting values
        }
    }
}
