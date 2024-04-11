using UnityEngine;
using UnityEngine.InputSystem;

namespace DiscountDelirium
{
    public class GamepadDetection : MonoBehaviour
    {
        [Header("Cursor")]
        [SerializeField] private GameObject m_cursor;
        [SerializeField] private bool m_isActive;

        private void Awake()
        {
            InputSystem.onActionChange += ChangeDevice;
        }

        public void ActivateCursor(bool state) 
        {
            m_isActive = state;
        }

        private void ChangeDevice(object arg1, InputActionChange inputActionChange)
        {
            if (!m_isActive) 
            {
                m_cursor.SetActive(false);
                return;
            }

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
                    if (m_cursor != null)
                    {
                        m_cursor.SetActive(true);
                        Cursor.visible = false;
                    }
                }
                else 
                {
                    if (m_cursor != null) 
                    {
                        m_cursor.SetActive(false);
                        Cursor.visible = true;
                    }
                }
            }
        }

    }
}
