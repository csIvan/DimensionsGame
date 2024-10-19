using System.Security.Cryptography.X509Certificates;
using System;
using UnityEngine;
using UnityEngine.UIElements;

[CreateAssetMenu(menuName = "ProjectR/Gameplay/Platforms/MoveBehavior", 
                 fileName = "MoveBehavior", order = 0)]
public class MoveBehaviorSO : PlatformBehaviorSO {

    public Vector3 StartOffset;
    public Vector3 EndOffset;
    public float speed = 1.0f;
    public float stopDuration = 1.0f;


    // --------------------------------------------------------------------
    public override void Execute(Platform PlatformAI) {
        if (PlatformAI.bStopped) {
            PlatformAI.Stop(stopDuration);
            return; 
        }
        else {
            PlatformAI.Move(PlatformAI.InitialPosition + StartOffset,
                            PlatformAI.InitialPosition + EndOffset, 
                            speed);
        }
    }
}