using BoxSystem;
using System.Collections;
using UnityEngine;
using static Manager.AudioManager;

namespace DiscountDelirium
{
    public class Melee : Weapon
    {
        [Header("References")]
        private Animator m_animator;
        [SerializeField] private TrailRenderer m_trailRenderer;

        [Header("Stats")]
        [SerializeField] private float[] m_speedLevel;
        [SerializeField] private GameObject[] m_models;

        private int m_weaponIndex;

        private void Awake()
        {       
            m_animator = GetComponent<Animator>();
            m_animator.speed = m_speedLevel[PlayerPrefs.GetInt("Melee", 0)];
            m_weaponIndex = PlayerPrefs.GetInt("Melee", 0);
            ChangeWeaponModel();

            m_trailRenderer.enabled = false;
        }

        private void OnTriggerEnter(Collider other)
        {
            Debug.LogWarning("Melee hitted target");
            if (other.gameObject.layer == LayerMask.NameToLayer("Target")) 
            {
                StealItems(other.gameObject.GetComponent<Target>().GetTower());
            }
        }

        public override void UseWeapon()
        {
            if (!m_canUseWeapon)
            {
                return;
            }
            m_animator.SetTrigger("ActivateWeapon");
        }

        public void ActivateTrail()
        {
            m_trailRenderer.enabled = true;
        }

        public void DeactivateTrail()
        {
            m_trailRenderer.enabled = false;
        }

        public void ChangeWeaponModel() 
        {
            foreach (var model in m_models) 
            {
                model.SetActive(false);
            }
            m_weaponIndex = PlayerPrefs.GetInt("Melee", 0);
            m_models[m_weaponIndex].SetActive(true);
        }

        public void UpdateMeleeWeapon() 
        {
            if (m_animator != null)
            {
                m_animator.speed = m_speedLevel[PlayerPrefs.GetInt("Melee", 0)];
            }
            ChangeWeaponModel();
        }

        public void PlaySound() 
        {
            _AudioManager.PlaySoundEffectsOneShot(ESound.MeleeSwoosh, transform.position, 0.5f);
        }

        public void StealItems(TowerBoxSystem clientTower)
        {
            Debug.LogWarning("STEAL WITH MELEE");
            base.StealItems(clientTower, PlayerPrefs.GetInt("Melee", 0));
        }
    }
}
