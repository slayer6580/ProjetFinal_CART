using UnityEngine;

namespace CartControl
{
	public class CartState_Idle2 : CartState2
	{
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
			if(currentState is CartState_TurnInPlace)
			{
				return (m_cartStateMachine.SteeringValue < 0 + GameConstants.DEADZONE && m_cartStateMachine.SteeringValue > 0 - GameConstants.DEADZONE);
				
			}

			if(m_cartStateMachine.CanBoost)
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