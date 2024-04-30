using CartControl;
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
			//Collision has been detected and we are processing it so we can remove some value to prepare for next collision

			m_blackboard.m_cartStateMachine.LastClientCollisionWith.gameObject.GetComponent<CartStateMachine>().WasAttacked();
			m_blackboard.m_cartStateMachine.LastClientCollisionWith = null;

			m_blackboard.m_lastAttackTimer = 0;
			m_blackboard.m_isAttacking = false;

			return State.Success;
		}
	}
}
