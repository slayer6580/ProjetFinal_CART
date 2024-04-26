namespace BehaviourTree
{
	public class CreateAttackPath : LeafNode
	{
		protected override void OnStart()
		{	
		}

		protected override void OnStop()
		{		
		}

		protected override State OnUpdate()
		{
			m_blackboard.m_chosenPathListCopy.Add(m_blackboard.m_clientInSight[m_blackboard.m_chosenTarget].gameObject);
			return State.Success;
		}
	}
}
