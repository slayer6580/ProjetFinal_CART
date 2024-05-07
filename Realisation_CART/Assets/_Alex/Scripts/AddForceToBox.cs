using UnityEngine;

namespace BoxSystem
{
    public class AddForceToBox : MonoBehaviour
    {
        [Header("Force multiplier for constant force")]
        [SerializeField] private float m_forceMultiplier;

        [Header("Force multiplier for constant force")]
        [SerializeField][Range(1, 10)] private float m_forceOverTimeReduction;

        private TowerHingePhysicsAlex m_towerPhysics;
        private bool m_pushIsActivated = false;
        private Vector3 m_pushForce = Vector3.zero;
        private float m_timeMultiplier = 1;
        private float m_debugForce;

        private void Awake()
        {
            m_towerPhysics = GetComponent<TowerHingePhysicsAlex>();
        }

        /// <summary> Add constant force to the top box of TowerPhysics </summary>
        public void AddConstantForceToBox(float force, float towerPushForceWhenMoving)
        {
            if (m_towerPhysics.GetTopBox() == null)
                return;

            if (Mathf.Abs(force) < 1)
            {
                m_pushIsActivated = false;
                m_timeMultiplier = 1;
                m_pushForce = Vector3.zero;
                return;
            }

            m_pushIsActivated = true;
            m_timeMultiplier += Time.deltaTime / m_forceOverTimeReduction;

            float totalForce = force * towerPushForceWhenMoving;
            m_pushForce = transform.right * totalForce * m_forceMultiplier * m_timeMultiplier;

        }


        private void AddForce()
        {
            if (m_towerPhysics.GetTopBox() == null)
                return;

            if (m_pushIsActivated)
            {
                Rigidbody body = m_towerPhysics.GetTopBox().GetComponent<Rigidbody>();
                if (body == null)
                    return;

                body.AddForce(m_pushForce, ForceMode.Force);
                Debug.LogWarning("Pushed");
            }
            else
            {
                Debug.LogWarning("Not Pushed");
            }

        }
        private void Update()
        {
            AddForce();

        }

    }
}
