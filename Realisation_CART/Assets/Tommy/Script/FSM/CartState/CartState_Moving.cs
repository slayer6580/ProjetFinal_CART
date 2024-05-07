using Manager;
using UnityEngine;
using static Manager.AudioManager;

namespace CartControl
{
	public class CartState_Moving : CartState
	{
		private float m_turningTimer;
		private float m_steerDirection;
		private int m_audioSourceIndex;

		public override void OnEnter()
		{
			if (m_cartStateMachine.m_showLogStateChange)
			{
				Debug.LogWarning("current state: MOVING");
			}
			m_audioSourceIndex = _AudioManager.PlaySoundEffectsLoopOnTransform(ESound.CartRolling, m_cartStateMachine.transform);
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
						m_cartStateMachine.ForceStartDrift = true;
					}
				}
			}
			ManageAnimation();

		
			


			
			ManageSfx();
		}

		public void ManageAnimation()
		{
			//Is only pressing break
			if ((m_cartStateMachine.BackwardPressedPercent > GameConstants.DEADZONE) && (m_cartStateMachine.LocalVelocity.z > 0))
			{
				m_cartStateMachine.HumanAnimCtrlr.SetBool("Breaking", true);
			}
			else
			{
				m_cartStateMachine.HumanAnimCtrlr.SetBool("Breaking", false);
			}
		}

		public void ManageSfx()
		{
			_AudioManager.ModifySound(
				m_audioSourceIndex,
				ESoundModification.Pitch,
				Mathf.Lerp(0.8f, 2f, m_cartStateMachine.LocalVelocity.magnitude / m_cartStateMachine.MaxSpeedUpgrades));

			_AudioManager.ModifySound(
				m_audioSourceIndex,
				ESoundModification.Volume,
				Mathf.Lerp(0f, 1f, m_cartStateMachine.LocalVelocity.magnitude / m_cartStateMachine.MaxSpeedUpgrades));
		}
		public override void OnFixedUpdate()
		{
			m_cartStateMachine.CartMovement.Move(m_cartStateMachine.AccelerationUpgrades, m_cartStateMachine.TurningDrag, m_cartStateMachine.MaxSpeedUpgrades);
			m_cartStateMachine.CartMovement.UpdateOrientation(m_cartStateMachine.MovingRotatingSpeedUpgrades, m_cartStateMachine.SteeringValue);
		}

		public override void OnExit()
		{
			_AudioManager.StopSoundEffectsLoop(m_audioSourceIndex);

			//For Animation
			m_cartStateMachine.HumanAnimCtrlr.SetBool("Breaking", false);
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