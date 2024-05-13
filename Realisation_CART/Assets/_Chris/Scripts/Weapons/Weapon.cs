using BoxSystem;
using System.Collections;
using UnityEngine;

namespace DiscountDelirium
{
    public abstract class Weapon : MonoBehaviour
    {
        [Header("Tower Reference")]
        [SerializeField] protected GrabItemTrigger m_playerGrabTrigger;

        [Header("Parameters")]
        [SerializeField] protected float m_animationTime = 1;

        [Header("Base Stats Weapon")]
        [SerializeField] private int[] m_itemsToStealLevel;

        protected bool m_canUseWeapon;
        protected bool m_isWeaponActive;
        protected void Start()
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
            //Debug.LogWarning("Weapon Enabled");
        }
        private void DisableWeapon() 
        {
            m_canUseWeapon = false;
            //Debug.Log("Weapon Disabled");
        }

        protected bool CanUseWeapon() 
        {
            return m_canUseWeapon;
        }

        //-----------------Stealing-----------------//

        public void StealItems(TowerBoxSystem clientTower, int nbsItem)
        {
            int itemToSteal = m_itemsToStealLevel[nbsItem];

            for(int i = 0; i < itemToSteal; i++)
            {
                m_playerGrabTrigger.StealItemFromOtherTower(clientTower);
			}
		}   
    }
}
