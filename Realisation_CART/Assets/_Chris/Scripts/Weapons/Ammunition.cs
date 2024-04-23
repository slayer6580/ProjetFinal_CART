using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DiscountDelirium
{
    public class Ammunition : MonoBehaviour
    {
        [field: SerializeField] private float m_timeBeforeDestroy;


        private void Awake()
        {
            Destroy(this, m_timeBeforeDestroy);
        }

        private void OnCollisionEnter(Collision collision)
        {
            Debug.Log(collision.gameObject.name);
            Destroy(this.gameObject);
        }

    }
}
