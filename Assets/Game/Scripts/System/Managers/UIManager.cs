using System.Collections;
using UnityEngine;
using TMPro;

public class UIManager : MonoBehaviour {

    [Header("Game UI References")]
    [SerializeField] private GameObject PauseUI;
    [SerializeField] private GameObject WinUI;
    [SerializeField] private Timer TimerUI;
    [SerializeField] private Timer CountdownUI;

    private bool timerStarted;


    // --------------------------------------------------------------------
    private IEnumerator Start() {
        yield return new WaitForSeconds(1.0f);

        PauseUI.SetActive(false);
        WinUI.SetActive(false);
        CountdownUI.gameObject.SetActive(true);
        CountdownUI.StartTimer();
        timerStarted = false;
    }


    // --------------------------------------------------------------------
    private void Update() {
        if(!timerStarted && CountdownUI.GetTimeInSeconds() <= 0.0f) {
            CountdownUI.gameObject.SetActive(false);
            TimerUI.StartTimer();
            timerStarted = true;
        }
    }

}