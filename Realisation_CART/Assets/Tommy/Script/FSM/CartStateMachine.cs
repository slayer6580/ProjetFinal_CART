using DiscountDelirium;
using UnityEngine;
using UnityEngine.Animations.Rigging;
using BoxSystem;
using UnityEngine.SceneManagement;

namespace CartControl
{
	public class CartStateMachine : StateMachine<CartState>
	{
		///
		[field: Header("Test Value")]
		public float velMagnitude;  //To delete

		public bool m_showLogStateChange;
		[field: SerializeField] public float ForwardPressedPercent { get; set; }	//To deserialize
		[field: SerializeField] public float BackwardPressedPercent { get; set; }   //To deserialize
		[field: SerializeField] public float SteeringValue { get; set; }    //To deserialize
		[field: SerializeField] public float DriftSteeringValue { get; set; }    //To deserialize
		[field: SerializeField] public Vector3 LocalVelocity { get; set; }  //To deserialize

		///
		[field: Header("Stats")]
		[field: SerializeField] public bool IsClient { get; set; }
		[field: SerializeField] public float Acceleration { get; set; }
		[field: SerializeField] public float MaxSpeed { get; set; }
		[field: SerializeField] public float MaxBackwardSpeed { get; private set; }
		[field: SerializeField] public float IdleRotatingSpeed { get; private set; }
		[field: SerializeField] public float MovingRotatingSpeed { get; set; }
		[field: SerializeField] public float TowerPushForceWhenTurning { get; set; }
		[field: SerializeField][field: Range(0, 3)] public float TurningDrag { get; set; }

		[Tooltip("This will multiply Turning Drag depending of the steer value. Left side of the curve = No steer. Right side = max steer.")]
		[field: SerializeField] public AnimationCurve TurningDragRelativeToJoystick { get; set; } //Not touching the controller on the left of the curve; Max steer at the right of the curve

		///
		[field: Header("Drifting")]
		[field: SerializeField] public float MinimumSpeedToAllowDrift { get; private set; }
		[field: SerializeField][field: Range(0, 3)] public float DriftingDrag { get; set; }
		[field: SerializeField] public float DriftingRotatingSpeed { get; private set; }
		[field: SerializeField] public float AddedRotatingSpeedWhenBreaking { get; private set; }
		[field: SerializeField] public float DriftingAcceleration { get; private set; }
		[field: SerializeField] public float TowerPushForceWhenDrifting { get; private set; }

		[field: Space]
		[field: SerializeField] public bool AutoDriftWhenTurning { get; private set; }
		[field: SerializeField] public float TurningTimeBeforeDrift { get; private set; }
		[field: SerializeField] public float DriftPressed { get; private set; }

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
        public AddForceToBox BoxForce { get; set; }
		public GameObject ParentOfAllVisual { get; private set; }
        public GameObject Cart { get; private set; }
        public Rigidbody CartRB { get; private set; }
        public Animator HumanAnimCtrlr { get; private set; }
        public Rig FeetOnCartRig { get; private set; }
        public CartMovement CartMovement { get; private set; }
        public ManageGrindVfx GrindVfx { get; private set; }


		//
		[Space]
		[Header("Temp Testing")]
		public TowerBalanceAnimCtrlr m_towerCtrlr;

		//
		//UpgradedStats
		[field: SerializeField] public float AccelerationUpgrades { get; private set; }
		[field: SerializeField] public float MaxSpeedUpgrades { get; private set; }
		[field: SerializeField] public float MovingRotatingSpeedUpgrades { get; private set; }
		[field: SerializeField] public float DriftingRotatingSpeedUpgrades { get; private set; }


		//

        [HideInInspector] public GameObject LastClientCollisionWith { get; set; }
        [HideInInspector] public bool ForceStartDrift { get; set; }
        [HideInInspector] public bool IsDrifting { get; set; }
        //[HideInInspector] public bool CanBoost { get; set; }
       // [HideInInspector] public bool IsBoosting { get; set; }
        [HideInInspector] public bool IsPaused { get; set; }
        [HideInInspector] public Vector3 CollisionOppositeDirection { get; private set; }


        private void Awake()
        {
            InitializeVariables();
        }

