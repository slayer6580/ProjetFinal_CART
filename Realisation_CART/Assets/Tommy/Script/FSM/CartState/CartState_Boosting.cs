using UnityEngine;

namespace CartControl
{
    public class CartState_Boosting : CartState
	{
		private float boostingTimer;
		private IState m_comingFromState;

		public override void OnEnter()
		{
			Debug.LogWarning("current state: BOOSTING");
			m_cartStateMachine.IsBoosting = true;
			m_cartStateMachine.CanBoost = false;

			//For Animation
			m_cartStateMachine.m_humanAnimator.SetBool("Boosting", true);

			//Some value must not be reset when coming from Stopped State
			if (m_comingFromState is CartState_Stopped)
			{
				return;
			}
			boostingTimer = 0;		
		}

		public override void OnUpdate()
		{
			boostingTimer += Time.deltaTime;

			//For animation
			if(boostingTimer >= m_cartStateMachine.BoostingTime - 0.5f)
			{
				m_cartStateMachine.m_humanAnimator.SetBool("Boosting", false);

				if (m_cartStateMachine.m_humanAnimator.GetCurrentAnimatorStateInfo(0).IsName("JumpingFeetOnCartReverse"))
				{				
					float weightByTime = Mathf.Clamp(m_cartStateMachine.m_humanAnimator.GetCurrentAnimatorStateInfo(0).normalizedTime, 0, 1);		
					m_cartStateMachine.m_feetOnCartRig.weight = 1 - weightByTime;
				}
			}
			else
			{
				if (m_cartStateMachine.m_humanAnimator.GetCurrentAnimatorStateInfo(0).IsName("JumpingFeetOnCart"))
				{
					float weightByTime = Mathf.Clamp(m_cartStateMachine.m_humanAnimator.GetCurrentAnimatorStateInfo(0).normalizedTime, 0, 1);
					m_cartStateMachine.m_feetOnCartRig.weight = weightByTime;
				}
			}

			
			
			

			if (boostingTimer >= m_cartStateMachine.BoostingTime)
			{
				m_cartStateMachine.IsBoosting = false;
			}

		}

		public override void OnFixedUpdate()
		{
			m_cartStateMachine.CartMovement.Move(m_cartStateMachine.BoostingAcceleration, m_cartStateMachine.BoostingTurnDrag, m_cartStateMachine.BoostingMaxSpeed);
			m_cartStateMachine.CartMovement.UpdateOrientation(m_cartStateMachine.MovingRotatingSpeed);
		}

		public override void OnExit()
		{
			m_cartStateMachine.IsBoosting = false;

			//For Animation
			m_cartStateMachine.m_humanAnimator.SetBool("Boosting", false);
			m_cartStateMachine.m_feetOnCartRig.weight = 0;

		}

		public override bool CanEnter(IState currentState)
		{
			if(currentState is CartState_Drifting)
			{
				return m_cartStateMachine.IsDrifting == false;
			}
			m_comingFromState = currentState;
			return m_cartStateMachine.CanBoost;
		}

		public override bool CanExit()
		{
			return true;
		}
	}
}

