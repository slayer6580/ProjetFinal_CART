namespace BehaviourTree
{
	public class ClearNavMeshPath : LeafNode
	{
		public bool m_alsoClearChosePathList;
		protected override void OnStart()
		{
		}

		protected override void OnStop()
		{	
		}

		protected override State OnUpdate()
		{
			if (m_alsoClearChosePathList)
			{
				m_blackboard.m_chosenPathListCopy.Clear();
			}

			m_blackboard.m_path.Clear();

			while (m_blackboard.m_pathDebugBox.Count > 0)
			{
				Destroy(m_blackboard.m_pathDebugBox[0]);
				m_blackboard.m_pathDebugBox.RemoveAt(0);
			}			

			return State.Success;
		}
	}
}
