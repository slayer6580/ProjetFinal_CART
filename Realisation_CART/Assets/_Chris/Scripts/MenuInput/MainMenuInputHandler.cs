using CartControl;
using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

namespace DiscountDelirium
{
    public class MainMenuInputHandler : MonoBehaviour
    {
        private MainInputs m_mainInputs;
        public static MainMenuInputHandler Instance;
        public static Action SelectEvent;
        public static Action BackEvent;

        private void Awake()
        {
            m_mainInputs = new MainInputs();

            if (Instance != null)
            {
                Destroy(this);
                return;
            }
            Instance = this;

            m_mainInputs.Menu.Select.started += OnSelect;
            m_mainInputs.Menu.Back.started += OnBack;
        }

        private void OnSelect(InputAction.CallbackContext context) 
        {
            SelectEvent.Invoke();
        }

        private void OnBack(InputAction.CallbackContext context)
        {
            BackEvent.Invoke();
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
