using System.Collections;
using UnityEngine;
using TMPro;

public class UIManager : MonoBehaviour {

    [Header("Game UI References")]
    [SerializeField] private GameObject PauseUI;
    [SerializeField] private GameObject WinUI;
    [SerializeField] private GameObject GameOverUI;
    [SerializeField] private TextMeshProUGUI FinalTime;
    [SerializeField] private Timer TimerUI;
    [SerializeField] private Timer CountdownUI;

    private bool bCountdownActive;
    private bool bGameEnded;


    // --------------------------------------------------------------------
    private IEnumerator Start() {
        AudioManager.Instance.Play("SFX_GameIntroAmbiance");
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        PauseUI.SetActive(false);
        WinUI.SetActive(false);

        bCountdownActive = false;
        bGameEnded = false;

        yield return new WaitForSeconds(1.0f);

        AudioManager.Instance.Play("SFX_Countdown");
        CountdownUI.gameObject.SetActive(true);
        CountdownUI.StartTimer();
        bCountdownActive = true;
    }


    // --------------------------------------------------------------------
    private void OnEnable() {
        WinTrigger.OnWinTriggered += DisplayWinUI;
        InputManager.OnInteract += DisplayGameOverUI;
        InputManager.OnPause += DisplayPauseUI;
    }


    // --------------------------------------------------------------------
    private void OnDisable() {
        WinTrigger.OnWinTriggered -= DisplayWinUI;
        InputManager.OnInteract -= DisplayGameOverUI;
        InputManager.OnPause -= DisplayPauseUI;
    }


    // --------------------------------------------------------------------
    private void Update() {
        if(bCountdownActive && CountdownUI.GetTimeInSeconds() <= 0.0f) {
            CountdownUI.EndCountdown();
            EventManager.StartGame();
            AudioManager.Instance.PlayGameTheme();
            TimerUI.StartTimer();
            bCountdownActive = false;
        }
    }


    // --------------------------------------------------------------------
    private void DisplayWinUI() {
        TimerUI.StopTimer();
        FinalTime.text = TimerUI.GetTimeText();
        TimerUI.gameObject.SetActive(false);
        WinUI.SetActive(true);
        bGameEnded = true;
    }


    // --------------------------------------------------------------------
    private void DisplayGameOverUI() {
        if (!bGameEnded) return;

        EventManager.GameOver();

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        GameOverUI.SetActive(true);
    }


    // --------------------------------------------------------------------
    private void DisplayPauseUI(bool bPause) {
        if (bGameEnded) return;

        Cursor.lockState = (bPause) ? CursorLockMode.None : CursorLockMode.Locked;
        Cursor.visible = bPause;
        PauseUI.SetActive(bPause);
    }

}