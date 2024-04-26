using UnityEngine;

namespace DynamicEnvironment
{
    /// <summary> Detects collision for dynamic environment's destructible items. </summary>
    public class CollisionDetector : MonoBehaviour
    {
        [field: SerializeField] private DynamicEnvironment _DynamicEnvironment { get; set; } = null;

        private bool m_isDestructionStageZero = false;
        private bool m_isDestructionStageOne = false;
        private bool m_m_isDestructionStageTwo = false;

        [SerializeField] private float m_max_health = 2500.0f;

        private float m_itemHealthPoints = 2500.0f;
        [SerializeField] private int m_id = 0;

        private void Awake()
        {
            ResetItem();
        }
        
        private void OnCollisionEnter(Collision collision)
        {
            if (collision.gameObject.layer != GameConstants.PLAYER_COLLIDER
                && collision.gameObject.layer != GameConstants.CLIENT_COLLIDER)
                return;

            float velocity = collision.impulse.magnitude;
            m_itemHealthPoints -= velocity;
            _DynamicEnvironment.SetItemDestructionStage(this);
        }

        /// <summary> Resets the item's health points and destruction stages </summary>
        internal void ResetItem()
        {
            m_itemHealthPoints = m_max_health;
            m_isDestructionStageZero = false;
            m_isDestructionStageOne = false;
            m_m_isDestructionStageTwo = false;
        }

        /// <summary> Returns the item's health points </summary>
        internal float GetHItemHealthPoints()
        {
            return m_itemHealthPoints;
        }

        /// <summary> Retrun true if the given has already been activated </summary>
        internal bool GetIsStageDestructionActive(int stage)
        {
            if (stage == 0)
                return m_isDestructionStageZero;
            else if (stage == 1)
                return m_isDestructionStageOne;
            else if (stage == 2)
                return m_m_isDestructionStageTwo;
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
                m_m_isDestructionStageTwo = value;
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
