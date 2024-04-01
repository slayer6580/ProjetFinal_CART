using BoxSystem;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DiscountDelirium
{
    public class CheckoutZone : MonoBehaviour
    {
        private void OnTriggerEnter(Collider other)
        {
            GrabItemTrigger itemTrigger = other.GetComponent<GrabItemTrigger>();
            if (itemTrigger == null) 
            {
                return;
            }
            int[] data = itemTrigger.TowerBoxSystem.EmptyCartAndGetScore();
            GameStateMachine.Instance.GetScoreFromCart(data);
        }
    }
}
