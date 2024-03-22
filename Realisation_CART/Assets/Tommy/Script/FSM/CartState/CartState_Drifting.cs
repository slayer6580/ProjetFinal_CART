using System.Collections;
using System.Collections.Generic;
using UnityEngine;


//m_cartStateMachine;
public class CartState_Drifting : CartState
{
	private float tempDragValue;
	private float tempTurnValue;
	private float tempDriftAccValue;

	public override void OnEnter()
	{
		m_cartStateMachine.CanDrift = false;
		Debug.LogWarning("current state: DRIFTING");
		
		tempDragValue = m_cartStateMachine.TurningDrag;
		m_cartStateMachine.TurningDrag = m_cartStateMachine.DriftingDrag;

		tempTurnValue = m_cartStateMachine.MovingRotatingSpeed;
		m_cartStateMachine.MovingRotatingSpeed = m_cartStateMachine.DriftingRotatingSpeed;

		tempDriftAccValue = m_cartStateMachine.Acceleration;
		m_cartStateMachine.Acceleration = m_cartStateMachine.DriftingAcceleration;
	}

	public override void OnUpdate()
	{
		m_cartStateMachine.MovingRotatingSpeed = m_cartStateMachine.DriftingRotatingSpeed + (Mathf.Abs(m_cartStateMachine.BackwardPressedPercent) * m_cartStateMachine.AddedRotatingSpeedWhenBreaking);
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
		//Reset all values
		m_cartStateMachine.TurningDrag = tempDragValue;
		m_cartStateMachine.MovingRotatingSpeed = tempTurnValue;
		m_cartStateMachine.Acceleration = tempDriftAccValue;
	}

	public override bool CanEnter(IState currentState)
	{
		if (m_cartStateMachine.CanDrift)
		{
			return true;
		}

		if (m_cartStateMachine.MinimumSpeedToDrift < m_cartStateMachine.m_cartRB.velocity.magnitude
			&& m_cartStateMachine.BackwardPressedPercent > 0.1f
			&& Mathf.Abs(m_cartStateMachine.SteeringValue) > 0.1f)
		{
			return true;
		}

		return false;
	}

	public override bool CanExit()
	{
		if (Mathf.Abs(m_cartStateMachine.LocalVelocity.x) < 0.1f)
		{
			return true;
		}
		return false;
	}

}

