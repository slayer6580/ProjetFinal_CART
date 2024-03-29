using CartControl;
using DiscountDelirium;
using UnityEngine;

namespace BoxSystem
{
    public class CollisionDetector : MonoBehaviour
    {

        private TowerPhysics _TowerPhysics { get; set; } = null;
        private const float MAX_VEL_TO_DROP_CONTENT = 15f;
        private int m_playerLayer = 6;
        private int m_clientLayer = 7;
        private int m_boxLayer = 9;
        private int m_groundLayer = 10;

        private void Start()
        {
            _TowerPhysics = GetComponentInChildren<TowerPhysics>();
            if (_TowerPhysics == null) Debug.LogError("TowerPhysics not found in parent");
        }

        private void OnCollisionEnter(Collision collision)
        {
            if (collision.gameObject.layer == m_clientLayer && gameObject.layer == m_playerLayer)
            {
                if (collision.relativeVelocity.magnitude < MAX_VEL_TO_DROP_CONTENT) return;

                Vector3 velocity = collision.relativeVelocity;
                _TowerPhysics.CheckIfCanDropContent(velocity);
            }
            else if (collision.gameObject.layer == m_groundLayer && gameObject.layer == m_boxLayer)
            {
                AutoDestruction autoDestruction = GetComponent<AutoDestruction>();
                if (autoDestruction != null && autoDestruction.enabled) return;

                Debug.Log("Collision with ground: " + gameObject.name);
                GetComponent<Box>().GetTower().RemoveLastBoxFromTower();
                autoDestruction.enabled = true;
            }
        }
    }
}
