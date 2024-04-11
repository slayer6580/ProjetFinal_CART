using BoxSystem;
using Manager;
using System.Collections;
using System.Collections.Generic;
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
			Vector3 data = itemTrigger.TowerBoxSystem.EmptyCartAndGetScore();     
            GameStateMachine.Instance.GetScoreFromCart(data);

        }
    }
}
