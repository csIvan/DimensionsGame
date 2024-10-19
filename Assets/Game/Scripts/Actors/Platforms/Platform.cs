using System;
using System.Collections.Generic;
using UnityEngine;

public class Platform : MonoBehaviour {

    [Header("List of Behaviors")]
    [SerializeField] private List<PlatformBehaviorSO> Behaviors;

    // Movement Properties
    public Vector3 InitialPosition { get; private set; }
    public bool bStopped { get; private set; }
    private bool movingForward = true;
    private float moveTimer = 0.0f;
    private float stopTimer = 0.0f;

    // References
    public Rigidbody PlatformRigidbody { get; private set; }


    // --------------------------------------------------------------------
    private void Start() {
        PlatformRigidbody = GetComponent<Rigidbody>();
        InitialPosition = transform.position;
    }


    // --------------------------------------------------------------------
    private void FixedUpdate() {
        foreach(PlatformBehaviorSO Behavior in Behaviors) {
            Behavior.Execute(this);
        }
    }
    
    
    // --------------------------------------------------------------------
    public void Move(Vector3 Start, Vector3 End, float speed) {
        moveTimer += Time.fixedDeltaTime * speed;
        float lerpValue = (movingForward) ? Mathf.Clamp01(moveTimer) : 1 - Mathf.Clamp01(moveTimer);
        float smoothedLerp = Mathf.SmoothStep(0, 1, lerpValue);

        Vector3 NewPosition = Vector3.Lerp(Start, End, smoothedLerp);
        PlatformRigidbody.MovePosition(NewPosition);

        bool bReachedLocation = (lerpValue >= 1.0f || lerpValue <= 0.0f);
        if (bReachedLocation) {
            bStopped = true;
        }
    }

    // --------------------------------------------------------------------
    public void Stop(float stopDuration) {
        stopTimer += Time.fixedDeltaTime;
        if (stopTimer >= stopDuration) {
            bStopped = false;
            movingForward = !movingForward;
            stopTimer = 0.0f;
            moveTimer = 0.0f;
        }
    }

    
    // --------------------------------------------------------------------
    public void Rotate(Quaternion Rotation) {
        PlatformRigidbody.MoveRotation(Rotation);
    }

}