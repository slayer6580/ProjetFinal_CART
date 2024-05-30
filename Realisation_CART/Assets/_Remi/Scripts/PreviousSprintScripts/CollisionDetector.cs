using UnityEngine;
using static DynamicEnvironment.DynamicEnvironment;

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
        private float m_maxTimeBetweenHits = 1f;
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
                //Debug.Log("m_timeSinceLastHit: " + m_timeSinceLastHit);
                if (m_timeSinceLastHit <= 0)
                {
                    //Debug.Log("Resetting m_isBeingHit");
                    m_isBeingHit = false;
                    m_timeSinceLastHit = m_maxTimeBetweenHits;
                }
            }
        }

        private void OnCollisionEnter(Collision collision)
        {
            //Debug.Log("Collision name: " + collision + " m_isBeingHit: " + m_isBeingHit);
            if (collision.gameObject.layer == GameConstants.PLAYER_COLLIDER && m_isBeingHit == false)
            {
                float velocity = collision.impulse.magnitude;
                m_currentHealth -= velocity * 3;
                _DynamicEnvironment.SetItemDestructionStage(this);
                m_isBeingHit = true;
                return;
            }
            else if (collision.gameObject.layer == GameConstants.CLIENT_COLLIDER && m_isBeingHit == false)
            {
                float velocity = collision.impulse.magnitude;
                m_currentHealth -= (velocity / 4); // NPCs are weaker than the player
                _DynamicEnvironment.SetItemDestructionStage(this);
                m_isBeingHit = true;
                return;
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
        internal bool GetIsStageDestructionActive(DestructionStage stage)
        {
            if (stage == DestructionStage.One)
                return m_isDestructionStageZero;
            else if (stage == DestructionStage.Two)
                return m_isDestructionStageOne;
            else if (stage == DestructionStage.Three)
                return m_isDestructionStageTwo;
            else
                return false;
        }

        /// <summary> Sets the given stage to active or inactive </summary>
        internal void SetIsStageDestructionActive(DestructionStage stage, bool value)
        {
            if (stage == DestructionStage.One)
                m_isDestructionStageZero = value;
            else if (stage == DestructionStage.Two)
                m_isDestructionStageOne = value;
            else if (stage == DestructionStage.Three)
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
    }
}
