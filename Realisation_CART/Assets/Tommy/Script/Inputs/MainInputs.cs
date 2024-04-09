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
                    ""name"": ""Drift"",
                    ""type"": ""Button"",
                    ""id"": ""03125146-057f-4d87-835c-6f23e97f6423"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                },
                {
                    ""name"": ""MeleeAttack"",
                    ""type"": ""Button"",
                    ""id"": ""fb94bad4-8e3f-4d81-a9a8-e8526b0e6f2b"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                },
                {
                    ""name"": ""RangeAttack"",
                    ""type"": ""Button"",
                    ""id"": ""ac97f67a-a2c3-483e-85a8-a95e5103a73b"",
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
                    ""id"": ""2ac94daf-57c6-4ae7-82f2-ca861165fd78"",
                    ""path"": ""<Keyboard>/w"",
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
                    ""name"": """",
                    ""id"": ""18f38dee-0ab2-498e-9351-f3614343a292"",
                    ""path"": ""<Keyboard>/s"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""CartBackward"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""63f28a02-ab03-41df-a5c8-eb76d926a205"",
                    ""path"": ""<Keyboard>/shift"",
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
                    ""name"": ""1D Axis"",
                    ""id"": ""74a8ae38-e673-4dba-9395-d7b4e86dab6e"",
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
                    ""id"": ""0ce21e4c-75d5-49db-9f08-8edfebf9b5ba"",
                    ""path"": ""<Keyboard>/a"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Steer"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""positive"",
                    ""id"": ""a0252c9b-cbea-4563-a0c4-f5bf1e20e6e0"",
                    ""path"": ""<Keyboard>/d"",
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
                    ""id"": ""174d3ecc-feed-4475-b4c1-1f817e5d80e1"",
                    ""path"": ""<Keyboard>/escape"",
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
                    ""path"": ""<XInputController>/buttonEast"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Drift"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""8673a7aa-1803-4339-be04-7f9003b2efc9"",
                    ""path"": ""<Keyboard>/space"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Drift"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""c2182888-b1d0-4dc0-9333-2e9a9191a44b"",
                    ""path"": ""<XInputController>/buttonSouth"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""MeleeAttack"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""eb02c7ee-93d7-402b-b720-7a5c14935bcc"",
                    ""path"": ""<Mouse>/leftButton"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""MeleeAttack"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""8937fa26-2bc3-405b-b9c3-0bd10e48c8e7"",
                    ""path"": ""<XInputController>/buttonEast"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""RangeAttack"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""da47a7c1-da97-4372-a8c2-3f8205c87272"",
                    ""path"": ""<Mouse>/rightButton"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""RangeAttack"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                }
            ]
        },
        {
            ""name"": ""Menu"",
            ""id"": ""9258e7e3-e54e-4b86-a68b-5f3991f10ecd"",
            ""actions"": [
                {
                    ""name"": ""Navigate"",
                    ""type"": ""Value"",
                    ""id"": ""3fe615a2-9926-4639-9e86-dd7e36eecae9"",
                    ""expectedControlType"": ""Vector2"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": true
                },
                {
                    ""name"": ""Select"",
                    ""type"": ""Button"",
                    ""id"": ""debda4eb-4f99-4bdd-88ca-f8472448879e"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                },
                {
                    ""name"": ""Back"",
                    ""type"": ""Button"",
                    ""id"": ""b94cc0d5-ede0-4fef-b5ce-e8a020c1ce89"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                }
            ],
            ""bindings"": [
                {
                    ""name"": """",
                    ""id"": ""c7e0989d-cf99-4f82-8dfa-9ae0021e4dde"",
                    ""path"": ""<XInputController>/buttonSouth"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Select"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""078d0113-e87d-4ca1-a0c1-29391b080475"",
                    ""path"": ""<XInputController>/buttonEast"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Back"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": ""2D Vector"",
                    ""id"": ""b79321f0-0e30-4364-a3a4-9c31be450f3f"",
                    ""path"": ""2DVector"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Navigate"",
                    ""isComposite"": true,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": ""up"",
                    ""id"": ""3248437f-1ada-406c-bd7d-ee37465d0cb5"",
                    ""path"": ""<Gamepad>/leftStick/up"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Navigate"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""down"",
                    ""id"": ""eb4d9625-f8bf-4710-9716-ef3c7c7410fd"",
                    ""path"": ""<Gamepad>/leftStick/down"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Navigate"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""left"",
                    ""id"": ""7e7f8091-f482-46c4-b998-1776cbddd762"",
                    ""path"": ""<Gamepad>/leftStick/left"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Navigate"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""right"",
                    ""id"": ""a6cc5d97-593f-4315-9743-447c01d4ab4c"",
                    ""path"": ""<Gamepad>/leftStick/right"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Navigate"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
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
            m_Cart_Drift = m_Cart.FindAction("Drift", throwIfNotFound: true);
            m_Cart_MeleeAttack = m_Cart.FindAction("MeleeAttack", throwIfNotFound: true);
            m_Cart_RangeAttack = m_Cart.FindAction("RangeAttack", throwIfNotFound: true);
            // Menu
            m_Menu = asset.FindActionMap("Menu", throwIfNotFound: true);
            m_Menu_Navigate = m_Menu.FindAction("Navigate", throwIfNotFound: true);
            m_Menu_Select = m_Menu.FindAction("Select", throwIfNotFound: true);
            m_Menu_Back = m_Menu.FindAction("Back", throwIfNotFound: true);
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
        private readonly InputAction m_Cart_Drift;
        private readonly InputAction m_Cart_MeleeAttack;
        private readonly InputAction m_Cart_RangeAttack;
        public struct CartActions
        {
            private @MainInputs m_Wrapper;
            public CartActions(@MainInputs wrapper) { m_Wrapper = wrapper; }
            public InputAction @CartForward => m_Wrapper.m_Cart_CartForward;
            public InputAction @CartBackward => m_Wrapper.m_Cart_CartBackward;
            public InputAction @Steer => m_Wrapper.m_Cart_Steer;
            public InputAction @Pause => m_Wrapper.m_Cart_Pause;
            public InputAction @Drift => m_Wrapper.m_Cart_Drift;
            public InputAction @MeleeAttack => m_Wrapper.m_Cart_MeleeAttack;
            public InputAction @RangeAttack => m_Wrapper.m_Cart_RangeAttack;
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
                @Drift.started += instance.OnDrift;
                @Drift.performed += instance.OnDrift;
                @Drift.canceled += instance.OnDrift;
                @MeleeAttack.started += instance.OnMeleeAttack;
                @MeleeAttack.performed += instance.OnMeleeAttack;
                @MeleeAttack.canceled += instance.OnMeleeAttack;
                @RangeAttack.started += instance.OnRangeAttack;
                @RangeAttack.performed += instance.OnRangeAttack;
                @RangeAttack.canceled += instance.OnRangeAttack;
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
                @Drift.started -= instance.OnDrift;
                @Drift.performed -= instance.OnDrift;
                @Drift.canceled -= instance.OnDrift;
                @MeleeAttack.started -= instance.OnMeleeAttack;
                @MeleeAttack.performed -= instance.OnMeleeAttack;
                @MeleeAttack.canceled -= instance.OnMeleeAttack;
                @RangeAttack.started -= instance.OnRangeAttack;
                @RangeAttack.performed -= instance.OnRangeAttack;
                @RangeAttack.canceled -= instance.OnRangeAttack;
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

        // Menu
        private readonly InputActionMap m_Menu;
        private List<IMenuActions> m_MenuActionsCallbackInterfaces = new List<IMenuActions>();
        private readonly InputAction m_Menu_Navigate;
        private readonly InputAction m_Menu_Select;
        private readonly InputAction m_Menu_Back;
        public struct MenuActions
        {
            private @MainInputs m_Wrapper;
            public MenuActions(@MainInputs wrapper) { m_Wrapper = wrapper; }
            public InputAction @Navigate => m_Wrapper.m_Menu_Navigate;
            public InputAction @Select => m_Wrapper.m_Menu_Select;
            public InputAction @Back => m_Wrapper.m_Menu_Back;
            public InputActionMap Get() { return m_Wrapper.m_Menu; }
            public void Enable() { Get().Enable(); }
            public void Disable() { Get().Disable(); }
            public bool enabled => Get().enabled;
            public static implicit operator InputActionMap(MenuActions set) { return set.Get(); }
            public void AddCallbacks(IMenuActions instance)
            {
                if (instance == null || m_Wrapper.m_MenuActionsCallbackInterfaces.Contains(instance)) return;
                m_Wrapper.m_MenuActionsCallbackInterfaces.Add(instance);
                @Navigate.started += instance.OnNavigate;
                @Navigate.performed += instance.OnNavigate;
                @Navigate.canceled += instance.OnNavigate;
                @Select.started += instance.OnSelect;
                @Select.performed += instance.OnSelect;
                @Select.canceled += instance.OnSelect;
                @Back.started += instance.OnBack;
                @Back.performed += instance.OnBack;
                @Back.canceled += instance.OnBack;
            }

            private void UnregisterCallbacks(IMenuActions instance)
            {
                @Navigate.started -= instance.OnNavigate;
                @Navigate.performed -= instance.OnNavigate;
                @Navigate.canceled -= instance.OnNavigate;
                @Select.started -= instance.OnSelect;
                @Select.performed -= instance.OnSelect;
                @Select.canceled -= instance.OnSelect;
                @Back.started -= instance.OnBack;
                @Back.performed -= instance.OnBack;
                @Back.canceled -= instance.OnBack;
            }

            public void RemoveCallbacks(IMenuActions instance)
            {
                if (m_Wrapper.m_MenuActionsCallbackInterfaces.Remove(instance))
                    UnregisterCallbacks(instance);
            }

            public void SetCallbacks(IMenuActions instance)
            {
                foreach (var item in m_Wrapper.m_MenuActionsCallbackInterfaces)
                    UnregisterCallbacks(item);
                m_Wrapper.m_MenuActionsCallbackInterfaces.Clear();
                AddCallbacks(instance);
            }
        }
        public MenuActions @Menu => new MenuActions(this);
        public interface ICartActions
        {
            void OnCartForward(InputAction.CallbackContext context);
            void OnCartBackward(InputAction.CallbackContext context);
            void OnSteer(InputAction.CallbackContext context);
            void OnPause(InputAction.CallbackContext context);
            void OnDrift(InputAction.CallbackContext context);
            void OnMeleeAttack(InputAction.CallbackContext context);
            void OnRangeAttack(InputAction.CallbackContext context);
        }
        public interface IMenuActions
        {
            void OnNavigate(InputAction.CallbackContext context);
            void OnSelect(InputAction.CallbackContext context);
            void OnBack(InputAction.CallbackContext context);
        }
    }
}
