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
            CheckOutTrigger itemTrigger = other.GetComponent<CheckOutTrigger>();
            if (itemTrigger == null) 
            {
                return;
            }
            Vector3 data = itemTrigger.TowerBoxSystem.EmptyCartAndGetScore();     
            GameStateMachine.Instance.GetScoreFromCart(data);
            // data[3] = nbOfBoxes;
        }
    }
}
