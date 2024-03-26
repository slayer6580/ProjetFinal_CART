using UnityEngine;

namespace BoxSystem
{
    public class CollisionDetector : MonoBehaviour
    {
        private Tower1 _Tower { get; set; } = null;

        private void Start()
        {
            _Tower = transform.parent.GetComponentInChildren<Tower1>();
        }

        private void OnTriggerEnter(Collider other)
        {
            Debug.Log("Velocity of " + other.gameObject.name + " is: " + other.attachedRigidbody.velocity.magnitude);

            if (other.attachedRigidbody.velocity.magnitude < 1) return;
            
            Vector3 velocity = other.attachedRigidbody.velocity;
            Vector3 direction = other.transform.position - transform.position;

            _Tower.DropContent(velocity, direction);
        }
    }
}
