using System;
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
        [field: SerializeField] private List<GameObject> SpecialFXGOFireStagesGO { get; set; } = new List<GameObject>();
        //private List<GameObject> StageOneEffectsGO { get; set; } = new List<GameObject>();
        private List<ParticleSystem> StageOneParticleSystems { get; set; } = new List<ParticleSystem>();
        // private List<GameObject> StageTwoEffects { get; set; } = new List<GameObject>();
        private List<ParticleSystem> StageTwoParticleSystems { get; set; } = new List<ParticleSystem>();
        //private List<GameObject> StageThreeEffects { get; set; } = new List<GameObject>();
        private List<ParticleSystem> StageThreeParticleSystems { get; set; } = new List<ParticleSystem>();


        private List<float> m_waterPuddleScales = new List<float>();
        private Vector3 m_targetScale = Vector3.zero;
        private Vector3 m_currentScale = Vector3.zero;
        [SerializeField] private float m_puddleAnimationSpeed = 0.1f;

        private const int NUMBER_OF_SPRINKLERS = 3;

        private float m_currentTimer;
        private CollisionDetector m_currentItem;

        private SlipperyFloorDetector[] m_slipperyFloorPhysics;
        private Transform m_fireTransform = null;
        private Transform m_sprinklersTransform = null;
        private bool m_isFireSprinklersActive = false;
        private int m_fireAudioboxId = 0;
        private int m_sprinflersAudioboxId = 0;

        public enum DestructionStage
        {
            One,
            Two,
            Three
        }

        private void Awake()
        {
            //m_firePosition = SpecialFXGOFireStages.transform.position;
            CollisionDetector[] collisionDetectors = GetComponentsInChildren<CollisionDetector>();
            int iterator = 0;
            foreach (CollisionDetector collisionDetector in collisionDetectors)
            {
                collisionDetector.SetId(iterator);
                GameObject StageOne = collisionDetector.transform.GetChild(0).gameObject;
                m_fireTransform = StageOne.transform.GetChild(0);
                SpecialFXGOFireStagesGO.Add(StageOne);
                //StageOneEffectsGO.Add(StageOne.transform.GetChild(0).gameObject);
                StageOneParticleSystems.Add(StageOne.transform.GetChild(0).GetComponent<ParticleSystem>());
                //StageOneEffectsGO.Add(StageOne.transform.GetChild(1).gameObject);
                StageOneParticleSystems.Add(StageOne.transform.GetChild(1).GetComponent<ParticleSystem>());

                GameObject StageTwo = collisionDetector.transform.GetChild(1).gameObject;
                SpecialFXGOFireStagesGO.Add(StageTwo.gameObject);
                //StageTwoEffects.Add(StageTwo.transform.GetChild(0).gameObject);
                StageTwoParticleSystems.Add(StageTwo.transform.GetChild(0).GetComponent<ParticleSystem>());
                //StageTwoEffects.Add(StageTwo.transform.GetChild(1).gameObject);
                StageTwoParticleSystems.Add(StageTwo.transform.GetChild(1).GetComponent<ParticleSystem>());

                GameObject StageThree = collisionDetector.transform.GetChild(2).gameObject;
                SpecialFXGOFireStagesGO.Add(StageThree.gameObject);
                //StageThreeEffects.Add(StageThree.transform.GetChild(0).gameObject);
                StageThreeParticleSystems.Add(StageThree.transform.GetChild(0).GetComponent<ParticleSystem>());
                //StageThreeEffects.Add(StageThree.transform.GetChild(1).gameObject);
                StageThreeParticleSystems.Add(StageThree.transform.GetChild(1).GetComponent<ParticleSystem>());

                if (collisionDetector.transform.GetChild(0).gameObject.name != "Stage01") Debug.LogError("Stage01 not found");
                iterator++;
            }

            for (int i = 0; i < SpecialFXGOWaterPuddles.transform.childCount; i++)
            {
                m_waterPuddleScales.Add(SpecialFXGOWaterPuddles.transform.GetChild(i).localScale.x);
                SpecialFXGOWaterPuddles.transform.GetChild(i).localScale = new Vector3(0.1f, 0.1f, 0.1f);
            }

            m_sprinklersTransform = SpecialFXGOFireSprinklers.transform.GetChild(0);

            m_slipperyFloorPhysics = FindObjectsOfType<SlipperyFloorDetector>();
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

            foreach (GameObject fireStage in SpecialFXGOFireStagesGO)
            {
                DeactivateAllParticles(fireStage);
            }

            DeactivateFireSprinklers();

            foreach (GameObject fireStage in SpecialFXGOFireStagesGO)
            {
                fireStage.SetActive(false);
            }

            m_isFireSprinklersActive = false;

            foreach (SlipperyFloorDetector slipperyFloorPhysic in m_slipperyFloorPhysics)
            {
                slipperyFloorPhysic.RemoveSlipperyFromAllCharacters();
            }
        }

        /// <summary> Sets the item's destruction stage </summary>
        public void SetItemDestructionStage(CollisionDetector item)
        {
            //debug.log("!item.getisstagedestructionactive(stage_zero): " + item.getisstagedestructionactive(stage_zero));
            m_currentItem = item;
            if (item.GetHItemCurrentHealth() <= (item.GetMaxHealth() * 3 / 4) && !item.GetIsStageDestructionActive(DestructionStage.One))
            {
                //Debug.Log("Set fist Item DestructionStage (stage zero)");
                m_currentTimer = 10.0f;
                ActivateAllPaticles(DestructionStage.One);
                item.SetIsStageDestructionActive(DestructionStage.One, true);
                Invoke("PlayFireSound", 0.25f);

            }
            else if (item.GetHItemCurrentHealth() <= (item.GetMaxHealth() * 2 / 4) && !item.GetIsStageDestructionActive(DestructionStage.Two))
            {
                Debug.Log("Set second ItemDestruction Stage  (stage one)");
                ActivateAllPaticles(DestructionStage.Two);
                item.SetIsStageDestructionActive(DestructionStage.Two, true);
            }
            else if (item.GetHItemCurrentHealth() <= (item.GetMaxHealth() * 1 / 4) && !item.GetIsStageDestructionActive(DestructionStage.Three))
            {
                Debug.Log("Set third ItemDestruction Stage  (stage two)");
                ActivateAllPaticles(DestructionStage.Three);
                item.SetIsStageDestructionActive(DestructionStage.Three, true);
                Invoke("ActivateFireSprinklers", 5.0f);
            }
        }

        private void PlayFireSound()
        {
            _AudioManager.PlaySoundEffectsOneShot(ESound.FireStart, m_fireTransform.position);
            m_fireAudioboxId = _AudioManager.PlaySoundEffectsLoopOnTransform(ESound.FireLoop, m_fireTransform);
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
        private void ActivateAllPaticles(DestructionStage stageNumber)
        {
            GetStage(stageNumber).SetActive(true);

            if (stageNumber == DestructionStage.One)
            {
                ActivateAllPaticles(StageOneParticleSystems);
            }
            else if (stageNumber == DestructionStage.Two)
            {
                ActivateAllPaticles(StageTwoParticleSystems);
            }
            else if (stageNumber == DestructionStage.Three)
            {
                ActivateAllPaticles(StageThreeParticleSystems);
            }
            //stage.SetActive(true);

            //foreach (Transform child in stage.transform)
            //{
            //    Debug.Log("child.name: " + child.name);
            //    ParticleSystem particleSystem = child.GetComponent<ParticleSystem>();
            //    if (particleSystem == null) continue;
            //    ParticleSystem.EmissionModule emissionModule = particleSystem.emission;
            //    particleSystem.Play();
            //    emissionModule.enabled = true;
            //}
        }

        private GameObject GetStage(DestructionStage stageNumber)
        {
            if (stageNumber == DestructionStage.One)
            {
                return SpecialFXGOFireStagesGO[0];
            }
            else if (stageNumber == DestructionStage.Two)
            {
                return SpecialFXGOFireStagesGO[1];
            }
            else if (stageNumber == DestructionStage.Three)
            {
                return SpecialFXGOFireStagesGO[2];
            }
            return null;
        }

        private void ActivateAllPaticles(List<ParticleSystem> particleSystems)
        {
            foreach (ParticleSystem particleSystem in particleSystems)
            {
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
            //Debug.Log("ResetItensInEveryStages");

            SpecialFXGOFireStagesGO[0].GetComponentInParent<CollisionDetector>().ResetItem();
        }
    }
}
