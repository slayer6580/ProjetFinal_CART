using BoxSystem;
using Manager;
using UnityEngine;
using static Manager.AudioManager;

namespace DiscountDelirium
{
    public class CheckoutZone : MonoBehaviour
    {
        private void OnTriggerEnter(Collider other)
        {
            CheckOutTrigger itemTrigger = other.GetComponent<CheckOutTrigger>();
            if (itemTrigger == null)
			{
				return;
            }
			_AudioManager.PlaySoundEffectsOneShot(ESound.CashRegister, transform.position, 1f);    
            Vector3 data = ScoreManager.EmptyCartAndGetScore();     
            GameStateMachine.Instance.GetScoreFromCart(data);

        }
    }
}
