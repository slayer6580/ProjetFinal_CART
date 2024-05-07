using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BehaviourTree
{
	public class ShouldIStopThePursuit : CompositeNode
	{

		protected override void OnStart()
		{
		}
		protected override void OnStop()
		{
			
		}

		protected override State OnUpdate()
		{


			if(m_blackboard.m_currentPursuitStartTime < Time.time - m_blackboard.m_pursuitTenacity)
			{
				//Debug.Log("PURSUIT TIME OVER");
				return m_children[0].Update();
			}
			else if(m_children.Count > 1)
			{
				Debug.Log("PURSUIT NOT OVER");
				return m_children[1].Update();
			}
			return State.Success;
		}
	}
}
