using BehaviourTree;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DiscountDelirium
{
	public class CreatePathNote : LeafNode
	{
		protected override void OnStart()
		{
			if(m_blackboard.m_path.Count == 0)
			{
				m_blackboard.m_path.Add(GameObject.Find("PathTarget_01").gameObject.transform.position);
				m_blackboard.m_path.Add(GameObject.Find("PathTarget_02").gameObject.transform.position);
				m_blackboard.m_path.Add(GameObject.Find("PathTarget_03").gameObject.transform.position);
			}
			
		}

		protected override void OnStop()
		{
			
		}

		protected override State OnUpdate()
		{
			return State.Success;
		}
	}
}
