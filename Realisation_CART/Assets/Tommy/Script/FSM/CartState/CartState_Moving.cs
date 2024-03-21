using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CartState_Moving : CartState
{
	public override void OnEnter()
	{
		Debug.LogWarning("current state: MOVING");
	}

	public override void OnUpdate()
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
		return true;
	}
}
