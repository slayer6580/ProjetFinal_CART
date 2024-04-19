using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BehaviourTree
{
	public class TryNoUnstuck : LeafNode
	{
		protected override void OnStart()
		{
		}

		protected override void OnStop()
		{
		}

		protected override State OnUpdate()
		{
			/*
			m_blackboard.m_navAgent.enabled = true;
			m_blackboard.m_navAgent.speed = 3;
			if(m_blackboard.m_cartStateMachine.LocalVelocity.z < 5)
			{
				Debug.Log("MOOOOVE");
				return State.Running;
			}
			*/

			m_blackboard.m_path.Clear();
			foreach(GameObject obj in m_blackboard.m_pathDebugBox)
			{
				Destroy(obj);
			}
			m_blackboard.m_pathDebugBox.Clear();
			return State.Success;
		}
	}
}
