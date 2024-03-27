using UnityEngine;

namespace BoxSystem
{
    public class DebugCollisionDetector : MonoBehaviour
    {
        private DebugTower _Tower { get; set; } = null;
        private const float MAX_VEL_TO_DROP_CONTENT = 10.0f;

        private void Start()
        {
            _Tower = transform.parent.GetComponentInChildren<DebugTower>();
        }

        private void OnTriggerEnter(Collider other)
        {
            if(other.gameObject.name != "front") return;
            if (other.attachedRigidbody.velocity.magnitude < MAX_VEL_TO_DROP_CONTENT) return;
            //Debug.Log("Velocity of " + other.gameObject.name + " is: " + other.attachedRigidbody.velocity.magnitude);
            
            Vector3 velocity = other.attachedRigidbody.velocity;
            //_Tower.DropContent(velocity);
        }
    }
}
