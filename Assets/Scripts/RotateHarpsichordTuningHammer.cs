/*using UnityEngine;

public class RotateHarpsichordTuningHammer
    : LimitedRotatableObject
{
    [Header("Testing")]

    public bool ShowDebugMessages = true;


    protected override void afterRotation(
        float actualRotation)
    {
        if (ShowDebugMessages)
        {
            Debug.Log(
                "Hammer rotation: " +
                actualRotation +
                " | Current angle: " +
                CurrentAngle
            );
        }

        checkLimits();
    }


    private void checkLimits()
    {
        if (CurrentAngle >= MaximumAngle)
        {
            Debug.Log(
                "Maximum tuning limit reached - string would break."
            );
        }

        else if (CurrentAngle <= MinimumAngle)
        {
            Debug.Log(
                "Minimum tuning limit reached - string is too loose."
            );
        }
    }
}
*/
using UnityEngine;

public class RotateHarpsichordTuningHammer
    : LimitedRotatableObject
{
    [Header("Tuning Test")]

    [Tooltip("Pitch change in cents for every degree of hammer rotation.")]
    public float CentsPerDegree = 1f;

    [Header("Testing")]

    public bool ShowDebugMessages = true;


    private CarryHarpsichordTuningHammer carryHammer;


    public override void Start()
    {
        base.Start();

        carryHammer =
            GetComponent<CarryHarpsichordTuningHammer>();
    }


    protected override void afterRotation(
        float actualRotation)
    {
        if (ShowDebugMessages)
        {
            Debug.Log(
                "Hammer rotation: " +
                actualRotation +
                " | Current angle: " +
                CurrentAngle
            );
        }


        updateTuningDisplay();

        checkLimits();
    }


    private void updateTuningDisplay()
    {
        if (carryHammer == null)
            return;

        if (carryHammer.AttachedPin == null)
            return;


        float cents =
            CurrentAngle *
            CentsPerDegree;


        bool limitReached =
            CurrentAngle >= MaximumAngle ||
            CurrentAngle <= MinimumAngle;


        carryHammer.AttachedPin.updateTuning(
            cents,
            limitReached
        );
    }


    private void checkLimits()
    {
        if (CurrentAngle >= MaximumAngle)
        {
            Debug.Log(
                "Maximum tuning limit reached - string would break."
            );
        }

        else if (CurrentAngle <= MinimumAngle)
        {
            Debug.Log(
                "Minimum tuning limit reached - string is too loose."
            );
        }
    }
}