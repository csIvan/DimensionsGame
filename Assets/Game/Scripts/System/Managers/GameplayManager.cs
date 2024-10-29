using UnityEngine;

public class GameplayManager : MonoBehaviour {
    public static GameplayManager Instance { get; private set; }


    // --------------------------------------------------------------------
    private void Awake() {
        if (Instance == null) {
            Instance = this;
        }
        else {
            Destroy(gameObject);
        }
    }


    // --------------------------------------------------------------------
    private void Start() {
        //Cursor.lockState = CursorLockMode.Locked;
        //Cursor.visible = false;
        //AudioManager.Instance.Play("Music_ForestAmbience");
    }


    // --------------------------------------------------------------------
    private void OnEnable() {

    }


    // --------------------------------------------------------------------
    private void OnDisable() {
        
    }


    // --------------------------------------------------------------------
    public void PauseGame() {
        Time.timeScale = 0f;
        //AudioManager.Instance.Pause();
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }


    // --------------------------------------------------------------------
    public void ResumeGame() {
        Time.timeScale = 1f;
        //AudioManager.Instance.UnPause();
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }
}