namespace BehaviourTree
{
	public class GoBackward : LeafNode
	{
		protected override void OnStart()
		{		
		}

		protected override void OnStop()
		{	
		}

		protected override State OnUpdate()
		{
			m_blackboard.m_cartStateMachine.OnForward(0);
			m_blackboard.m_cartStateMachine.OnBackward(1);
			return State.Success;
		}
	}
}
