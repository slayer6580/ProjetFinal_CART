using System.Collections.Generic;
using UnityEngine;
using static Manager.AudioManager;

namespace DynamicEnvironment
{
    /// <summary> Dynamic environment that handles the destruction of hazardous items </summary>
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

        private float m_currentTimer;
        private CollisionDetector m_currentItem;

        private Transform m_fireTransform = null;
        private Transform m_sprinklersTransform = null;
        private bool m_isFireSprinklersActive = false;
        private int m_fireAudioboxId = 0;
        private int m_sprinflersAudioboxId = 0;

        private void Awake()
        {
            //m_firePosition = SpecialFXGOFireStages.transform.position;
            CollisionDetector[] collisionDetectors = GetComponentsInChildren<CollisionDetector>();
            int iterator = 0;
            foreach (CollisionDetector collisionDetector in collisionDetectors)
            {
                collisionDetector.SetId(iterator);
                GameObject StageOne = collisionDetector.transform.GetChild(0).gameObject;
                m_fireTransform = StageOne.transform.GetChild(0).transform;
                SpecialFXGOFireStages.Add(StageOne);

                SpecialFXGOFireStages.Add(collisionDetector.transform.GetChild(1).gameObject);
                SpecialFXGOFireStages.Add(collisionDetector.transform.GetChild(2).gameObject);
                if (collisionDetector.transform.GetChild(0).gameObject.name != "Stage01") Debug.LogError("Stage01 not found");
                iterator++;

            }

            for (int i = 0; i < SpecialFXGOWaterPuddles.transform.childCount; i++)
            {
                m_waterPuddleScales.Add(SpecialFXGOWaterPuddles.transform.GetChild(i).localScale.x);
                SpecialFXGOWaterPuddles.transform.GetChild(i).localScale = new Vector3(0.1f, 0.1f, 0.1f);
            }

            m_sprinklersTransform = SpecialFXGOFireSprinklers.transform.GetChild(0);
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
                child.localScale = Vector3.Lerp(m_currentScale, m_targetScale, Time.deltaTime * m_puddleAnimationSpeed);

                if (child.localScale.x == m_targetScale.x - 0.1f)
                {
                    m_isFireSprinklersActive = false;
                }
            }

