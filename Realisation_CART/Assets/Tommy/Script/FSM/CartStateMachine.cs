using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CartControl
{
	public class CartStateMachine : StateMachine<CartState>
	{
		///
		[field: Header("Test Value")]
		public float velMagnitude;  //To delete
		[field: SerializeField] public float ForwardPressedPercent { get; set; }
		[field: SerializeField] public float BackwardPressedPercent { get; set; }
		[field: SerializeField] public float SteeringValue { get; set; }
		[field: SerializeField] public Vector3 LocalVelocity { get; set; }

		///
		[field: Header("Stats")]
		[field: SerializeField] public float Acceleration { get; set; }
		[field: SerializeField] public float MaxSpeed { get; set; }
		[field: SerializeField] public float MaxBackwardSpeed { get; private set; }
		[field: SerializeField] public float IdleRotatingSpeed { get; private set; }
		[field: SerializeField] public float MovingRotatingSpeed { get; set; }
		[field: SerializeField][field: Range(0, 3)] public float TurningDrag { get; set; }

		[Tooltip("This will multiply Turning Drag depending of the steer value. Left side of the curve = No steer. Right side = max steer.")]
		[field: SerializeField] public AnimationCurve TurningDragRelativeToJoystick { get; set; } //Not touching the controller on the left of the curve; Max steer at the right of the curve

		///
		[field: Header("Drifting")]
		[field: SerializeField] public float MinimumSpeedToDrift { get; private set; }
		[field: SerializeField][field: Range(0, 3)] public float DriftingDrag { get; private set; }

		[Tooltip("This will multiply Drifting Drag depending of the steer value. Left side of the curve = No steer. Right side = max steer.")]
		[field: SerializeField] public AnimationCurve DriftingDragRelativeToJoystick { get; private set; } //Not used for now
		[field: SerializeField] public float DriftingRotatingSpeed { get; private set; }
		[field: SerializeField] public float AddedRotatingSpeedWhenBreaking { get; private set; }
		[field: SerializeField] public float DriftingAcceleration { get; private set; }


		[field: Space]
		[field: SerializeField] public bool AutoDriftWhenTurning { get; private set; }
		[field: SerializeField] public float TurningTimeBeforeDrift { get; private set; }

		///
		[Header("To Set")]
		public GameObject m_cart;
		public Rigidbody m_cartRB;
		[field: SerializeField] public CartMovement CartMovement { get; private set; }

		//
		[HideInInspector] public bool CanDrift { get; set; }

		protected override void Start()
		{
			base.Start();
			CartMovement.SM = this;

			foreach (CartState state in m_possibleStates)
			{
				state.OnStart(this);
			}
		}

		protected override void Update()
		{
			//For test value
			velMagnitude = m_cartRB.velocity.magnitude;

			m_currentState.OnUpdate();
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
			m_possibleStates.Add(new CartState_Drifting());
		}




		//Receive Input
		public void OnForward(float pressValue)
		{
			ForwardPressedPercent = pressValue;
		}

		public void OnBackward(float pressValue)
		{
			BackwardPressedPercent = pressValue;
		}

		public void OnSteer(float steerValue)
		{
			SteeringValue = steerValue;
		}

	}
}
