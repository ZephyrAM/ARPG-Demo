// GENERATED AUTOMATICALLY FROM 'Assets/InputManager.inputactions'

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Utilities;

public class @InputManager : IInputActionCollection, IDisposable
{
    public InputActionAsset asset { get; }
    public @InputManager()
    {
        asset = InputActionAsset.FromJson(@"{
    ""name"": ""InputManager"",
    ""maps"": [
        {
            ""name"": ""PlayerController"",
            ""id"": ""a6dff51f-3fa7-492f-ba70-7580003b6ea6"",
            ""actions"": [
                {
                    ""name"": ""Axis Move"",
                    ""type"": ""PassThrough"",
                    ""id"": ""23c9966f-1eee-45fd-b232-53dd1e14f38e"",
                    ""expectedControlType"": ""Vector2"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""Mouse Click"",
                    ""type"": ""Button"",
                    ""id"": ""1d30feb4-b9b3-4de7-b031-faaee2c13359"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""Stop Move"",
                    ""type"": ""Button"",
                    ""id"": ""f0840c1f-ced2-4a33-9afa-2f015a851f4c"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                }
            ],
            ""bindings"": [
                {
                    ""name"": ""WASD"",
                    ""id"": ""9b558f84-5040-4508-b0e5-21a0c21dcf11"",
                    ""path"": ""2DVector(mode=1)"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Axis Move"",
                    ""isComposite"": true,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": ""up"",
                    ""id"": ""85cb0146-a112-4095-9178-393ba160c5f3"",
                    ""path"": ""<Keyboard>/w"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Axis Move"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""down"",
                    ""id"": ""15e2f92c-feb1-4a43-9a12-1936c4a2a532"",
                    ""path"": ""<Keyboard>/s"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Axis Move"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""left"",
                    ""id"": ""270556f4-7d7a-4350-bf0e-7e9b3746e363"",
                    ""path"": ""<Keyboard>/a"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Axis Move"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""right"",
                    ""id"": ""ba73fd88-b3d6-4c2e-922e-f05c95c39c88"",
                    ""path"": ""<Keyboard>/d"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Axis Move"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""Gamepad"",
                    ""id"": ""67ceb155-2f40-41f3-8f1e-5598cf14def2"",
                    ""path"": ""2DVector(mode=1)"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Axis Move"",
                    ""isComposite"": true,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": ""up"",
                    ""id"": ""4e555aec-4249-428e-ace8-0e3d90ea52f3"",
                    ""path"": ""<Gamepad>/leftStick/up"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Axis Move"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""down"",
                    ""id"": ""affdfe74-31bd-4fb3-a360-70ae7d23f1df"",
                    ""path"": ""<Gamepad>/leftStick/down"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Axis Move"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""left"",
                    ""id"": ""26005b38-fb36-4e0d-942d-222e6de0eeaf"",
                    ""path"": ""<Gamepad>/leftStick/left"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Axis Move"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""right"",
                    ""id"": ""0f6e2476-50f1-4c21-b0af-59b968953c4f"",
                    ""path"": ""<Gamepad>/leftStick/right"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Axis Move"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": """",
                    ""id"": ""d74f6586-0148-4018-b1ea-c6918a403652"",
                    ""path"": ""<Mouse>/leftButton"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Mouse Click"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""3cb8e45e-f71e-407d-a600-713b7fb4f129"",
                    ""path"": ""<Keyboard>/shift"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Stop Move"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                }
            ]
        },
        {
            ""name"": ""CinematicControl"",
            ""id"": ""1e1134f3-af90-464b-a57a-d8ed5c907c28"",
            ""actions"": [
                {
                    ""name"": ""Escape Action"",
                    ""type"": ""Button"",
                    ""id"": ""534ec024-1383-4daa-8751-055407aa7081"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                }
            ],
            ""bindings"": [
                {
                    ""name"": """",
                    ""id"": ""073bffda-001d-4a27-8c30-0d8e76c86edd"",
                    ""path"": ""<Keyboard>/escape"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Escape Action"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                }
            ]
        },
        {
            ""name"": ""SaveSystem"",
            ""id"": ""0a509fd7-ed77-464e-aa4e-634def70d6a5"",
            ""actions"": [
                {
                    ""name"": ""Save"",
                    ""type"": ""Button"",
                    ""id"": ""0a6bbe74-ffd6-494a-843e-de7f598890db"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""Load"",
                    ""type"": ""Button"",
                    ""id"": ""1aa36ecf-f541-4e02-85d1-4edff3497e21"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""Delete"",
                    ""type"": ""Button"",
                    ""id"": ""008692ad-100a-47eb-978d-cbad9ec7afc9"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                }
            ],
            ""bindings"": [
                {
                    ""name"": """",
                    ""id"": ""0ebb0911-8ebd-4955-9e6a-9d26677ab09e"",
                    ""path"": ""<Keyboard>/f4"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Save"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""edf0f6e9-a496-4c43-aec1-06ef4e93a80e"",
                    ""path"": ""<Keyboard>/f5"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Load"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""dcd118b7-14d3-4a26-be65-f7ff90035404"",
                    ""path"": ""<Keyboard>/f8"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Delete"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                }
            ]
        }
    ],
    ""controlSchemes"": []
}");
        // PlayerController
        m_PlayerController = asset.FindActionMap("PlayerController", throwIfNotFound: true);
        m_PlayerController_AxisMove = m_PlayerController.FindAction("Axis Move", throwIfNotFound: true);
        m_PlayerController_MouseClick = m_PlayerController.FindAction("Mouse Click", throwIfNotFound: true);
        m_PlayerController_StopMove = m_PlayerController.FindAction("Stop Move", throwIfNotFound: true);
        // CinematicControl
        m_CinematicControl = asset.FindActionMap("CinematicControl", throwIfNotFound: true);
        m_CinematicControl_EscapeAction = m_CinematicControl.FindAction("Escape Action", throwIfNotFound: true);
        // SaveSystem
        m_SaveSystem = asset.FindActionMap("SaveSystem", throwIfNotFound: true);
        m_SaveSystem_Save = m_SaveSystem.FindAction("Save", throwIfNotFound: true);
        m_SaveSystem_Load = m_SaveSystem.FindAction("Load", throwIfNotFound: true);
        m_SaveSystem_Delete = m_SaveSystem.FindAction("Delete", throwIfNotFound: true);
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

    // PlayerController
    private readonly InputActionMap m_PlayerController;
    private IPlayerControllerActions m_PlayerControllerActionsCallbackInterface;
    private readonly InputAction m_PlayerController_AxisMove;
    private readonly InputAction m_PlayerController_MouseClick;
    private readonly InputAction m_PlayerController_StopMove;
    public struct PlayerControllerActions
    {
        private @InputManager m_Wrapper;
        public PlayerControllerActions(@InputManager wrapper) { m_Wrapper = wrapper; }
        public InputAction @AxisMove => m_Wrapper.m_PlayerController_AxisMove;
        public InputAction @MouseClick => m_Wrapper.m_PlayerController_MouseClick;
        public InputAction @StopMove => m_Wrapper.m_PlayerController_StopMove;
        public InputActionMap Get() { return m_Wrapper.m_PlayerController; }
        public void Enable() { Get().Enable(); }
        public void Disable() { Get().Disable(); }
        public bool enabled => Get().enabled;
        public static implicit operator InputActionMap(PlayerControllerActions set) { return set.Get(); }
        public void SetCallbacks(IPlayerControllerActions instance)
        {
            if (m_Wrapper.m_PlayerControllerActionsCallbackInterface != null)
            {
                @AxisMove.started -= m_Wrapper.m_PlayerControllerActionsCallbackInterface.OnAxisMove;
                @AxisMove.performed -= m_Wrapper.m_PlayerControllerActionsCallbackInterface.OnAxisMove;
                @AxisMove.canceled -= m_Wrapper.m_PlayerControllerActionsCallbackInterface.OnAxisMove;
                @MouseClick.started -= m_Wrapper.m_PlayerControllerActionsCallbackInterface.OnMouseClick;
                @MouseClick.performed -= m_Wrapper.m_PlayerControllerActionsCallbackInterface.OnMouseClick;
                @MouseClick.canceled -= m_Wrapper.m_PlayerControllerActionsCallbackInterface.OnMouseClick;
                @StopMove.started -= m_Wrapper.m_PlayerControllerActionsCallbackInterface.OnStopMove;
                @StopMove.performed -= m_Wrapper.m_PlayerControllerActionsCallbackInterface.OnStopMove;
                @StopMove.canceled -= m_Wrapper.m_PlayerControllerActionsCallbackInterface.OnStopMove;
            }
            m_Wrapper.m_PlayerControllerActionsCallbackInterface = instance;
            if (instance != null)
            {
                @AxisMove.started += instance.OnAxisMove;
                @AxisMove.performed += instance.OnAxisMove;
                @AxisMove.canceled += instance.OnAxisMove;
                @MouseClick.started += instance.OnMouseClick;
                @MouseClick.performed += instance.OnMouseClick;
                @MouseClick.canceled += instance.OnMouseClick;
                @StopMove.started += instance.OnStopMove;
                @StopMove.performed += instance.OnStopMove;
                @StopMove.canceled += instance.OnStopMove;
            }
        }
    }
    public PlayerControllerActions @PlayerController => new PlayerControllerActions(this);

    // CinematicControl
    private readonly InputActionMap m_CinematicControl;
    private ICinematicControlActions m_CinematicControlActionsCallbackInterface;
    private readonly InputAction m_CinematicControl_EscapeAction;
    public struct CinematicControlActions
    {
        private @InputManager m_Wrapper;
        public CinematicControlActions(@InputManager wrapper) { m_Wrapper = wrapper; }
        public InputAction @EscapeAction => m_Wrapper.m_CinematicControl_EscapeAction;
        public InputActionMap Get() { return m_Wrapper.m_CinematicControl; }
        public void Enable() { Get().Enable(); }
        public void Disable() { Get().Disable(); }
        public bool enabled => Get().enabled;
        public static implicit operator InputActionMap(CinematicControlActions set) { return set.Get(); }
        public void SetCallbacks(ICinematicControlActions instance)
        {
            if (m_Wrapper.m_CinematicControlActionsCallbackInterface != null)
            {
                @EscapeAction.started -= m_Wrapper.m_CinematicControlActionsCallbackInterface.OnEscapeAction;
                @EscapeAction.performed -= m_Wrapper.m_CinematicControlActionsCallbackInterface.OnEscapeAction;
                @EscapeAction.canceled -= m_Wrapper.m_CinematicControlActionsCallbackInterface.OnEscapeAction;
            }
            m_Wrapper.m_CinematicControlActionsCallbackInterface = instance;
            if (instance != null)
            {
                @EscapeAction.started += instance.OnEscapeAction;
                @EscapeAction.performed += instance.OnEscapeAction;
                @EscapeAction.canceled += instance.OnEscapeAction;
            }
        }
    }
    public CinematicControlActions @CinematicControl => new CinematicControlActions(this);

    // SaveSystem
    private readonly InputActionMap m_SaveSystem;
    private ISaveSystemActions m_SaveSystemActionsCallbackInterface;
    private readonly InputAction m_SaveSystem_Save;
    private readonly InputAction m_SaveSystem_Load;
    private readonly InputAction m_SaveSystem_Delete;
    public struct SaveSystemActions
    {
        private @InputManager m_Wrapper;
        public SaveSystemActions(@InputManager wrapper) { m_Wrapper = wrapper; }
        public InputAction @Save => m_Wrapper.m_SaveSystem_Save;
        public InputAction @Load => m_Wrapper.m_SaveSystem_Load;
        public InputAction @Delete => m_Wrapper.m_SaveSystem_Delete;
        public InputActionMap Get() { return m_Wrapper.m_SaveSystem; }
        public void Enable() { Get().Enable(); }
        public void Disable() { Get().Disable(); }
        public bool enabled => Get().enabled;
        public static implicit operator InputActionMap(SaveSystemActions set) { return set.Get(); }
        public void SetCallbacks(ISaveSystemActions instance)
        {
            if (m_Wrapper.m_SaveSystemActionsCallbackInterface != null)
            {
                @Save.started -= m_Wrapper.m_SaveSystemActionsCallbackInterface.OnSave;
                @Save.performed -= m_Wrapper.m_SaveSystemActionsCallbackInterface.OnSave;
                @Save.canceled -= m_Wrapper.m_SaveSystemActionsCallbackInterface.OnSave;
                @Load.started -= m_Wrapper.m_SaveSystemActionsCallbackInterface.OnLoad;
                @Load.performed -= m_Wrapper.m_SaveSystemActionsCallbackInterface.OnLoad;
                @Load.canceled -= m_Wrapper.m_SaveSystemActionsCallbackInterface.OnLoad;
                @Delete.started -= m_Wrapper.m_SaveSystemActionsCallbackInterface.OnDelete;
                @Delete.performed -= m_Wrapper.m_SaveSystemActionsCallbackInterface.OnDelete;
                @Delete.canceled -= m_Wrapper.m_SaveSystemActionsCallbackInterface.OnDelete;
            }
            m_Wrapper.m_SaveSystemActionsCallbackInterface = instance;
            if (instance != null)
            {
                @Save.started += instance.OnSave;
                @Save.performed += instance.OnSave;
                @Save.canceled += instance.OnSave;
                @Load.started += instance.OnLoad;
                @Load.performed += instance.OnLoad;
                @Load.canceled += instance.OnLoad;
                @Delete.started += instance.OnDelete;
                @Delete.performed += instance.OnDelete;
                @Delete.canceled += instance.OnDelete;
            }
        }
    }
    public SaveSystemActions @SaveSystem => new SaveSystemActions(this);
    public interface IPlayerControllerActions
    {
        void OnAxisMove(InputAction.CallbackContext context);
        void OnMouseClick(InputAction.CallbackContext context);
        void OnStopMove(InputAction.CallbackContext context);
    }
    public interface ICinematicControlActions
    {
        void OnEscapeAction(InputAction.CallbackContext context);
    }
    public interface ISaveSystemActions
    {
        void OnSave(InputAction.CallbackContext context);
        void OnLoad(InputAction.CallbackContext context);
        void OnDelete(InputAction.CallbackContext context);
    }
}
