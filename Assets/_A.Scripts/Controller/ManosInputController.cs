using System.Collections.Generic;
using UnityEngine.InputSystem;
using System.Collections;
using UnityEngine;

public class ManosInputController : MonoBehaviour
{
    public static ManosInputController Instance { get; private set; }

    internal InputAction Move { get; private set; }
    internal InputAction Zoom { get; private set; }
    internal InputAction Space { get; private set; }
    internal InputAction Click { get; private set; }
    internal InputAction Rotate { get; private set; }
    internal InputAction RightClick { get; private set; }
    internal InputAction RotateLeft { get; private set; }
    internal InputAction RotateRight { get; private set; }
    internal InputAction ShowAllHUD { get; private set; }
    internal InputAction OpenSettings { get; private set; }
    internal InputAction PointerDelta { get; private set; }
    internal InputAction PointerPosition { get; private set; }
    internal InputAction SwitchSelectedPlayer { get; private set; }
    internal InputAction SelectActionWithNumbers { get; private set; }

    private PlayerActions _inputActions;

    private void Awake()
    {
        if (Instance && Instance != this)
            Destroy(gameObject);

        Instance = this;
        _inputActions = new PlayerActions();
    }

    private void OnEnable()
    {
        Move = _inputActions.PlayerCamera.Move;
        Zoom = _inputActions.PlayerCamera.Zoom;
        Space = _inputActions.PlayerCamera.Space;
        Click = _inputActions.PlayerCamera.Click;
        Rotate = _inputActions.PlayerCamera.Rotate;
        RightClick = _inputActions.PlayerCamera.RightClick;
        RotateLeft = _inputActions.PlayerCamera.RotateLeft;
        RotateRight = _inputActions.PlayerCamera.RotateRight;
        ShowAllHUD = _inputActions.PlayerCamera.ShowAllHUD;
        OpenSettings = _inputActions.PlayerCamera.OpenSettings;
        PointerDelta = _inputActions.PlayerCamera.PointerDelta;
        PointerPosition = _inputActions.PlayerCamera.PointerPosition;
        SwitchSelectedPlayer = _inputActions.PlayerCamera.SwitchSelectedPlayer;
        SelectActionWithNumbers = _inputActions.PlayerCamera.SelectActionWithNumbers;

        _inputActions.PlayerCamera.Enable();
    }

    private void OnDisable()
    {
        _inputActions.PlayerCamera.Disable();
    }

    public Vector3 GetMoveDirection(Transform movingObject)
    {
        Vector2 _playerMoveInput = Move.ReadValue<Vector2>();
        Vector3 _InputAsVector3 = new Vector3(_playerMoveInput.x, 0, _playerMoveInput.y);
        Vector3 _moveDirection = movingObject.forward * _InputAsVector3.z + movingObject.right * _InputAsVector3.x;

        return _moveDirection.normalized;
    }

    public Vector3 GetPointerPosition()
    {
        Vector2 _pointerPosition = PointerPosition.ReadValue<Vector2>();
        return _pointerPosition;
    }

    public Vector3 GetRotateCamBy()
    {
        float _playerRotateInput = Rotate.ReadValue<float>();
        return new Vector3(0, _playerRotateInput, 0);
    }

}