using CartControl;
using System.Collections.Generic;
using UnityEngine;

namespace DynamicEnvironment
{
    /// <summary> Zone that changes the characters movement to simulate a slippery floor </summary>
    public class SlipperyFloorDetector : MonoBehaviour
    {

        private List<CartStateMachine> m_currentCarts = new List<CartStateMachine>();
        private bool m_isSlippery = false;


        private void OnTriggerEnter(Collider other)
        {
            if (m_isSlippery) return;

            if (other.gameObject.layer != GameConstants.PLAYER_COLLIDER
                && other.gameObject.layer != GameConstants.CLIENT_COLLIDER)
                return;

            //Debug.Log("Character Enter Slippery Floor");
            CartStateMachine cartStateMachine = other.gameObject.GetComponentInParent<CartStateMachine>();
            if (cartStateMachine == null) Debug.LogError("CartStateMachine not found in parent");

            m_currentCarts.Add(cartStateMachine);

            cartStateMachine.TurningDrag = cartStateMachine._CartPhysicsPresets.SlipperyTurningDrag;
            cartStateMachine.DriftingDrag = cartStateMachine._CartPhysicsPresets.SlipperyDriftingDrag;

            m_isSlippery = true;
        }

        private void OnTriggerExit(Collider other)
        {
            if (!m_isSlippery) return;
            if (other.gameObject.layer != GameConstants.PLAYER_COLLIDER
                && other.gameObject.layer != GameConstants.CLIENT_COLLIDER)
                return;

            Debug.Log("Character Exit Slippery Floor");
            CartStateMachine cartStateMachine = other.gameObject.GetComponentInParent<CartStateMachine>();

            if (cartStateMachine == null) Debug.LogError("CartStateMachine not found in parent");

            if (!m_currentCarts.Contains(cartStateMachine))
                return;

            CartStateMachine currentCart = other.gameObject.GetComponentInParent<CartStateMachine>();
            RemoveSlipperyEffect(currentCart);

            // TODO Remi: Ask Tommy if clients and player have the same Turning and Drifting values
        }

        private void RemoveSlipperyEffect(CartStateMachine currentCart)
        {
            currentCart.TurningDrag = currentCart._CartPhysicsPresets.NormalDriftingDrag;
            currentCart.DriftingDrag = currentCart._CartPhysicsPresets.NormalDriftingDrag;

            m_currentCarts.Remove(currentCart);

            m_isSlippery = false;
        }

        public void RemoveSlipperyFromAllCharacters()
        {
            foreach (CartStateMachine cart in m_currentCarts)
            {
                RemoveSlipperyEffect(cart);
            }

            m_currentCarts.Clear();
            m_isSlippery = false;
        }
    }
}
