using System.Collections;
using UnityEngine;
using static UnityEngine.UI.Image;
using UnityEngine.UIElements;

namespace DiscountDelirium
{
    public class Range : Weapon
    {
        [Header("References")]
        [SerializeField] private GameObject m_projectile;
        [SerializeField] private GameObject m_pointToShoot;
        [SerializeField] private ParticleSystem m_particleSmoke;

        [Header("Stats")]
        [SerializeField][Range(0, 4)] private int m_level;
        [SerializeField] private float[] m_force;
        [SerializeField] private float[] m_fireRate;
        
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
            m_particleSmoke.Play();
            GameObject projectile = Instantiate(m_projectile, m_pointToShoot.transform.position, m_pointToShoot.transform.rotation);
            Vector3 force = m_pointToShoot.transform.forward * m_force[m_level];
            projectile.GetComponent<Rigidbody>().AddForce(force, ForceMode.Impulse);
            yield return new WaitForSeconds(1f / m_fireRate[m_level]);
            m_canFire = true;
        }
    }
}
