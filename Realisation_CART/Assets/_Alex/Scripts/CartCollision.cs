using BoxSystem;
using UnityEngine;
using static Manager.AudioManager;

namespace DiscountDelirium
{
    public class CartCollision : MonoBehaviour
    {
        [SerializeField] private TowerBoxSystem m_towerScript;
        private float m_collisionVolume = 0.1f;
        private float m_collisionPitch = 1;
        private Rigidbody m_playerRB = null;

        private void Start()
        {
            m_playerRB = GetComponent<Rigidbody>();
            if (m_playerRB == null) Debug.LogError("Player Rigidbody not found");
        }

        private void OnCollisionEnter(Collision collision)
        {

            if (collision.gameObject.layer == GameConstants.DEFAULT || collision.gameObject.layer == GameConstants.SHELF_COLLIDER)
            {
                //Debug.Log("Collision with " + collision.gameObject.name + " Layer: " + collision.gameObject.layer);
                if (Vector3.Dot(-collision.relativeVelocity.normalized, transform.forward) > 0.7)
                    m_towerScript.ActivateCollisionAnimation(collision.relativeVelocity.magnitude);

                if (collision.gameObject.layer == GameConstants.GROUND_COLLIDER)
                    return;

                float speed = 0;

                if (collision.relativeVelocity.magnitude > m_playerRB.velocity.magnitude)
                    speed = collision.relativeVelocity.magnitude;
                else
                    speed = m_playerRB.velocity.magnitude;

                //Debug.Log("Collision with " + collision.gameObject.name + " Layer: " + collision.gameObject.layer);
                _AudioManager.PlayCollisionAudio(collision.transform.position, speed, m_collisionVolume, m_collisionPitch);
            }
        }
    }
}
