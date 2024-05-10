using BoxSystem;
using System.Collections;
using UnityEngine;

namespace DiscountDelirium
{
    public abstract class Weapon : MonoBehaviour
    {
        [Header("Tower Reference")]
        [SerializeField] protected TowerBoxSystem m_playerTower;

        [Header("Parameters")]
        [SerializeField] protected float m_animationTime = 1;

        [Header("Base Stats Weapon")]
        [SerializeField] private int[] m_itemsToStealLevel;

        protected bool m_canUseWeapon;
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
            int itemsStolen = 0;
            int itemToSteal = m_itemsToStealLevel[nbsItem];
            for (int i = 0; i < itemToSteal; i++)
            {   
                if (clientTower.GetBoxCount() == 0)
                    return;

                GameObject itemTaken = clientTower.GetStolenItem();

                if (itemTaken == null)
                    return;

                ItemData.ESize size = itemTaken.GetComponent<Item>().m_data.m_size;

                StartCoroutine(WaitForAnimation(itemTaken, size));
                itemsStolen++;
            }
            Debug.LogWarning("stole: " + itemsStolen);

        }

        IEnumerator WaitForAnimation(GameObject itemTaken, ItemData.ESize size)
        {
            yield return new WaitForSeconds(m_animationTime);
            TakeItem(itemTaken, size);
        }

        private void TakeItem(GameObject itemTaken, ItemData.ESize size)
        {
            Rigidbody rb = itemTaken.GetComponent<Rigidbody>();
            if (rb)
            {
                Destroy(rb);
            }

            if (!m_playerTower.CanTakeObjectInTheActualBox(size))
            {
                m_playerTower.AddBoxToTower();
            }

            m_playerTower.PutObjectInTopBox(itemTaken);
            Debug.LogWarning("nom de l'objet pris: " + itemTaken.gameObject.name);
        }
    }
}
