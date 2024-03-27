using UnityEngine;

namespace BoxSystem
{
    public class CollisionDetector : MonoBehaviour
    {
        private Tower1 _Tower { get; set; } = null;
        private const float MAX_VEL_TO_DROP_CONTENT = 1.0f;

        private void Start()
        {
            _Tower = transform.parent.GetComponentInChildren<Tower1>();
        }

        private void OnTriggerEnter(Collider other)
        {
            //if (other.gameObject.name != "front") return;
            //Debug.Log("Velocity of " + other.gameObject.name + " is: " + other.attachedRigidbody.velocity.magnitude);
            if (other.attachedRigidbody.velocity.magnitude < MAX_VEL_TO_DROP_CONTENT) return;
            //Debug.Log("Velocity of " + other.gameObject.name + " is: " + other.attachedRigidbody.velocity.magnitude);
            
            Vector3 velocity = other.attachedRigidbody.velocity;
            _Tower.CheckIfCanDropContent(velocity);
        }
    }
}
