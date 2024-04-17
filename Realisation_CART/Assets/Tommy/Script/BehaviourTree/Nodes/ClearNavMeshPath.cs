using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BehaviourTree
{
	public class ClearNavMeshPath : LeafNode
	{
		protected override void OnStart()
		{
			
		}

		protected override void OnStop()
		{
			
		}

		protected override State OnUpdate()
		{
			m_blackboard.m_path.Clear();

			while (m_blackboard.m_pathDebugBox.Count > 0)
			{
				Destroy(m_blackboard.m_pathDebugBox[0]);
				m_blackboard.m_pathDebugBox.RemoveAt(0);
			}			

			return State.Success;
		}
	}
}
