using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BehaviourTree
{
	public class DebugLogNode : LeafNode
	{
		public string m_message;
		public bool m_isWarning;
		protected override void OnStart()
		{
			//Debug.Log($"OnStart{message}");
		}

		protected override void OnStop()
		{
			//Debug.Log($"OnStop{message}");
		}

		protected override State OnUpdate()
		{
			if (m_isWarning)
			{
				Debug.LogWarning($"OnUpdate{m_message}");
			}
			else
			{
				Debug.Log($"OnUpdate{m_message}");
			}
			
			
			return State.Success;
		}
	}
}
