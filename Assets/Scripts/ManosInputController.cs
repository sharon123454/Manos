using System.Collections.Generic;
using UnityEngine.InputSystem;
using System.Collections;
using UnityEngine;

public class ManosInputController : MonoBehaviour
{
    public static ManosInputController Instance { get; private set; }

    internal PlayerInput PlayerInput { get; private set; }
    internal InputAction Move { get; private set; }
    internal InputAction Space { get; private set; }
    internal InputAction Pause { get; private set; }
    internal InputAction Scroll { get; private set; }
    internal InputAction Click { get; private set; }
    internal InputAction Rotate { get; private set; }
    internal InputAction RotateLeft { get; private set; }
    internal InputAction RotateRight { get; private set; }
    internal InputAction PointerDelta { get; private set; }
    internal InputAction PointerPosition { get; private set; }
    internal InputAction SwitchSelectedPlayer { get; private set; }

    private void Awake()
    {
        if (Instance && Instance != this)
            Destroy(gameObject);

        Instance = this;
        PlayerInput = GetComponent<PlayerInput>();
    }

    private void OnEnable()
    {
        Move = PlayerInput.actions["Move"];
        Click = PlayerInput.actions["Click"];
        Space = PlayerInput.actions["Space"];
        Pause = PlayerInput.actions["Pause"];
        Scroll = PlayerInput.actions["Scroll"];
        Rotate = PlayerInput.actions["Rotate"];
        RotateLeft = PlayerInput.actions["RotateLeft"];
        RotateRight = PlayerInput.actions["RotateRight"];
        PointerDelta = PlayerInput.actions["PointerDelta"];
        PointerPosition = PlayerInput.actions["PointerPosition"];
        SwitchSelectedPlayer = PlayerInput.actions["SwitchSelectedPlayer"];
        Move.Enable();
        Click.Enable();
        Space.Enable();
        Pause.Enable();
        Scroll.Enable();
        Rotate.Enable();
        RotateLeft.Enable();
        RotateRight.Enable();
        PointerDelta.Enable();
        PointerPosition.Enable();
        SwitchSelectedPlayer.Enable();
    }

    private void OnDisable()
    {
        Move.Disable();
        Click.Disable();
        Space.Disable();
        Pause.Disable();
        Scroll.Disable();
        Rotate.Disable();
        RotateLeft.Disable();
        RotateRight.Disable();
        PointerDelta.Disable();
        PointerPosition.Disable();
        SwitchSelectedPlayer.Disable();
    }

    public Vector3 GetPointerPosition()
    {
        Vector2 _pointerPosition = PointerPosition.ReadValue<Vector2>();
        return _pointerPosition;
    }

    public Vector3 GetMoveDirection(Transform movingObject)
    {
        Vector2 _playerMoveInput = Move.ReadValue<Vector2>();
        Vector3 _InputAsVector3 = new Vector3(_playerMoveInput.x, 0, _playerMoveInput.y);
        Vector3 _moveDirection = movingObject.forward * _InputAsVector3.z + movingObject.right * _InputAsVector3.x;

        return _moveDirection;
    }

    public Vector3 GetRotateCamBy()
    {
        float _playerRotateInput = Rotate.ReadValue<float>();
        return new Vector3(0, _playerRotateInput, 0);
    }

}