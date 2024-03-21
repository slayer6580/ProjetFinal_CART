using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CartState : IState
{
	private StateMachine<CartState> m_cartStateMachine;

	public virtual void OnStart(StateMachine<CartState> cartStateMachine)
	{
		m_cartStateMachine = cartStateMachine;
	}

	public virtual void OnEnter()
	{
		throw new System.NotImplementedException();
	}

	public virtual void OnUpdate()
	{
		throw new System.NotImplementedException();
	}

	public virtual void OnExit()
	{
		throw new System.NotImplementedException();
	}

	public virtual bool CanEnter(IState currentState)
	{
		throw new System.NotImplementedException();
	}

	public virtual bool CanExit()
	{
		throw new System.NotImplementedException();
	}

	

	

	
}
