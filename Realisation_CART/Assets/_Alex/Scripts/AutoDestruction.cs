using UnityEngine;

namespace DiscountDelirium
{
    public class AutoDestruction : MonoBehaviour
    {
        [SerializeField] private float m_delayUntilDelete;

        public void DestroyItem()
        {
            Destroy(this.gameObject, m_delayUntilDelete);
        }
    }
}
