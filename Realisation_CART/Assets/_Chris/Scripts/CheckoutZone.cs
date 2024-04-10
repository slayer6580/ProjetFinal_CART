using BoxSystem;
using Manager;
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
            Vector3 data = ScoreManager.EmptyCartAndGetScore();     
            GameStateMachine.Instance.GetScoreFromCart(data);
            // data[3] = nbOfBoxes;
        }
    }
}
