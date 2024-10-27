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
        yield return new WaitForSeconds(1.0f);

        PauseUI.SetActive(false);
        WinUI.SetActive(false);
        CountdownUI.gameObject.SetActive(true);
        CountdownUI.StartTimer();
        bTimerStarted = false;
        bGameEnded = false;
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
        GameOverUI.SetActive(true);
    }


    // --------------------------------------------------------------------
    private void DisplayPauseUI(bool bPause) {
        if (bGameEnded) return;

        PauseUI.SetActive(bPause);
    }

}