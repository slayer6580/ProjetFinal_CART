using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CartControl
{
    public class CartState_Stunned : CartState
	{
		public override void OnEnter()
		{
			if (m_cartStateMachine.m_showLogStateChange)
			{
				Debug.LogWarning("current state: STUNNED");
			}
			
		}

		public override void OnUpdate()
		{
			Debug.Log("HOW DID YOU GET HERE! ABORT!!");
		}

		public override void OnFixedUpdate()
		{
			
		}

		public override void OnExit()
		{

		}

		public override bool CanEnter(IState currentState)
		{
			return false;
		}

		public override bool CanExit()
		{
			return false;
		}

	}
}