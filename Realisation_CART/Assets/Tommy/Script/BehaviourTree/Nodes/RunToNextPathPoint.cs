using BehaviourTree;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DiscountDelirium
{
    public class RunToNextPathPoint : LeafNode
    {
		protected override void OnStart()
		{

		}

		protected override void OnStop()
		{

		}

		protected override State OnUpdate()
		{
			//Manage acceleration
			m_blackboard.m_cartStateMachine.OnBackward(0);
			m_blackboard.m_cartStateMachine.OnForward(1);

			return State.Success;
		}


	}
}
