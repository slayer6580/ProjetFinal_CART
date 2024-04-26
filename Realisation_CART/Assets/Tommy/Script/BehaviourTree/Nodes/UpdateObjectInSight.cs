using CartControl;
using UnityEngine;

namespace BehaviourTree
{
	public class UpdateObjectInSight : LeafNode
	{
		protected override void OnStart()
		{		
		}

		protected override void OnStop()
		{	
		}

		protected override State OnUpdate()
		{
			m_blackboard.m_clientInSight.Clear();

			//Create a colllider to detect surrounding
			Collider[] hitColliders = Physics.OverlapSphere(m_blackboard.m_thisClient.transform.position, m_blackboard.m_sightRange);
			foreach (var hitCollider in hitColliders)
			{
				//Only get the root of the client/player
				if (hitCollider.gameObject.layer == LayerMask.NameToLayer("PlayerCollider") && hitCollider.gameObject.GetComponent<CartStateMachine>() != null)
				{
					if(hitCollider.gameObject != m_blackboard.m_thisClient)
					{
						//Verify if the detected client is in vision limits
						Vector3 targetDir = new Vector3(hitCollider.gameObject.transform.position.x,
											m_blackboard.m_thisClient.transform.position.y,
											hitCollider.gameObject.transform.position.z) - m_blackboard.m_thisClient.transform.position;

						Vector3 forward = m_blackboard.m_thisClient.transform.forward;
						float angle = Vector3.SignedAngle(targetDir, forward, Vector3.up);

						if(angle < m_blackboard.m_sightHalfAngle && angle > -m_blackboard.m_sightHalfAngle)
						{
							m_blackboard.m_clientInSight.Add(hitCollider.gameObject);
						}
						
					}
				}		
			}
			return State.Success;
		}
	}
}
