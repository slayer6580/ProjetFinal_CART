using UnityEngine;

namespace BehaviourTree
{
	public class CheckIfOwnTowerIsBigEnough : CompositeNode
	{
		public int m_minBoxRequired;
		public int m_maxBoxRequired;
		private int boxNeeded;

		protected override void OnStart()
		{
			boxNeeded = Random.Range(m_minBoxRequired, m_maxBoxRequired);
		}

		protected override void OnStop()
		{
			
		}

		protected override State OnUpdate()
		{
			if(m_blackboard.m_thisTower.GetBoxCount() >= boxNeeded && m_blackboard.m_chosenPathListCopy.Count == 0)
			{
				return m_children[0].Update();
			}
			else
			{
				if (m_children.Count > 1)
				{
					return m_children[1].Update();
				}
			}

			return State.Success;
		}
	}
}
