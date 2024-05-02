using UnityEngine;

namespace CartControl
{
	public class CartMovement : MonoBehaviour
	{
		public CartStateMachine SM { private get; set; }

		private void Start()
		{
			
		}
		//public void Move(float acceleration, float turnDrag, float maxSpeed)
		public void Move(float acceleration, float turnDrag, float maxSpeed, bool isClient, int clientID)
		{
            //if (isClient && clientID == 2) Debug.Log("Client : OnFixedUpdate : ClientID: " + clientID);
            //Transform default velocity (which is global) to a local velocity
            SM.LocalVelocity = SM.Cart.transform.InverseTransformDirection(SM.CartRB.velocity);

			if(SM.CartRB.velocity.magnitude < maxSpeed)
			{
                //Check for movement input
				if (isClient && clientID == 1 && SM.ForwardPressedPercent == 1) Debug.Log("Client : OnFixedUpdate : ClientID: " + clientID + " SM.ForwardPressedPercent: " + SM.ForwardPressedPercent);
				if (isClient && clientID == 2 && SM.ForwardPressedPercent == 1) Debug.Log("Client : OnFixedUpdate : ClientID: " + clientID + " SM.ForwardPressedPercent: " + SM.ForwardPressedPercent);
				//if (isClient && clientID == 2) Debug.Log("Client : OnFixedUpdate : ClientID: " + clientID);
                if (SM.ForwardPressedPercent > GameConstants.DEADZONE || SM.BackwardPressedPercent > GameConstants.DEADZONE)
				{
					if (maxSpeed > SM.LocalVelocity.z && -SM.MaxBackwardSpeed < SM.LocalVelocity.z)
					{
						SM.CartRB.AddForce(transform.forward
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