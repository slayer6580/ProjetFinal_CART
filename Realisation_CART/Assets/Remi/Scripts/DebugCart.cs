using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Burst.CompilerServices;
using UnityEngine;

namespace DiscountDelirium
{
    public class DebugCart : MonoBehaviour
    {
        private const float m_speed = 2.0f;

        void Start()
        {
            
            StartCoroutine(AutoDestroy());
        }

        IEnumerator AutoDestroy()
        {
            yield return new WaitForSeconds(2);
            Destroy(gameObject);
        }

        void FixedUpdate()
        {
            GetComponent<Rigidbody>().AddForce(transform.forward
                                            * GameConstants.BASE_ADD_FORCE
                                            * m_speed
                                            * Time.fixedDeltaTime
                                        );
        }
    }
}
