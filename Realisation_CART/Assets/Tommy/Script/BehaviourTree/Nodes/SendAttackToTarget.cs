using CartControl;

namespace BehaviourTree
{
	public class SendAttackToTarget : LeafNode
	{
		protected override void OnStart()
		{
		
		}

		protected override void OnStop()
		{
			
		}

		protected override State OnUpdate()
		{
			m_blackboard.m_cartStateMachine.LastClientCollisionWith = null;
			m_blackboard.m_chosenPathListCopy[0].gameObject.GetComponent<CartStateMachine>().WasAttacked();
			return State.Success;
		}
	}
}
