using BehaviourTree;
using CartControl;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

namespace BehaviourTree
{
    public class InitializeClient : LeafNode
	{
		protected override void OnStart()
		{
			m_blackboard.m_thisClient = GameObject.Find(m_blackboard.m_name);
			m_blackboard.m_cartStateMachine = m_blackboard.m_thisClient.GetComponent<CartStateMachine>();

		}

		protected override void OnStop()
		{
		}

		protected override State OnUpdate()
		{
			if(m_blackboard.m_cartStateMachine != null)
			{
				Debug.Log("SUCESS");
				return State.Success;
			}
			Debug.Log("FALURE");
			return State.Failure;
		}

    }
}
