namespace BehaviourTree
{
	public class EveryXLoop : DecoratorNode
	{
		public int m_everyXLoop;
		public int m_loopDone = 5;

		protected override void OnStart()
		{
		}

		protected override void OnStop()
		{
			m_loopDone++;
		}

		protected override State OnUpdate()
		{

			if (m_loopDone >= m_everyXLoop)
			{
				if (m_child.Update() == State.Success)
				{
					m_loopDone = 0;
					return State.Success;
				}

				if (m_child.Update() == State.Running)
				{
					return State.Running;
				}

				if (m_child.Update() == State.Failure)
				{
					return State.Failure;
				}
				
			}
		
			return State.Success;
		}
	}
}
