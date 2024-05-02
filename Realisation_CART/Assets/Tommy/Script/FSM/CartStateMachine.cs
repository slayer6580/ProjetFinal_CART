using Cinemachine;
using DiscountDelirium;
using UnityEngine;
using UnityEngine.Animations.Rigging;
using BoxSystem;
using UnityEngine.VFX;
using UnityEditor.Experimental.GraphView;
using UnityEngine.SceneManagement;
using BehaviourTree;

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
        private CinemachineBrain CamBrain { get; set; }
        private GameObject VirtualCamera { get; set; }

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
        [HideInInspector] public bool CanBoost { get; set; }
        [HideInInspector] public bool IsBoosting { get; set; }
        [HideInInspector] public bool IsPaused { get; set; }
        [HideInInspector] public Vector3 CollisionOppositeDirection { get; private set; }

		[HideInInspector] public bool IsClient { get; set; } = false; // Debug to remove
		static private int m_clientIDIterator = 0;
		[HideInInspector] public int ClientID { get; private set; } = 0;

        private void Awake()
        {
			if (GetComponentInChildren<BehaviourTreeRunner>()) 
			{
				IsClient = true;
                ClientID = ++m_clientIDIterator;

            }

            Scene scene = gameObject.scene;
            GameObject[] gameObjects = scene.GetRootGameObjects();

            GameObject cameras = gameObjects[0];
            if (cameras == null || cameras.name != "CameraSystem") Debug.LogError("Cameras not found");

            foreach (Transform child in cameras.transform)
            {
                if (child.name == "Virtual Camera")
                {
                    VirtualCamera = child.gameObject;
                }
                else if (child.name == "Main Camera")
                {
                    CamBrain = child.GetComponent<CinemachineBrain>();
                }
                else
                {
                    continue;
                }
            }

            Cart = gameObject;
            CartRB = GetComponent<Rigidbody>();

            Animator animator = GetComponentInChildren<Animator>();
            if (animator == null || animator.gameObject.name != "SM_Chr_Kid_Adventure_01") Debug.LogError("Animator not found");

            HumanAnimCtrlr = animator;

            GameObject rigGO = animator.transform.GetChild(1).gameObject;
            if (rigGO == null || rigGO.name != "FeetOnCart") Debug.LogError("FeetOnCart not found");
            Rig rig = rigGO.GetComponent<Rig>();

            FeetOnCartRig = rig;
            CartMovement = GetComponent<CartMovement>();
            GrindVfx = GetComponentInChildren<ManageGrindVfx>();

			ParentOfAllVisual = transform.GetChild(0).gameObject;

            if (ParentOfAllVisual.name == "GrindVFX") return; // Vérify if Player still has GrindVFX as first child.
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
			PlayerPrefs.GetInt("Acceleration", 0);
			PlayerPrefs.GetInt("MaxSpeed", 0);
			PlayerPrefs.GetInt("Handling", 0);

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
			m_possibleStates.Add(new CartState_Boosting());
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
