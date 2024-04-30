using BoxSystem;
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
			m_blackboard.m_thisClient.gameObject.GetComponentInChildren<GrabItemTrigger>().StealItemFromOtherTower(m_blackboard.m_chosenPathListCopy[0].gameObject.GetComponentInChildren<TowerBoxSystem>());
			return State.Success;
		}
	}
}
