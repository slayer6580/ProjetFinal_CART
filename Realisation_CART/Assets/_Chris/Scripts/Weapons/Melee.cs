using UnityEngine;

namespace DiscountDelirium
{
    public class Melee : Weapon
    {
        [Header("References")]
        private Animator m_animator;

        [Header("Stats")]
        [SerializeField][Range(0, 4)] private int m_level;//temporary
        [SerializeField] private float[] m_speedLevel;

        private void Awake()
        {
            WeaponInputHandler.MeleeAttack += UseWeapon;
            m_animator = GetComponent<Animator>();
            m_animator.speed = m_speedLevel[m_level];
            //m_animator.speed = SpeedLevel[PlayerPrefs.GetInt("Melee", 0);];
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

        }
    }
}
