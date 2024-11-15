using System.Threading;
using UnityEngine;
using UnityEngine.EventSystems;

public class Player3DController : Character {

    // References
    private Transform CameraTransform;

    // --------------------------------------------------------------------
    protected override void Awake() {
        base.Awake();
        
    }

    private void Start() {
        CameraTransform = Camera.main.transform;
    }


    // --------------------------------------------------------------------
    protected override void OnEnable() {
        base.OnEnable();
        InputManager.OnJumpStarted += Jump;
        InputManager.OnJumpEnded += EndJump;
    }


    // --------------------------------------------------------------------
    protected override void OnDisable() {
        base.OnDisable();
        InputManager.OnJumpStarted -= Jump;
        InputManager.OnJumpEnded -= EndJump;
    }


    // --------------------------------------------------------------------
    protected override void FixedUpdate() {
        base.FixedUpdate();

        if (Status == CharacterStatus.Alive && !bGamePaused) {
            MoveAndRotate();
        }
    }


    // --------------------------------------------------------------------
    private void MoveAndRotate() {
        MoveDirection = InputManager.Instance.GetMoveDirectionNormalized();
        MoveDirection = UpdateDirectionRelativeToCamera().normalized;
        bMoving = (MoveDirection != Vector3.zero);
        CharacterAnimator.SetBool("bMoving", bMoving);

        Vector3 NewVelocity;
        Vector3 ExistingVelocity = CharacterRigidbody.linearVelocity;
        Vector3 MoveVelocity = moveSpeed * Time.fixedDeltaTime * MoveDirection;

        if (bGrounded) {
            NewVelocity = AdjustVelocityToSlope(MoveVelocity);

            if (bOnPlatform && !bJumping) {
                NewVelocity += ApplyMovingPlatformVelocity();
            }
        }
        else {
            NewVelocity = AdjustAirVelocity(ExistingVelocity, MoveVelocity);     
        }

        CharacterRigidbody.linearVelocity = NewVelocity;


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
    private Vector3 ApplyMovingPlatformVelocity() {
        Vector3 LinearVelocity = PlatformRigidbody.linearVelocity;
        Vector3 AngularVelocity = PlatformRigidbody.angularVelocity;

        Vector3 PlatformToPlayer = transform.position - PlatformRigidbody.position;
        Vector3 RotationalVelocity = Vector3.Cross(AngularVelocity, PlatformToPlayer);

        return LinearVelocity + RotationalVelocity;
    }


    // --------------------------------------------------------------------
    private void Jump() {
        if (Status != CharacterStatus.Alive) return;

        if (bGrounded && !bJumping) {
            CharacterAnimator.SetTrigger("Jump");
            AudioManager.Instance.Play("SFX_Jump");
            bJumping = true;
            CharacterRigidbody.AddForce(jumpForce * transform.up, ForceMode.Impulse);
        }
    }


    // --------------------------------------------------------------------
    private void EndJump() {
        bJumping = false;
    }
}