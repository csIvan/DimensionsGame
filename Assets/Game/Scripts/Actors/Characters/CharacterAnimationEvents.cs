using UnityEngine;

public class CharacterAnimationEvents : MonoBehaviour {


    // --------------------------------------------------------------------
    public void PlayFlyingSFX() {
        if (AudioManager.Instance.IsPlaying("SFX_Flying")) { return; }

        AudioManager.Instance.Play("SFX_Flying");
    }


    // --------------------------------------------------------------------
    public void StopFlyingSFX() {
        if (!AudioManager.Instance.IsPlaying("SFX_Flying")) { return; }

        AudioManager.Instance.Stop("SFX_Flying");
    }
}