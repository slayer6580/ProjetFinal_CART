using BehaviourTree;
using UnityEngine;

namespace DiscountDelirium
{
	public class GoToTargetAndStop : LeafNode
	{
		private float m_targetDistance;
		private float m_distanceToSlowDown = 15;

		protected override void OnStart()
		{		
		}

		protected override void OnStop()
		{		
		}

		/// <summary>
		/// NOT USED FOR NOW
		/// </summary>
	

		protected override State OnUpdate()
		{
			TargetDistance();

			//Manage acceleration
			if (m_targetDistance > m_distanceToSlowDown)
			{
				m_blackboard.m_cartStateMachine.OnForward(1);
				m_blackboard.m_cartStateMachine.OnBackward(0);
			}
            else if (m_targetDistance > 1)
			{
				m_blackboard.m_cartStateMachine.OnForward(m_targetDistance / m_distanceToSlowDown);
			}
			else
			{
				m_blackboard.m_cartStateMachine.OnForward(0);
				
			}

			//Manage Break
			if (m_targetDistance < m_distanceToSlowDown && m_targetDistance > 1)
			{
				float currentSpeedPercent = m_blackboard.m_cartStateMachine.LocalVelocity.z / m_blackboard.m_cartStateMachine.MaxSpeedUpgrades;
				float closeDistancePercent = m_distanceToSlowDown / Mathf.Clamp(1f, m_distanceToSlowDown,(m_distanceToSlowDown - m_targetDistance));
				m_blackboard.m_cartStateMachine.OnBackward(1 * closeDistancePercent * currentSpeedPercent);
			}

			return State.Success;
		}

		private void TargetDistance()
		{
			m_targetDistance = Vector3.Distance(m_blackboard.m_target, m_blackboard.m_thisClient.transform.position);
		}
	}
}
