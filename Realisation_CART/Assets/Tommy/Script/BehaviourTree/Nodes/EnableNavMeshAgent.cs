using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BehaviourTree
{
	public class EnableNavMeshAgent : LeafNode
	{
		protected override void OnStart()
		{
			
		}

		protected override void OnStop()
		{
			
		}

		protected override State OnUpdate()
		{
			m_blackboard.m_navAgent.enabled = true;
			return State.Success;
		}
	}
}
