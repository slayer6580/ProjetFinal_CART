using BoxSystem;
using System.Collections;
using UnityEngine;

namespace DiscountDelirium
{
    public class Ammunition : MonoBehaviour
    {
        [Header("References")]
        [SerializeField] private float m_timeBeforeDeactivate;
        [SerializeField] private GameObject m_model;
        [SerializeField] private ParticleSystem ImpactVFX;
        [SerializeField] private SphereCollider AmmunitionCollider;
        [SerializeField] private SphereCollider SphereTrigger;
        private Rigidbody m_rb;
        [SerializeField] private Range m_rangeWeapon;

        [Header("Stats")]
        [SerializeField] float m_speed;
        [SerializeField] float m_rotateSpeed;

        [Header("Target")]
        [SerializeField] private GameObject m_target;
        [SerializeField] private float m_minimumDistance;
        private bool m_hasTarget;

        private void Awake()
        {
            m_rb = GetComponent<Rigidbody>();
        }

        private void OnEnable()
        {
            StartCoroutine("DeactivateAmmo");
        }

        private void OnCollisionEnter(Collision collision)
        {
            HideAmmo();
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.layer == LayerMask.NameToLayer("Target"))
            {
                if (m_target == null) 
                {
                    m_speed = m_rb.velocity.magnitude;
                    m_hasTarget = true;
                    m_target = other.gameObject;
                }
            }
        }

        private void Update()
        {
            CheckDistanceWithTarget();
        }

        private void FixedUpdate() 
        {
            if (m_hasTarget == true) 
            {
                HomingToTarget();
            }
        }

        private void HomingToTarget()
        {
            Vector3 direction = m_target.transform.position - m_rb.position;
            direction.Normalize();

            Vector3 amountToRotate = Vector3.Cross(direction, transform.forward) * Vector3.Angle(transform.forward, direction);

            m_rb.angularVelocity = -amountToRotate * m_rotateSpeed;

            m_rb.velocity = transform.forward * m_speed;
        }

        private void HideAmmo() 
        {
            ImpactVFX.Play();
            m_target = null;
            m_hasTarget = false;
            GetComponent<Rigidbody>().isKinematic = true;
            AmmunitionCollider.enabled = false;
            SphereTrigger.enabled = false;
            m_model.SetActive(false);
            StartCoroutine("DeactivateAmmo");
        }

        public void ShowAmmo()
        {
            m_model.SetActive(true);
            GetComponent<Rigidbody>().isKinematic = false;
            GetComponent<Rigidbody>().velocity = Vector3.zero;
            AmmunitionCollider.enabled = true;
            SphereTrigger.enabled = true;
        }

        IEnumerator DeactivateAmmo() 
        {
            yield return new WaitForSeconds(m_timeBeforeDeactivate);
            HideAmmo();
            this.gameObject.SetActive(false);
        }

        private void CheckDistanceWithTarget() 
        {
            if (m_target != null)
            {
                if ((m_target.transform.position - transform.position).magnitude < m_minimumDistance)
                {
                    StopTarget();
                    HideAmmo();
                }
            }
        }

        private void StopTarget() 
        {
            m_target.GetComponent<Target>().StopMovement();
        }

    }
}
