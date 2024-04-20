using BoxSystem;
using Manager;
using UnityEngine;
using static Manager.AudioManager;
using static Manager.ScoreManager;

namespace DiscountDelirium
{
    public class CheckoutZone : MonoBehaviour
    {
        [SerializeField] private ShelfManager m_shelfManager;
        private void OnTriggerEnter(Collider other)
        {
            CheckOutTrigger itemTrigger = other.GetComponent<CheckOutTrigger>();

            if (itemTrigger == null)            
                return;
            
            if (itemTrigger.TowerBoxSystem.GetBoxCount() == 0)
                return;

            _AudioManager.PlaySoundEffectsOneShot(ESound.CashRegister, transform.position, 1f);

            bool isPlayer = other.transform.parent.name == "Character";
            if (isPlayer)
            {
				Vector3 data = _ScoreManager.EmptyCartAndGetScore();
				GameStateMachine.Instance.GetScoreFromCart(data);

				m_shelfManager.ResetAllShelves();
			}
            else
            {
                _ScoreManager.RemoveAllBoxImpulse(itemTrigger.TowerBoxSystem);

			}

           


		}
    }
}
