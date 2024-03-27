using UnityEngine;

namespace CartControl
{
    public class CartState_Stopped : CartState
	{
		private Vector3 m_tempCartVelocity;
		private IState m_comingFromState;

		public override void OnEnter()
		{
			Debug.LogWarning("current state: STOPPED");

			//Save the velocity
			m_tempCartVelocity = m_cartStateMachine.CartRB.velocity;
			
			//Stop all movement
			m_cartStateMachine.CartMovement.StopMovement();
			m_cartStateMachine.CartRB.isKinematic = true;
			m_cartStateMachine.VirtualCamera.SetActive(false);
			m_cartStateMachine.CamBrain.enabled = false;

			//If wanted, depending of the coming state, reactivate variables so we can get back to these State after this one
			if (!m_cartStateMachine.BackToIdleAfterStop)
			{
				if (m_comingFromState is CartState_Boosting)
				{
					m_cartStateMachine.CanBoost = true;
				}

				if (m_comingFromState is CartState_Drifting)
				{
					m_cartStateMachine.CanDrift = true;
				}
			}
				
		}

		public override void OnUpdate()
		{

		}

		public override void OnFixedUpdate()
		{
			
		}

		public override void OnExit()
		{
			m_cartStateMachine.CartRB.isKinematic = false;
			m_cartStateMachine.CamBrain.enabled = true;
			m_cartStateMachine.VirtualCamera.SetActive(true);

			//Reactivate movement as they was if wanted
			if (!m_cartStateMachine.BackToIdleAfterStop)
			{			
				m_cartStateMachine.CartRB.velocity = m_tempCartVelocity;			
			}
			
		}

		public override bool CanEnter(IState currentState)
		{
			m_comingFromState = currentState;
			return m_cartStateMachine.IsPaused;
		}

		public override bool CanExit()
		{
			return !m_cartStateMachine.IsPaused;
		}

	}
}

