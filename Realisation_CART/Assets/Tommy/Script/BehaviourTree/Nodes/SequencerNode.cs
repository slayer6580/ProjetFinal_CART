namespace BehaviourTree
{
	public class SequencerNode : CompositeNode
	{
		private int m_current;

		protected override void OnStart()
		{
			m_current = 0;
		}

		protected override void OnStop()
		{		
		}

		protected override State OnUpdate()
		{
			var child = m_children[m_current];

			switch (child.Update())
			{
				case State.Running:
					return State.Running;
				case State.Failure:
					return State.Failure;
				case State.Success:
					m_current++;
					break;
			}

			return m_current == m_children.Count ? State.Success : State.Running;
		}
	}
}
