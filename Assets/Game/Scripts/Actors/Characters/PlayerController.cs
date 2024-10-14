using System.Threading;
using UnityEngine;
using UnityEngine.EventSystems;

public class PlayerController : Character {

    // References
    private Transform CameraTransform;

    // --------------------------------------------------------------------
    protected new void Awake() {
        base.Awake();
        
    }

    private void Start() {
        CameraTransform = Camera.main.transform;
    }


    // --------------------------------------------------------------------
    private void OnEnable() {
        InputManager.Instance.OnJumpStarted += Jump;
        InputManager.Instance.OnJumpEnded += EndJump;
        InputManager.Instance.OnCrouch += Crouch;
    }


    // --------------------------------------------------------------------
    private void OnDisable() {
        InputManager.Instance.OnJumpStarted -= Jump;
        InputManager.Instance.OnJumpEnded -= EndJump;
        InputManager.Instance.OnCrouch -= Crouch;
    }


    // --------------------------------------------------------------------
    protected new void FixedUpdate() {
        base.FixedUpdate();

        if (Status == CharacterStatus.Alive) {
            MoveAndRotate();
        }
    }


    // --------------------------------------------------------------------
    private void MoveAndRotate() {
        MoveDirection = InputManager.Instance.GetMoveDirectionNormalized();
        MoveDirection = UpdateDirectionRelativeToCamera().normalized;
        bMoving = MoveDirection != Vector3.zero;

        Vector3 NewVelocity;
        Vector3 ExistingVelocity = RigidBody.linearVelocity;
        Vector3 MoveVelocity = moveSpeed * Time.fixedDeltaTime * MoveDirection;

        if (IsGrounded()) {
            NewVelocity = AdjustVelocityToSlope(MoveVelocity);
        }
        else {
            NewVelocity = AdjustAirVelocity(ExistingVelocity, MoveVelocity);         
        }

        RigidBody.linearVelocity = NewVelocity;


        if (bMoving) { 
            Quaternion LookRotation = Quaternion.LookRotation(MoveDirection);
            transform.rotation = Quaternion.Slerp(transform.rotation, LookRotation, 
                                                rotateSpeed * Time.fixedDeltaTime);
        }
    }


    // --------------------------------------------------------------------
    private Vector3 UpdateDirectionRelativeToCamera() {
        Vector3 CameraForward = CameraTransform.forward;
        Vector3 CameraRight = CameraTransform.right;
        CameraForward.y = 0.0f;
        CameraRight.y = 0.0f;

        return (CameraForward * MoveDirection.z + CameraRight * MoveDirection.x);
    }


    // --------------------------------------------------------------------
    private Vector3 AdjustVelocityToSlope(Vector3 MoveVelocity) {
        Vector3 GroundCheckOffset = transform.position + transform.up * 0.5f;
        Vector3 SlopeNormal = Vector3.up;

        RaycastHit SlopeHit;
        if (Physics.Raycast(GroundCheckOffset, Vector3.down, out SlopeHit, 
                            groundCheckDistance, GroundLayer)) {
            SlopeNormal = SlopeHit.normal;
        }

        return Vector3.ProjectOnPlane(MoveVelocity, SlopeNormal);
    }


    // --------------------------------------------------------------------
    private Vector3 AdjustAirVelocity(Vector3 ExistingVelocity, Vector3 MoveVelocity) {
        float maxSpeed = moveSpeed * Time.fixedDeltaTime;

        Vector3 HorizontalVelocity = new Vector3(ExistingVelocity.x, 0.0f, ExistingVelocity.z);
        HorizontalVelocity += airControl * MoveVelocity;

        if (HorizontalVelocity.magnitude > maxSpeed) {
            HorizontalVelocity = HorizontalVelocity.normalized * maxSpeed;
        }

        // Smooth out air velocity 
        HorizontalVelocity = Vector3.Lerp(new Vector3(ExistingVelocity.x, 0.0f, ExistingVelocity.z), 
                                            HorizontalVelocity, airControl * Time.fixedDeltaTime);

        return new Vector3(HorizontalVelocity.x, ExistingVelocity.y, HorizontalVelocity.z);
    }

    
    // --------------------------------------------------------------------
    private void Jump() {
        if (IsGrounded() && !bJumping) {
            bJumping = true;
            AirMoveDirection = MoveDirection;
            RigidBody.AddForce(jumpForce * transform.up, ForceMode.Impulse);
        }
    }


    // --------------------------------------------------------------------
    private void EndJump() {
        bJumping = false;
    }


    // --------------------------------------------------------------------
    private void Crouch() {
        Debug.Log("Crouching");
    }

}