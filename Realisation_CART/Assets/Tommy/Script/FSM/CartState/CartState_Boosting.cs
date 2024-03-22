using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CartControl
{
    public class CartState_Boosting : CartState
	{
		private float boostingTimer;

		public override void OnEnter()
		{
			Debug.LogWarning("current state: BOOSTING");
			m_cartStateMachine.CanBoost = false;
			boostingTimer = 0;
		}

		public override void OnUpdate()
		{
			boostingTimer += Time.deltaTime;
			
		}

		public override void OnFixedUpdate()
		{
			m_cartStateMachine.CartMovement.Move(m_cartStateMachine.BoostingAcceleration, m_cartStateMachine.BoostingTurnDrag, m_cartStateMachine.BoostingMaxSpeed);
			m_cartStateMachine.CartMovement.UpdateOrientation(m_cartStateMachine.MovingRotatingSpeed);
		}

		public override void OnExit()
		{

		}

		public override bool CanEnter(IState currentState)
		{
			return m_cartStateMachine.CanBoost;
		}

		public override bool CanExit()
		{
			return boostingTimer >= m_cartStateMachine.BoostingTime;
		}
	}
}

