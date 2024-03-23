using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CartControl
{
	public class CartState_Drifting : CartState
	{
		private float m_driftingTimer;


		public override void OnEnter()
		{
			Debug.LogWarning("current state: DRIFTING");
			m_cartStateMachine.CanDrift = false;
			m_driftingTimer = 0;
		}

		public override void OnUpdate()
		{
			m_driftingTimer += Time.deltaTime;
			if(m_driftingTimer >= m_cartStateMachine.DriftTimeToEarnBoost)
			{
				Debug.Log("CAN BOOST");
				m_cartStateMachine.CanBoost = true;
			}
		}

		public override void OnFixedUpdate()
		{
			m_cartStateMachine.CartMovement.Move(m_cartStateMachine.DriftingAcceleration, m_cartStateMachine.DriftingDrag, m_cartStateMachine.MaxSpeed);
			m_cartStateMachine.CartMovement.UpdateOrientation(m_cartStateMachine.DriftingRotatingSpeed + (Mathf.Abs(m_cartStateMachine.BackwardPressedPercent) * m_cartStateMachine.AddedRotatingSpeedWhenBreaking));
		}

		public override void OnExit()
		{
			
		}

		public override bool CanEnter(IState currentState)
		{
			if (m_cartStateMachine.CanDrift)
			{
				return true;
			}

			if (m_cartStateMachine.MinimumSpeedToDrift < m_cartStateMachine.m_cartRB.velocity.magnitude
				&& m_cartStateMachine.BackwardPressedPercent > GameConstants.DEADZONE
				&& Mathf.Abs(m_cartStateMachine.SteeringValue) > GameConstants.DEADZONE)
			{
				return true;
			}

			return false;
		}

		public override bool CanExit()
		{
			if (Mathf.Abs(m_cartStateMachine.LocalVelocity.x) < GameConstants.DEADZONE)
			{
				return true;
			}
			return false;
		}
	}
}