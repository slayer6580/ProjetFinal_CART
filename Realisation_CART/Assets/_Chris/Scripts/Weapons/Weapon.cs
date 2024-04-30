using UnityEngine;

namespace DiscountDelirium
{
    public abstract class Weapon : MonoBehaviour
    {
        protected bool m_canUseWeapon;
        private void Start()
        {
            PauseState.OnPause += DisableWeapon;
            PauseState.OnResume += EnableWeapon;
            GetReadyState.OnGameStarted += EnableWeapon;
            EndGameState.OnEndGame += DisableWeapon;
        }

        public abstract void UseWeapon();

        private void EnableWeapon() 
        {
            m_canUseWeapon = true;
            Debug.Log("Weapon Enabled");
        }
        private void DisableWeapon() 
        {
            m_canUseWeapon = false;
            Debug.Log("Weapon Disabled");
        }

        protected bool CanUseWeapon() 
        {
            return m_canUseWeapon;
        }
    }
}
