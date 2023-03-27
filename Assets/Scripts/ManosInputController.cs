using System.Collections.Generic;
using UnityEngine.InputSystem;
using System.Collections;
using UnityEngine;

public class ManosInputController : MonoBehaviour
{
    public static ManosInputController Instance { get; private set; }

    private PlayerInput playerInput;
    public InputAction Move { get; private set; }
    public InputAction Space { get; private set; }
    public InputAction Pause { get; private set; }
    public InputAction Rotate { get; private set; }
    public InputAction LeftClick { get; private set; }
    public InputAction PointerPosition{ get; private set; }
    public InputAction SwitchSelectedPlayer{ get; private set; }

    private void Awake()
    {
        if (Instance && Instance != this)
            Destroy(gameObject);

        Instance = this;
        playerInput = GetComponent<PlayerInput>();
    }

    private void OnEnable()
    {
        Move = playerInput.actions["Move"];
        Space = playerInput.actions["Space"];
        Pause = playerInput.actions["Pause"];
        Rotate = playerInput.actions["Rotate"];
        LeftClick = playerInput.actions["LeftClick"];
        PointerPosition = playerInput.actions["PointerPosition"];
        SwitchSelectedPlayer = playerInput.actions["SwitchSelectedPlayer"];
        Move.Enable();
        Space.Enable();
        Pause.Enable();
        Rotate.Enable();
        LeftClick.Enable();
        PointerPosition.Enable();
        SwitchSelectedPlayer.Enable();

        LeftClick.performed += leftClickPerformed;
    }

    private void OnDisable()
    {
        Move.Disable();
        Space.Disable();
        Pause.Disable();
        Rotate.Disable();
        LeftClick.Disable();
        PointerPosition.Disable();
        SwitchSelectedPlayer.Disable();
    }

    public PlayerInput GetPlayerInput() { return playerInput; }

    public Vector3 GetPointerPosition()
    {
        Vector2 _pointerPosition = PointerPosition.ReadValue<Vector2>();
        return _pointerPosition; 
    }

    public Vector3 GetMoveDirection()
    {
        Vector2 _playerMoveInput = Move.ReadValue<Vector2>();
        return new Vector3(_playerMoveInput.x, 0, _playerMoveInput.y);
    }

    public Vector3 RotateCamBy() 
    {
        if (Rotate.IsPressed())
        {
            float _playerRotateInput = Rotate.ReadValue<float>();
            return new Vector3(_playerRotateInput, 0, 0);
        }

        return Vector3.zero;
    }

    private void leftClickPerformed(InputAction.CallbackContext context)
    {
    }

}