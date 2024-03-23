using CartControl;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CartControl
{
    public class CartState_Stopped : CartState
	{
		private Vector3 m_tempCartVelocity;

		public override void OnEnter()
		{
			Debug.LogWarning("current state: STOPPED");
			m_tempCartVelocity = m_cartStateMachine.m_cartRB.velocity;
			Debug.Log("m_tempCartVelocity AAAA: " + m_tempCartVelocity);
			m_cartStateMachine.CartMovement.StopMovement();
			m_cartStateMachine.m_cartRB.isKinematic = true;

			m_cartStateMachine.m_gameplayCamera.SetActive(false);
			m_cartStateMachine.m_camBrain.enabled = false;
			
		}

		public override void OnUpdate()
		{

		}

		public override void OnFixedUpdate()
		{
			
		}

		public override void OnExit()
		{
			m_cartStateMachine.m_cartRB.isKinematic = false;
			Debug.Log("m_tempCartVelocity BBB: " + m_tempCartVelocity);
			m_cartStateMachine.m_cartRB.velocity = m_tempCartVelocity;

			m_cartStateMachine.m_camBrain.enabled = true;
			m_cartStateMachine.m_gameplayCamera.SetActive(true);

		}

		public override bool CanEnter(IState currentState)
		{		
			return m_cartStateMachine.IsPaused;
		}

		public override bool CanExit()
		{
			return !m_cartStateMachine.IsPaused;
		}

	}
}

