using BoxSystem;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using Unity.VisualScripting;
using UnityEngine;

namespace DiscountDelirium
{
    public class ClientPathHelper : MonoBehaviour
    {
        public List<Shelf> m_closeShelfList = new List<Shelf>();
		[SerializeField] BoxCollider m_thisCol;
		// Start is called before the first frame update

		private void Start()
		{
			Vector3 colScale = new Vector3(transform.localScale.x * m_thisCol.size.x, transform.localScale.y * m_thisCol.size.y, transform.localScale.z * m_thisCol.size.z);
		
			Collider[] hitColliders = Physics.OverlapBox(this.transform.position + m_thisCol.center, colScale / 2, Quaternion.identity);
			foreach (var hitCollider in hitColliders)
			{
				if (hitCollider.GetComponent<Shelf>() != null)
				{
					m_closeShelfList.Add(hitCollider.gameObject.GetComponent<Shelf>());
				}
			}
		}

	}
}