        private void InitializeVariables()
        {
            Scene scene = gameObject.scene;
            GameObject[] gameObjects = scene.GetRootGameObjects();

            GameObject cameras = gameObjects[0];
            if (cameras == null || cameras.name != "CameraSystem") Debug.LogError("Cameras not found");

            Cart = gameObject;
            CartRB = GetComponent<Rigidbody>();

            Animator animator = GetComponentInChildren<Animator>();
            if ((animator.gameObject.name == "CharacterBuilder" || animator.gameObject.name == "SM_Chr_Kid_Adventure_01") == false) Debug.LogError("Animator not found or renamed. Current GameObject is: " + animator.gameObject.name);
            if (animator == null) Debug.LogError("Animator not found");

            HumanAnimCtrlr = animator;

            GameObject rigGO = animator.transform.GetChild(1).gameObject;
            if (rigGO == null || rigGO.name != "FeetOnCart") Debug.LogError("FeetOnCart not found. Current GameObject is: " + rigGO.name);
            Rig rig = rigGO.GetComponent<Rig>();

            FeetOnCartRig = rig;
            CartMovement = GetComponent<CartMovement>();
            GrindVfx = GetComponentInChildren<ManageGrindVfx>();

            ParentOfAllVisual = transform.GetChild(0).gameObject;

            if (ParentOfAllVisual.name == "GrindVFX") return; // V�rify if Player still has GrindVFX as first child.
            if (ParentOfAllVisual.name != "Parent") Debug.LogWarning("Not a client or Parent not found or not named Parent. Current name: " + ParentOfAllVisual.name);
        }

        protected override void Start()
		{
			base.Start();
			CartMovement.SM = this;

			foreach (CartState state in m_possibleStates)
			{
				state.OnStart(this);
			}
            m_currentState.OnEnter();

			LoadUpgrade();
        }

		private void LoadUpgrade()
		{
			if (IsClient)
			{
				AccelerationUpgrades = Acceleration;
				MaxSpeedUpgrades = MaxSpeed;
				MovingRotatingSpeedUpgrades = MovingRotatingSpeed;
				DriftingRotatingSpeedUpgrades = DriftingRotatingSpeed;
			}
			
			AccelerationUpgrades = Acceleration + 10 * PlayerPrefs.GetInt("Acceleration", 0);
			MaxSpeedUpgrades = MaxSpeed + 5 * PlayerPrefs.GetInt("MaxSpeed", 0);
			MovingRotatingSpeedUpgrades = MovingRotatingSpeed + 5 * PlayerPrefs.GetInt("Handling", 0);
			DriftingRotatingSpeedUpgrades = DriftingRotatingSpeed + 5 * PlayerPrefs.GetInt("Handling", 0);

		}
		protected override void Update()
        {
            //For test value
            velMagnitude = CartRB.velocity.magnitude;

            m_currentState.OnUpdate();
            TryToChangeState();

            //For Animation
            HumanAnimCtrlr.SetFloat("RunningSpeed", LocalVelocity.z / MaxSpeedUpgrades);

			//Testing tower tilt with Animation
			//TEMPORARY DEACTIVATED
			//m_towerCtrlr.SetTowerTilt(Mathf.Round(LocalVelocity.x));

			if (!IsClient)
			{
				AddForceToTowerPhysics();
			}
           

			

        }

        private void AddForceToTowerPhysics()
        {
            if (m_currentState is CartState_Moving)
			{
				Debug.Log("AA");
				BoxForce.AddConstantForceToBox(LocalVelocity.x, TowerPushForceWhenTurning, 4f);
			}          
                    
            else if (m_currentState is CartState_Drifting)
			{
				Debug.Log("BB");
				BoxForce.AddConstantForceToBox(LocalVelocity.x, TowerPushForceWhenDrifting, 1);
			}      
                     
            else if(BoxForce.m_pushIsStop == false)           
                BoxForce.StopForce();
            
        }


        protected override void FixedUpdate()
		{
			m_currentState.OnFixedUpdate();		
		}

		public void OnCollisionEnter(Collision collision)
		{
			if(collision.gameObject.layer == LayerMask.NameToLayer("PlayerCollider"))
			{
				if(collision.gameObject.GetComponent<CartStateMachine>() != null)
				{
					CollisionOppositeDirection = collision.contacts[0].normal;
					LastClientCollisionWith = collision.gameObject;
					
					
				}
			}
		}

	

		protected override void CreatePossibleStateList()
		{
			m_possibleStates.Add(new CartState_Idle());
			m_possibleStates.Add(new CartState_Moving());
			m_possibleStates.Add(new CartState_Drifting());
			m_possibleStates.Add(new CartState_Stopped());
			m_possibleStates.Add(new CartState_TurnInPlace());
			m_possibleStates.Add(new CartState_Stunned());
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
		public void OnDrift(float isDrifting)
		{
			DriftPressed = isDrifting;
		}
		public void OnPause()
		{
			IsPaused = !IsPaused;
		}
	}
}
