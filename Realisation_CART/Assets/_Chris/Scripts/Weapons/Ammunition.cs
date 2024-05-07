using System.Collections;
using UnityEngine;

namespace DiscountDelirium
{
    public class Ammunition : MonoBehaviour
    {
        [SerializeField] private float m_timeBeforeDeactivate;
        [SerializeField] private GameObject m_model;
        [SerializeField] private ParticleSystem ImpactVFX;

        private void OnEnable()
        {
            StartCoroutine("DeactivateAmmo");
        }

        private void OnCollisionEnter(Collision collision)
        {
            ImpactVFX.Play();
            HideAmmo();
        }

        private void HideAmmo() 
        {
            GetComponent<Rigidbody>().isKinematic = true;
            GetComponent<SphereCollider>().enabled = false;
            m_model.SetActive(false);
            StartCoroutine("DeactivateAmmo");
        }

        IEnumerator DeactivateAmmo() 
        {
            yield return new WaitForSeconds(m_timeBeforeDeactivate);
            this.gameObject.SetActive(false);
        }

    }
}
