using UnityEngine;

namespace CartControl
{
	public class CartState_Moving2 : CartState2
	{
		private float m_turningTimer;
		private float m_steerDirection;

		public override void OnEnter()
		{
			Debug.LogWarning("current state: MOVING");
		}

		public override void OnUpdate()
		{
			if (m_cartStateMachine.AutoDriftWhenTurning)
			{
				//Increase timer only when turning at maximum value
				if (Mathf.Abs(m_cartStateMachine.SteeringValue) > (1 - GameConstants.DEADZONE))
				{
					if (m_steerDirection == 0)
					{
						m_steerDirection = m_cartStateMachine.SteeringValue;
					}
					//Increase only if turning in the same direction,else reset everything
					if (m_steerDirection == m_cartStateMachine.SteeringValue)
					{
						m_turningTimer += Time.deltaTime;
					}
					else
					{
						m_steerDirection = 0;
						m_turningTimer = 0;
					}

					//Activate the possibility to change State
					if (m_turningTimer >= m_cartStateMachine.TurningTimeBeforeDrift)
					{
						m_cartStateMachine.CanDrift = true;
					}
				}
			}

			//For animation:
			//Is only pressing break
			if ((m_cartStateMachine.BackwardPressedPercent > GameConstants.DEADZONE) && (m_cartStateMachine.LocalVelocity.z > 0))
			{
				//m_cartStateMachine.HumanAnimCtrlr.SetBool("Breaking", true);
			}
			else
			{
				//m_cartStateMachine.HumanAnimCtrlr.SetBool("Breaking", false);
			}
		}

		public override void OnFixedUpdate()
		{
			m_cartStateMachine.CartMovement.Move(m_cartStateMachine.Acceleration, m_cartStateMachine.TurningDrag, m_cartStateMachine.MaxSpeed);
			m_cartStateMachine.CartMovement.UpdateOrientation(m_cartStateMachine.MovingRotatingSpeed);
		}

		public override void OnExit()
		{
			//For Animation
			//m_cartStateMachine.HumanAnimCtrlr.SetBool("Breaking", false);
		}

		public override bool CanEnter(IState currentState)
		{		
			if(currentState is CartState_Boosting)
			{
				return m_cartStateMachine.IsBoosting == false;
			}
			if (currentState is CartState_Drifting)
			{
				return m_cartStateMachine.IsDrifting == false;
			}

			if (m_cartStateMachine.CanBoost)
			{
				return false;
			}
		
			if (m_cartStateMachine.ForwardPressedPercent < GameConstants.DEADZONE
				&& m_cartStateMachine.BackwardPressedPercent < GameConstants.DEADZONE
				&& m_cartStateMachine.CartRB.velocity.magnitude < GameConstants.DEADZONE)
			{
				return false;
			}
			return true;			
		}

		public override bool CanExit()
		{
			return true;
		}
	}
}