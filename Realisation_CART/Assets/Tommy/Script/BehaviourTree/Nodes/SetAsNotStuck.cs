using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BehaviourTree
{
	public class SetAsNotStuck : LeafNode
	{
		protected override void OnStart()
		{
		
		}

		protected override void OnStop()
		{
			
		}

		protected override State OnUpdate()
		{

			m_blackboard.m_stuckAtTime = 0;
			m_blackboard.m_timeStuck = 0;
			return State.Success;
		}
	}
}
