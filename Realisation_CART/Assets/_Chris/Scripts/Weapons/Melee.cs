using UnityEngine;

namespace DiscountDelirium
{
    public class Melee : Weapon
    {
        private Animator m_animator;
        [field: SerializeField] private float Speed { get; set; } = 1;

        private void Awake()
        {
            WeaponInputHandler.MeleeAttack += UseWeapon;
            m_animator = GetComponent<Animator>();
            m_animator.speed = Speed;
        }
        public override void UseWeapon()
        {
            m_animator.SetTrigger("ActivateWeapon");
        }
    }
}
