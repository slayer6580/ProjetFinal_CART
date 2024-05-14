using UnityEngine;

namespace BoxSystem
{
    public class AddForceToBox : MonoBehaviour
    {

        [Header("Force multiplier for constant force by balance level")]
        [SerializeField] private float[] m_forceMultiplier;

        [Header("Reduce forceMultiplier over time")]
        [SerializeField][Range(1, 10)] private float m_forceOverTimeReduction;

        [Header("Starting multiplier")]
        [SerializeField] private float m_startingTimeMultiplier;

        private TowerHingePhysicsAlex m_towerPhysics;
        private bool m_pushIsActivated = false;
        private Vector3 m_pushForce = Vector3.zero;
        private float m_timeMultiplier;
        public bool m_pushIsStop = false;

        private void Awake()
        {
            m_towerPhysics = GetComponent<TowerHingePhysicsAlex>();
            m_timeMultiplier = m_startingTimeMultiplier;
        }

        /// <summary> Add constant force to the top box of TowerPhysics </summary>
        public void AddConstantForceToBox(float force, float towerPushForceWhenMoving, float limit)
        {
            if (m_towerPhysics.GetTopBox() == null)
                return;

            if (Mathf.Abs(force) < limit)
            {
                m_pushIsStop = true;
				m_pushIsActivated = false;
                m_timeMultiplier = m_startingTimeMultiplier;
                m_pushForce = Vector3.zero;
                return;
            }
			m_pushIsStop = false;

			m_pushIsActivated = true;
            m_timeMultiplier += Time.deltaTime / m_forceOverTimeReduction;

            float totalForce = force * towerPushForceWhenMoving;
            m_pushForce = transform.right * totalForce * m_forceMultiplier[PlayerPrefs.GetInt("Balance",0)] * m_timeMultiplier;

        }
  
        public void StopForce(bool zeroMultiplier = false)
        {
			m_pushIsStop = true;
			m_pushIsActivated = false;
            if (zeroMultiplier)
            {
                m_timeMultiplier = 0f;
			}
            else
            {
				m_timeMultiplier = m_startingTimeMultiplier;
			}			
			m_pushForce = Vector3.zero;
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
            }


        }
        private void Update()
        {
            AddForce();

        }

    }
}
