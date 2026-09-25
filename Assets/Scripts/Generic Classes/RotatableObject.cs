using UnityEngine;

public class RotatableObject : InteractableObject
{
    [Header("Rotation Input")]

    public Axes MouseMovementAxis = Axes.X_Axis;

    [Tooltip("Multiplier applied to mouse movement.")]
    public float RotationSensitivity = 0.2f;

    [Tooltip("1 = normal direction, -1 = reversed.")]
    public int RotationDirection = 1;

    protected Quaternion StartingRotation;


    public override void Start()
    {
        base.Start();

        StartingRotation = transform.rotation;
    }


    public override void rotate(Vector2 mouseMovement)
    {
        float movement;

        if (MouseMovementAxis == Axes.X_Axis)
            movement = mouseMovement.x;
        else
            movement = mouseMovement.y;

        movement *= RotationDirection;

        if (Mathf.Abs(movement) < 0.001f)
            return;

        float requestedRotation =
            movement * RotationSensitivity;

        applyRotation(requestedRotation);
    }


    public virtual void applyRotation(float requestedRotation)
    {
    }


    public virtual void setRotation(float totalAngle)
    {
        transform.rotation =
            Quaternion.AngleAxis(
                totalAngle,
                Vector3.up
            )
            * StartingRotation;
    }


    public override void doneRotating()
    {
    }
}