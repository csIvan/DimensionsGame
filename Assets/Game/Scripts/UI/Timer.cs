using System;
using System.Collections.Generic;
using System.Collections;
using TMPro;
using UnityEngine;

public enum TimerType {
    Countdown,
    Stopwatch
}

public enum TimerFormat {
    Whole,
    TenthDecimal,
    Clock 
}

public class Timer : MonoBehaviour {

    [Header("Timer Settings")]
    [SerializeField] private TimerType Type;
    [SerializeField] private TimerFormat Format;
    [SerializeField] private TextMeshProUGUI TimerText;
    [SerializeField] private float timerDuration;
    [SerializeField] private string countdownEndText;
    [SerializeField] private bool hasEndText;

    Dictionary<TimerFormat, string> TimeFormats = new Dictionary<TimerFormat, string>();
    private float currentTime;
    private bool bRunning;


    // --------------------------------------------------------------------
    private void Awake() {
        TimeFormats.Add(TimerFormat.Whole, @"%s");
        TimeFormats.Add(TimerFormat.TenthDecimal, @"ss\.ff");
        TimeFormats.Add(TimerFormat.Clock, @"mm\:ss");
        currentTime = (Type == TimerType.Countdown) ? timerDuration : 0;
        bRunning = false;
    }


    // --------------------------------------------------------------------
    private void Update() {
        if (!bRunning) return;
        if (Type == TimerType.Countdown && currentTime < 0.0f) return;

        UpdateTime();
    }


    // --------------------------------------------------------------------
    private void UpdateTime() {
        currentTime += (Type == TimerType.Countdown) ? -Time.deltaTime : Time.deltaTime;
        TimeSpan time = TimeSpan.FromSeconds(Mathf.Ceil(currentTime));

        if(hasEndText && currentTime < 0.0f && Type == TimerType.Countdown) {
            TimerText.text = countdownEndText;
        }
        else {
            TimerText.text = time.ToString(TimeFormats[Format]);
        }
    }


    // --------------------------------------------------------------------
    public void StartTimer() {
        bRunning = true;
    }


    // --------------------------------------------------------------------
    public void StopTimer() {
        bRunning = false;
    }


    // --------------------------------------------------------------------
    public void EndCountdown() {
        if(Type == TimerType.Countdown) {
            StartCoroutine(PersistFinalText(1.0f));
        }
    }


    // --------------------------------------------------------------------
    private IEnumerator PersistFinalText(float secondsToDisplay) {
        yield return new WaitForSeconds(secondsToDisplay);
        gameObject.SetActive(false);
    }

    // --------------------------------------------------------------------
    public float GetTimeInSeconds() {
        return currentTime;
    }

    // --------------------------------------------------------------------
    public string GetTimeText() {
        return TimerText.text;
    }


}