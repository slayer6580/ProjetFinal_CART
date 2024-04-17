using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BehaviourTree
{
	public class HasReachedPathTarget : CompositeNode
	{
		private float m_targetDistance;
		public float m_minDistanceToSetAsReached;
		protected override void OnStart()
		{
		}

		protected override void OnStop()
		{
			
		}

		protected override State OnUpdate()
		{
			var child = m_children[0];

			TargetDistance();
			if(m_targetDistance < m_minDistanceToSetAsReached)
			{
				child = m_children[0];
				return child.Update();
			}
			else
			{
				if (m_children.Count > 1)
				{
					child = m_children[1];
					return child.Update();
				}
				
			}

			return State.Success;
		}

		private void TargetDistance()
		{
			m_targetDistance = Vector3.Distance(m_blackboard.m_target, m_blackboard.m_thisClient.transform.position);
		}
	}
}
