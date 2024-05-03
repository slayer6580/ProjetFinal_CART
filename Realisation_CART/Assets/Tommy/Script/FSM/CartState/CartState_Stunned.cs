using BoxSystem;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using UnityEngine.Rendering;

namespace CartControl
{
	
    public class CartState_Stunned : CartState
	{
		private const float TIME_TO_BE_STUNNED = 1;
		private const float MIN_TIME_BETWEEN_STUN = 2;

		private Vector3 m_shakePos = Vector3.zero;
		private float m_shakeSpeed = 10f;
		private float m_shakeAmount = 0.05f;
		private float m_lastStunTime = 0;
		private float m_stunTimer;
		private bool m_hitWithoutSteal;

		private float m_startRotation;
		private float m_endRotation;

		public override void OnEnter()
		{
			m_startRotation = m_cartStateMachine.ParentOfAllVisual.transform.eulerAngles.y;
			m_endRotation = m_startRotation + 360;

			m_hitWithoutSteal = false;
			m_stunTimer = 0;

			ManageAnimation();

			if (m_cartStateMachine.m_showLogStateChange)
			{
				Debug.LogWarning("current state: STUNNED");
			}

			StealItems();

			m_cartStateMachine.CartRB.velocity = Vector3.zero;
			m_cartStateMachine.CartRB.AddForce(m_cartStateMachine.CollisionOppositeDirection * 500, ForceMode.Impulse);

		}

		public void StealItems()
		{
			/*
			Vector3 colliderDir = new Vector3(m_cartStateMachine.LastClientCollisionWith.transform.position.x,
											m_cartStateMachine.ParentOfAllVisual.gameObject.transform.position.y,
											m_cartStateMachine.LastClientCollisionWith.transform.position.z)
				- m_cartStateMachine.gameObject.transform.position;

			Vector3 forward = m_cartStateMachine.ParentOfAllVisual.gameObject.transform.forward;
			float angle = Vector3.SignedAngle(colliderDir, forward, Vector3.up);
			*/
			float angle = Vector3.Dot(m_cartStateMachine.LastClientCollisionWith.transform.forward, m_cartStateMachine.transform.forward);
			//if 1 == regarde meme direction
			//if 0 == 90' degré
			//if -1 == 180' degré
			int nmOfItemToSteal = 0;

			Debug.Log(" Angle: " + angle);
			if (angle < 0.707 && angle > -0.707)
			{

				//Steal more items if hit in good angle
				nmOfItemToSteal += (int)Mathf.Lerp(1, 10, (45 - angle / 45));

				//Steal more items depending of speed collision
				nmOfItemToSteal += (int)Mathf.Lerp(1, 10, (m_cartStateMachine.LocalVelocity.z / 10));

				Debug.Log(m_cartStateMachine.gameObject.transform.name + " steal: " + nmOfItemToSteal);

				for (int i = 0; i < nmOfItemToSteal; i++)
				{
					m_cartStateMachine.gameObject.GetComponentInChildren<GrabItemTrigger>().StealItemFromOtherTower(m_cartStateMachine.LastClientCollisionWith.gameObject.GetComponentInChildren<TowerBoxSystem>());
				}
			}
			else
			{
				m_hitWithoutSteal = true;
			}
		}
		public void ManageAnimation()
		{
			m_cartStateMachine.HumanAnimCtrlr.SetBool("Stun", true);
		}

		public override void OnUpdate()
		{
			m_stunTimer += Time.deltaTime;


			if (m_hitWithoutSteal && m_cartStateMachine.LastClientCollisionWith.GetComponent<CartStateMachine>().LocalVelocity.z > 7)
			{
				m_shakePos = m_cartStateMachine.Cart.transform.position;
				m_shakePos.x += Mathf.Sin(Time.time * m_shakeSpeed) * m_shakeAmount;
				m_shakePos.z += Mathf.Sin(Time.time * m_shakeSpeed) * m_shakeAmount;

				m_cartStateMachine.Cart.transform.position = m_shakePos;


				if (m_stunTimer < TIME_TO_BE_STUNNED)
				{
					float yRotation = Mathf.Lerp(m_startRotation, m_endRotation, m_stunTimer / TIME_TO_BE_STUNNED) % 360.0f;
					m_cartStateMachine.ParentOfAllVisual.transform.eulerAngles = new Vector3(m_cartStateMachine.ParentOfAllVisual.transform.eulerAngles.x, yRotation, m_cartStateMachine.ParentOfAllVisual.transform.eulerAngles.z);
	
				}

			}
		}

		public override void OnFixedUpdate()
		{
		
		}

		public override void OnExit()
		{
			m_cartStateMachine.ParentOfAllVisual.transform.eulerAngles = new Vector3(m_cartStateMachine.ParentOfAllVisual.transform.eulerAngles.x, m_startRotation, m_cartStateMachine.ParentOfAllVisual.transform.eulerAngles.z);
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