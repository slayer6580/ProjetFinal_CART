using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DiscountDelirium
{
    public class Ammunition : MonoBehaviour
    {
        [SerializeField] private float m_timeBeforeDestroy;
        [SerializeField] private GameObject m_model;

        private void Awake()
        {
            Destroy(this, m_timeBeforeDestroy);
        }

        private void OnCollisionEnter(Collision collision)
        {
            Debug.Log(collision.gameObject.name);
            HideAmmo();
        }

        private void HideAmmo() 
        {
            GetComponent<Rigidbody>().isKinematic = true;
            GetComponent<SphereCollider>().enabled = false;
            m_model.SetActive(false);
            Destroy(this, 5f);
        }

    }
}