            m_currentTimer -= Time.deltaTime;
            //Debug.Log("m_currentTimer : " + m_currentTimer);
            if (m_currentTimer <= 0.0f)
            {
                ResetDynamicEnvironement();
            }
        }

        private void ResetDynamicEnvironement()
        {
            Debug.Log("Reset everything");
            _AudioManager.StopSoundEffectsLoop(m_fireAudioboxId);
            m_fireAudioboxId = 0;
            _AudioManager.StopSoundEffectsLoop(m_sprinflersAudioboxId);
            m_sprinflersAudioboxId = 0;
            m_currentItem.ResetItemCurrentHealth();
            ResetItensInEveryStages();
            DeactivateWaterPuddles();

            foreach (GameObject fireStage in SpecialFXGOFireStages)
            {
                DeactivateAllParticles(fireStage);
            }

            DeactivateFireSprinklers();

            foreach (GameObject fireStage in SpecialFXGOFireStages)
            {
                fireStage.SetActive(false);
            }

            m_isFireSprinklersActive = false;
        }

        /// <summary> Sets the item's destruction stage </summary>
        public void SetItemDestructionStage(CollisionDetector item)
        {
            Debug.Log("!item.GetIsStageDestructionActive(STAGE_ZERO): " + item.GetIsStageDestructionActive(STAGE_ZERO));
            m_currentItem = item;
            if (item.GetHItemCurrentHealth() <= (item.GetMaxHealth() * 3 / 4) && !item.GetIsStageDestructionActive(STAGE_ZERO))
            {
                Debug.Log("Set fist Item DestructionStage (stage zero)");
                m_currentTimer = 10.0f;
                ActivateAllPaticles(SpecialFXGOFireStages[GetSprinklerElement(STAGE_ZERO, item.GetId())]);
                item.SetIsStageDestructionActive(STAGE_ZERO, true);
                _AudioManager.PlaySoundEffectsOneShot(ESound.FireStart, m_fireTransform.position);
                m_fireAudioboxId = _AudioManager.PlaySoundEffectsLoopOnTransform(ESound.FireLoop, m_fireTransform);

            }
            else if (item.GetHItemCurrentHealth() <= (item.GetMaxHealth() * 2 / 4) && !item.GetIsStageDestructionActive(STAGE_ONE))
            {
                Debug.Log("Set second ItemDestruction Stage  (stage one)");
                ActivateAllPaticles(SpecialFXGOFireStages[GetSprinklerElement(STAGE_ONE, item.GetId())]);
                item.SetIsStageDestructionActive(STAGE_ONE, true);
            }
            else if (item.GetHItemCurrentHealth() <= (item.GetMaxHealth() * 1 / 4) && !item.GetIsStageDestructionActive(STAGE_TWO))
            {
                Debug.Log("Set third ItemDestruction Stage  (stage two)");
                ActivateAllPaticles(SpecialFXGOFireStages[GetSprinklerElement(STAGE_TWO, item.GetId())]);
                item.SetIsStageDestructionActive(STAGE_TWO, true);
                Invoke("ActivateFireSprinklers", 5.0f);
            }
        }

        /// <summary> Returns the sprinkler element from the given Id and destruction stage </summary>
        private int GetSprinklerElement(int destructionStage, int itemId)
        {
            if (itemId == 0) return destructionStage;

            int element = destructionStage * itemId;
            element += NUMBER_OF_SPRINKLERS;

            return element;
        }

        /// <summary> Activates all the particles in the given hazardous item stage GameObject </summary>
        private void ActivateAllPaticles(GameObject stage)
        {
            stage.SetActive(true);

            foreach (Transform child in stage.transform)
            {
                ParticleSystem particleSystem = child.GetComponent<ParticleSystem>();
                if (particleSystem == null) continue;
                ParticleSystem.EmissionModule emissionModule = particleSystem.emission;
                particleSystem.Play();
                emissionModule.enabled = true;
            }
        }

        /// <summary> Deactivates all the particles in the given hazardous item stage GameObject </summary>
        private void DeactivateAllParticles(GameObject stage)
        {
            stage.SetActive(false);

            foreach (Transform child in stage.transform)
            {
                ParticleSystem particleSystem = child.GetComponent<ParticleSystem>();
                if (particleSystem == null) continue;
                ParticleSystem.EmissionModule emissionModule = particleSystem.emission;
                emissionModule.enabled = false;
                particleSystem.Stop();
            }
        }

        /// <summary> Activates the fire sprinklers </summary>
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
            m_sprinflersAudioboxId = _AudioManager.PlaySoundEffectsLoopOnTransform(ESound.Sprinklers, m_sprinklersTransform);
            _AudioManager.ModifyAudio(m_sprinflersAudioboxId, EAudioModification.SoundVolume, 1.0f);
            Invoke("ActivateWaterPuddles", 1.0f);
        }

        /// <summary> Deactivates the fire sprinklers </summary>
        private void DeactivateFireSprinklers()
        {
            foreach (Transform child in SpecialFXGOFireSprinklers.transform)
            {
                ParticleSystem particleSystem = child.GetComponent<ParticleSystem>();
                if (particleSystem == null) continue;
                ParticleSystem.EmissionModule emissionModule = particleSystem.emission;
                emissionModule.enabled = false;
                particleSystem.Stop();
            }

            SpecialFXGOFireSprinklers.SetActive(false);
        }

        /// <summary> Activates the water puddles </summary>
        private void ActivateWaterPuddles()
        {
            Debug.Log("ActivateWaterPuddles");
            m_isFireSprinklersActive = true;
            SpecialFXGOWaterPuddles.SetActive(true);
        }

        /// <summary> Deactivates the water puddles </summary>
        private void DeactivateWaterPuddles()
        {
            SpecialFXGOWaterPuddles.SetActive(false);
        }

        /// <summary> Resets the item's destruction stages </summary>
        public void ResetItensInEveryStages()
        {
            Debug.Log("ResetItensInEveryStages");

            SpecialFXGOFireStages[0].GetComponentInParent<CollisionDetector>().ResetItem();
        }
    }
}
