using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

namespace CartControl
{
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
			m_cartStateMachine.CartMovement.Move(m_cartStateMachine.Acceleration, m_cartStateMachine.TurningDrag, m_cartStateMachine.MaxSpeed);
			m_cartStateMachine.CartMovement.UpdateOrientation(m_cartStateMachine.IdleRotatingSpeed);
		}

		public override void OnExit()
		{

		}

		public override bool CanEnter(IState currentState)
		{
			if(m_cartStateMachine.CanBoost)
			{
				return false;
			}

			if (m_cartStateMachine.ForwardPressedPercent < GameConstants.DEADZONE
				&& m_cartStateMachine.BackwardPressedPercent < GameConstants.DEADZONE
				&& m_cartStateMachine.m_cartRB.velocity.magnitude < GameConstants.DEADZONE)
			{
				return true;
			}

			return false;
		}

		public override bool CanExit()
		{
			return true;
		}

	}
}