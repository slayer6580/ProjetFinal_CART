using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BehaviourTree
{
	public class SetIsAttacking : LeafNode
	{
		public bool m_isAttacking;
		protected override void OnStart()
		{
			
		}

		protected override void OnStop()
		{
			
		}

		protected override State OnUpdate()
		{
			m_blackboard.m_isAttacking = m_isAttacking;
			return State.Success;
		}
	}
}
