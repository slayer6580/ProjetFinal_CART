using BoxSystem;
using UnityEngine;
using static Manager.AudioManager;
using static Manager.ScoreManager;

namespace DiscountDelirium
{
    public class CheckoutZone : MonoBehaviour
    {
        private void OnTriggerEnter(Collider other)
        {
            CheckOutTrigger itemTrigger = other.GetComponent<CheckOutTrigger>();

            if (itemTrigger == null)            
                return;
            
            if (itemTrigger.TowerBoxSystem.GetBoxCount() == 0)
                return;

            _AudioManager.PlaySoundEffectsOneShot(ESound.CashRegister, transform.position, 1f);

            bool isPlayer = other.transform.parent.parent.name == "Character";
            if (isPlayer)
            {
				Vector3 data = _ScoreManager.EmptyCartAndGetScore();
				GameStateMachine.Instance.GetScoreFromCart(data);
			}
            else
            {
                _ScoreManager.RemoveAllBoxImpulse(itemTrigger.TowerBoxSystem);
			}

		}
    }
}
