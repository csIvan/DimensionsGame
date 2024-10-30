using System;
using UnityEngine;
using UnityEngine.TextCore.Text;

public class ModeTrigger : MonoBehaviour {

    public static event Action<Transform> OnModeSwitchTriggered;
    [SerializeField] private Transform Destination;
    [SerializeField] private bool Is3D;


    // --------------------------------------------------------------------
    private void OnTriggerEnter(Collider other) {
        if (other.transform.parent == null) return;

        if (other.transform.parent.TryGetComponent(out Character PlayerCharacter)) {
            AudioManager.Instance.Play("SFX_MagicPoof");
            if (Is3D) {
                VFXManager.Instance.Play("VFX_StarTrigger3D", PlayerCharacter.transform.position, Quaternion.identity);
            }
            else {
                VFXManager.Instance.Play("VFX_StarTrigger2Dv2", PlayerCharacter.transform.position, Quaternion.identity);
            }
            OnModeSwitchTriggered?.Invoke(Destination);
            gameObject.SetActive(false);
        }
    }
}