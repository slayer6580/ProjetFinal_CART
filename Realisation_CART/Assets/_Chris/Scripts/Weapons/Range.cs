using System.Collections;
using UnityEngine;
using static UnityEngine.UI.Image;
using UnityEngine.UIElements;

namespace DiscountDelirium
{
    public class Range : Weapon
    {
        [field: SerializeField] private GameObject m_projectile;
        [field: SerializeField] private float m_force;
        [field: SerializeField] private GameObject m_pointToShoot;
        [field: SerializeField] private float m_fireRate { get; set; } = 1;

        private bool m_canFire = true;

        private void Awake()
        {
            WeaponInputHandler.RangeAttack += UseWeapon;
        }
        public override void UseWeapon()
        {
            if (m_canFire) 
            {
                StartCoroutine("Fire");
            }
        }

        IEnumerator Fire()
        {
            Debug.Log("Range Weapon Used");
            m_canFire = false;
            GameObject projectile = Instantiate(m_projectile, m_pointToShoot.transform.position, m_pointToShoot.transform.rotation);
            Vector3 force = m_pointToShoot.transform.forward * m_force;
            projectile.GetComponent<Rigidbody>().AddForce(force, ForceMode.Impulse);
            yield return new WaitForSeconds(1f/m_fireRate);
            m_canFire = true;
        }
    }
}
