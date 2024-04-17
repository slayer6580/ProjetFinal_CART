using BehaviourTree;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BehaviourTree
{
	public class AdjustRotation : LeafNode
	{

		protected override void OnStart()
		{

		}

		protected override void OnStop()
		{
			
		}

		protected override State OnUpdate()
		{
			Vector3 targetDir = new Vector3(m_blackboard.m_target.x,
											m_blackboard.m_thisClient.transform.position.y,
											m_blackboard.m_target.z) - m_blackboard.m_thisClient.transform.position;

			Vector3 forward = m_blackboard.m_thisClient.transform.forward;
			float angle = Vector3.SignedAngle(targetDir, forward, Vector3.up);
			m_blackboard.m_targetAngle = angle;


			if (angle > 2)
			{
				m_blackboard.m_cartStateMachine.OnSteer(-1);
			}
			else if (angle < -2)
			{
				m_blackboard.m_cartStateMachine.OnSteer(1);
			}
			else
			{
				m_blackboard.m_cartStateMachine.OnSteer(0);
			}

			return State.Success;
		}
	}
}
