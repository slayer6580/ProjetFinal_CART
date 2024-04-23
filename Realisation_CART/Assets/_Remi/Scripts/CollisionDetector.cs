using UnityEngine;
using static UnityEngine.ParticleSystem;

namespace DynamicEnvironment
{
    public class CollisionDetector : MonoBehaviour
    {
        [field: SerializeField] private GameObject SpecialFXGOFireStage01 { get; set; } = null;
        [field: SerializeField] private GameObject SpecialFXGOFireStage02 { get; set; } = null;
        [field: SerializeField] private GameObject SpecialFXGOFireStage03 { get; set; } = null;
        [field: SerializeField] private GameObject SpecialFXGOFireSprinklers { get; set; } = null;

        private const int PLAYER_COLLIDER = 6;
        private const int CLIENT_COLLIDER = 7;

        [SerializeField] private float m_max_health = 10000.0f;
        private float m_itemHealthPoints = 10000.0f;

        private bool m_isStage01FireActive = false;
        private bool m_isStage02FireActive = false;
        private bool m_isStage03FireActive = false;

        private void Awake()
        {
            m_itemHealthPoints = m_max_health;
        }

        private void OnCollisionEnter(Collision collision)
        {
            Debug.Log("Collision detected from layer: " + collision.gameObject.layer);
            if (collision.gameObject.layer != PLAYER_COLLIDER
                && collision.gameObject.layer != CLIENT_COLLIDER) 
                return;

            Debug.Log("Collision detected from player or client");
            float velocity = collision.impulse.magnitude;
            Debug.Log($"Velocity: {velocity}");

            m_itemHealthPoints -= velocity;
            Debug.Log($"Item health points: {m_itemHealthPoints}");

            if (m_itemHealthPoints <= (m_max_health * 3 / 4) && !m_isStage01FireActive)
            {
                Debug.Log("Stage 1 fire");
                ActivateAllPaticles(SpecialFXGOFireStage01);
                m_isStage01FireActive = true;
            }
            else if (m_itemHealthPoints <= (m_max_health * 2 / 4) && !m_isStage02FireActive)
            {
                Debug.Log("Stage 2 fire");
                ActivateAllPaticles(SpecialFXGOFireStage02);
                m_isStage02FireActive = true;
            }
            else if (m_itemHealthPoints <= (m_max_health * 1 / 4) && !m_isStage03FireActive)
            {
                Debug.Log("Stage 3 fire");
                ActivateAllPaticles(SpecialFXGOFireStage03);
                m_isStage03FireActive = true;
                // Delay ActivateAllPaticles(SpecialFXPrefabFireSprinklers) by 5 seconds
                Invoke("ActivateFireSprinklers", 5.0f);
            }
        }

        private void ActivateAllPaticles(GameObject fireStage)
        {
            fireStage.SetActive(true);

            foreach (Transform child in fireStage.transform)
            {
                ParticleSystem particleSystem = child.GetComponent<ParticleSystem>();
                if (particleSystem == null) continue;
                particleSystem.Play();
                ParticleSystem.EmissionModule emissionModule = particleSystem.emission;
                emissionModule.enabled = true;
            }
        }

        private void ActivateFireSprinklers()
        {
            SpecialFXGOFireSprinklers.SetActive(true);

            foreach (Transform child in SpecialFXGOFireSprinklers.transform)
            {
                ParticleSystem particleSystem = child.GetComponent<ParticleSystem>();
                if (particleSystem == null) continue;
                particleSystem.Play();
                ParticleSystem.EmissionModule emissionModule = particleSystem.emission;
                emissionModule.enabled = true;
            }
        }

        public void ResetItem()
        {
            m_itemHealthPoints = m_max_health;
            m_isStage01FireActive = false;
            m_isStage02FireActive = false;
            m_isStage03FireActive = false;
        }
    }
}
