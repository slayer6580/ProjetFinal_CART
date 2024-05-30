using DiscountDelirium;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BehaviourTree
{
	public class WaitForGameStart : LeafNode
	{

		protected override void OnStart()
		{
			GetReadyState.OnGameStarted += GameStarted;
		}

		protected override void OnStop()
		{
		}

		public void GameStarted()
		{
			Debug.Log("GAME IS STARTED");
			m_blackboard.m_isGameStarted = true;
		}
		protected override State OnUpdate()
		{
			if (m_blackboard.m_isGameStarted)
			{
				return State.Success;
			}
			else
			{
				return State.Running;
			}
		}
	}
}
