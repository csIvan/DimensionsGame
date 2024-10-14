using UnityEngine;


public enum CharacterStatus {
    Alive,
    Paused,
    Dead
}

[RequireComponent (typeof(Rigidbody))]
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
    protected Vector3 AirMoveDirection;

    [Header("Ground Check")]
    [SerializeField] protected float groundCheckDistance;
    [SerializeField] protected Vector3 GroundCheckBoxExtents;
    [SerializeField] protected LayerMask GroundLayer;

    // References
    protected Rigidbody RigidBody;
    protected CharacterStatus Status;


    // Locomotion States
    public bool bMoving { get; protected set; }
    public bool bJumping { get; protected set; }


    // --------------------------------------------------------------------
    protected void Awake() {
        RigidBody = GetComponent<Rigidbody>();
        Status = CharacterStatus.Alive;

        bMoving = false;
    }

    protected void FixedUpdate() {
        AdjustGravityScale();
    }


    // --------------------------------------------------------------------
    private void AdjustGravityScale() {
        bool bFalling = RigidBody.linearVelocity.y < 0.0f;
        bool bEarlyJumpRelease = !bFalling && !bJumping;

        if (bFalling) {
            RigidBody.linearVelocity += Physics.gravity.y * fallMultiplier 
                                        * Time.fixedDeltaTime * Vector3.up;
        }
        else if(bEarlyJumpRelease) {
            RigidBody.linearVelocity += Physics.gravity.y * lowJumpMultiplier 
                                        * Time.fixedDeltaTime * Vector3.up;
        }
    }


    // --------------------------------------------------------------------
    protected bool IsGrounded() {
        Collider[] HitColliders = Physics.OverlapBox(transform.position, 
                        GroundCheckBoxExtents, Quaternion.identity, GroundLayer);

        return HitColliders.Length > 0;
    }


    // --------------------------------------------------------------------
    // Debugging Helper Functions
    private void OnDrawGizmos() {
        Gizmos.color = Color.magenta;
        Gizmos.DrawCube(transform.position, GroundCheckBoxExtents);
        Gizmos.DrawSphere(transform.position + transform.up * 0.5f, 0.025f);
    }

}