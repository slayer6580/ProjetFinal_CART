//------------------------------------------------------------------------------
// <auto-generated>
//     This code was auto-generated by com.unity.inputsystem:InputActionCodeGenerator
//     version 1.7.0
//     from Assets/Tommy/Script/Inputs/MainInputs.inputactions
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Utilities;

namespace CartControl
{
    public partial class @MainInputs: IInputActionCollection2, IDisposable
    {
        public InputActionAsset asset { get; }
        public @MainInputs()
        {
            asset = InputActionAsset.FromJson(@"{
    ""name"": ""MainInputs"",
    ""maps"": [
        {
            ""name"": ""Cart"",
            ""id"": ""ab38f77a-0c38-4829-b954-0844bed98e45"",
            ""actions"": [
                {
                    ""name"": ""CartForward"",
                    ""type"": ""Value"",
                    ""id"": ""af42b16c-ba6f-408f-b92d-503494fc27d8"",
                    ""expectedControlType"": ""Analog"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": true
                },
                {
                    ""name"": ""CartBackward"",
                    ""type"": ""Value"",
                    ""id"": ""77cd7733-3d9a-4818-9235-6f09a7b60448"",
                    ""expectedControlType"": ""Analog"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": true
                },
                {
                    ""name"": ""Steer"",
                    ""type"": ""Value"",
                    ""id"": ""1781cc4d-3a2f-4ee8-8bc7-e39f86373427"",
                    ""expectedControlType"": ""Axis"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": true
                },
                {
                    ""name"": ""Pause"",
                    ""type"": ""Button"",
                    ""id"": ""a8cbc41b-8379-414f-a9f2-8f36b6766b10"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                },
                {
                    ""name"": ""GrabItem"",
                    ""type"": ""Button"",
                    ""id"": ""03125146-057f-4d87-835c-6f23e97f6423"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                }
            ],
            ""bindings"": [
                {
                    ""name"": """",
                    ""id"": ""459baca9-0066-446e-9c31-5034ff83dc42"",
                    ""path"": ""<XInputController>/rightTrigger"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""CartForward"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""10cef959-29e6-4bdb-8310-2a5d4d9831d9"",
                    ""path"": ""<XInputController>/leftTrigger"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""CartBackward"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": ""1D Axis"",
                    ""id"": ""c79e2156-cf2b-4dee-aa69-2840719c675a"",
                    ""path"": ""1DAxis"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Steer"",
                    ""isComposite"": true,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": ""negative"",
                    ""id"": ""7f2d8b9f-acbe-4f56-9a3f-1ff6dee08997"",
                    ""path"": ""<Gamepad>/leftStick/left"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Steer"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""positive"",
                    ""id"": ""6596ab19-a1be-4883-9b11-459bc61aa6c5"",
                    ""path"": ""<Gamepad>/leftStick/right"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Steer"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": """",
                    ""id"": ""409238d5-0f7a-438b-a14f-96b159ee05d5"",
                    ""path"": ""<Gamepad>/start"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Pause"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""c5a71c97-a82b-4930-9933-44d8ca1dba03"",
                    ""path"": ""<XInputController>/buttonSouth"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""GrabItem"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                }
            ]
        }
    ],
    ""controlSchemes"": []
}");
            // Cart
            m_Cart = asset.FindActionMap("Cart", throwIfNotFound: true);
            m_Cart_CartForward = m_Cart.FindAction("CartForward", throwIfNotFound: true);
            m_Cart_CartBackward = m_Cart.FindAction("CartBackward", throwIfNotFound: true);
            m_Cart_Steer = m_Cart.FindAction("Steer", throwIfNotFound: true);
            m_Cart_Pause = m_Cart.FindAction("Pause", throwIfNotFound: true);
            m_Cart_GrabItem = m_Cart.FindAction("GrabItem", throwIfNotFound: true);
        }

        public void Dispose()
        {
            UnityEngine.Object.Destroy(asset);
        }

        public InputBinding? bindingMask
        {
            get => asset.bindingMask;
            set => asset.bindingMask = value;
        }

        public ReadOnlyArray<InputDevice>? devices
        {
            get => asset.devices;
            set => asset.devices = value;
        }

        public ReadOnlyArray<InputControlScheme> controlSchemes => asset.controlSchemes;

        public bool Contains(InputAction action)
        {
            return asset.Contains(action);
        }

        public IEnumerator<InputAction> GetEnumerator()
        {
            return asset.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public void Enable()
        {
            asset.Enable();
        }

        public void Disable()
        {
            asset.Disable();
        }

        public IEnumerable<InputBinding> bindings => asset.bindings;

        public InputAction FindAction(string actionNameOrId, bool throwIfNotFound = false)
        {
            return asset.FindAction(actionNameOrId, throwIfNotFound);
        }

        public int FindBinding(InputBinding bindingMask, out InputAction action)
        {
            return asset.FindBinding(bindingMask, out action);
        }

        // Cart
        private readonly InputActionMap m_Cart;
        private List<ICartActions> m_CartActionsCallbackInterfaces = new List<ICartActions>();
        private readonly InputAction m_Cart_CartForward;
        private readonly InputAction m_Cart_CartBackward;
        private readonly InputAction m_Cart_Steer;
        private readonly InputAction m_Cart_Pause;
        private readonly InputAction m_Cart_GrabItem;
        public struct CartActions
        {
            private @MainInputs m_Wrapper;
            public CartActions(@MainInputs wrapper) { m_Wrapper = wrapper; }
            public InputAction @CartForward => m_Wrapper.m_Cart_CartForward;
            public InputAction @CartBackward => m_Wrapper.m_Cart_CartBackward;
            public InputAction @Steer => m_Wrapper.m_Cart_Steer;
            public InputAction @Pause => m_Wrapper.m_Cart_Pause;
            public InputAction @GrabItem => m_Wrapper.m_Cart_GrabItem;
            public InputActionMap Get() { return m_Wrapper.m_Cart; }
            public void Enable() { Get().Enable(); }
            public void Disable() { Get().Disable(); }
            public bool enabled => Get().enabled;
            public static implicit operator InputActionMap(CartActions set) { return set.Get(); }
            public void AddCallbacks(ICartActions instance)
            {
                if (instance == null || m_Wrapper.m_CartActionsCallbackInterfaces.Contains(instance)) return;
                m_Wrapper.m_CartActionsCallbackInterfaces.Add(instance);
                @CartForward.started += instance.OnCartForward;
                @CartForward.performed += instance.OnCartForward;
                @CartForward.canceled += instance.OnCartForward;
                @CartBackward.started += instance.OnCartBackward;
                @CartBackward.performed += instance.OnCartBackward;
                @CartBackward.canceled += instance.OnCartBackward;
                @Steer.started += instance.OnSteer;
                @Steer.performed += instance.OnSteer;
                @Steer.canceled += instance.OnSteer;
                @Pause.started += instance.OnPause;
                @Pause.performed += instance.OnPause;
                @Pause.canceled += instance.OnPause;
                @GrabItem.started += instance.OnGrabItem;
                @GrabItem.performed += instance.OnGrabItem;
                @GrabItem.canceled += instance.OnGrabItem;
            }

            private void UnregisterCallbacks(ICartActions instance)
            {
                @CartForward.started -= instance.OnCartForward;
                @CartForward.performed -= instance.OnCartForward;
                @CartForward.canceled -= instance.OnCartForward;
                @CartBackward.started -= instance.OnCartBackward;
                @CartBackward.performed -= instance.OnCartBackward;
                @CartBackward.canceled -= instance.OnCartBackward;
                @Steer.started -= instance.OnSteer;
                @Steer.performed -= instance.OnSteer;
                @Steer.canceled -= instance.OnSteer;
                @Pause.started -= instance.OnPause;
                @Pause.performed -= instance.OnPause;
                @Pause.canceled -= instance.OnPause;
                @GrabItem.started -= instance.OnGrabItem;
                @GrabItem.performed -= instance.OnGrabItem;
                @GrabItem.canceled -= instance.OnGrabItem;
            }

            public void RemoveCallbacks(ICartActions instance)
            {
                if (m_Wrapper.m_CartActionsCallbackInterfaces.Remove(instance))
                    UnregisterCallbacks(instance);
            }

            public void SetCallbacks(ICartActions instance)
            {
                foreach (var item in m_Wrapper.m_CartActionsCallbackInterfaces)
                    UnregisterCallbacks(item);
                m_Wrapper.m_CartActionsCallbackInterfaces.Clear();
                AddCallbacks(instance);
            }
        }
        public CartActions @Cart => new CartActions(this);
        public interface ICartActions
        {
            void OnCartForward(InputAction.CallbackContext context);
            void OnCartBackward(InputAction.CallbackContext context);
            void OnSteer(InputAction.CallbackContext context);
            void OnPause(InputAction.CallbackContext context);
            void OnGrabItem(InputAction.CallbackContext context);
        }
    }
}
