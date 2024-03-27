using BoxSystem;
using UnityEngine;
using static BoxSystem.Box1;

namespace DiscountDelirium
{
    public class AutoDestruction1 : MonoBehaviour
    {
        [SerializeField] private float m_delayUntilDelete;

        private void OnEnable()
        {
            Invoke("DestroyItem", m_delayUntilDelete);
        }

        private void DestroyItem()
        {
            // If it is a box
            if (GetComponent<Box1>() != null)
            {
                GetComponent<Box1>().GetTower().RemoveBoxFromTower();
                return;
            }
            else Debug.Log("AutoDestruction: No Box component found on this object");

            // Else should then be an item
            if (GetComponent<Item1>() == null)
            {
                Debug.LogError("AutoDestruction: No Item or Box component found on this object");
                return;
            }
            
            ItemInBox lastItemInBox = GetComponent<Item1>().GetBox().GetLastItem();
            GetComponent<Item1>().GetBox().RemoveLastItemFromBox();

        }
    }
}
