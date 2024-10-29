using UnityEngine;
using System;

public class FallDetectionTrigger : MonoBehaviour {

    public static event Action OnFallDetected;

    // --------------------------------------------------------------------
    private void OnTriggerEnter(Collider other) {
        if (other.transform.parent == null) return;

        if (other.transform.parent.TryGetComponent(out Character Winner)) {
            OnFallDetected?.Invoke();
        }
    }
}