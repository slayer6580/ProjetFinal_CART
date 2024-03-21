using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Windows;

namespace PlayerInput
{
    public class MainInputsHandler : MonoBehaviour
    {
        private MainInputs m_mainInputs;
        public static MainInputsHandler Instance;
		private void Awake()
		{
			m_mainInputs = new MainInputs();
			
			if (Instance != null)
			{
                Destroy(this);
				return;
			}
			Instance = this;

			m_mainInputs.Cart.CartForward.started += OnStartForward;
			m_mainInputs.Cart.CartForward.performed += OnForward;
			m_mainInputs.Cart.CartForward.canceled += OnCancelForward;
		}

		public void OnStartForward(InputAction.CallbackContext context)
		{
			print("START: ");
			Debug.Log(context.ReadValue<float>());
		}

		public void OnForward(InputAction.CallbackContext context)
		{
			Debug.Log(context.ReadValue<float>());
		}

		public void OnCancelForward(InputAction.CallbackContext context)
		{
			print("CANCEL: ");
			Debug.Log(context.ReadValue<float>());
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
