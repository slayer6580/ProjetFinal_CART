using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CartStateMachine : StateMachine<CartState>
{
	private const float BASE_ADD_FORCE = 1000 ;

	[Header("Test Value")]
	public float m_forwardPressedPercent;
	public float m_backwardPressedPercent;
	public float m_steeringValue;
	public float velMagnitude;	//To delete

	[Header("Stats")]
	public float m_acceleration;
	public float m_maxSpeed;
	public float m_maxBackwardSpeed;
	public float m_idleRotatingSpeed;
	public float m_movingRotatingSpeed;

	[Space]
	[Range(0,3)] public float m_turningDrag;
	[Tooltip("This will multiply Turning Drag depending of the steer value. Left side of the curve = No steer. Right side = max steer.")]
	public AnimationCurve m_turningDragDependingOnJoystickValue; //Not touching the controller on the left of the curve; Max steer at the right of the curve

	[Header("To Set")]
	public GameObject m_cart;
	public Rigidbody m_cartRB;


	protected override void Start()
	{
		base.Start();

		foreach(CartState state in m_possibleStates)
		{
			state.OnStart(this);
		}

		
	}

	protected override void Update()
	{
		//For test value
		velMagnitude = m_cartRB.velocity.magnitude;

		TryToChangeState();
	}

	protected override void FixedUpdate()
	{
		m_currentState.OnFixedUpdate();
	}

	protected override void CreatePossibleStateList() 
	{
		m_possibleStates.Add(new CartState_Idle());
		m_possibleStates.Add(new CartState_Moving());
	}




	//Receive Input
	public void OnForward(float pressValue)
	{
		m_forwardPressedPercent = pressValue;
	}

	public void OnBackward(float pressValue)
	{
		m_backwardPressedPercent = pressValue;
	}

	public void OnSteer(float steerValue)
	{
		m_steeringValue = steerValue;
	}






	public void Move()
	{
		//Transform default velocity (which is global) to a local velocity
		Vector3 localVelocity = m_cart.transform.InverseTransformDirection(m_cartRB.velocity);

		//Debug.Log("X vel: " + localVelocity.x);
		if (m_forwardPressedPercent > 0.1f || m_backwardPressedPercent > 0.1f)
		{
			if (m_maxSpeed > localVelocity.z && -m_maxBackwardSpeed < localVelocity.z)
			{
				m_cartRB.GetComponent<Rigidbody>().AddForce(transform.forward * BASE_ADD_FORCE * m_acceleration * Time.deltaTime
				* (m_forwardPressedPercent - m_backwardPressedPercent)
				);
			}
		}

		Vector3 sideToPush = -transform.right * Mathf.Clamp(localVelocity.x, -1f, 1f);
		float pushForce = BASE_ADD_FORCE * m_acceleration * m_turningDrag;
		float pushMultiply = m_turningDragDependingOnJoystickValue.Evaluate(Mathf.Abs(m_steeringValue));
		print("MULTIPLY" + pushMultiply);

		m_cartRB.GetComponent<Rigidbody>().AddForce(sideToPush * pushForce * pushMultiply * Time.deltaTime);


		/*
		if (m_steeringValue != 0)
		{
			if (Mathf.Abs(localVelocity.x) > m_turnSideSpeedLimitWhileTurning)
			{
				m_cartRB.GetComponent<Rigidbody>().AddForce(-transform.right * BASE_ADD_FORCE * m_acceleration * Time.deltaTime
					* Mathf.Clamp(localVelocity.x, -1f, 1f)
					);
			}
		}
		
		else
		{
			if (Mathf.Abs(localVelocity.x) > m_turnSideSpeedLimitAfterTurning)
			{
				Debug.Log("REPLACE");
				m_cartRB.GetComponent<Rigidbody>().AddForce(-transform.right * BASE_ADD_FORCE * m_acceleration * Time.deltaTime
					* Mathf.Clamp(localVelocity.x, -1f, 1f)
					);
			}
		}
		
		*/




	}
}
