using UnityEngine;

namespace BoxSystem
{
    public class AddForceToBox : MonoBehaviour
    {
        [SerializeField] private float m_impulseForce;
        [SerializeField] private float m_constantForce;
        private TowerHingePhysicsAlex m_towerPhysics;
        [SerializeField] private float m_forceMultiplier;

        private void Awake()
        {
            m_towerPhysics = GetComponent<TowerHingePhysicsAlex>();
        }

        public void AddConstantForceToBox(float force)
        {
            if (m_towerPhysics.GetTopBox() == null)
                return;

            Vector3 pushDirection = force < 0.0f ? -transform.right : transform.right;
            Vector3 pushForce = pushDirection * m_forceMultiplier;
            m_towerPhysics.GetTopBox().GetComponent<Rigidbody>().AddForce(pushForce, ForceMode.Force);


        }

        private void Update()
        {
            if (Input.GetKeyUp(KeyCode.Z))
            {
                Vector3 leftPush = -transform.right * m_impulseForce;
                m_towerPhysics.GetTopBox().GetComponent<Rigidbody>().AddForce(leftPush, ForceMode.Impulse);
            }
            else if (Input.GetKeyUp(KeyCode.C))
            {
                Vector3 rightPush = transform.right * m_impulseForce;
                m_towerPhysics.GetTopBox().GetComponent<Rigidbody>().AddForce(rightPush, ForceMode.Impulse);
            }

            if (Input.GetKey(KeyCode.Q))
            {
                Vector3 leftPush = -transform.right * m_constantForce;
                m_towerPhysics.GetTopBox().GetComponent<Rigidbody>().AddForce(leftPush, ForceMode.Force);
            }
            else if (Input.GetKey(KeyCode.E))
            {
                Vector3 rightPush = transform.right * m_constantForce;
                m_towerPhysics.GetTopBox().GetComponent<Rigidbody>().AddForce(rightPush, ForceMode.Force);
            }

        }

      
    }
}
