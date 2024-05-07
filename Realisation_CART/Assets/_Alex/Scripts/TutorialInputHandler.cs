using SavingSystem;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace DiscountDelirium
{
    public class TutorialInputHandler : MonoBehaviour
    {
        private TutorialInputs m_mainInputs;
        private TutorialManager m_manager;

        private void Awake()
        {
            m_mainInputs = new TutorialInputs();
            m_manager = GetComponent<TutorialManager>();

            m_mainInputs.Navigation.Accept.started += OnAccept;
            m_mainInputs.Navigation.Decline.started += OnDecline;

            Debug.Log("handler initialize");
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
