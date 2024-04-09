using Cinemachine;
using DiscountDelirium;
using UnityEngine;
using UnityEngine.Animations.Rigging;
using BoxSystem;

namespace CartControl
{
	public class CartStateMachine2 : StateMachine2<CartState2>
	{
		///
		[field: Header("Test Value")]
		public float velMagnitude;  //To delete
		[field: SerializeField] public float ForwardPressedPercent { get; set; }	//To deserialize
		[field: SerializeField] public float BackwardPressedPercent { get; set; }   //To deserialize
		[field: SerializeField] public float SteeringValue { get; set; }    //To deserialize
		[field: SerializeField] public Vector3 LocalVelocity { get; set; }  //To deserialize

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
		[field: SerializeField] public float MinimumSpeedToAllowDrift { get; private set; }
		[field: SerializeField][field: Range(0, 3)] public float DriftingDrag { get; private set; }
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

		///
		[field: Header("Stopped")]
		[field: SerializeField] public bool BackToIdleAfterStop { get; set; }

		///
		[field: Header("To Set")]
		[field: SerializeField] public GameObject Cart { get; private set; }
		[field: SerializeField] public Rigidbody CartRB { get; private set; }
		[field: SerializeField] public CinemachineBrain CamBrain { get; private set; }
		[field: SerializeField] public GameObject VirtualCamera { get; private set; }

		[field: SerializeField] public Animator HumanAnimCtrlr { get; private set; }
		[field: SerializeField] public Rig FeetOnCartRig { get; private set; }
		[field: SerializeField] public CartMovement2 CartMovement { get; private set; }

		//
		[Space]
		[Header("Temp Testing")]
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

			foreach (CartState2 state in m_possibleStates)
			{
				state.OnStart(this);
			}
            m_currentState.OnEnter();
        }

		protected override void Update()
		{
			//For test value
			velMagnitude = CartRB.velocity.magnitude;

			m_currentState.OnUpdate();
			TryToChangeState();

			//For Animation
			//HumanAnimCtrlr.SetFloat("RunningSpeed", LocalVelocity.z / MaxSpeed);

			//Testing tower tilt with Animation
			//TEMPORARY DEACTIVATED
			//m_towerCtrlr.SetTowerTilt(Mathf.Round(LocalVelocity.x));
		}

		protected override void FixedUpdate()
		{
			m_currentState.OnFixedUpdate();		
		}

		protected override void CreatePossibleStateList()
		{
			m_possibleStates.Add(new CartState_Idle2());
			m_possibleStates.Add(new CartState_Moving2());
			m_possibleStates.Add(new CartState_Drifting2());
			m_possibleStates.Add(new CartState_Boosting2());
			m_possibleStates.Add(new CartState_Stopped2());
			m_possibleStates.Add(new CartState_TurnInPlace2());
			m_possibleStates.Add(new CartState_Stunned2());
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