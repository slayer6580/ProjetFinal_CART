using CartControl;
using DiscountDelirium;
using UnityEngine;

namespace BoxSystem
{
    public class CollisionDetector : MonoBehaviour
    {

        private TowerPhysics _TowerPhysics { get; set; } = null;
        private const float MAX_VEL_TO_DROP_CONTENT = 15f;
        private int m_clientLayer = 7;
        private int m_groundLayer = 10;

        private void Start()
        {
            _TowerPhysics = transform.parent.GetComponentInChildren<TowerPhysics>();
            if (_TowerPhysics == null) Debug.LogError("TowerPhysics not found in parent");
        }

        private void OnCollisionEnter(Collision collision)
        {
            if (collision.gameObject.layer == m_clientLayer)
            {
                if (collision.relativeVelocity.magnitude < MAX_VEL_TO_DROP_CONTENT) return;

                Vector3 velocity = collision.relativeVelocity;
                _TowerPhysics.CheckIfCanDropContent(velocity);
            }
            else if (collision.gameObject.layer == m_groundLayer)
            {
                GetComponent<AutoDestruction>().enabled = true;
            }
        }
    }
}
