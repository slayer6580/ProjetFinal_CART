using BoxSystem;
using UnityEngine;

namespace DiscountDelirium
{
    public class CartCollision : MonoBehaviour
    {
        [SerializeField] private TowerBoxSystem m_towerScript;
        private void OnCollisionEnter(Collision collision)
        {
            if (collision.gameObject.layer == LayerMask.NameToLayer("Default") || collision.gameObject.layer == LayerMask.NameToLayer("ShelfCollider"))
            {
                if (Vector3.Dot(-collision.relativeVelocity.normalized, transform.forward) > 0.7)                
                    m_towerScript.ActivateCollisionAnimation(collision.relativeVelocity.magnitude);

            }
        }
    }
}
