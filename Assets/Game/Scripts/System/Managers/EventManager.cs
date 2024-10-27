using System;
using UnityEditor;
using UnityEngine;

public static class EventManager {


    public static event Action OnGameStarted;
    public static event Action OnGameEnded;


    // --------------------------------------------------------------------
    public static void StartGame() {
        OnGameStarted?.Invoke();
    }


    // --------------------------------------------------------------------
    public static void GameOver() {
        OnGameEnded?.Invoke();
    }


}
