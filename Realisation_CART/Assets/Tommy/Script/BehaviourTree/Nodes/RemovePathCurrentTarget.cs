namespace BehaviourTree
{
	public class RemovePathCurrentTarget : LeafNode
	{
		protected override void OnStart()
		{
			
		}

		protected override void OnStop()
		{		
		}

		protected override State OnUpdate()
		{
			if(m_blackboard.m_path.Count > 0)
			{
				m_blackboard.m_path.RemoveAt(0);

				//Debug
				Destroy(m_blackboard.m_pathDebugBox[0]);
				m_blackboard.m_pathDebugBox.RemoveAt(0);

				//If the client has reached the real target, not just the navmesh corner points
				if(m_blackboard.m_path.Count == 0)
				{
					m_blackboard.m_chosenPathListCopy.RemoveAt(0);
				}

				return State.Success;
			}
			return State.Failure;
		}
	}
}
