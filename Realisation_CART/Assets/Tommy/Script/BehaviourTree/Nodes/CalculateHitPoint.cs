using UnityEngine;

namespace BehaviourTree
{
	public class CalculateHitPoint : LeafNode
	{
		private Vector3 m_target = Vector3.zero;
		private float m_timeToTarget;
		protected override void OnStart()
		{
		}

		protected override void OnStop()
		{
		}

		protected override State OnUpdate()
		{
			if (m_blackboard.m_isAttacking)
			{
				m_target = m_blackboard.m_chosenPathListCopy[0].gameObject.transform.position;
				float distanceToTarget = Vector3.Distance(m_blackboard.m_thisClient.transform.position, m_target);
				m_timeToTarget = distanceToTarget / m_blackboard.m_currentSpeed;
				Debug.Log("Time to reach target: " + m_timeToTarget);
			}
		

			return State.Success;
		}
	}
}
