using CartControl;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

namespace DiscountDelirium
{
    public class MainMenuInputHandler : MonoBehaviour
    {
        const float SPEED = 50.0f;

        private MainInputs m_mainInputs;
        public static MainMenuInputHandler Instance;
        [SerializeField] private UnityEvent NaviguateEvent;
        [SerializeField] private UnityEvent SelectEvent;
        [SerializeField] private UnityEvent BackEvent;

        [SerializeField] private Cursor m_cursor;

        private Vector2 m_direction;

        private void Awake()
        {
            m_mainInputs = new MainInputs();

            if (Instance != null)
            {
                Destroy(this);
                return;
            }
            Instance = this;

            m_mainInputs.Menu.Navigate.started += OnNavigate;
            m_mainInputs.Menu.Navigate.performed += OnNavigate;
            m_mainInputs.Menu.Navigate.canceled += OnNavigate;

            m_mainInputs.Menu.Select.started += OnSelect;
            m_mainInputs.Menu.Back.started += OnBack;
        }

        private void OnNavigate(InputAction.CallbackContext context)
        {
            m_direction = context.ReadValue<Vector2>();
            Debug.Log(m_direction);
        }

        private void OnSelect(InputAction.CallbackContext context) 
        {
            NaviguateEvent.Invoke();
        }

        private void OnBack(InputAction.CallbackContext context)
        {
            BackEvent.Invoke();
        }

        void Update()
        {
            m_cursor.Move(m_direction * Time.deltaTime * SPEED);
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
