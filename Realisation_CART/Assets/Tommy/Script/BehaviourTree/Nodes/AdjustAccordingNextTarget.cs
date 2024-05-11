using UnityEngine;

namespace BehaviourTree
{
	public class AdjustAccordingNextTarget : LeafNode
	{
		private Vector3 m_nextTarget;
		private float m_targetDistance;
		public float m_minAngleToStartDrifting;
		public float m_distanceToStartDriftingBeforeReachTarget;

		private Vector3 m_targetDir;
		private Vector3 m_forward;
		private float m_angle;
		protected override void OnStart()
		{
		}

		protected override void OnStop()
		{		
		}


		/// <summary>
		///  NOT USED FOR NOW
		/// </summary>



		protected override State OnUpdate()
		{
			if (m_blackboard.m_path.Count > 1)
			{
				m_nextTarget = m_blackboard.m_path[1];

				m_targetDir = new Vector3(m_nextTarget.x,
											m_blackboard.m_thisClient.transform.position.y,
											m_nextTarget.z) - m_blackboard.m_thisClient.transform.position;

				m_forward = m_blackboard.m_thisClient.transform.forward;
				m_angle = Vector3.SignedAngle(m_targetDir, m_forward, Vector3.up);

				TargetDistance();

				if(m_targetDistance < m_distanceToStartDriftingBeforeReachTarget)
				{
					if (m_angle > 5)
					{
						m_blackboard.m_cartStateMachine.OnDrift(1);
					}
					else if (m_angle < -5)
					{
						m_blackboard.m_cartStateMachine.OnDrift(1);
					}
				}			
			}

			return State.Success;
		}

		private void TargetDistance()
		{
			m_targetDistance = Vector3.Distance(m_blackboard.m_target, m_blackboard.m_thisClient.transform.position);
		}
	}
}
