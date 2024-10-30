using System.Collections;
using UnityEditor.MemoryProfiler;
using UnityEngine;


public enum CharacterStatus {
    Alive,
    Paused,
    Dead
}

public class Character : MonoBehaviour {

    [Header("Movement")]
    [SerializeField] protected float moveSpeed;
    [SerializeField] protected float rotateSpeed;
    protected Vector3 MoveDirection;

    [Header("Jumping")]
    [SerializeField] protected float jumpForce;
    [SerializeField] protected float airControl;
    [SerializeField] protected float fallMultiplier;
    [SerializeField] protected float lowJumpMultiplier;

    [Header("Ground Check")]
    [SerializeField] protected float groundCheckDistance;
    [SerializeField] protected Vector3 GroundCheckBoxExtents;
    [SerializeField] protected LayerMask GroundLayer;

    [Header("References")]
    [SerializeField] protected Animator CharacterAnimator;
    [SerializeField] protected GameObject Visuals;
    protected Rigidbody CharacterRigidbody;
    protected Rigidbody PlatformRigidbody;
    protected CharacterStatus Status;
    protected Vector3 LastCheckpoint;

    // Locomotion States
    public bool bMoving { get; protected set; }
    public bool bGrounded { get; protected set; }
    public bool bJumping { get; protected set; }
    public bool bOnPlatform { get; protected set; }



    // --------------------------------------------------------------------
    protected virtual void Awake() {
        CharacterRigidbody = GetComponent<Rigidbody>();
        Status = CharacterStatus.Paused;
        bMoving = false;
        LastCheckpoint = transform.position;
    }


    // --------------------------------------------------------------------
    protected virtual void OnEnable() {
        EventManager.OnGameStarted += HandleGameStart;
        EventManager.OnGameEnded += HandleGameOver;
        InputManager.OnPause += HandleGamePause;
        FallDetectionTrigger.OnFallDetected += HandleOutOfBounds;
        CheckpointTrigger.OnCheckpointTriggered += SetCheckpoint;
    }


    // --------------------------------------------------------------------
    protected virtual void OnDisable() {
        EventManager.OnGameStarted -= HandleGameStart;
        EventManager.OnGameEnded -= HandleGameOver;
        InputManager.OnPause -= HandleGamePause;
        FallDetectionTrigger.OnFallDetected -= HandleOutOfBounds;
        CheckpointTrigger.OnCheckpointTriggered -= SetCheckpoint;
    }


    // --------------------------------------------------------------------
    protected virtual void FixedUpdate() {
        if (Status == CharacterStatus.Alive) {
            AdjustGravityScale();
            CheckGrounded();
        }
    }


    // --------------------------------------------------------------------
    protected virtual void HandleGameStart() {
        Status = CharacterStatus.Alive;
    }

    // --------------------------------------------------------------------
    protected virtual void HandleGameOver() {
        SetCharacterActive(false);
    }


    // --------------------------------------------------------------------
    private void AdjustGravityScale() {
        bool bFalling = CharacterRigidbody.linearVelocity.y < 0.0f && !bGrounded;
        bool bEarlyJumpRelease = !bFalling && !bJumping;

        if (bFalling) {
            CharacterRigidbody.linearVelocity += Physics.gravity.y * fallMultiplier 
                                        * Time.fixedDeltaTime * Vector3.up;
        }
        else if(bEarlyJumpRelease) {
            CharacterRigidbody.linearVelocity += Physics.gravity.y * lowJumpMultiplier 
                                        * Time.fixedDeltaTime * Vector3.up;
        }
    }


    // --------------------------------------------------------------------
    protected void CheckGrounded() {
        Collider[] HitColliders = Physics.OverlapBox(transform.position, 
                        GroundCheckBoxExtents, Quaternion.identity, GroundLayer);

        bGrounded = HitColliders.Length > 0;
        CharacterAnimator.SetBool("bGrounded", bGrounded);
    }


    // --------------------------------------------------------------------
    public virtual void SwitchRigidbodyMode(bool To3DMode) {
        CharacterRigidbody.rotation = Quaternion.identity; 
        if (To3DMode) {
            CharacterRigidbody.useGravity = true;
            CharacterRigidbody.constraints = RigidbodyConstraints.FreezeRotation;
        }
        else {
            CharacterRigidbody.useGravity = false;
            CharacterRigidbody.constraints = RigidbodyConstraints.FreezeRotation | 
                                             RigidbodyConstraints.FreezePositionY;
        }
    }


    // --------------------------------------------------------------------
    public virtual void SetCharacterActive(bool bActive) {
        Status = (bActive) ? CharacterStatus.Alive : CharacterStatus.Paused;
        CharacterRigidbody.isKinematic = !bActive;
        AudioManager.Instance.Stop("SFX_Flying");

    }    
    
    
    // --------------------------------------------------------------------
    protected virtual void HandleGamePause(bool bPause) {
        Status = (bPause) ? CharacterStatus.Paused : CharacterStatus.Alive;
    }


    // --------------------------------------------------------------------
    protected virtual void HandleOutOfBounds() {
        StartCoroutine(OutOfBoundsTransition());
    }



    // --------------------------------------------------------------------
    protected IEnumerator OutOfBoundsTransition() {
        // Reset to last Checkpoint
        SetCharacterActive(false);
        Visuals.SetActive(false);

        yield return new WaitForSeconds(1.0f);

        transform.position = LastCheckpoint;
        transform.rotation = Quaternion.identity;

        yield return new WaitForSeconds(0.5f);

        SetCharacterActive(true);
        Visuals.SetActive(true);
    }


    // --------------------------------------------------------------------
    protected virtual void SetCheckpoint(Vector3 CheckpointPosition) {
        LastCheckpoint = CheckpointPosition;
    }


    // --------------------------------------------------------------------
    private void OnTriggerStay(Collider other) {
        if (other.transform.parent && other.transform.parent.TryGetComponent(out Platform PlatformAI)) {
            bOnPlatform = true;
            PlatformRigidbody = PlatformAI.PlatformRigidbody;
        }
    }


    // --------------------------------------------------------------------
    private void OnTriggerExit(Collider other) {
        if (other.transform.parent && other.transform.parent.TryGetComponent(out Platform PlatformAI)) {
            bOnPlatform = false;
            PlatformRigidbody = null;
        }
    }


    // --------------------------------------------------------------------
    // Debugging Helper Functions
    private void OnDrawGizmos() {
        Gizmos.color = Color.magenta;
        Gizmos.DrawCube(transform.position, GroundCheckBoxExtents);
        Gizmos.DrawSphere(transform.position + transform.up * 0.5f, 0.025f);
        if(PlatformRigidbody != null) {
            Vector3 PlatformToPlayer = transform.position - PlatformRigidbody.position;
            Vector3 AngularVelocity = PlatformRigidbody.angularVelocity;
            Vector3 RotationalMovement = Vector3.Cross(AngularVelocity, PlatformToPlayer);
            Gizmos.DrawLine(transform.position, transform.position + RotationalMovement * 2.0f);
        }
    }

}