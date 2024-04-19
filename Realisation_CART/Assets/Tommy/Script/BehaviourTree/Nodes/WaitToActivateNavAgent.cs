using BehaviourTree;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BehaviourTree
{
    public class WaitToActivateNavAgent : DecoratorNode
	{

		protected override void OnStart()
		{
			m_blackboard.m_navAgent.enabled = true;
		}

		protected override void OnStop()
		{

		}

		protected override State OnUpdate()
		{
			if (m_blackboard.m_navAgent.isActiveAndEnabled)
			{
				Debug.Log("Waiting to activate agent");
				return m_child.Update();
			}
			return State.Running;
		}
	}
}
