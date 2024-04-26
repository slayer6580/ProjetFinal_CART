using CartControl;
using Manager;
using System.Collections;
using UnityEngine;
using static Manager.AudioManager;

namespace DiscountDelirium
{
    public class Range : Weapon
    {
        [Header("References")]
        [SerializeField] private GameObject m_projectile;
        [SerializeField] private GameObject m_pointToShoot;
        [SerializeField] private ParticleSystem m_particleSmoke;
        [SerializeField] private CartStateMachine m_cartStateMachine;

        [Header("Stats")]
        [SerializeField][Range(0, 4)] private int m_level;
        [SerializeField] private float[] m_force;
        [SerializeField] private float[] m_fireRate;

        [Header("Test")]
        [SerializeField] private float LocalVelocity_Y;

        private bool m_canFire = true;

        public override void UseWeapon()
        {
            if (m_canFire) 
            {
                _AudioManager.PlaySoundEffectsOneShot(ESound.CannonSound, transform.position, 0.25f);
                StartCoroutine("Fire"); 
            }
        }

        private void Update()
        {
            LocalVelocity_Y = m_cartStateMachine.LocalVelocity.y;
        }

        IEnumerator Fire()
        {
            Debug.Log("Range Weapon Used");
            m_canFire = false;
            m_particleSmoke.Play();

            GameObject projectile = Instantiate(m_projectile, m_pointToShoot.transform.position, m_pointToShoot.transform.rotation);
            Vector3 force = m_pointToShoot.transform.forward * m_force[PlayerPrefs.GetInt("Ranged", 0)];
            force *= 1 + m_cartStateMachine.LocalVelocity.z * 0.08f;
            projectile.GetComponent<Rigidbody>().AddForce(force, ForceMode.Impulse);

            yield return new WaitForSeconds(1f / m_fireRate[PlayerPrefs.GetInt("Ranged", 0)]);
            m_canFire = true;
        }
    }
}
