using CartControl;
using DiscountDelirium;
using UnityEngine;

namespace BehaviourTree
{
	public class UpdateObjectInSight : LeafNode
	{
		private Vector3 m_raycastDir;
		private Vector3 m_targetDir;
		private Vector3 m_forward;
		private float m_angle;
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
				
				m_raycastDir = client.transform.position - m_blackboard.m_thisClient.transform.position;

				RaycastHit hit;
				if (Physics.Raycast(m_blackboard.m_thisClient.transform.position, m_raycastDir, out hit, m_blackboard.m_sightRange, layerMask))
				{
					//Verify if the detected client is in vision limits
					m_targetDir = new Vector3(client.transform.position.x,
										m_blackboard.m_thisClient.transform.position.y,
										client.transform.position.z) - m_blackboard.m_thisClient.transform.position;

					m_forward = m_blackboard.m_thisClient.transform.forward;
					m_angle = Vector3.SignedAngle(m_targetDir, m_forward, Vector3.up);

					if (m_angle < m_blackboard.m_sightHalfAngle && m_angle > -m_blackboard.m_sightHalfAngle)
					{
						m_blackboard.m_clientInSight.Add(client);
					}
				}
			}

		
			return State.Success;
		}
	}
}
