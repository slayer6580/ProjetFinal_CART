using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CartControl
{
	public class CartMovement : MonoBehaviour
	{
		public CartStateMachine SM { private get; set; }

		public void Move(float acceleration, float turnDrag, float maxSpeed)
		{
			//Transform default velocity (which is global) to a local velocity
			SM.LocalVelocity = SM.m_cart.transform.InverseTransformDirection(SM.m_cartRB.velocity);

			//Check for movement input
			if (SM.ForwardPressedPercent > GameConstants.DEADZONE || SM.BackwardPressedPercent > GameConstants.DEADZONE)
			{
				if (maxSpeed > SM.LocalVelocity.z && -SM.MaxBackwardSpeed < SM.LocalVelocity.z)
				{
					SM.m_cartRB.GetComponent<Rigidbody>().AddForce(transform.forward
						* GameConstants.BASE_ADD_FORCE
						* acceleration
						* Time.fixedDeltaTime
						* (SM.ForwardPressedPercent - SM.BackwardPressedPercent)
					);
				}
			}

			//Manage drag when turning
			//If the cart is not ONLY going forward (it's going a bit sideway because it's turning) push in opposite direction to stabilize
			Vector3 sideToPush = -transform.right * Mathf.Clamp(SM.LocalVelocity.x, -1f, 1f);
			float pushForce = GameConstants.BASE_ADD_FORCE * acceleration * turnDrag;
			float pushMultiply = SM.TurningDragRelativeToJoystick.Evaluate(Mathf.Abs(SM.SteeringValue));

			SM.m_cartRB.GetComponent<Rigidbody>().AddForce(sideToPush * pushForce * pushMultiply * Time.fixedDeltaTime);
		}

		public void UpdateOrientation(float rotationSpeed)
		{
			if (SM.SteeringValue != 0)
			{
				SM.m_cart.transform.Rotate(Vector3.up
					* rotationSpeed
					* SM.SteeringValue
					* Time.fixedDeltaTime);
			}
		}

		public void StopMovement()
		{
			SM.m_cartRB.GetComponent<Rigidbody>().velocity = Vector3.zero;
		}
	}
}