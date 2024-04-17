using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BehaviourTree
{
	public class RemovePathCurrentTarget : LeafNode
	{
		protected override void OnStart()
		{
			
		}

		protected override void OnStop()
		{
			
		}

		protected override State OnUpdate()
		{
			if(m_blackboard.m_path.Count > 0)
			{
				m_blackboard.m_path.RemoveAt(0);
				return State.Success;
			}
			return State.Failure;
		}
	}
}
