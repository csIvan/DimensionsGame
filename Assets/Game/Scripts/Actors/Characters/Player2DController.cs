using System.Collections;
using UnityEngine;

public class Player2DController : Character {

    [SerializeField] private LayerMask SolidObjectLayer;
    [SerializeField] private LayerMask PlatformEdgeLayer;


    // --------------------------------------------------------------------
    protected override void Awake() {
        base.Awake();

    }


    // --------------------------------------------------------------------
    protected void Update() {
        if (Status == CharacterStatus.Alive) {
            Handle2DMovement();
        }
    }


    // --------------------------------------------------------------------
    protected override void FixedUpdate() {

    }


    // --------------------------------------------------------------------
    private void Handle2DMovement() {
        if (!bMoving) {
            MoveDirection = InputManager.Instance.GetMoveDirectionNormalized();
            if (MoveDirection.x != 0) {
                MoveDirection.z = 0;
            }

            if (MoveDirection != Vector3.zero) {
                CharacterAnimator.SetFloat("moveX", MoveDirection.x);
                CharacterAnimator.SetFloat("moveZ", MoveDirection.z);

                Vector3 TargetPosition = transform.position;
                TargetPosition.x += Mathf.Ceil(MoveDirection.x);
                TargetPosition.z += Mathf.Ceil(MoveDirection.z);

                if (IsWalkable(TargetPosition)) {
                    StartCoroutine(Move(TargetPosition));

                }
            }
        }
        CharacterAnimator.SetBool("bMoving", bMoving);
    }


    // --------------------------------------------------------------------
    private IEnumerator Move(Vector3 TargetPosition) {
        bMoving = true;
        while((TargetPosition - transform.position).sqrMagnitude > Mathf.Epsilon) {
            transform.position = Vector3.MoveTowards(transform.position, TargetPosition, 
                                                    moveSpeed * Time.deltaTime);
            yield return null;
        }
        transform.position = TargetPosition;

        bMoving = false;
        if (IsEdge(TargetPosition)) {
            HandleOutOfBounds();
        }
    }


    // --------------------------------------------------------------------
    private bool IsWalkable(Vector3 TargetPosition) {
        Collider[] Colliders = Physics.OverlapSphere(TargetPosition, 0.3f, SolidObjectLayer);
        
        if(Colliders.Length >= 1) {
            return false;
        }
        return true;
    }    
    
    // --------------------------------------------------------------------
    private bool IsEdge(Vector3 TargetPosition) {
        Collider[] Colliders = Physics.OverlapSphere(TargetPosition, 0.3f, PlatformEdgeLayer);
        return (Colliders.Length >= 1);
    }
}