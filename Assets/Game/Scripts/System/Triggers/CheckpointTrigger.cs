using System;
using UnityEngine;

public class CheckpointTrigger : MonoBehaviour {

    public static event Action<Vector3> OnCheckpointTriggered;

    [SerializeField] private bool playSFX = true;
    [SerializeField] private bool Is3D = true;
    private bool bActivated;

    
    // --------------------------------------------------------------------
    private void Awake() {
        bActivated = false;
    }


    // --------------------------------------------------------------------
    private void OnTriggerEnter(Collider other) {
        if (other.transform.parent == null) return;

        if (other.transform.parent.TryGetComponent(out Character Player)) {
            if (bActivated) return;

            bActivated = true;
            OnCheckpointTriggered?.Invoke(transform.position);
            if (playSFX) {
                AudioManager.Instance.Play("SFX_Checkpoint");
                if (Is3D) {
                    VFXManager.Instance.Play("VFX_Checkpoint3D", transform.position, transform.rotation);
                }
                else {
                    VFXManager.Instance.Play("VFX_Checkpoint2D", transform.position, transform.rotation);
                }
            }
        }
    }

}
