using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BehaviourTree
{
	public class EveryXLoop : DecoratorNode
	{
		public int m_everyXLoop;
		private int m_loopDone = 0;
		protected override void OnStart()
		{
			m_loopDone++;
		}

		protected override void OnStop()
		{
			
		}

		protected override State OnUpdate()
		{

			if(m_loopDone >= m_everyXLoop)
			{
				if(m_child.Update() == State.Success)
				{
					m_loopDone = 0;
				}
			}
		
			return State.Success;
		}
	}
}
