using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CartState : IState
{
	protected CartStateMachine m_cartStateMachine;

	public virtual void OnStart(CartStateMachine cartStateMachine)
	{
		Debug.Log("ON START");
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

	public virtual void OnFixedUpdate()
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
