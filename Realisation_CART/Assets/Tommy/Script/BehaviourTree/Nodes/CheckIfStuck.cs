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
			//Debug.Log("AAA");
			if (m_blackboard.m_cartStateMachine.LocalVelocity.z < m_underSpeedToCountAsStuck && m_blackboard.m_cartStateMachine.LocalVelocity.z > -m_underSpeedToCountAsStuck && Time.time > 5)
			{
				//Debug.Log("BBB");
				if (m_blackboard.m_stuckAtTime == 0)
				{
					//Debug.Log("CCC");
					m_blackboard.m_stuckAtTime = Time.time;
					m_blackboard.m_timeStuck = 0;
				}
				else
				{
					//Debug.Log("DDD");
					m_blackboard.m_timeStuck = Time.time - m_blackboard.m_stuckAtTime;
				}

			}
			else
			{
				//Debug.Log("EEE");
				m_blackboard.m_stuckAtTime = 0;
				m_blackboard.m_timeStuck = 0;
			}

			if(m_blackboard.m_timeStuck > m_timeBeforeUnstuck)
			{
				//Debug.Log("FFF");
				return m_children[0].Update();				
			}
			else
			{
				if(m_children.Count > 1)
				{
					//Debug.Log("GGG");
					return m_children[1].Update();
				}
			}
			//Debug.Log("HHH");
			return State.Success;
		}
	}
}
