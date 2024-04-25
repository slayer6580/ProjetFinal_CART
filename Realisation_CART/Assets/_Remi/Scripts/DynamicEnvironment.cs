using System.Collections.Generic;
using UnityEngine;

namespace DynamicEnvironment
{
    public class DynamicEnvironment : MonoBehaviour
    {
        [field: SerializeField] private GameObject SpecialFXGOFireSprinklers { get; set; } = null;
        [field: SerializeField] private GameObject SpecialFXGOWaterPuddles { get; set; } = null;
        [field: SerializeField] private List<GameObject> SpecialFXGOFireStages { get; set; } = new List<GameObject>();


        private List<float> m_waterPuddleScales = new List<float>();
        private Vector3 m_targetScale = Vector3.zero;
        private Vector3 m_currentScale = Vector3.zero;
        [SerializeField] private float m_puddleAnimationSpeed = 0.1f;

        private const int NUMBER_OF_SPRINKLERS = 3;
        private const int STAGE_ZERO = 0;
        private const int STAGE_ONE = 1;
        private const int STAGE_TWO = 2;


        [SerializeField] private float m_max_health = 2500.0f;

        private bool m_isFireSprinklersActive = false;

        private void Awake()
        {
            CollisionDetector[] collisionDetectors = GetComponentsInChildren<CollisionDetector>();
            int iterator = 0;
            foreach (CollisionDetector collisionDetector in collisionDetectors)
            {
                collisionDetector.SetId(iterator);
                SpecialFXGOFireStages.Add(collisionDetector.transform.GetChild(0).gameObject);
                SpecialFXGOFireStages.Add(collisionDetector.transform.GetChild(1).gameObject);
                SpecialFXGOFireStages.Add(collisionDetector.transform.GetChild(2).gameObject);
                if (collisionDetector.transform.GetChild(0).gameObject.name != "Stage01") Debug.LogError("Stage01 not found");
                iterator++;

            }

            for (int i = 0; i < SpecialFXGOWaterPuddles.transform.childCount; i++)
            {
                m_waterPuddleScales.Add(SpecialFXGOWaterPuddles.transform.GetChild(i).localScale.x);
                //Debug.Log("Water puddle scale: " + m_waterPuddleScales[i]);
                SpecialFXGOWaterPuddles.transform.GetChild(i).localScale = new Vector3(0.1f, 0.1f, 0.1f);
            }
        }

        private void Update()
        {
            if (m_isFireSprinklersActive == false) return;

            for (int i = 0; i < SpecialFXGOWaterPuddles.transform.childCount; i++)
            {
                Transform child = SpecialFXGOWaterPuddles.transform.GetChild(i);
                float currentScale = child.localScale.x;
                float targetScale = m_waterPuddleScales[i];
                m_currentScale.x = currentScale;
                m_currentScale.y = currentScale;
                m_currentScale.z = currentScale;
                m_targetScale.x = targetScale;
                m_targetScale.y = targetScale;
                m_targetScale.z = targetScale;
                //Debug.Log("Current scale: " + m_currentScale);
                //Debug.Log("Target scale: " + m_targetScale);
                child.localScale = Vector3.Lerp(m_currentScale, m_targetScale, Time.deltaTime * m_puddleAnimationSpeed);
                if (child.localScale.x == m_targetScale.x - 0.1f)
                {
                    m_isFireSprinklersActive = false;
                }
            }
        }

        public void SetItemDestructionStage(CollisionDetector item)
        {
            if (item.GetHItemHealthPoints() <= (m_max_health * 3 / 4) && !item.GetIsStageDestructionActive(STAGE_ZERO))
            {
                Debug.Log("Stage 0 fire");
                ActivateAllPaticles(SpecialFXGOFireStages[GetSprinklerElement(STAGE_ZERO, item.GetId())]);
                Debug.Log("Item id: " + item.GetId());
                item.SetIsStageDestructionActive(STAGE_ZERO, true);
            }
            else if (item.GetHItemHealthPoints() <= (m_max_health * 2 / 4) && !item.GetIsStageDestructionActive(STAGE_ONE))
            {
                Debug.Log("Stage 1 fire");
                ActivateAllPaticles(SpecialFXGOFireStages[GetSprinklerElement(STAGE_ONE, item.GetId())]);
                item.SetIsStageDestructionActive(STAGE_ONE, true);
            }
            else if (item.GetHItemHealthPoints() <= (m_max_health * 1 / 4) && !item.GetIsStageDestructionActive(STAGE_TWO))
            {
                Debug.Log("Stage 2 fire");
                ActivateAllPaticles(SpecialFXGOFireStages[GetSprinklerElement(STAGE_TWO, item.GetId())]);
                item.SetIsStageDestructionActive(STAGE_TWO, true);
                Invoke("ActivateFireSprinklers", 5.0f);
            }
        }

        private int GetSprinklerElement(int destructionStage, int itemId)
        {
            Debug.Log("destructionStage: " + destructionStage);
            Debug.Log("Item id: " + itemId);
            if (itemId == 0) return destructionStage;

            int element = destructionStage * itemId;
            element += NUMBER_OF_SPRINKLERS;
            Debug.Log("return: " + element);
            return element;
        }

        private void ActivateAllPaticles(GameObject fireStage)
        {
            fireStage.SetActive(true);

            foreach (Transform child in fireStage.transform)
            {
                ParticleSystem particleSystem = child.GetComponent<ParticleSystem>();
                if (particleSystem == null) continue;
                ParticleSystem.EmissionModule emissionModule = particleSystem.emission;
                particleSystem.Play();
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
                ParticleSystem.EmissionModule emissionModule = particleSystem.emission;

                // Warning: The order of particle activation is inverted
                //          for sprinklers so their paricle FX plays correctly        
                emissionModule.enabled = true;
                particleSystem.Play();
            }

            Invoke("ActivateWaterPuddles", 1.0f);
        }

        private void ActivateWaterPuddles()
        {
            m_isFireSprinklersActive = true;
            SpecialFXGOWaterPuddles.SetActive(true);
        }

        public void ResetItem()
        {
            foreach (GameObject fireStage in SpecialFXGOFireStages)
            {
                fireStage.GetComponent<CollisionDetector>().ResetItem();
            }

            m_isFireSprinklersActive = false;
        }
    }
}
