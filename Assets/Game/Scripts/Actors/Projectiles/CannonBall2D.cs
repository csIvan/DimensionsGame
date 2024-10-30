using TMPro;
using UnityEngine;

public class CannonBall2D : MonoBehaviour {

    [SerializeField] private float moveSpeed = 2.0f;  
    [SerializeField] private float timeToDestroy = 5.0f;
    private Vector3 moveDirection;
    private bool isInitialized;
    private Vector3 targetPosition;  


    // --------------------------------------------------------------------
    private void Awake() {
        isInitialized = false;
        moveDirection = Vector3.zero;
    }


    // --------------------------------------------------------------------
    public void Initialize(Vector3 direction) {
        isInitialized = true;
        moveDirection = direction.normalized;
        targetPosition = transform.position + moveDirection;  
        Destroy(gameObject, timeToDestroy);  
    }


    // --------------------------------------------------------------------
    private void Update() {
        if (!isInitialized) {
            return;
        }

        transform.position = Vector3.MoveTowards(transform.position, targetPosition, moveSpeed * Time.deltaTime);

        if (Vector3.Distance(transform.position, targetPosition) < 0.01f) {
            targetPosition += moveDirection;
        }
    }
}