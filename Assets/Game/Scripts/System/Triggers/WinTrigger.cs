using System;
using UnityEngine;
using UnityEngine.TextCore.Text;

public class WinTrigger : MonoBehaviour {

    public static event Action OnWinTriggered;

    // --------------------------------------------------------------------
    private void OnTriggerEnter(Collider other) {
        if (other.transform.parent == null) return;

        if (other.transform.parent.TryGetComponent(out Character Winner)) {
            AudioManager.Instance.Play("SFX_MagicPoof");
            VFXManager.Instance.Play("VFX_Victory", Winner.transform.position, Quaternion.identity);
            VFXManager.Instance.Play("VFX_StarTrigger3D", Winner.transform.position, Quaternion.identity);
            OnWinTriggered?.Invoke();
            gameObject.SetActive(false);
        }
    }

}