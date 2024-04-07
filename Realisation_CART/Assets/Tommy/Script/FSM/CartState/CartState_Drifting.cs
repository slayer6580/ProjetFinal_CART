using UnityEngine;

namespace CartControl
{
	public class CartState_Drifting : CartState
	{
		private float m_driftingTimer;
		private float m_boostPercentageObtain;
		private IState m_comingFromState;

		public override void OnEnter()
		{
			if (m_cartStateMachine.m_showLogStateChange)
			{
				Debug.LogWarning("current state: DRIFTING");
			}
			

			m_cartStateMachine.CanDrift = false;
			m_cartStateMachine.IsDrifting = true;

			m_cartStateMachine.HumanAnimCtrlr.SetBool("IsDrifting", true);
			//Some value must not be reset when coming from Stopped State
			if (m_comingFromState is CartState_Stopped)
			{
				return;
			}
			m_driftingTimer = 0;
			m_boostPercentageObtain = 0;


		}

		public override void OnUpdate()
		{
			m_driftingTimer += Time.deltaTime;

			//After minimum drifting time obtain. Calculate how much boost to give
			if(m_driftingTimer >= m_cartStateMachine.DriftTimeToEarnMinBoost)
			{
				if(m_boostPercentageObtain < 1)
				{
					m_boostPercentageObtain = (m_driftingTimer - m_cartStateMachine.MinBoostTime) / (m_cartStateMachine.MaxBoostTime - m_cartStateMachine.MinBoostTime);			
				}
				else
				{
					m_boostPercentageObtain = 1;
				}
				m_cartStateMachine.CanBoost = true;
			}

			if (Mathf.Abs(m_cartStateMachine.LocalVelocity.x) < GameConstants.DEADZONE)
			{
				m_cartStateMachine.IsDrifting = false;
			}

			//For animation
				//Drift
				//m_cartStateMachine.HumanAnimCtrlr.SetFloat("DriftingValue", Mathf.Clamp(m_cartStateMachine.LocalVelocity.x / 30, -1, 1));
			if (m_cartStateMachine.HumanAnimCtrlr.GetCurrentAnimatorStateInfo(0).IsName("JumpingFeetOnCart"))
			{
				float weightByTime = Mathf.Clamp(m_cartStateMachine.HumanAnimCtrlr.GetCurrentAnimatorStateInfo(0).normalizedTime, 0, 1);
				m_cartStateMachine.FeetOnCartRig.weight = weightByTime;
			}

		}

		public override void OnFixedUpdate()
		{
			m_cartStateMachine.CartMovement.Move(m_cartStateMachine.DriftingAcceleration, m_cartStateMachine.DriftingDrag, m_cartStateMachine.MaxSpeed);
			m_cartStateMachine.CartMovement.UpdateOrientation(m_cartStateMachine.DriftingRotatingSpeed + (Mathf.Abs(m_cartStateMachine.BackwardPressedPercent) * m_cartStateMachine.AddedRotatingSpeedWhenBreaking));
		}

		public override void OnExit()
		{
			//Calculate boosting time
			m_cartStateMachine.BoostingTime = m_cartStateMachine.MinBoostTime + (m_boostPercentageObtain * (m_cartStateMachine.MaxBoostTime - m_cartStateMachine.MinBoostTime));

			//For animation
			m_cartStateMachine.HumanAnimCtrlr.SetFloat("DriftingValue", 0);
			m_cartStateMachine.HumanAnimCtrlr.SetBool("IsDrifting", false);
			m_cartStateMachine.FeetOnCartRig.weight = 0;
		}

		public override bool CanEnter(IState currentState)
		{
			m_comingFromState = currentState;

			if (m_cartStateMachine.CanDrift)
			{
				return true;
			}

			if (m_cartStateMachine.MinimumSpeedToAllowDrift < m_cartStateMachine.CartRB.velocity.magnitude
				&& m_cartStateMachine.BackwardPressedPercent > GameConstants.DEADZONE
				&& Mathf.Abs(m_cartStateMachine.SteeringValue) > GameConstants.DEADZONE)
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