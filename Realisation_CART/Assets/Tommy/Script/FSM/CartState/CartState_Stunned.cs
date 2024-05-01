using BoxSystem;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

namespace CartControl
{
	
    public class CartState_Stunned : CartState
	{
		private const float TIME_TO_BE_STUNNED = 2;
		private const float MIN_TIME_BETWEEN_STUN = 5;

		private Vector3 m_shakePos = Vector3.zero;
		private float m_shakeSpeed = 20f;
		private float m_shakeAmount = 0.05f;	
		private float m_lastStunTime = 0;
		private float m_stunTimer;
		private bool m_hitWithoutSteal;

		public override void OnEnter()
		{
			
			m_hitWithoutSteal = false;
			m_stunTimer = 0;

			if (m_cartStateMachine.m_showLogStateChange)
			{
				Debug.LogWarning("current state: STUNNED");
			}

			Vector3 colliderDir = new Vector3(m_cartStateMachine.LastClientCollisionWith.transform.position.x,
											m_cartStateMachine.gameObject.transform.position.y,
											m_cartStateMachine.LastClientCollisionWith.transform.position.z)
				- m_cartStateMachine.gameObject.transform.position;

			Vector3 forward = m_cartStateMachine.gameObject.transform.forward;
			float angle = Vector3.SignedAngle(colliderDir, forward, Vector3.up);

			int nmOfItemToSteal = 0;

			if (angle < 45 && angle > -45)
			{
				m_cartStateMachine.CartRB.velocity = Vector3.zero;

				//Steal more items if hit in good angle
				nmOfItemToSteal += (int)Mathf.Lerp(1, 10, (45 - angle / 45));

				//Steal more items depending of speed collision
				nmOfItemToSteal += (int)Mathf.Lerp(1, 10, (m_cartStateMachine.LocalVelocity.z / 10));


				for (int i = 0; i < nmOfItemToSteal; i++)
				{
					m_cartStateMachine.gameObject.GetComponentInChildren<GrabItemTrigger>().StealItemFromOtherTower(m_cartStateMachine.LastClientCollisionWith.gameObject.GetComponentInChildren<TowerBoxSystem>());
				}
			}
			else
			{
				m_hitWithoutSteal = true;
			}

			m_cartStateMachine.CartRB.AddForce(-m_cartStateMachine.CollisionOppositeDirection * 100, ForceMode.Impulse);

		}

		public void ManageAnimation()
		{
			m_cartStateMachine.HumanAnimCtrlr.SetBool("Stun", true);
		}

		public override void OnUpdate()
		{
			m_stunTimer += Time.deltaTime;
			ManageAnimation();
			if (m_hitWithoutSteal)
			{
				m_shakePos = m_cartStateMachine.Cart.transform.position;
				m_shakePos.x += Mathf.Sin(Time.time * m_shakeSpeed) * m_shakeAmount;
				m_shakePos.z += Mathf.Sin(Time.time * m_shakeSpeed) * m_shakeAmount;

				m_cartStateMachine.Cart.transform.position = m_shakePos;
				
			}

		}

		public override void OnFixedUpdate()
		{
			m_cartStateMachine.Cart.transform.Rotate(0, 200 * Time.fixedDeltaTime,0);
			//m_cartStateMachine.Cart.transform.rotation = Quaternion.AngleAxis(30, Vector3.up);
		}

		public override void OnExit()
		{
			m_cartStateMachine.HumanAnimCtrlr.SetBool("Stun", false);
			m_cartStateMachine.LastClientCollisionWith = null;
			m_lastStunTime = Time.time;
		}

		public override bool CanEnter(IState currentState)
		{
			if(m_cartStateMachine.LastClientCollisionWith != null)
			{
				if((m_lastStunTime + MIN_TIME_BETWEEN_STUN) < Time.time)
				{
					return true;
				}
				else
				{
					m_cartStateMachine.LastClientCollisionWith = null;
				}
				
			}
			return false;
		}

		public override bool CanExit()
		{
			if (m_hitWithoutSteal)
			{
				return m_stunTimer > TIME_TO_BE_STUNNED;
			}
			return m_stunTimer > TIME_TO_BE_STUNNED/2;
		}

	}
}