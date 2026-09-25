using UnityEngine;

public class LimitedRotatableObject : RotatableObject
{
    [Header("Rotation Limits")]

    public float MinimumAngle = -60f;

    public float MaximumAngle = 60f;


    [Header("Mechanical Speed")]

    [Tooltip("Maximum rotation speed in degrees per second.")]
    public float MaximumRotationSpeed = 20f;


    [Header("Live State")]

    [SerializeField]
    protected float CurrentAngle = 0f;


    public override void applyRotation(
        float requestedRotation)
    {
        // Maximum amount the hammer is allowed
        // to rotate during this frame.
        float maximumRotationThisFrame =
            MaximumRotationSpeed *
            Time.deltaTime;

        // Restrict very fast mouse movement.
        float allowedRotation =
            Mathf.Clamp(
                requestedRotation,
                -maximumRotationThisFrame,
                maximumRotationThisFrame
            );

        // Where the hammer would like to go.
        float targetAngle =
            CurrentAngle +
            allowedRotation;

        // Keep that target inside the mechanical limits.
        float clampedTargetAngle =
            Mathf.Clamp(
                targetAngle,
                MinimumAngle,
                MaximumAngle
            );

        // Find how much movement really happened.
        float actualRotation =
            clampedTargetAngle -
            CurrentAngle;

        if (Mathf.Abs(actualRotation) < 0.0001f)
            return;

        // Save the new absolute angle.
        CurrentAngle =
            clampedTargetAngle;

        // Set the visual orientation from the ORIGINAL orientation.
        setRotation(CurrentAngle);

        // Tell subclasses how much actual motion occurred.
        afterRotation(actualRotation);
    }


    protected virtual void afterRotation(
        float actualRotation)
    {
    }
}