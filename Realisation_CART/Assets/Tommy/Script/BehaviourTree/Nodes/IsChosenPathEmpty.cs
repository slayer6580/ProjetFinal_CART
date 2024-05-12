using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//using static Unity.VisualScripting.Metadata;

namespace BehaviourTree
{
    public class IsChosenPathEmpty : CompositeNode
    {
		protected override void OnStart()
		{
		}

		protected override void OnStop()
		{
		}

		protected override State OnUpdate()
		{
			if (m_blackboard.m_chosenPathListCopy.Count == 0)
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
