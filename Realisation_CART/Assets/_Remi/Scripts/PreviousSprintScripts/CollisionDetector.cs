using UnityEngine;

namespace DynamicEnvironment
{
    /// <summary> Detects collision for dynamic environment's destructible items. </summary>
    public class CollisionDetector : MonoBehaviour
    {
        [field: SerializeField] private DynamicEnvironment _DynamicEnvironment { get; set; } = null;

        private bool m_isDestructionStageZero = false;
        private bool m_isDestructionStageOne = false;
        private bool m_isDestructionStageTwo = false;

        private float m_maxHealth = 500.0f;
        private float m_currentHealth = 0.0f;

        [SerializeField] private int m_id = 0;
        private bool m_isBeingHit = false;
        private float m_maxTimeBetweenHits = 2.0f;
        private float m_timeSinceLastHit = 0.0f;

        private void Awake()
        {
            ResetItem();
            m_timeSinceLastHit = m_maxTimeBetweenHits;
        }

        private void Update()
        {
            if (m_isBeingHit)
            {
                m_timeSinceLastHit -= Time.deltaTime;
                Debug.Log("m_timeSinceLastHit: " + m_timeSinceLastHit);
                if (m_timeSinceLastHit >= 0)
                {
                    m_isBeingHit = false;
                    m_timeSinceLastHit = m_maxTimeBetweenHits;
                }
            }
        }

        private void OnCollisionEnter(Collision collision)
        {
            if (collision.gameObject.layer == GameConstants.PLAYER_COLLIDER && m_isBeingHit == false)
            {
                Debug.Log("m_isBeingHit: " + m_isBeingHit + " Parent fame object: " + gameObject.transform.parent.gameObject.name);
                Debug.Log("Player collision: " + collision.gameObject.name);
                float velocity = collision.impulse.magnitude;
                m_currentHealth -= velocity;
                _DynamicEnvironment.SetItemDestructionStage(this);
                m_isBeingHit = true;
                Debug.Log("m_isBeingHit: " + m_isBeingHit);
            }
            else if (collision.gameObject.layer == GameConstants.CLIENT_COLLIDER && m_isBeingHit == false)
            {
                float velocity = collision.impulse.magnitude;
                //if (velocity > 40.0f) velocity = 20; // NPCs are stronger in the five first seconds of the game
                m_currentHealth -= (velocity / 4); // NPCs are weaker than the player
                //Debug.Log("impact: " + (velocity /  1.5f));
                //Debug.Log("velocity: " + velocity);
                //Debug.Log("Current health: " + m_currentHealth);
                _DynamicEnvironment.SetItemDestructionStage(this);
                m_isBeingHit = true;
            }
        }

        /// <summary> Resets the item's health points and destruction stages </summary>
        internal void ResetItem()
        {
            //Debug.Log("Resetting item");
            m_currentHealth = m_maxHealth;
            m_isDestructionStageZero = false;
            m_isDestructionStageOne = false;
            m_isDestructionStageTwo = false;
        }

        /// <summary> Returns the item's health points </summary>
        internal float GetHItemCurrentHealth()
        {
            return m_currentHealth;
        }

        internal void ResetItemCurrentHealth()
        {
            //Debug.Log("Resetting item health");
            m_currentHealth = m_maxHealth;
        }

        /// <summary> Returns the item's max health points </summary>
        internal float GetMaxHealth()
        {
            return m_maxHealth;
        }

        /// <summary> Retrun true if the given has already been activated </summary>
        internal bool GetIsStageDestructionActive(int stage)
        {
            if (stage == 0)
                return m_isDestructionStageZero;
            else if (stage == 1)
                return m_isDestructionStageOne;
            else if (stage == 2)
                return m_isDestructionStageTwo;
            else
                return false;
        }

        /// <summary> Sets the given stage to active or inactive </summary>
        internal void SetIsStageDestructionActive(int stage, bool value)
        {
            if (stage == 0)
                m_isDestructionStageZero = value;
            else if (stage == 1)
                m_isDestructionStageOne = value;
            else if (stage == 2)
                m_isDestructionStageTwo = value;
        }

        /// <summary> Returns the item's id </summary>
        public int GetId()
        {
            return m_id;
        }

        /// <summary> Sets the item's id </summary>
        public void SetId(int id)
        {
            m_id = id;
        }

        public void SetIsBeingHit(bool value)
        {
            m_isBeingHit = value;
        }
    }
}
