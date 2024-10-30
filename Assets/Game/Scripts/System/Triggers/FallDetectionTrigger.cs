using UnityEngine;
using System;

public class FallDetectionTrigger : MonoBehaviour {

    public static event Action OnFallDetected;

    // --------------------------------------------------------------------
    private void OnTriggerEnter(Collider other) {
        if (other.transform.parent == null) return;

        if (other.transform.parent.TryGetComponent(out Character Character3D)) {
            AudioManager.Instance.Play("SFX_Death");
            VFXManager.Instance.Play("VFX_Death3D", Character3D.transform.position, Quaternion.identity);
            OnFallDetected?.Invoke();
        }
    }
}