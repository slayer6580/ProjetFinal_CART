using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace DiscountDelirium
{
    public class GamepadDetection : MonoBehaviour
    {
        [Header("Cursor")]
        [SerializeField] private GameObject m_cursor;

        private void Awake()
        {
            InputSystem.onActionChange += ChangeDevice;
        }

        private void ChangeDevice(object arg1, InputActionChange inputActionChange)
        {
            if (inputActionChange == InputActionChange.ActionPerformed && arg1 is InputAction) 
            {
                InputAction inputAction = arg1 as InputAction;
                if (inputAction.activeControl.device.displayName == "VirtualMouse") 
                {
                    // Ignore virtual mouse
                    return;
                }
                if (inputAction.activeControl.device is Gamepad) 
                {
                    m_cursor.SetActive(true);
                    Cursor.visible = false;
                }
                else 
                {
                    m_cursor.SetActive(false);
                    Cursor.visible = true;
                }
            }
        }

    }
}
