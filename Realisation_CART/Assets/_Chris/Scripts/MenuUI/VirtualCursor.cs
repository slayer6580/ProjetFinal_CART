using UnityEngine;
using UnityEngine.InputSystem.LowLevel;
using UnityEngine.InputSystem.UI;

namespace DiscountDelirium
{
    public class VirtualCursor : MonoBehaviour
    {
        [SerializeField] private RectTransform canvasRectTransform;
        private VirtualMouseInput m_virtualMouseInput;

        private void Awake()
        {
            m_virtualMouseInput = GetComponent<VirtualMouseInput>();
        }

        private void Update()
        {
            transform.localScale = Vector3.one * (1.0f / canvasRectTransform.localScale.x);
            transform.SetAsLastSibling();
        }

        private void LateUpdate()
        {
            Vector2 virtualMousePosition = m_virtualMouseInput.virtualMouse.position.value;
            virtualMousePosition.x = Mathf.Clamp(virtualMousePosition.x, 0f, Screen.width);
            virtualMousePosition.y = Mathf.Clamp(virtualMousePosition.y, 0f, Screen.height);
            InputState.Change(m_virtualMouseInput.virtualMouse.position, virtualMousePosition);
        }
    }
}
