using CartControl;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CartControl
{
    public class CartState_TurnInPlace : CartState
	{
		public override void OnEnter()
		{
			Debug.LogWarning("current state: TURN IN PLACE");

			m_cartStateMachine.HumanAnimCtrlr.SetBool("TurnInPlace", true);
		}

		public override void OnUpdate()
		{
			m_cartStateMachine.HumanAnimCtrlr.SetFloat("SteeringValue", m_cartStateMachine.SteeringValue);
		}

		public override void OnFixedUpdate()
		{
			m_cartStateMachine.CartMovement.UpdateOrientation(m_cartStateMachine.IdleRotatingSpeed);
		}

		public override void OnExit()
		{
			m_cartStateMachine.HumanAnimCtrlr.SetBool("TurnInPlace", false);
		}

		public override bool CanEnter(IState currentState)
		{
			if(currentState is CartState_Idle)
			{
				if(m_cartStateMachine.SteeringValue > 0 + GameConstants.DEADZONE || m_cartStateMachine.SteeringValue < 0 - GameConstants.DEADZONE)
				{
					return true;
				}
			}
			return false;
		}

		public override bool CanExit()
		{
			if(m_cartStateMachine.SteeringValue < 0 + GameConstants.DEADZONE && m_cartStateMachine.SteeringValue > 0 - GameConstants.DEADZONE)
			{
				return true;
			}

			if(m_cartStateMachine.ForwardPressedPercent > 0 + GameConstants.DEADZONE || m_cartStateMachine.BackwardPressedPercent > 0+ GameConstants.DEADZONE)
			{
				return true;
			}
			return false;
		}

	}
}