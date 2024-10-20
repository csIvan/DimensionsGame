using UnityEngine;

[CreateAssetMenu(menuName = "ProjectR/Gameplay/Platforms/RotateBehavior", 
                 fileName = "RotateBehavior", order = 1)]
public class RotateBehaviorSO : PlatformBehaviorSO {

    public enum Axis { X, Y, Z };
    public Axis RotationAxis = Axis.Y;
    public float rotationSpeed = 10.0f;

    private Vector3 AxisToRotate;

    // --------------------------------------------------------------------
    public override void Execute(Platform PlatformAI) {
        switch (RotationAxis) {
            case Axis.X:
                AxisToRotate = new Vector3(1, 0, 0);
                break;
            case Axis.Y:
                AxisToRotate = new Vector3(0, 1, 0);
                break;
            case Axis.Z:
                AxisToRotate = new Vector3(0, 0, 1);
                break;
        }

        float angle = rotationSpeed * Time.fixedDeltaTime;
        Quaternion NewRotation = Quaternion.AngleAxis(angle, AxisToRotate);
        PlatformAI.Rotate(PlatformAI.PlatformRigidbody.rotation * NewRotation);
    }
}