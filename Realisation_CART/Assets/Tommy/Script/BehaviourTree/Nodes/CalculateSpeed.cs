using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BehaviourTree
{
	public class CalculateSpeed : LeafNode
	{
		private float m_lastTimer;
		private Vector3 m_lastPosition;
		protected override void OnStart()
		{
			if(m_lastTimer == 0f)
			{
				m_lastTimer = Time.time;
				m_lastPosition = m_blackboard.m_thisClient.transform.position;
			}
		}

		protected override void OnStop()
		{
		}

		protected override State OnUpdate()
		{
			float timeSinceLastLoop = Time.time - m_lastTimer;
			float distanceSinceLastLoop = Vector3.Distance(m_lastPosition, m_blackboard.m_thisClient.transform.position);
			m_blackboard.m_currentSpeed = distanceSinceLastLoop * (1 / timeSinceLastLoop);

			return State.Success;
		}
	}
}
