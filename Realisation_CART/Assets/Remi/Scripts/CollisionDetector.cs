using CartControl;
using UnityEngine;

namespace BoxSystem
{
    public class CollisionDetector : MonoBehaviour
    {
        private TowerPhysics _TowerPhysics { get; set; } = null;
        private const float MAX_VEL_TO_DROP_CONTENT = 15f;
        private bool m_isPlayer = true;

        private void Start()
        {
            if (transform.parent.GetComponent<MainInputsHandler>() == null) // TODO Remi: Ask the team for a player id 
                m_isPlayer = false; // If is not main player, disable
            else
            {
                _TowerPhysics = transform.parent.GetComponentInChildren<TowerPhysics>();
                if (_TowerPhysics == null) Debug.LogError("TowerPhysics not found in parent");
            }
        }

        private void OnCollisionEnter(Collision collision)
        {
            if (!m_isPlayer) return;
            if (collision.relativeVelocity.magnitude < MAX_VEL_TO_DROP_CONTENT) return;

            Vector3 velocity = collision.relativeVelocity;
            _TowerPhysics.CheckIfCanDropContent(velocity);
        }
    }
}
