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

        private void Awake()
        {
            ResetItem();
        }

        private void OnCollisionEnter(Collision collision)
        {
            if (collision.gameObject.layer == GameConstants.PLAYER_COLLIDER)
            {
                float velocity = collision.impulse.magnitude;
                m_currentHealth -= velocity;
                _DynamicEnvironment.SetItemDestructionStage(this);
            }
            else if (collision.gameObject.layer == GameConstants.CLIENT_COLLIDER)
            {
                float velocity = collision.impulse.magnitude;
                //if (velocity > 40.0f) velocity = 20; // NPCs are stronger in the five first seconds of the game
                m_currentHealth -= (velocity / 4); // NPCs are weaker than the player
                //Debug.Log("impact: " + (velocity /  1.5f));
                //Debug.Log("velocity: " + velocity);
                Debug.Log("Current health: " + m_currentHealth);
                _DynamicEnvironment.SetItemDestructionStage(this);
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
    }
}
