using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CartControl
{
    public class CartState_Stunned2 : CartState2
	{
		public override void OnEnter()
		{
			Debug.LogWarning("current state: STUNNED");
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