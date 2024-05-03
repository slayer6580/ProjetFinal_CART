using Manager;
using UnityEngine;
using static Manager.AudioManager;

namespace CartControl
{
	public class CartState_Drifting : CartState
	{
		private float m_driftingTimer;
		private float m_boostPercentageObtain;
		private IState m_comingFromState;
		private int m_driftDirection;
		private int m_audioSourceIndex;

		public override void OnEnter()
		{
			if (m_cartStateMachine.m_showLogStateChange)
			{
				Debug.LogWarning("current state: DRIFTING");
			}

			//SFX
			_AudioManager.PlaySoundEffectsOneShot(ESound.DriftBegin, m_cartStateMachine.transform.position, 0.25f);
			m_audioSourceIndex = _AudioManager.PlaySoundEffectsLoopOnTransform(ESound.DriftLoop, m_cartStateMachine.transform);
			
			m_cartStateMachine.ForceStartDrift = false;
			m_cartStateMachine.IsDrifting = true;

			m_cartStateMachine.HumanAnimCtrlr.SetBool("IsDrifting", true);
			m_cartStateMachine.GrindVfx.PlayVfx();

			//Some value must not be reset when coming from Stopped State
			if (m_comingFromState is CartState_Stopped)
			{
				return;
			}
			m_driftingTimer = 0;
			m_boostPercentageObtain = 0;
			m_driftDirection = 0;

		}

		public override void OnUpdate()
		{
			if(m_driftDirection == 0)
			{
				if(m_cartStateMachine.SteeringValue > 0 + GameConstants.DEADZONE)
				{
					m_driftDirection = 1;
				}
				else if(m_cartStateMachine.SteeringValue < 0 - GameConstants.DEADZONE)
				{
					m_driftDirection = -1;
				}
			}

			if(m_driftDirection == -1)
			{
				m_cartStateMachine.DriftSteeringValue = Mathf.Clamp(m_cartStateMachine.SteeringValue - 1f, -1.5f,-0.5f);
			}
			if (m_driftDirection == 1)
			{
				m_cartStateMachine.DriftSteeringValue = Mathf.Clamp(m_cartStateMachine.SteeringValue + 1f, 0.5f, 1.5f);
			}

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

			ManageAnimation();
			ManageSfx();

			//Exit drift condition
			if (m_cartStateMachine.DriftPressed == 0 || m_cartStateMachine.LocalVelocity.z < m_cartStateMachine.MinimumSpeedToAllowDrift)
			{
				m_cartStateMachine.IsDrifting = false;
			}

			if (Mathf.Abs(m_cartStateMachine.LocalVelocity.x) > 1)
			{
				m_cartStateMachine.BoxForce.AddConstantForceToBox(m_cartStateMachine.LocalVelocity.x * m_cartStateMachine.TowerPushForceWhenDrifting);
			}

		}

		public void ManageAnimation()
		{
			if (m_cartStateMachine.HumanAnimCtrlr.GetCurrentAnimatorStateInfo(0).IsName("JumpingFeetOnCart"))
			{
				float weightByTime = Mathf.Clamp(m_cartStateMachine.HumanAnimCtrlr.GetCurrentAnimatorStateInfo(0).normalizedTime, 0, 1);
				m_cartStateMachine.FeetOnCartRig.weight = weightByTime;
			}
		}
		public void ManageSfx()
		{
			_AudioManager.ModifySound(
				m_audioSourceIndex,
				ESoundModification.Pitch,
				Mathf.Lerp(0.6f, 1.45f, m_cartStateMachine.LocalVelocity.magnitude / m_cartStateMachine.MaxSpeedUpgrades));

			_AudioManager.ModifySound(
				m_audioSourceIndex,
				ESoundModification.Volume,
				Mathf.Lerp(0f, 1f, m_cartStateMachine.LocalVelocity.magnitude / m_cartStateMachine.MaxSpeedUpgrades));
		}

		public override void OnFixedUpdate()
		{
			m_cartStateMachine.CartMovement.Move(m_cartStateMachine.DriftingAcceleration, m_cartStateMachine.DriftingDrag, m_cartStateMachine.MaxSpeedUpgrades);
			m_cartStateMachine.CartMovement.UpdateOrientation(m_cartStateMachine.DriftingRotatingSpeedUpgrades + (Mathf.Abs(m_cartStateMachine.BackwardPressedPercent) * m_cartStateMachine.AddedRotatingSpeedWhenBreaking), m_cartStateMachine.DriftSteeringValue);
		}

		public override void OnExit()
		{
			_AudioManager.StopSoundEffectsLoop(m_audioSourceIndex);

			m_cartStateMachine.GrindVfx.StopVfx();
			m_cartStateMachine.DriftSteeringValue = 0;

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

			if (m_cartStateMachine.ForceStartDrift)
			{
				return true;
			}

			if(currentState is CartState_Moving)
			{
				return m_cartStateMachine.DriftPressed > 0 && m_cartStateMachine.LocalVelocity.z >= m_cartStateMachine.MinimumSpeedToAllowDrift;
			}
			
			return false;
		}

		public override bool CanExit()
		{			
			return true;
		}
	}
}