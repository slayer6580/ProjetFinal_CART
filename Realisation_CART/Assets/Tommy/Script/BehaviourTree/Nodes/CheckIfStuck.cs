using UnityEngine;

namespace BehaviourTree
{
	public class CheckIfStuck : CompositeNode
	{
		public float m_underSpeedToCountAsStuck;
		public float m_timeBeforeUnstuck;

		protected override void OnStart()
		{			
		}

		protected override void OnStop()
		{		
		}

		protected override State OnUpdate()
		{
			if (m_blackboard.m_cartStateMachine.LocalVelocity.z < m_underSpeedToCountAsStuck)
			{
				if (m_blackboard.m_stuckAtTime == 0)
				{
					m_blackboard.m_stuckAtTime = Time.time;
					m_blackboard.m_timeStuck = 0;
				}
				else
				{
					m_blackboard.m_timeStuck = Time.time - m_blackboard.m_stuckAtTime;
				}

			}
			else
			{
				m_blackboard.m_stuckAtTime = 0;
				m_blackboard.m_timeStuck = 0;
			}

			if(m_blackboard.m_timeStuck > m_timeBeforeUnstuck)
			{
				return m_children[0].Update();
				
			}

			return State.Success;
		}
	}
}
