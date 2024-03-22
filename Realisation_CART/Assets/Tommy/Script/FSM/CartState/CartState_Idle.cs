using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class CartState_Idle : CartState
{
	//m_cartStateMachine;

	public override void OnEnter()
	{
		Debug.LogWarning("current state: IDLE");
	}

	public override void OnUpdate()
	{
		
	}

	public override void OnFixedUpdate()
	{
		m_cartStateMachine.Move();
		UpdateOrientation();
		
	}
	
	
	private void UpdateOrientation()
	{
		if(m_cartStateMachine.SteeringValue != 0)
		{
			m_cartStateMachine.m_cart.transform.Rotate(Vector3.up 
				* m_cartStateMachine.IdleRotatingSpeed
				* m_cartStateMachine.SteeringValue
				* Time.fixedDeltaTime);
		}
	}
	
	public override void OnExit()
	{

	}

	public override bool CanEnter(IState currentState)
	{
		if(m_cartStateMachine.ForwardPressedPercent < 0.1f 
			&& m_cartStateMachine.BackwardPressedPercent < 0.1f
			&& m_cartStateMachine.m_cartRB.velocity.magnitude < 0.5f)
		{
			return true;
		}
		return false;
	}

	public override bool CanExit()
	{
		return m_cartStateMachine.m_cartRB.velocity.magnitude > 0.5f;
	}

}
