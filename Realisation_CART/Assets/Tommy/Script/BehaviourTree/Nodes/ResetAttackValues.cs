using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BehaviourTree
{
	public class ResetAttackValues : LeafNode
	{
		protected override void OnStart()
		{
		}

		protected override void OnStop()
		{
		}

		protected override State OnUpdate()
		{
			m_blackboard.m_lastAttackTimer = 0;
			m_blackboard.m_isAttacking = false;
			return State.Success;
		}
	}
}
