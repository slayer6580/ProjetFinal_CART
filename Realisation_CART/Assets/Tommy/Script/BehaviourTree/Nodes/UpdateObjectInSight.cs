using CartControl;
using DiscountDelirium;
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
			int layerMask = 1 << 6;

			m_blackboard.m_clientInSight.Clear();

			
			foreach(GameObject client in GameStateMachine.Instance.ClientsList)
			{
				if(client == m_blackboard.m_thisClient)
					continue;
				
				Vector3 raycastDir = client.transform.position - m_blackboard.m_thisClient.transform.position;

				RaycastHit hit;
				if (Physics.Raycast(m_blackboard.m_thisClient.transform.position, raycastDir, out hit, m_blackboard.m_sightRange, layerMask))
				{
					//Verify if the detected client is in vision limits
					Vector3 targetDir = new Vector3(client.transform.position.x,
										m_blackboard.m_thisClient.transform.position.y,
										client.transform.position.z) - m_blackboard.m_thisClient.transform.position;

					Vector3 forward = m_blackboard.m_thisClient.transform.forward;
					float angle = Vector3.SignedAngle(targetDir, forward, Vector3.up);

					if (angle < m_blackboard.m_sightHalfAngle && angle > -m_blackboard.m_sightHalfAngle)
					{
						m_blackboard.m_clientInSight.Add(client);
					}
				}
			}

		
			return State.Success;
		}
	}
}
