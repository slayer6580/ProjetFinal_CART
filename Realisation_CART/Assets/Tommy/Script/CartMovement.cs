using UnityEngine;

namespace CartControl
{
	public class CartMovement : MonoBehaviour
	{
		public CartStateMachine SM { private get; set; }

		public void Move(float acceleration, float turnDrag, float maxSpeed)
		{
			//Transform default velocity (which is global) to a local velocity
			SM.LocalVelocity = SM.Cart.transform.InverseTransformDirection(SM.CartRB.velocity);

			if(SM.CartRB.velocity.magnitude < maxSpeed)
			{
				//Check for movement input
				if (SM.ForwardPressedPercent > GameConstants.DEADZONE || SM.BackwardPressedPercent > GameConstants.DEADZONE)
				{
					if (maxSpeed > SM.LocalVelocity.z && -SM.MaxBackwardSpeed < SM.LocalVelocity.z)
					{
						SM.CartRB.GetComponent<Rigidbody>().AddForce(transform.forward
							* GameConstants.BASE_ADD_FORCE
							* acceleration
							* Time.fixedDeltaTime
							* (SM.ForwardPressedPercent - SM.BackwardPressedPercent)
						);
                    }
				}
			}
			
			//Manage drag when turning
			//If the cart is not ONLY going forward (it's going a bit sideway because it's turning) push in opposite direction to stabilize
			Vector3 sideToPush = -transform.right * Mathf.Clamp(SM.LocalVelocity.x, -1f, 1f);
			float pushForce = GameConstants.BASE_ADD_FORCE * acceleration * turnDrag;
			float pushMultiply = SM.TurningDragRelativeToJoystick.Evaluate(Mathf.Abs(SM.SteeringValue));

			SM.CartRB.GetComponent<Rigidbody>().AddForce(sideToPush * pushForce * pushMultiply * Time.fixedDeltaTime);
		}


		public void UpdateOrientation(float rotationSpeed, float steeringValue)
		{
		
			if (steeringValue != 0)
			{
				SM.Cart.transform.Rotate(Vector3.up
					* rotationSpeed
					* steeringValue
					* Time.fixedDeltaTime);
			}
		}

		public void StopMovement()
		{
			SM.CartRB.GetComponent<Rigidbody>().velocity = Vector3.zero;
		}
	}
}