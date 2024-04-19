using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BehaviourTree
{
	public class AdjustAccordingNextTarget : LeafNode
	{
		private Vector3 m_nextTarget;
		private float m_targetDistance;
		public float m_minAngleToStartDrifting;
		public float m_distanceToStartDriftingBeforeReachTarget;

		protected override void OnStart()
		{
			
		}

		protected override void OnStop()
		{
			
		}

		protected override State OnUpdate()
		{
			if (m_blackboard.m_path.Count > 1)
			{
				m_nextTarget = m_blackboard.m_path[1];

				Vector3 targetDir = new Vector3(m_nextTarget.x,
											m_blackboard.m_thisClient.transform.position.y,
											m_nextTarget.z) - m_blackboard.m_thisClient.transform.position;

				Vector3 forward = m_blackboard.m_thisClient.transform.forward;
				float angle = Vector3.SignedAngle(targetDir, forward, Vector3.up);

				TargetDistance();

				if(m_targetDistance < m_distanceToStartDriftingBeforeReachTarget)
				{
					if (angle > 5)
					{
						//m_blackboard.m_cartStateMachine.OnSteer(-1);
						m_blackboard.m_cartStateMachine.OnDrift(1);
					}
					else if (angle < -5)
					{
						//m_blackboard.m_cartStateMachine.OnSteer(1);
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
