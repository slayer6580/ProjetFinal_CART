using UnityEngine;

namespace BehaviourTree
{
	public class HasReachedPathTarget : CompositeNode
	{
		private float m_targetDistance;
		public float m_slowReachDistance;
		public float m_fastReachDistance;
		public float m_minSpeedForFastDistance;

		protected override void OnStart()
		{
		}

		protected override void OnStop()
		{		
		}

		protected override State OnUpdate()
		{
			TargetDistance();
			
			if(m_blackboard.m_cartStateMachine.LocalVelocity.z < m_minSpeedForFastDistance)
			{
				if(m_targetDistance < m_slowReachDistance)
				{
					return m_children[0].Update();
				}
			}
            else
            {
				if (m_targetDistance < m_fastReachDistance)
				{
					return m_children[0].Update();
				}
			}

			if (m_children.Count > 1)
			{
				return m_children[1].Update();
			}
			

			return State.Success;
		}

		private void TargetDistance()
		{
			m_targetDistance = Vector3.Distance(m_blackboard.m_target, m_blackboard.m_thisClient.transform.position);
		}
	}
}
