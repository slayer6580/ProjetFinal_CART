using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BehaviourTree
{
	public class GoForwardNode : LeafNode
	{
		protected override void OnStart()
		{
			m_blackboard.m_cartStateMachine.OnForward(1.0f);
		}

		protected override void OnStop()
		{
		}

		protected override State OnUpdate()
		{
			return State.Success;
		}
	}
}
