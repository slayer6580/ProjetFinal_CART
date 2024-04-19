using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DiscountDelirium
{
    public class Follow : MonoBehaviour
    {
        [SerializeField] private GameObject m_target;
     
        void Update()
        {
            this.transform.position = m_target.transform.position;
        }
    }
}
