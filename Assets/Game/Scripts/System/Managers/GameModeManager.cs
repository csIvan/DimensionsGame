using System;
using System.Collections;
using UnityEngine;

public class GameModeManager : MonoBehaviour {

    [Header("Cameras Setup")]
    [SerializeField] private Camera TransitionCamera;
    [SerializeField] private Camera Camera3D;
    [SerializeField] private Camera Camera2D;
    [SerializeField] private float transitionDuration = 1.0f; 
    [SerializeField] private float transitionDelay = 0.5f;
    [SerializeField] private AnimationCurve TransitionCurve;

    [Header("References")]
    [SerializeField] private Player3DController Controller3D;
    [SerializeField] private Player2DController Controller2D;
    [SerializeField] private GameObject Visuals3D;
    [SerializeField] private GameObject Visuals2D;
    [SerializeField] private GameObject PixelGrid;


    private Character ActiveController;
    private Character TargetController;
    private GameObject ActiveVisuals;
    private GameObject TargetVisuals;
    private Camera ActiveCamera;
    private Camera TargetCamera;
    private Vector3 GameModeStartPosition;
    private bool Is3DMode;


    // --------------------------------------------------------------------
    private void Awake() {
        Is3DMode = true;
    }


    // --------------------------------------------------------------------
    private void Start() {
        Camera3D.gameObject.SetActive(true);
        Camera2D.gameObject.SetActive(false);
        TransitionCamera.gameObject.SetActive(false);
        ActiveController = Controller3D;
        PixelGrid.SetActive(false);
    }


    // --------------------------------------------------------------------
    private void OnEnable() {
        ModeTrigger.OnModeSwitchTriggered += SwitchMode;
    }


    // --------------------------------------------------------------------
    private void OnDisable() {
        ModeTrigger.OnModeSwitchTriggered -= SwitchMode;
    }


    // --------------------------------------------------------------------
    private void SwitchMode(Transform Destination) {
        GameModeStartPosition = Destination.position;
        StartCoroutine(HandleTransition());
    }


    // --------------------------------------------------------------------
    private IEnumerator HandleTransition() {
        SetActiveAndTargetComponents();
        DisableActiveController();

        yield return new WaitForSeconds(transitionDelay);

        Vector3 StartPosition = ActiveCamera.transform.position;
        Quaternion StartRotation = ActiveCamera.transform.rotation;
        float startFOV = ActiveCamera.fieldOfView;

        UpdatePlayerPosition();

        Vector3 EndPosition = TargetCamera.transform.position;
        Quaternion EndRotation = TargetCamera.transform.rotation;
        float endFOV = TargetCamera.fieldOfView;

        StartTransition();

        yield return CameraTransition(StartPosition, EndPosition,
                                      StartRotation, EndRotation,
                                      startFOV, endFOV);

        EndTransition();
    }


    // --------------------------------------------------------------------
    private void SetActiveAndTargetComponents() {
        ActiveCamera = (Is3DMode) ? Camera3D : Camera2D;
        TargetCamera = (Is3DMode) ? Camera2D : Camera3D;

        ActiveController = (Is3DMode) ? Controller3D : Controller2D;
        TargetController = (Is3DMode) ? Controller2D : Controller3D;

        ActiveVisuals = (Is3DMode) ? Visuals3D : Visuals2D;
        TargetVisuals = (Is3DMode) ? Visuals2D : Visuals3D;
    }


    // --------------------------------------------------------------------
    private void DisableActiveController() {
        ActiveController.SetCharacterPause(true);
        ActiveController.SwitchRigidbodyMode(!Is3DMode);
        ActiveController.enabled = false;
        ActiveVisuals.SetActive(false);
    }   
    
    
    // --------------------------------------------------------------------
    private void UpdatePlayerPosition() {
        ActiveController.transform.position = GameModeStartPosition;

        if (!Is3DMode && Camera3D.gameObject.TryGetComponent(out CameraFollow ThirdPersonCamera)) {
            ThirdPersonCamera.ResetThirdPersonCamera();
        }
    }


    // --------------------------------------------------------------------
    private void StartTransition() {
        PixelGrid.SetActive((Is3DMode) ? true : false);

        ActiveCamera.gameObject.SetActive(false);
        TransitionCamera.gameObject.SetActive(true);
    }


    // --------------------------------------------------------------------
    private IEnumerator CameraTransition(Vector3 StartPosition, Vector3 EndPosition,
                                        Quaternion StartRotation, Quaternion EndRotation,
                                        float startFOV, float endFOV) {
        float elapsedTime = 0.0f;
        while (elapsedTime < transitionDuration) {
            elapsedTime += Time.deltaTime;
            float curveValue = TransitionCurve.Evaluate(elapsedTime / transitionDuration);

            TransitionCamera.transform.position = Vector3.Lerp(StartPosition, EndPosition, curveValue);
            TransitionCamera.transform.rotation = Quaternion.Lerp(StartRotation, EndRotation, curveValue);
            TransitionCamera.fieldOfView = (Is3DMode) ? Mathf.Lerp(startFOV, 100.5f, curveValue)
                                                      : Mathf.Lerp(100.5f, endFOV, curveValue);

            yield return null;
        }
    }

    // --------------------------------------------------------------------
    private void EndTransition() {
        Is3DMode = !Is3DMode;

        TargetController.enabled = true;
        TargetVisuals.SetActive(true);
        
        ActiveController = (Is3DMode) ? Controller3D : Controller2D;
        ActiveController.SetCharacterPause(false);

        TransitionCamera.transform.position = TargetCamera.transform.position;
        TransitionCamera.transform.rotation = TargetCamera.transform.rotation;
        TransitionCamera.gameObject.SetActive(false);
        TargetCamera.gameObject.SetActive(true);
    }

}