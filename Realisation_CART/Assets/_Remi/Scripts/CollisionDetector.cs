using System;
using UnityEngine;

namespace DynamicEnvironment
{
    public class CollisionDetector : MonoBehaviour
    {
        [field: SerializeField] private DynamicEnvironment _DynamicEnvironment { get; set; } = null;
        private const int PLAYER_COLLIDER = 6; // TODO Remi: Repetition: Ask the team for global variable script
        private const int CLIENT_COLLIDER = 7;

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
            //Debug.Log("Collision detected from layer: " + collision.gameObject.layer);
            if (collision.gameObject.layer != PLAYER_COLLIDER
                && collision.gameObject.layer != CLIENT_COLLIDER)
                return;

            //Debug.Log("Collision detected from player or client");
            float velocity = collision.impulse.magnitude;
            //Debug.Log($"Velocity: {velocity}");
            m_itemHealthPoints -= velocity;
            //Debug.Log($"Item health points: {_DynamicEnvironment.GetItemHealthPoints()}");
            Debug.Log("Item Id: " + GetId()); 
            _DynamicEnvironment.SetItemDestructionStage(this);
        }

        public int GetId()
        {
            return m_id;
        }

        public void SetId(int id)
        {
            m_id = id;
        }

        internal void ResetItem()
        {
            m_itemHealthPoints = m_max_health;
            m_isDestructionStageZero = false;
            m_isDestructionStageOne = false;
            m_m_isDestructionStageTwo = false;
        }

        internal float GetHItemHealthPoints()
        {
            return m_itemHealthPoints;
        }

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

        internal void SetIsStageDestructionActive(int stage, bool value)
        {
            if (stage == 0)
                m_isDestructionStageZero = value;
            else if (stage == 1)
                m_isDestructionStageOne = value;
            else if (stage == 2)
                m_m_isDestructionStageTwo = value;
        }
    }
}
