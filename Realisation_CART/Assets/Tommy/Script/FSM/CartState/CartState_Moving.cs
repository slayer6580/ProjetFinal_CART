using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CartState_Moving : CartState
{
	private float m_turningTimer;
	private float m_steerDirection;

	public override void OnEnter()
	{
		Debug.LogWarning("current state: MOVING");
	}

	public override void OnUpdate()
	{
		if(m_cartStateMachine.AutoDriftWhenTurning)
		{
			if(Mathf.Abs(m_cartStateMachine.SteeringValue) == 1)
			{
				if(m_steerDirection == 0)
				{
					m_steerDirection = m_cartStateMachine.SteeringValue;
				}
				if(m_steerDirection == m_cartStateMachine.SteeringValue)
				{
					m_turningTimer += Time.deltaTime;
				}
				else
				{
					m_steerDirection = 0;
					m_turningTimer = 0;
				}

				if(m_turningTimer >= m_cartStateMachine.TurningTimeBeforeDrift)
				{
					m_cartStateMachine.CanDrift = true;
				}
			}
		}
	}
	public override void OnFixedUpdate()
	{
		m_cartStateMachine.Move();
		UpdateOrientation();
	}

	private void UpdateOrientation()
	{
		if (m_cartStateMachine.SteeringValue != 0)
		{
			m_cartStateMachine.m_cart.transform.Rotate(Vector3.up
				* m_cartStateMachine.MovingRotatingSpeed
				* m_cartStateMachine.SteeringValue
				* Time.fixedDeltaTime);
		}
	}

	public override void OnExit()
	{

	}

	public override bool CanEnter(IState currentState)
	{
		if (m_cartStateMachine.ForwardPressedPercent < 0.1f
			&& m_cartStateMachine.BackwardPressedPercent < 0.1f
			&& m_cartStateMachine.m_cartRB.velocity.magnitude < 0.5f)
		{
			return false;
		}
		return true;
	}

	public override bool CanExit()
	{
		return true;
		//return m_cartStateMachine.m_cartRB.velocity.magnitude < 0.5f;
	}
}
