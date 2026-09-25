
using UnityEngine;

public class RotateHarpsichordTuningHammer
    : LimitedRotatableObject
{
    [Header("Tuning Range")]

    [Tooltip(
        "Maximum flat tuning error. " +
        "-400 cents = two whole tones flat."
    )]
    public float MinimumTuningCents = -400f;


    [Tooltip(
        "Maximum sharp tuning error. " +
        "+200 cents = one whole tone sharp."
    )]
    public float MaximumTuningCents = 200f;


    [Header("Testing")]

    public bool ShowDebugMessages = true;


    private CarryHarpsichordTuningHammer carryHammer;



    public override void Start()
    {
        base.Start();


        carryHammer =
            GetComponent<
                CarryHarpsichordTuningHammer>();
    }


    /*
       The harpsichord currently uses
       DefaultTuningSensitivity = 0.01.

       This is the same sensitivity used
       by HarpsichordTuningPin unless that
       pin has a custom sensitivity.
       */
    private void calculateRotationLimits(
    float tuningSensitivity)
    {
        MinimumAngle =
            (
                Mathf.Pow(
                    2f,
                    (2f * MinimumTuningCents)
                    / 1200f
                )
                - 1f
            )
            / tuningSensitivity;


        MaximumAngle =
            (
                Mathf.Pow(
                    2f,
                    (2f * MaximumTuningCents)
                    / 1200f
                )
                - 1f
            )
            / tuningSensitivity;
    }
   

    //prepares the hammer for the correct angle of a detuned pin
    public void prepareForPin(
     HarpsichordTuningPin pin)
    {
        if (pin == null)
            return;


        float tuningSensitivity =
            pin.getTuningSensitivity();


        calculateRotationLimits(
            tuningSensitivity
        );


        CurrentAngle =
            pin.getCurrentTuningAngle();


        CurrentAngle =
            Mathf.Clamp(
                CurrentAngle,
                MinimumAngle,
                MaximumAngle
            );


        
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


        updateTuning();

        checkLimits();
    }


    private void updateTuning()
    {
        if (carryHammer == null)
            return;


        if (carryHammer.AttachedPin == null)
            return;


        bool limitReached =
            CurrentAngle >= MaximumAngle ||
            CurrentAngle <= MinimumAngle;


      


        /*
        Physical tuning model.

        The angle is sent to the tuning pin,
        which calculates tension, frequency
        and cents.
        */

        carryHammer.AttachedPin
            .updateTuningFromAngle(
                CurrentAngle,
                limitReached
            );
    }


    private void checkLimits()
    {
        if (CurrentAngle >= MaximumAngle)
        {
            if (ShowDebugMessages)
            {
                Debug.Log(
                    "Maximum tuning limit reached - " +
                    "string is too sharp."
                );
            }
        }
        else if (CurrentAngle <= MinimumAngle)
        {
            if (ShowDebugMessages)
            {
                Debug.Log(
                    "Minimum tuning limit reached - " +
                    "string is too flat."
                );
            }
        }
    }
}