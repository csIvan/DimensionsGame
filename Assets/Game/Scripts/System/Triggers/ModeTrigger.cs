using System;
using UnityEngine;

public class ModeTrigger : MonoBehaviour {

    public static event Action<Transform> OnModeSwitchTriggered;
    [SerializeField] private Transform Destination;


    // --------------------------------------------------------------------
    private void OnTriggerEnter(Collider other) {
        if (other.transform.parent == null) return;

        if (other.transform.parent.TryGetComponent(out Character PlayerCharacter)) {
            AudioManager.Instance.Play("SFX_MagicPoof");
            OnModeSwitchTriggered?.Invoke(Destination);
        }
    }
}