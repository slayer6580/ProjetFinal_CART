using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DiscountDelirium
{
    public class CustomLookAt : MonoBehaviour
    {
        [SerializeField] private Transform m_target;
        [SerializeField] private Vector3 m_rotationOffset;
		private Quaternion m_newRotation;

		// Update is called once per frame
		void Update()
        {
			//m_newRotation.SetLookRotation((m_target.position - this.transform.position), Vector3.up);
			


			//this.transform.rotation = m_newRotation;
			//this.transform.LookAt(m_target, Vector3.up);

			Quaternion lookAt = Quaternion.LookRotation(m_target.position - this.transform.position);
			Quaternion correction = Quaternion.Euler(m_rotationOffset);

			this.transform.rotation = lookAt * correction;
			/*
			m_newRotation.x = 0 + m_rotationOffset.x;
			m_newRotation.y = this.transform.rotation.y + m_rotationOffset.y;
			m_newRotation.z = 0 + m_rotationOffset.z;
			*/

			//this.transform.rotation = m_newRotation;

		}
    }
}
