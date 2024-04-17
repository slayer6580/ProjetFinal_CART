using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

namespace BehaviourTree
{
    public class DisableNavMeshAgent : LeafNode
    {
		protected override void OnStart()
		{

		}

		protected override void OnStop()
		{

		}

		protected override State OnUpdate()
		{
			m_blackboard.m_navAgent.enabled = false;

			if (m_blackboard.m_navAgent.isActiveAndEnabled)
			{
				return State.Running;
			}
			return State.Success;
		}
	}
}
