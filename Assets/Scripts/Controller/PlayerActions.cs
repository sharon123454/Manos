//------------------------------------------------------------------------------
// <auto-generated>
//     This code was auto-generated by com.unity.inputsystem:InputActionCodeGenerator
//     version 1.3.0
//     from Assets/Scripts/Controller/PlayerActions.inputactions
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

public partial class @PlayerActions : IInputActionCollection2, IDisposable
{
    public InputActionAsset asset { get; }
    public @PlayerActions()
    {
        asset = InputActionAsset.FromJson(@"{
    ""name"": ""PlayerActions"",
    ""maps"": [
        {
            ""name"": ""PlayerCamera"",
            ""id"": ""9eebd91f-6555-48fa-851f-de4c4f07c1fa"",
            ""actions"": [
                {
                    ""name"": ""Move"",
                    ""type"": ""Value"",
                    ""id"": ""42e73851-6490-4c96-a172-7a66322b371d"",
                    ""expectedControlType"": ""Vector2"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": true
                },
                {
                    ""name"": ""Zoom"",
                    ""type"": ""Value"",
                    ""id"": ""cb5593f5-acdc-4974-86ef-89543d5b771b"",
                    ""expectedControlType"": ""Vector2"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": true
                },
                {
                    ""name"": ""Space"",
                    ""type"": ""Button"",
                    ""id"": ""7087b88c-b3d4-4128-8472-cef39548d369"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                },
                {
                    ""name"": ""Click"",
                    ""type"": ""PassThrough"",
                    ""id"": ""cac6c08f-b188-4d28-9f66-18dacc0bcb4a"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": true
                },
                {
                    ""name"": ""Rotate"",
                    ""type"": ""Button"",
                    ""id"": ""f4f2cb01-766a-4971-a683-a36783eaa274"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                },
                {
                    ""name"": ""RightClick"",
                    ""type"": ""PassThrough"",
                    ""id"": ""9bfcac76-8563-491a-87cc-d91d215de0dd"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": true
                },
                {
                    ""name"": ""RotateLeft"",
                    ""type"": ""Button"",
                    ""id"": ""4f955c3a-d5eb-4177-9ecc-4f6cfb45f677"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                },
                {
                    ""name"": ""RotateRight"",
                    ""type"": ""Button"",
                    ""id"": ""e682ad32-67d8-4a57-9ba4-36c0fd49ccec"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                },
                {
                    ""name"": ""OpenSettings"",
                    ""type"": ""Button"",
                    ""id"": ""7c701c6f-e597-4a26-9bbf-85a7662976cf"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                },
                {
                    ""name"": ""PointerDelta"",
                    ""type"": ""Value"",
                    ""id"": ""899cc6e3-609a-4d49-a4a0-78ab46671dd5"",
                    ""expectedControlType"": ""Vector2"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": true
                },
                {
                    ""name"": ""PointerPosition"",
                    ""type"": ""Value"",
                    ""id"": ""893f0c85-ea82-45bc-ae4e-c5414b8278b5"",
                    ""expectedControlType"": ""Vector2"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": true
                },
                {
                    ""name"": ""SwitchSelectedPlayer"",
                    ""type"": ""Button"",
                    ""id"": ""2a4450ae-14d1-49fa-87e3-dff5597483f7"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                },
                {
                    ""name"": ""SelectActionWithNumbers"",
                    ""type"": ""Value"",
                    ""id"": ""83574331-0aa9-45e3-80fa-66bd64fb801e"",
                    ""expectedControlType"": """",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": true
                },
                {
                    ""name"": ""Interact"",
                    ""type"": ""Button"",
                    ""id"": ""6c05cc9a-97e2-4839-b6b4-1ed7b3bd2013"",
                    ""expectedControlType"": """",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                }
            ],
            ""bindings"": [
                {
                    ""name"": """",
                    ""id"": ""1fcf5d5a-9527-4ac2-bce2-86859e675b5a"",
                    ""path"": ""<Mouse>/delta"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard"",
                    ""action"": ""PointerDelta"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""52ef2567-655d-4634-aa42-264b1f16d482"",
                    ""path"": ""<Mouse>/position"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard"",
                    ""action"": ""PointerPosition"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""c07f0f53-9ce4-46f3-9b92-c2017e42e857"",
                    ""path"": ""<Keyboard>/space"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard"",
                    ""action"": ""Space"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": ""WASD"",
                    ""id"": ""bf038f62-b08e-4412-b90c-1824f7ae1f9e"",
                    ""path"": ""2DVector"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Move"",
                    ""isComposite"": true,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": ""up"",
                    ""id"": ""6e1add0f-8a38-438a-833b-32384241fe9b"",
                    ""path"": ""<Keyboard>/w"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard"",
                    ""action"": ""Move"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""down"",
                    ""id"": ""b7e3f90b-88ea-4545-94f0-add125d90e1c"",
                    ""path"": ""<Keyboard>/s"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard"",
                    ""action"": ""Move"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""left"",
                    ""id"": ""c54fdbce-acc7-4181-a6eb-1685ea242cc6"",
                    ""path"": ""<Keyboard>/a"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard"",
                    ""action"": ""Move"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""right"",
                    ""id"": ""12b4882b-b3e7-4091-8919-840aeb424c45"",
                    ""path"": ""<Keyboard>/d"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard"",
                    ""action"": ""Move"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""Up/Left/Down/Right"",
                    ""id"": ""854e82ca-3a19-4895-b95f-bf61f20886e8"",
                    ""path"": ""2DVector"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Move"",
                    ""isComposite"": true,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": ""right"",
                    ""id"": ""cded1b47-85a7-4b78-b856-775f1e5f2723"",
                    ""path"": ""<Keyboard>/rightArrow"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard"",
                    ""action"": ""Move"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""left"",
                    ""id"": ""479081f4-e63b-4a05-83a2-cc795a29994b"",
                    ""path"": ""<Keyboard>/leftArrow"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard"",
                    ""action"": ""Move"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""down"",
                    ""id"": ""6218abb1-2e3a-4db5-bc96-1e42ab9a7a2a"",
                    ""path"": ""<Keyboard>/downArrow"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard"",
                    ""action"": ""Move"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""up"",
                    ""id"": ""64dd8251-2f4e-4739-aa76-919774f7b447"",
                    ""path"": ""<Keyboard>/upArrow"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard"",
                    ""action"": ""Move"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": """",
                    ""id"": ""6aac2106-8097-4ca0-bed0-e15c034f3e8a"",
                    ""path"": ""<Keyboard>/1"",
                    ""interactions"": """",
                    ""processors"": ""Scale(factor=0)"",
                    ""groups"": ""Keyboard"",
                    ""action"": ""SelectActionWithNumbers"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""03a185b0-2c03-49f1-91e8-0642e07666e9"",
                    ""path"": ""<Keyboard>/2"",
                    ""interactions"": """",
                    ""processors"": ""Scale"",
                    ""groups"": ""Keyboard"",
                    ""action"": ""SelectActionWithNumbers"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""e70beca6-494d-4df3-b806-17780e7c46fe"",
                    ""path"": ""<Keyboard>/3"",
                    ""interactions"": """",
                    ""processors"": ""Scale(factor=2)"",
                    ""groups"": ""Keyboard"",
                    ""action"": ""SelectActionWithNumbers"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""b9952843-0993-4f1c-a4b0-1b375c5f12e6"",
                    ""path"": ""<Keyboard>/4"",
                    ""interactions"": """",
                    ""processors"": ""Scale(factor=3)"",
                    ""groups"": ""Keyboard"",
                    ""action"": ""SelectActionWithNumbers"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""63bf89c2-c4f2-4bac-9d66-06682557fc0b"",
                    ""path"": ""<Keyboard>/5"",
                    ""interactions"": """",
                    ""processors"": ""Scale(factor=4)"",
                    ""groups"": ""Keyboard"",
                    ""action"": ""SelectActionWithNumbers"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""6d755625-d55f-416e-858d-71ccfa2ccf48"",
                    ""path"": ""<Keyboard>/6"",
                    ""interactions"": """",
                    ""processors"": ""Scale(factor=5)"",
                    ""groups"": ""Keyboard"",
                    ""action"": ""SelectActionWithNumbers"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""74c1b448-5cfa-49de-9544-ae53d6467777"",
                    ""path"": ""<Keyboard>/7"",
                    ""interactions"": """",
                    ""processors"": ""Scale(factor=6)"",
                    ""groups"": ""Keyboard"",
                    ""action"": ""SelectActionWithNumbers"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""0968d2cb-25b1-4cf7-a1c8-fee87a47404e"",
                    ""path"": ""<Keyboard>/8"",
                    ""interactions"": """",
                    ""processors"": ""Scale(factor=7)"",
                    ""groups"": ""Keyboard"",
                    ""action"": ""SelectActionWithNumbers"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""f89d3c46-1db9-4310-b551-80845ae5ab9e"",
                    ""path"": ""<Keyboard>/9"",
                    ""interactions"": """",
                    ""processors"": ""Scale(factor=8)"",
                    ""groups"": ""Keyboard"",
                    ""action"": ""SelectActionWithNumbers"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""a7fed3b4-3264-4fcd-967d-4023b8a578de"",
                    ""path"": ""<Keyboard>/0"",
                    ""interactions"": """",
                    ""processors"": ""Scale(factor=9)"",
                    ""groups"": ""Keyboard"",
                    ""action"": ""SelectActionWithNumbers"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""f7d44202-7206-44e6-9936-dad7f0e20fcd"",
                    ""path"": ""<Keyboard>/tab"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard"",
                    ""action"": ""SwitchSelectedPlayer"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""f72b7178-c877-4370-aa50-482b7d6beb56"",
                    ""path"": ""<Keyboard>/escape"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard"",
                    ""action"": ""OpenSettings"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""83d19233-2b92-4769-a8e9-c2265a3701b2"",
                    ""path"": ""<Mouse>/rightButton"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard"",
                    ""action"": ""RotateRight"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""89c06352-04bc-4281-b015-b6b22030af8c"",
                    ""path"": ""<Mouse>/rightButton"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard"",
                    ""action"": ""RotateLeft"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": ""Q/E"",
                    ""id"": ""0973528c-56b8-457d-8993-fb92a3996523"",
                    ""path"": ""1DAxis"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Rotate"",
                    ""isComposite"": true,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": ""positive"",
                    ""id"": ""33722ce7-3b1b-42fd-8fd8-7e91d64c18a9"",
                    ""path"": ""<Keyboard>/q"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Rotate"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""negative"",
                    ""id"": ""400e313b-29e5-41bb-9c2f-55fa4a8282c6"",
                    ""path"": ""<Keyboard>/e"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Rotate"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": """",
                    ""id"": ""4a77cbee-def2-4166-ba5c-1c4c3c9dee4c"",
                    ""path"": ""<Keyboard>/e"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard"",
                    ""action"": ""Interact"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""c03d0871-0f07-4d95-a30a-006a723202cb"",
                    ""path"": ""<Mouse>/rightButton"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard"",
                    ""action"": ""RightClick"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""513dfe54-f9f8-493c-9892-bfab8abc6e8b"",
                    ""path"": ""<Mouse>/leftButton"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard"",
                    ""action"": ""Click"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""1a3fd627-2aa6-4824-b187-1229101aaeed"",
                    ""path"": ""<Mouse>/scroll"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Zoom"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                }
            ]
        }
    ],
    ""controlSchemes"": [
        {
            ""name"": ""Gamepad"",
            ""bindingGroup"": ""Gamepad"",
            ""devices"": [
                {
                    ""devicePath"": ""<Gamepad>"",
                    ""isOptional"": false,
                    ""isOR"": false
                }
            ]
        },
        {
            ""name"": ""Keyboard"",
            ""bindingGroup"": ""Keyboard"",
            ""devices"": [
                {
                    ""devicePath"": ""<Keyboard>"",
                    ""isOptional"": false,
                    ""isOR"": false
                },
                {
                    ""devicePath"": ""<Mouse>"",
                    ""isOptional"": false,
                    ""isOR"": false
                }
            ]
        }
    ]
}");
        // PlayerCamera
        m_PlayerCamera = asset.FindActionMap("PlayerCamera", throwIfNotFound: true);
        m_PlayerCamera_Move = m_PlayerCamera.FindAction("Move", throwIfNotFound: true);
        m_PlayerCamera_Zoom = m_PlayerCamera.FindAction("Zoom", throwIfNotFound: true);
        m_PlayerCamera_Space = m_PlayerCamera.FindAction("Space", throwIfNotFound: true);
        m_PlayerCamera_Click = m_PlayerCamera.FindAction("Click", throwIfNotFound: true);
        m_PlayerCamera_Rotate = m_PlayerCamera.FindAction("Rotate", throwIfNotFound: true);
        m_PlayerCamera_RightClick = m_PlayerCamera.FindAction("RightClick", throwIfNotFound: true);
        m_PlayerCamera_RotateLeft = m_PlayerCamera.FindAction("RotateLeft", throwIfNotFound: true);
        m_PlayerCamera_RotateRight = m_PlayerCamera.FindAction("RotateRight", throwIfNotFound: true);
        m_PlayerCamera_OpenSettings = m_PlayerCamera.FindAction("OpenSettings", throwIfNotFound: true);
        m_PlayerCamera_PointerDelta = m_PlayerCamera.FindAction("PointerDelta", throwIfNotFound: true);
        m_PlayerCamera_PointerPosition = m_PlayerCamera.FindAction("PointerPosition", throwIfNotFound: true);
        m_PlayerCamera_SwitchSelectedPlayer = m_PlayerCamera.FindAction("SwitchSelectedPlayer", throwIfNotFound: true);
        m_PlayerCamera_SelectActionWithNumbers = m_PlayerCamera.FindAction("SelectActionWithNumbers", throwIfNotFound: true);
        m_PlayerCamera_Interact = m_PlayerCamera.FindAction("Interact", throwIfNotFound: true);
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

    // PlayerCamera
    private readonly InputActionMap m_PlayerCamera;
    private IPlayerCameraActions m_PlayerCameraActionsCallbackInterface;
    private readonly InputAction m_PlayerCamera_Move;
    private readonly InputAction m_PlayerCamera_Zoom;
    private readonly InputAction m_PlayerCamera_Space;
    private readonly InputAction m_PlayerCamera_Click;
    private readonly InputAction m_PlayerCamera_Rotate;
    private readonly InputAction m_PlayerCamera_RightClick;
    private readonly InputAction m_PlayerCamera_RotateLeft;
    private readonly InputAction m_PlayerCamera_RotateRight;
    private readonly InputAction m_PlayerCamera_OpenSettings;
    private readonly InputAction m_PlayerCamera_PointerDelta;
    private readonly InputAction m_PlayerCamera_PointerPosition;
    private readonly InputAction m_PlayerCamera_SwitchSelectedPlayer;
    private readonly InputAction m_PlayerCamera_SelectActionWithNumbers;
    private readonly InputAction m_PlayerCamera_Interact;
    public struct PlayerCameraActions
    {
        private @PlayerActions m_Wrapper;
        public PlayerCameraActions(@PlayerActions wrapper) { m_Wrapper = wrapper; }
        public InputAction @Move => m_Wrapper.m_PlayerCamera_Move;
        public InputAction @Zoom => m_Wrapper.m_PlayerCamera_Zoom;
        public InputAction @Space => m_Wrapper.m_PlayerCamera_Space;
        public InputAction @Click => m_Wrapper.m_PlayerCamera_Click;
        public InputAction @Rotate => m_Wrapper.m_PlayerCamera_Rotate;
        public InputAction @RightClick => m_Wrapper.m_PlayerCamera_RightClick;
        public InputAction @RotateLeft => m_Wrapper.m_PlayerCamera_RotateLeft;
        public InputAction @RotateRight => m_Wrapper.m_PlayerCamera_RotateRight;
        public InputAction @OpenSettings => m_Wrapper.m_PlayerCamera_OpenSettings;
        public InputAction @PointerDelta => m_Wrapper.m_PlayerCamera_PointerDelta;
        public InputAction @PointerPosition => m_Wrapper.m_PlayerCamera_PointerPosition;
        public InputAction @SwitchSelectedPlayer => m_Wrapper.m_PlayerCamera_SwitchSelectedPlayer;
        public InputAction @SelectActionWithNumbers => m_Wrapper.m_PlayerCamera_SelectActionWithNumbers;
        public InputAction @Interact => m_Wrapper.m_PlayerCamera_Interact;
        public InputActionMap Get() { return m_Wrapper.m_PlayerCamera; }
        public void Enable() { Get().Enable(); }
        public void Disable() { Get().Disable(); }
        public bool enabled => Get().enabled;
        public static implicit operator InputActionMap(PlayerCameraActions set) { return set.Get(); }
        public void SetCallbacks(IPlayerCameraActions instance)
        {
            if (m_Wrapper.m_PlayerCameraActionsCallbackInterface != null)
            {
                @Move.started -= m_Wrapper.m_PlayerCameraActionsCallbackInterface.OnMove;
                @Move.performed -= m_Wrapper.m_PlayerCameraActionsCallbackInterface.OnMove;
                @Move.canceled -= m_Wrapper.m_PlayerCameraActionsCallbackInterface.OnMove;
                @Zoom.started -= m_Wrapper.m_PlayerCameraActionsCallbackInterface.OnZoom;
                @Zoom.performed -= m_Wrapper.m_PlayerCameraActionsCallbackInterface.OnZoom;
                @Zoom.canceled -= m_Wrapper.m_PlayerCameraActionsCallbackInterface.OnZoom;
                @Space.started -= m_Wrapper.m_PlayerCameraActionsCallbackInterface.OnSpace;
                @Space.performed -= m_Wrapper.m_PlayerCameraActionsCallbackInterface.OnSpace;
                @Space.canceled -= m_Wrapper.m_PlayerCameraActionsCallbackInterface.OnSpace;
                @Click.started -= m_Wrapper.m_PlayerCameraActionsCallbackInterface.OnClick;
                @Click.performed -= m_Wrapper.m_PlayerCameraActionsCallbackInterface.OnClick;
                @Click.canceled -= m_Wrapper.m_PlayerCameraActionsCallbackInterface.OnClick;
                @Rotate.started -= m_Wrapper.m_PlayerCameraActionsCallbackInterface.OnRotate;
                @Rotate.performed -= m_Wrapper.m_PlayerCameraActionsCallbackInterface.OnRotate;
                @Rotate.canceled -= m_Wrapper.m_PlayerCameraActionsCallbackInterface.OnRotate;
                @RightClick.started -= m_Wrapper.m_PlayerCameraActionsCallbackInterface.OnRightClick;
                @RightClick.performed -= m_Wrapper.m_PlayerCameraActionsCallbackInterface.OnRightClick;
                @RightClick.canceled -= m_Wrapper.m_PlayerCameraActionsCallbackInterface.OnRightClick;
                @RotateLeft.started -= m_Wrapper.m_PlayerCameraActionsCallbackInterface.OnRotateLeft;
                @RotateLeft.performed -= m_Wrapper.m_PlayerCameraActionsCallbackInterface.OnRotateLeft;
                @RotateLeft.canceled -= m_Wrapper.m_PlayerCameraActionsCallbackInterface.OnRotateLeft;
                @RotateRight.started -= m_Wrapper.m_PlayerCameraActionsCallbackInterface.OnRotateRight;
                @RotateRight.performed -= m_Wrapper.m_PlayerCameraActionsCallbackInterface.OnRotateRight;
                @RotateRight.canceled -= m_Wrapper.m_PlayerCameraActionsCallbackInterface.OnRotateRight;
                @OpenSettings.started -= m_Wrapper.m_PlayerCameraActionsCallbackInterface.OnOpenSettings;
                @OpenSettings.performed -= m_Wrapper.m_PlayerCameraActionsCallbackInterface.OnOpenSettings;
                @OpenSettings.canceled -= m_Wrapper.m_PlayerCameraActionsCallbackInterface.OnOpenSettings;
                @PointerDelta.started -= m_Wrapper.m_PlayerCameraActionsCallbackInterface.OnPointerDelta;
                @PointerDelta.performed -= m_Wrapper.m_PlayerCameraActionsCallbackInterface.OnPointerDelta;
                @PointerDelta.canceled -= m_Wrapper.m_PlayerCameraActionsCallbackInterface.OnPointerDelta;
                @PointerPosition.started -= m_Wrapper.m_PlayerCameraActionsCallbackInterface.OnPointerPosition;
                @PointerPosition.performed -= m_Wrapper.m_PlayerCameraActionsCallbackInterface.OnPointerPosition;
                @PointerPosition.canceled -= m_Wrapper.m_PlayerCameraActionsCallbackInterface.OnPointerPosition;
                @SwitchSelectedPlayer.started -= m_Wrapper.m_PlayerCameraActionsCallbackInterface.OnSwitchSelectedPlayer;
                @SwitchSelectedPlayer.performed -= m_Wrapper.m_PlayerCameraActionsCallbackInterface.OnSwitchSelectedPlayer;
                @SwitchSelectedPlayer.canceled -= m_Wrapper.m_PlayerCameraActionsCallbackInterface.OnSwitchSelectedPlayer;
                @SelectActionWithNumbers.started -= m_Wrapper.m_PlayerCameraActionsCallbackInterface.OnSelectActionWithNumbers;
                @SelectActionWithNumbers.performed -= m_Wrapper.m_PlayerCameraActionsCallbackInterface.OnSelectActionWithNumbers;
                @SelectActionWithNumbers.canceled -= m_Wrapper.m_PlayerCameraActionsCallbackInterface.OnSelectActionWithNumbers;
                @Interact.started -= m_Wrapper.m_PlayerCameraActionsCallbackInterface.OnInteract;
                @Interact.performed -= m_Wrapper.m_PlayerCameraActionsCallbackInterface.OnInteract;
                @Interact.canceled -= m_Wrapper.m_PlayerCameraActionsCallbackInterface.OnInteract;
            }
            m_Wrapper.m_PlayerCameraActionsCallbackInterface = instance;
            if (instance != null)
            {
                @Move.started += instance.OnMove;
                @Move.performed += instance.OnMove;
                @Move.canceled += instance.OnMove;
                @Zoom.started += instance.OnZoom;
                @Zoom.performed += instance.OnZoom;
                @Zoom.canceled += instance.OnZoom;
                @Space.started += instance.OnSpace;
                @Space.performed += instance.OnSpace;
                @Space.canceled += instance.OnSpace;
                @Click.started += instance.OnClick;
                @Click.performed += instance.OnClick;
                @Click.canceled += instance.OnClick;
                @Rotate.started += instance.OnRotate;
                @Rotate.performed += instance.OnRotate;
                @Rotate.canceled += instance.OnRotate;
                @RightClick.started += instance.OnRightClick;
                @RightClick.performed += instance.OnRightClick;
                @RightClick.canceled += instance.OnRightClick;
                @RotateLeft.started += instance.OnRotateLeft;
                @RotateLeft.performed += instance.OnRotateLeft;
                @RotateLeft.canceled += instance.OnRotateLeft;
                @RotateRight.started += instance.OnRotateRight;
                @RotateRight.performed += instance.OnRotateRight;
                @RotateRight.canceled += instance.OnRotateRight;
                @OpenSettings.started += instance.OnOpenSettings;
                @OpenSettings.performed += instance.OnOpenSettings;
                @OpenSettings.canceled += instance.OnOpenSettings;
                @PointerDelta.started += instance.OnPointerDelta;
                @PointerDelta.performed += instance.OnPointerDelta;
                @PointerDelta.canceled += instance.OnPointerDelta;
                @PointerPosition.started += instance.OnPointerPosition;
                @PointerPosition.performed += instance.OnPointerPosition;
                @PointerPosition.canceled += instance.OnPointerPosition;
                @SwitchSelectedPlayer.started += instance.OnSwitchSelectedPlayer;
                @SwitchSelectedPlayer.performed += instance.OnSwitchSelectedPlayer;
                @SwitchSelectedPlayer.canceled += instance.OnSwitchSelectedPlayer;
                @SelectActionWithNumbers.started += instance.OnSelectActionWithNumbers;
                @SelectActionWithNumbers.performed += instance.OnSelectActionWithNumbers;
                @SelectActionWithNumbers.canceled += instance.OnSelectActionWithNumbers;
                @Interact.started += instance.OnInteract;
                @Interact.performed += instance.OnInteract;
                @Interact.canceled += instance.OnInteract;
            }
        }
    }
    public PlayerCameraActions @PlayerCamera => new PlayerCameraActions(this);
    private int m_GamepadSchemeIndex = -1;
    public InputControlScheme GamepadScheme
    {
        get
        {
            if (m_GamepadSchemeIndex == -1) m_GamepadSchemeIndex = asset.FindControlSchemeIndex("Gamepad");
            return asset.controlSchemes[m_GamepadSchemeIndex];
        }
    }
    private int m_KeyboardSchemeIndex = -1;
    public InputControlScheme KeyboardScheme
    {
        get
        {
            if (m_KeyboardSchemeIndex == -1) m_KeyboardSchemeIndex = asset.FindControlSchemeIndex("Keyboard");
            return asset.controlSchemes[m_KeyboardSchemeIndex];
        }
    }
    public interface IPlayerCameraActions
    {
        void OnMove(InputAction.CallbackContext context);
        void OnZoom(InputAction.CallbackContext context);
        void OnSpace(InputAction.CallbackContext context);
        void OnClick(InputAction.CallbackContext context);
        void OnRotate(InputAction.CallbackContext context);
        void OnRightClick(InputAction.CallbackContext context);
        void OnRotateLeft(InputAction.CallbackContext context);
        void OnRotateRight(InputAction.CallbackContext context);
        void OnOpenSettings(InputAction.CallbackContext context);
        void OnPointerDelta(InputAction.CallbackContext context);
        void OnPointerPosition(InputAction.CallbackContext context);
        void OnSwitchSelectedPlayer(InputAction.CallbackContext context);
        void OnSelectActionWithNumbers(InputAction.CallbackContext context);
        void OnInteract(InputAction.CallbackContext context);
    }
}
