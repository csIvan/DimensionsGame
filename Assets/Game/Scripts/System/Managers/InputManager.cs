using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputManager : MonoBehaviour {

    public static InputManager Instance;

    // Input Map
    private PlayerInputActions InputActions;

    // Input Events
    public event Action OnJumpStarted;
    public event Action OnJumpEnded;
    public event Action OnCrouch;


    // --------------------------------------------------------------------
    private void Awake() {
        if(Instance != null && Instance != this) {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
        InputActions = new PlayerInputActions();
    }


    // --------------------------------------------------------------------
    private void OnEnable() {
        InputActions.Player.Enable();
        InputActions.Player.Jump.performed += JumpButtonPressed;
        InputActions.Player.Jump.canceled += JumpButtonReleased;
        InputActions.Player.Crouch.performed += CrouchButtonPressed;
    }


    // --------------------------------------------------------------------
    private void OnDisable() {
        InputActions.Player.Disable();
        InputActions.Player.Jump.performed -= JumpButtonPressed;
        InputActions.Player.Jump.canceled -= JumpButtonReleased;
        InputActions.Player.Crouch.performed -= CrouchButtonPressed;
    }


    // --------------------------------------------------------------------
    public Vector3 GetMoveDirectionNormalized() {
        Vector2 InputVector = InputActions.Player.Move.ReadValue<Vector2>();
        Vector3 MoveDirection = new Vector3(InputVector.x, 0.0f, InputVector.y);

        return MoveDirection.normalized;
    }


    // --------------------------------------------------------------------
    public Vector2 GetMouseInput() {
        Vector2 MouseDelta = InputActions.Player.Look.ReadValue<Vector2>();
        return MouseDelta;
    }


    // --------------------------------------------------------------------
    private void JumpButtonPressed(InputAction.CallbackContext context) {
        OnJumpStarted?.Invoke();
    }


    // --------------------------------------------------------------------
    private void JumpButtonReleased(InputAction.CallbackContext context) {
        OnJumpEnded?.Invoke();
    }


    // --------------------------------------------------------------------
    private void CrouchButtonPressed(InputAction.CallbackContext context) {
        OnCrouch?.Invoke();
    }
}