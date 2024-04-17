using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BehaviourTree
{
	public class SetNextPathTarget : LeafNode
	{
		protected override void OnStart()
		{
			if(m_blackboard.m_path.Count > 0)
			{
				m_blackboard.m_target = m_blackboard.m_path[0];
			}
			
		}

		protected override void OnStop()
		{
			
		}

		protected override State OnUpdate()
		{
			if (m_blackboard.m_path.Count > 0)
			{
				return State.Success;
			}
			return State.Failure;
				
		}
	}
}
