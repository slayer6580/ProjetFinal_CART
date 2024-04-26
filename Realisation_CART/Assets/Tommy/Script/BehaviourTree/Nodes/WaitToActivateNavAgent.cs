namespace BehaviourTree
{
    public class WaitToActivateNavAgent : DecoratorNode
	{

		protected override void OnStart()
		{
			m_blackboard.m_navAgent.enabled = true;
		}

		protected override void OnStop()
		{
		}

		protected override State OnUpdate()
		{
			if (m_blackboard.m_navAgent.isActiveAndEnabled)
			{
				return m_child.Update();
			}
			return State.Running;
		}
	}
}
