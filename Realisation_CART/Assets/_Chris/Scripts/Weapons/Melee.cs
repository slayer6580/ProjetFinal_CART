using UnityEngine;

namespace DiscountDelirium
{
    public class Melee : Weapon
    {
        [Header("References")]
        private Animator m_animator;
        [SerializeField] TrailRenderer m_trailRenderer;

        [Header("Stats")]
        [SerializeField][Range(0, 4)] private int m_level;//temporary
        [SerializeField] private float[] m_speedLevel;
        [SerializeField] private float[] m_forceLevel;
        [SerializeField] private GameObject[] m_models;

        private int m_weaponIndex;

        private void Awake()
        {
            WeaponInputHandler.MeleeAttack += UseWeapon;

            //m_animator.speed = SpeedLevel[PlayerPrefs.GetInt("Melee", 0);];
            m_animator = GetComponent<Animator>();
            m_animator.speed = m_speedLevel[m_level];

            //m_weaponIndex = PlayerPrefs.GetInt("Melee", 0);];
            m_weaponIndex = m_level;
            ChangeWeaponModel();

            m_trailRenderer.enabled = false;
        }

        private void OnTriggerEnter(Collider other)
        {
            
        }

        public override void UseWeapon()
        {
            m_animator.SetTrigger("ActivateWeapon");
        }

        public void OnValidate()
        {
            if (m_animator != null) 
            {
                m_animator.speed = m_speedLevel[m_level];
                //m_animator.speed = SpeedLevel[PlayerPrefs.GetInt("Melee", 0);];
            }
            ChangeWeaponModel();

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
            m_models[m_weaponIndex].SetActive(false);
            m_weaponIndex = m_level;
            m_models[m_weaponIndex].SetActive(true);
        }
    }
}
