using System;
using UnityEngine;

public class CheckpointTrigger : MonoBehaviour {

    public static event Action<Vector3> OnCheckpointTriggered;

    [SerializeField] private bool playSFX = true;
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
            }
        }
    }

}
