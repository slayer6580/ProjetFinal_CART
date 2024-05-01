using BoxSystem;
using CartControl;

namespace BehaviourTree
{
	public class StealAttack : LeafNode
	{
		public int m_nmOfItemToSteal;
		protected override void OnStart()
		{
		
		}

		protected override void OnStop()
		{
			
		}

		protected override State OnUpdate()
		{
			for(int i = 0; i < m_nmOfItemToSteal; i++)
			{
				m_blackboard.m_thisClient.gameObject.GetComponentInChildren<GrabItemTrigger>().StealItemFromOtherTower(m_blackboard.m_chosenPathListCopy[0].gameObject.GetComponentInChildren<TowerBoxSystem>());
			}
			
			return State.Success;
		}
	}
}
