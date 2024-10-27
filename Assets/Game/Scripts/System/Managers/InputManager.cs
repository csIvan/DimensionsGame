using System;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputManager : MonoBehaviour {

    public static InputManager Instance;

    // Input Map
    private PlayerInputActions InputActions;

    // Input Events
    public static event Action OnJumpStarted;
    public static event Action OnJumpEnded;
    public static event Action OnInteract;
    public static event Action<bool> OnPause;


    private bool bGamePaused;


    // --------------------------------------------------------------------
    private void Awake() {
        if(Instance != null && Instance != this) {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        InputActions = new PlayerInputActions();
        bGamePaused = false;
    }


    // --------------------------------------------------------------------
    private void OnEnable() {
        InputActions.Player.Enable();
        InputActions.Player.Jump.performed += JumpButtonPressed;
        InputActions.Player.Jump.canceled += JumpButtonReleased;
        InputActions.Player.Interact.performed += InteractButtonPressed;
        InputActions.Player.Pause.performed += PauseButtonPressed;
    }


    // --------------------------------------------------------------------
    private void OnDisable() {
        InputActions.Player.Disable();
        InputActions.Player.Jump.performed -= JumpButtonPressed;
        InputActions.Player.Jump.canceled -= JumpButtonReleased;
        InputActions.Player.Interact.performed -= InteractButtonPressed;
        InputActions.Player.Pause.performed -= PauseButtonPressed;
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
    private void InteractButtonPressed(InputAction.CallbackContext context) {
        OnInteract?.Invoke();
    }


    // --------------------------------------------------------------------
    private void PauseButtonPressed(InputAction.CallbackContext context) {
        if (context.performed) {
            PauseGame();
        }
    }


    // --------------------------------------------------------------------
    public void PauseGame() {
        bGamePaused = !bGamePaused;
        OnPause?.Invoke(bGamePaused);
    }
}