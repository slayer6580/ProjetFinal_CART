using UnityEngine;

namespace DiscountDelirium
{
    public abstract class Weapon : MonoBehaviour
    {
        const float levels = 4;
        public abstract void UseWeapon();
    }
}
