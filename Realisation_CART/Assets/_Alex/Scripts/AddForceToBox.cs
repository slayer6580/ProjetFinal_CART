using UnityEngine;

namespace BoxSystem
{
    public class AddForceToBox : MonoBehaviour
    {
        [Header("Constant force for debug with (Q) or (E)")]
        [SerializeField] private float m_constantForce;
        [Header("Force multiplier for constant force")]
        [SerializeField] private float m_forceMultiplier;

        private TowerHingePhysicsAlex m_towerPhysics;

        private void Awake()
        {
            m_towerPhysics = GetComponent<TowerHingePhysicsAlex>();
        }

        /// <summary> Add constant force to the top box of TowerPhysics </summary>
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
            DebugConstantForce();
        }

        /// <summary> For constant force debugging </summary>
        private void DebugConstantForce()
        {
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
