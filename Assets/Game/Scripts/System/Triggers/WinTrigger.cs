using System;
using UnityEngine;

public class WinTrigger : MonoBehaviour {

    public static event Action OnWinTriggered;

    // --------------------------------------------------------------------
    private void OnTriggerEnter(Collider other) {
        if (other.transform.parent == null) return;

        if (other.transform.parent.TryGetComponent(out Character Winner)) {
            AudioManager.Instance.Play("SFX_MagicPoof");
            OnWinTriggered?.Invoke();
        }
    }

}