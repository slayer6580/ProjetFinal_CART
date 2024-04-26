using UnityEngine;

namespace BehaviourTree
{
	public class DebugLogNode : LeafNode
	{
		public string m_message;
		public bool m_isWarning;
		protected override void OnStart()
		{
		}

		protected override void OnStop()
		{
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
