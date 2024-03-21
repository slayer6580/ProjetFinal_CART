using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CartStateMachine : StateMachine<CartState>
{
	protected override void Start()
	{
		foreach(CartState state in m_possibleStates)
		{
			state.OnStart(this);
		}

		base.Start();
	}

	protected override void CreatePossibleStateList() 
	{
		m_possibleStates.Add(new CartState_Idle());
	}
}
