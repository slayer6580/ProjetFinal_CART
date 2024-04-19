using UnityEngine;

namespace DiscountDelirium
{
    public class Range : Weapon
    {
        private void Awake()
        {
            WeaponInputHandler.RangeAttack += UseWeapon;
        }
        public override void UseWeapon()
        {
            Debug.Log("Range Weapon Used");
        }
    }
}
