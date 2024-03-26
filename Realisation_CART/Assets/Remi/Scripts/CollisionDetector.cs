using UnityEngine;

namespace BoxSystem
{
    public class CollisionDetector : MonoBehaviour
    {
        private Tower1 _Tower { get; set; } = null;
        private const float MAX_VEL_TO_DROP_CONTENT = 0.4f;

        private void Start()
        {
            _Tower = transform.parent.GetComponentInChildren<Tower1>();
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.attachedRigidbody.velocity.magnitude < MAX_VEL_TO_DROP_CONTENT) return;
            Debug.Log("Velocity of " + other.gameObject.name + " is: " + other.attachedRigidbody.velocity.magnitude);
            
            Vector3 velocity = other.attachedRigidbody.velocity;
            _Tower.DropContent(velocity);
        }
    }
}
