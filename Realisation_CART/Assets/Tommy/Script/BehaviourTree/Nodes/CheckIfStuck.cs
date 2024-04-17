using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BehaviourTree
{
	public class CheckIfStuck : CompositeNode
	{
		protected override void OnStart()
		{
			
		}

		protected override void OnStop()
		{
			
		}

		protected override State OnUpdate()
		{
			if (m_blackboard.m_cartStateMachine.LocalVelocity.z < 5)
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


			if(m_blackboard.m_timeStuck > 2)
			{
				//m_blackboard.m_timeStuck = 0;
				return m_children[0].Update();
				
			}

			return State.Success;
		}
	}
}
