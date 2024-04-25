using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DiscountDelirium
{
    public class Ammunition : MonoBehaviour
    {
        [SerializeField] private float m_timeBeforeDestroy;
        [SerializeField] private GameObject m_model;
        [SerializeField] private ParticleSystem ImpactVFX;

        private void Awake()
        {
            Destroy(this, m_timeBeforeDestroy);
        }

        private void OnCollisionEnter(Collision collision)
        {
            Debug.Log(collision.gameObject.name);
            ImpactVFX.Play();
            HideAmmo();
        }

        private void HideAmmo() 
        {
            GetComponent<Rigidbody>().isKinematic = true;
            GetComponent<SphereCollider>().enabled = false;
            m_model.SetActive(false);
            Destroy(this.gameObject, 5f);
        }

    }
}
