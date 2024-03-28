using UnityEngine;

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
            Destroy(this.gameObject);
        }
    }
}
