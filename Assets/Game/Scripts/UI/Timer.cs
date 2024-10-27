using System;
using System.Collections.Generic;
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
    [SerializeField] private TimerType TimerStyle;
    [SerializeField] private TimerFormat Format;
    [SerializeField] private TextMeshProUGUI TimerText;
    [SerializeField] private float timerDuration;
    [SerializeField] private string countdownEndText;
    [SerializeField] private bool hasEndText;

    Dictionary<TimerFormat, string> TimeFormats = new Dictionary<TimerFormat, string>();
    private float currentTime;
    private bool bRunning;


    // --------------------------------------------------------------------
    private void Start() {
        TimeFormats.Add(TimerFormat.Whole, @"%s");
        TimeFormats.Add(TimerFormat.TenthDecimal, @"ss\.ff");
        TimeFormats.Add(TimerFormat.Clock, @"mm\:ss");
        currentTime = (TimerStyle == TimerType.Countdown) ? timerDuration : 0;
        bRunning = false;
    }


    // --------------------------------------------------------------------
    private void Update() {
        if (!bRunning) return;
        if (TimerStyle == TimerType.Countdown && currentTime < 0.0f) return;

        UpdateTime();
    }


    // --------------------------------------------------------------------
    private void UpdateTime() {
        currentTime += (TimerStyle == TimerType.Countdown) ? -Time.deltaTime : Time.deltaTime;
        TimeSpan time = TimeSpan.FromSeconds(currentTime);

        if( hasEndText && currentTime < 1.0f && TimerStyle == TimerType.Countdown) {
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
    public float GetTimeInSeconds() {
        return currentTime;
    }

    // --------------------------------------------------------------------
    public string GetTimeText() {
        return TimerText.text;
    }


}