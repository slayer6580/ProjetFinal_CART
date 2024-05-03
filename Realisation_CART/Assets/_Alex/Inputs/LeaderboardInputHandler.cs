using CartControl;
using SavingSystem;
using UnityEngine;
using UnityEngine.InputSystem;

namespace DiscountDelirium
{
    public class LeaderboardInputHandler : MonoBehaviour
    {
        private LeaderboardInputs m_mainInputs;
        private LeaderboardManager m_manager;

        private void Awake()
        {
            m_mainInputs = new LeaderboardInputs();
            m_manager = GetComponent<LeaderboardManager>();

            m_mainInputs.Navigation.Navigate.started += OnNavigate;
            m_mainInputs.Navigation.Accept.started += OnAccept;
            m_mainInputs.Navigation.Decline.started += OnDecline;

            Debug.Log("handler initialize");
        }

        public void OnNavigate(InputAction.CallbackContext context)
        {
            Debug.Log("horizontal detected");
            m_manager.OnNavigate(context.ReadValue<Vector2>());
        }

        public void OnAccept(InputAction.CallbackContext context)
        {
            m_manager.OnAccept();
        }
        public void OnDecline(InputAction.CallbackContext context)
        {
            m_manager.OnDecline();
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
