namespace BehaviourTree
{
	public class IsPathEmpty : CompositeNode
	{
		protected override void OnStart()
		{
		}

		protected override void OnStop()
		{		
		}

		protected override State OnUpdate()
		{		
			if (m_blackboard.m_path.Count == 0)
			{
				return m_children[0].Update();
			}
			else
			{
				if (m_children.Count > 1)
				{
					return m_children[1].Update();
				}
			}

			return State.Success;
		}
	}
}
