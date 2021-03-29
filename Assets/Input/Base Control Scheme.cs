// GENERATED AUTOMATICALLY FROM 'Assets/Input/Base Control Scheme.inputactions'

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Utilities;

public class @BaseControlScheme : IInputActionCollection, IDisposable
{
    public InputActionAsset asset { get; }
    public @BaseControlScheme()
    {
        asset = InputActionAsset.FromJson(@"{
    ""name"": ""Base Control Scheme"",
    ""maps"": [
        {
            ""name"": ""ArrowMove"",
            ""id"": ""210d3e7a-2c8f-4f2b-a6e6-261cad753857"",
            ""actions"": [
                {
                    ""name"": ""Move"",
                    ""type"": ""Button"",
                    ""id"": ""a91e3b79-981a-407c-a366-809363c92e0d"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""Shoot"",
                    ""type"": ""Button"",
                    ""id"": ""2f212cbd-4bf7-47fd-a453-59dc211f7168"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": ""Clamp(max=1)"",
                    ""interactions"": """"
                }
            ],
            ""bindings"": [
                {
                    ""name"": ""2D Vector"",
                    ""id"": ""ffbe8be7-a3bc-4247-911d-505af30ed41d"",
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
                    ""id"": ""52b73217-ed34-4921-b9d4-e1c1e77ee0a6"",
                    ""path"": ""<Keyboard>/w"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Move"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""down"",
                    ""id"": ""16f6e9d8-ce35-49a2-8384-813f67aaa906"",
                    ""path"": ""<Keyboard>/s"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Move"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""left"",
                    ""id"": ""2cd689b1-645c-4b90-8277-1ff408d72bf6"",
                    ""path"": ""<Keyboard>/a"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Move"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""right"",
                    ""id"": ""1ec156d9-8208-425d-a0c1-1525d3d5a7ca"",
                    ""path"": ""<Keyboard>/d"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Move"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""2D Vector"",
                    ""id"": ""93846d2c-c997-49d7-83c1-d5e766cc649e"",
                    ""path"": ""2DVector(mode=1)"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Shoot"",
                    ""isComposite"": true,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": ""up"",
                    ""id"": ""8eae32ac-fdd7-4408-98ec-7098dcf63aa7"",
                    ""path"": ""<Keyboard>/upArrow"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Shoot"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""down"",
                    ""id"": ""a1faff7f-05f7-4409-9d7c-e9661ed43f56"",
                    ""path"": ""<Keyboard>/downArrow"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Shoot"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""left"",
                    ""id"": ""ce1ba7ce-1e5d-43ca-8171-121d5cd5c96a"",
                    ""path"": ""<Keyboard>/leftArrow"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Shoot"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""right"",
                    ""id"": ""80603147-cca0-4ef8-b513-c8c55673114e"",
                    ""path"": ""<Keyboard>/rightArrow"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Shoot"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                }
            ]
        }
    ],
    ""controlSchemes"": []
}");
        // ArrowMove
        m_ArrowMove = asset.FindActionMap("ArrowMove", throwIfNotFound: true);
        m_ArrowMove_Move = m_ArrowMove.FindAction("Move", throwIfNotFound: true);
        m_ArrowMove_Shoot = m_ArrowMove.FindAction("Shoot", throwIfNotFound: true);
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

    // ArrowMove
    private readonly InputActionMap m_ArrowMove;
    private IArrowMoveActions m_ArrowMoveActionsCallbackInterface;
    private readonly InputAction m_ArrowMove_Move;
    private readonly InputAction m_ArrowMove_Shoot;
    public struct ArrowMoveActions
    {
        private @BaseControlScheme m_Wrapper;
        public ArrowMoveActions(@BaseControlScheme wrapper) { m_Wrapper = wrapper; }
        public InputAction @Move => m_Wrapper.m_ArrowMove_Move;
        public InputAction @Shoot => m_Wrapper.m_ArrowMove_Shoot;
        public InputActionMap Get() { return m_Wrapper.m_ArrowMove; }
        public void Enable() { Get().Enable(); }
        public void Disable() { Get().Disable(); }
        public bool enabled => Get().enabled;
        public static implicit operator InputActionMap(ArrowMoveActions set) { return set.Get(); }
        public void SetCallbacks(IArrowMoveActions instance)
        {
            if (m_Wrapper.m_ArrowMoveActionsCallbackInterface != null)
            {
                @Move.started -= m_Wrapper.m_ArrowMoveActionsCallbackInterface.OnMove;
                @Move.performed -= m_Wrapper.m_ArrowMoveActionsCallbackInterface.OnMove;
                @Move.canceled -= m_Wrapper.m_ArrowMoveActionsCallbackInterface.OnMove;
                @Shoot.started -= m_Wrapper.m_ArrowMoveActionsCallbackInterface.OnShoot;
                @Shoot.performed -= m_Wrapper.m_ArrowMoveActionsCallbackInterface.OnShoot;
                @Shoot.canceled -= m_Wrapper.m_ArrowMoveActionsCallbackInterface.OnShoot;
            }
            m_Wrapper.m_ArrowMoveActionsCallbackInterface = instance;
            if (instance != null)
            {
                @Move.started += instance.OnMove;
                @Move.performed += instance.OnMove;
                @Move.canceled += instance.OnMove;
                @Shoot.started += instance.OnShoot;
                @Shoot.performed += instance.OnShoot;
                @Shoot.canceled += instance.OnShoot;
            }
        }
    }
    public ArrowMoveActions @ArrowMove => new ArrowMoveActions(this);
    public interface IArrowMoveActions
    {
        void OnMove(InputAction.CallbackContext context);
        void OnShoot(InputAction.CallbackContext context);
    }
}
