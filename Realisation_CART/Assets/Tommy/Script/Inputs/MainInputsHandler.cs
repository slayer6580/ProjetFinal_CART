using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Events;


namespace CartControl
{
    public class MainInputsHandler : MonoBehaviour
    {
        private MainInputs m_mainInputs;
        public static MainInputsHandler Instance;
		[SerializeField] private CartStateMachine m_cartStateMachine;
		[SerializeField] private UnityEvent m_grabItemEvent;

		private void Awake()
		{
			m_mainInputs = new MainInputs();
			
			if (Instance != null)
			{
                Destroy(this);
				return;
			}
			Instance = this;

			m_mainInputs.Cart.CartForward.started += OnForward;
			m_mainInputs.Cart.CartForward.performed += OnForward;
			m_mainInputs.Cart.CartForward.canceled += OnForward;

			m_mainInputs.Cart.CartBackward.started += OnBackward;
			m_mainInputs.Cart.CartBackward.performed += OnBackward;
			m_mainInputs.Cart.CartBackward.canceled += OnBackward;

			m_mainInputs.Cart.Steer.started += OnSteer;
			m_mainInputs.Cart.Steer.performed += OnSteer;
			m_mainInputs.Cart.Steer.canceled += OnSteer;

			m_mainInputs.Cart.Pause.started += OnPause;


			m_mainInputs.Cart.GrabItem.started += OnGrabItem;

		}

		public void OnForward(InputAction.CallbackContext context)
		{
			m_cartStateMachine.OnForward(context.ReadValue<float>());
		}

		public void OnBackward(InputAction.CallbackContext context)
		{
			m_cartStateMachine.OnBackward(context.ReadValue<float>());
		}

		public void OnSteer(InputAction.CallbackContext context)
		{
			m_cartStateMachine.OnSteer(context.ReadValue<float>());
		}

		public void OnPause(InputAction.CallbackContext context)
		{
			m_cartStateMachine.OnPause();
		}

		public void OnGrabItem(InputAction.CallbackContext context)
		{
			m_grabItemEvent.Invoke();
		}

		private void OnEnable()
		{
			m_mainInputs.Enable();
		}
		private void OnDisable()
		{
			m_mainInputs.Disable();
		}
	}
}
