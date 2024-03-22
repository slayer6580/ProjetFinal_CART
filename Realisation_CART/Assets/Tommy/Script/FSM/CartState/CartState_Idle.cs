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
		if(m_cartStateMachine.m_steeringValue != 0)
		{
			m_cartStateMachine.m_cart.transform.Rotate(Vector3.up 
				* m_cartStateMachine.m_idleRotatingSpeed 
				* m_cartStateMachine.m_steeringValue
				* Time.deltaTime);
		}
	}
	
	public override void OnExit()
	{

	}

	public override bool CanEnter(IState currentState)
	{
		if(m_cartStateMachine.m_forwardPressedPercent < 0.1f 
			&& m_cartStateMachine.m_backwardPressedPercent < 0.1f
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
