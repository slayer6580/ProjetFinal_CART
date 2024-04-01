using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BehaviourTree
{
	public class DebugLogNode : LeafNode
	{
		public string message;
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
			Debug.Log($"OnUpdate{message}");
			Debug.Log($"Blackboard:{m_Blackboard.moveToPosition.x}");

			return State.Success;
		}
	}
}
