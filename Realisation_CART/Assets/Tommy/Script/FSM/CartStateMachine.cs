using Cinemachine;
using DiscountDelirium;
using UnityEngine;
using UnityEngine.Animations.Rigging;

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

		//
		[field: Header("Boosting")]
		[field: SerializeField] public float BoostingAcceleration { get; private set; }
		[field: SerializeField] public float BoostingMaxSpeed { get; private set; }
		[field: SerializeField] public float BoostingTurnDrag { get; private set; }
		[field: SerializeField] public float DriftTimeToEarnMinBoost { get; private set; }
		[field: SerializeField] public float DriftTimeToEarnMaxBoost { get; private set; }

		[field: HideInInspector] public float BoostingTime { get; set; }
		[field: SerializeField] public float MinBoostTime { get; private set; }
		[field: SerializeField] public float MaxBoostTime { get; private set; }

		[field: Header("Stopped")]
		[field: SerializeField] public bool BackToIdleAfterStop { get; set; }

		///
		[Header("To Set")]
		public GameObject m_cart;
		public Rigidbody m_cartRB;
		public GameObject m_gameplayCamera;
		public CinemachineBrain m_camBrain;
		public Animator m_humanAnimator;
		public Rig m_feetOnCartRig;
		[field: SerializeField] public CartMovement CartMovement { get; private set; }

		[Space]
		public TowerBalanceAnimCtrlr m_towerCtrlr;

		//
		[HideInInspector] public bool CanDrift { get; set; }
		[HideInInspector] public bool IsDrifting { get; set; }
		[HideInInspector] public bool CanBoost { get; set; }
		[HideInInspector] public bool IsBoosting { get; set; }
		[HideInInspector] public bool IsPaused { get; set; }
		

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


			//Update Animation
			//print("VEL: " + LocalVelocity.z / MaxSpeed);
			m_humanAnimator.SetFloat("RunningSpeed", LocalVelocity.z / MaxSpeed);

			//Testing tower tilt with Animation
			m_towerCtrlr.SetTowerTilt(Mathf.Round(LocalVelocity.x));
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
			m_possibleStates.Add(new CartState_Boosting());
			m_possibleStates.Add(new CartState_Stopped());
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

		public void OnPause()
		{
			IsPaused = !IsPaused;
		}

	}
}
