using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BehaviourTree
{
	public class HasCollisionOccur : CompositeNode
	{
		protected override void OnStart()
		{
			
		}

		protected override void OnStop()
		{
			
		}

		protected override State OnUpdate()
		{
			if(m_blackboard.m_cartStateMachine.LastClientCollisionWith != null)
			{
				Debug.Log("Colision Occur");
				return m_children[0].Update();
			}

			else if(m_children.Count > 1)
			{
				Debug.Log("No Col Occur");
				return m_children[1].Update();
			}

			return State.Success;
		}
	}
}