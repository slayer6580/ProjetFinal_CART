using CartControl;
using DiscountDelirium;
using UnityEngine;

namespace BoxSystem
{
    public class CollisionDetector : MonoBehaviour
    {
        private TowerPhysics _TowerPhysics { get; set; } = null;
        private Collider m_previousCollider = null;
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

        //private void OnTriggerEnter(Collider other)
        //{
        //    if (!m_isPlayer) return;

        //    //Debug.Log("OnTriggerEnter " + other.name);
        //    if (other.gameObject.GetComponentInParent<DebugCart>() == null) return; // TODO Remi: Verify later if it is a client cart
        //                                                                            // TODO Remi: Ask the team for a client type id 
        //    if (m_previousCollider == other)
        //    {
        //        Debug.Log("Same collider as before");
        //        m_previousCollider = null;
        //        return;
        //    }
        //    //if (other.gameObject.name != "front") return;
        //    //Debug.Log("Velocity of " + other.gameObject.name + " is: " + other.attachedRigidbody.velocity.magnitude);
        //    if (other.attachedRigidbody.velocity.magnitude < MAX_VEL_TO_DROP_CONTENT) return;
        //    Debug.Log("Velocity of " + other.gameObject.name + " is: " + other.attachedRigidbody.velocity.magnitude);

        //    Vector3 velocity = other.attachedRigidbody.velocity;
        //    _TowerPhysics.CheckIfCanDropContent(velocity);
        //    m_previousCollider = other;
        //}


        private void OnCollisionEnter(Collision collision)
        {
            //if (other.gameObject.name != "front") return;
            //Debug.Log("Velocity of " + collision.gameObject.name + " is: " + collision.relativeVelocity.magnitude);
            if (collision.relativeVelocity.magnitude < MAX_VEL_TO_DROP_CONTENT) return;
            //Debug.Log("Velocity of " + other.gameObject.name + " is: " + other.attachedRigidbody.velocity.magnitude);

            Vector3 velocity = collision.relativeVelocity;

            _TowerPhysics.CheckIfCanDropContent(velocity);
        }
    }
}
