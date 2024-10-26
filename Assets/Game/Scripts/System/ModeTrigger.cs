using System;
using UnityEngine;

public class ModeTrigger : MonoBehaviour {

    public static event Action<Transform> OnModeSwitchTriggered;
    [SerializeField] private Transform Destination;


    // --------------------------------------------------------------------
    private void OnTriggerEnter(Collider other) {
        bool hasParent = other.transform.parent != null;
        if(hasParent && other.transform.parent.TryGetComponent(out Character PlayerCharacter)) {
            OnModeSwitchTriggered?.Invoke(Destination);
        }
    }
}