using BoxSystem;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DiscountDelirium
{
    public class CheckoutZone2 : MonoBehaviour
    {
        private void OnTriggerEnter(Collider other)
        {
            GrabItemTrigger2 itemTrigger = other.GetComponent<GrabItemTrigger2>();
            if (itemTrigger == null) 
            {
                return;
            }
            int[] data = itemTrigger.TowerBoxSystem.EmptyCartAndGetScore();
            GameStateMachine2.Instance.GetScoreFromCart(data);
        }
    }
}
