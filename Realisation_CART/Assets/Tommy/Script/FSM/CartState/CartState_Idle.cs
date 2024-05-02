using UnityEngine;

namespace CartControl
{
	public class CartState_Idle : CartState
	{
		public override void OnEnter()
		{
			if (m_cartStateMachine.m_showLogStateChange)
			{
				Debug.LogWarning("current state: IDLE");
			}
			
		}

		public override void OnUpdate()
		{

		}

		public override void OnFixedUpdate()
		{
			m_cartStateMachine.CartMovement.Move(m_cartStateMachine.AccelerationUpgrades, m_cartStateMachine.TurningDrag, m_cartStateMachine.MaxSpeedUpgrades);
			m_cartStateMachine.CartMovement.UpdateOrientation(m_cartStateMachine.IdleRotatingSpeed, m_cartStateMachine.SteeringValue);
		}

		public override void OnExit()
		{

		}

		public override bool CanEnter(IState currentState)
		{
			if(currentState is CartState_TurnInPlace)
			{
				return (m_cartStateMachine.SteeringValue < 0 + GameConstants.DEADZONE && m_cartStateMachine.SteeringValue > 0 - GameConstants.DEADZONE);
				
			}

			if (currentState is CartState_Stunned)
			{
				return true;
			}

				if (m_cartStateMachine.CanBoost)
			{
				return false;
			}

			return (m_cartStateMachine.ForwardPressedPercent < GameConstants.DEADZONE
				&& m_cartStateMachine.BackwardPressedPercent < GameConstants.DEADZONE
				&& m_cartStateMachine.CartRB.velocity.magnitude < GameConstants.DEADZONE);
			
		}

		public override bool CanExit()
		{
			return true;
		}

	}
}