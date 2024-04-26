namespace BehaviourTree
{
	public class RepeatNode : DecoratorNode
	{
		protected override void OnStart()
		{		
		}

		protected override void OnStop()
		{
		}

		protected override State OnUpdate()
		{
			m_child.Update();
			return State.Running;
		}
	}
}
