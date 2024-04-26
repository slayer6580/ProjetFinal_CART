namespace BehaviourTree
{
	public class FindCashRegisterPath : LeafNode
	{
		protected override void OnStart()
		{		
		}

		protected override void OnStop()
		{	
		}

		protected override State OnUpdate()
		{
			m_blackboard.m_chosenPathListCopy.Clear();
			m_blackboard.m_chosenPathListCopy.Add(m_blackboard.m_cashRegister);

			return State.Success;
			
		}
	}
}
