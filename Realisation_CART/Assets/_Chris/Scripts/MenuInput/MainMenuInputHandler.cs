using CartControl;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

namespace DiscountDelirium
{
    public class MainMenuInputHandler : MonoBehaviour
    {
        private MainInputs m_mainInputs;
        public static MainMenuInputHandler Instance;
        [SerializeField] private UnityEvent NaviguateEvent;
        [SerializeField] private UnityEvent SelectEvent;
        [SerializeField] private UnityEvent BackEvent;

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
            
        }

        private void OnSelect(InputAction.CallbackContext context) 
        {
            NaviguateEvent.Invoke();
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
