using BehaviourTree;
using BoxSystem;
using CartControl;
using DiscountDelirium;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

namespace BehaviourTree
{
    public class InitializeClient : LeafNode
	{
		protected override void OnStart()
		{
			m_blackboard.m_thisClient = GameObject.Find(m_blackboard.m_name);
			m_blackboard.m_cartStateMachine = m_blackboard.m_thisClient.GetComponent<CartStateMachine>();
			m_blackboard.m_navAgent = m_blackboard.m_thisClient.GetComponentInChildren<NavMeshAgent>();
			m_blackboard.m_possiblePathScript = m_blackboard.m_thisClient.GetComponent<ClientPathList>();
			m_blackboard.m_thisTower = m_blackboard.m_thisClient.transform.Find("Tower").GetComponent<TowerBoxSystem>();
			m_blackboard.m_cashRegister = GameObject.Find("AiCashRegisterTarget");
		}

		protected override void OnStop()
		{
		}

		protected override State OnUpdate()
		{
			if(m_blackboard.m_cartStateMachine != null)
			{
				return State.Success;
			}
			return State.Failure;
		}

    }
}
