using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class CameraFollow : MonoBehaviour {

    [Header("Camera Settings")]
    [SerializeField] private Transform Target;
    [SerializeField] private Vector3 CameraOffset = new Vector3(0, 1f, -6);
    [SerializeField] private float cameraSensitivity = 20.0f;
    [SerializeField] private float minCloseDistance = 0.5f;
    [SerializeField] private float smoothSpeed = 20f;
    [SerializeField] private LayerMask CollisionLayers;

    [Header("Clamp Settings")]
    [SerializeField] private float minVerticalAngle = -40.0f;
    [SerializeField] private float maxVerticalAngle = 60.0f;

    private float VerticalAngle = 0.0f;
    private float HorizontalAngle = 0.0f;


    // --------------------------------------------------------------------
    private void Start() {
        ResetThirdPersonCamera();
    }


    // --------------------------------------------------------------------
    private void LateUpdate() {
        HandleCameraFollow();
    }


    // --------------------------------------------------------------------
    private void HandleCameraFollow(bool smooth = true) {
        Vector2 MouseInput = InputManager.Instance.GetMouseInput();
        float mouseX = MouseInput.x * Time.deltaTime * cameraSensitivity;
        float mouseY = MouseInput.y * Time.deltaTime * cameraSensitivity;

        HorizontalAngle += mouseX;
        VerticalAngle -= mouseY;
        VerticalAngle = Mathf.Clamp(VerticalAngle, minVerticalAngle, maxVerticalAngle);

        Quaternion CameraRotation = Quaternion.Euler(VerticalAngle, HorizontalAngle, 0);
        Vector3 CameraPosition = Target.position + CameraRotation * CameraOffset;

        AdjustCameraIfBlocked(Target.position, ref CameraPosition);
        float smoothValue = smoothSpeed * Time.deltaTime;
        transform.position = (smooth) ? Vector3.Lerp(transform.position, CameraPosition, smoothValue)
                                      : CameraPosition;

        transform.LookAt(Target.position + Vector3.up);
    }

    
    // --------------------------------------------------------------------
    private void AdjustCameraIfBlocked(Vector3 LookAtPosition, ref Vector3 NewPosition) {
        Vector3 PlayerToCameraDirection = NewPosition - LookAtPosition;

        RaycastHit hit;
        if (Physics.Raycast(LookAtPosition, PlayerToCameraDirection, out hit,
                            CameraOffset.magnitude, CollisionLayers)) {
            NewPosition = hit.point + hit.normal * minCloseDistance;
        }
    }


    // --------------------------------------------------------------------
    public void ResetThirdPersonCamera() {
        HandleCameraFollow(false);
    }

}