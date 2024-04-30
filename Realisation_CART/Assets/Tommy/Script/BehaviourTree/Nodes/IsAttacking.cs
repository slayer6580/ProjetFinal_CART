
namespace BehaviourTree
{
	public class IsAttacking : CompositeNode
	{
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
