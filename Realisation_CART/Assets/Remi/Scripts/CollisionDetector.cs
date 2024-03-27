using DiscountDelirium;
using UnityEngine;

namespace BoxSystem
{
    public class CollisionDetector : MonoBehaviour
    {
        private TowerPhysics _TowerPhysics { get; set; } = null;
        private Collider m_previousCollider = null;
        private const float MAX_VEL_TO_DROP_CONTENT = 1.0f;

        private void Start()
        {
            _TowerPhysics = transform.parent.GetComponentInChildren<TowerPhysics>();
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.GetComponent<DebugCart>() == null) return; // TODO Remi: Verify later if it is a client cart
            Debug.Log("OnTriggerEnter " + other);
            if (m_previousCollider == other)
            {
                Debug.Log("Same collider as before");
                m_previousCollider = null;
                return;
            }
            //if (other.gameObject.name != "front") return;
            //Debug.Log("Velocity of " + other.gameObject.name + " is: " + other.attachedRigidbody.velocity.magnitude);
            if (other.attachedRigidbody.velocity.magnitude < MAX_VEL_TO_DROP_CONTENT) return;
            //Debug.Log("Velocity of " + other.gameObject.name + " is: " + other.attachedRigidbody.velocity.magnitude);

            Vector3 velocity = other.attachedRigidbody.velocity;
            _TowerPhysics.CheckIfCanDropContent(velocity);
            m_previousCollider = other;
        }
    }
}
