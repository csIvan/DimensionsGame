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

    private bool bTimerStarted;
    private bool bGameEnded;


    // --------------------------------------------------------------------
    private IEnumerator Start() {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        PauseUI.SetActive(false);
        WinUI.SetActive(false);

        bTimerStarted = false;
        bGameEnded = false;

        yield return new WaitForSeconds(1.0f);

        CountdownUI.gameObject.SetActive(true);
        CountdownUI.StartTimer();
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
        if(!bTimerStarted && CountdownUI.GetTimeInSeconds() <= 0.0f) {
            CountdownUI.gameObject.SetActive(false);
            EventManager.StartGame();
            TimerUI.StartTimer();
            bTimerStarted = true;
        }
    }


    // --------------------------------------------------------------------
    private void DisplayWinUI() {
        TimerUI.StopTimer();
        FinalTime.text = TimerUI.GetTimeText();
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