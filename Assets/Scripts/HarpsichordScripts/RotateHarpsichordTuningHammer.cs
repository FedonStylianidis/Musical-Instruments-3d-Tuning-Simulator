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
/*using UnityEngine;

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


       float cents =     //for the cent-based tuning system
            CurrentAngle *
            CentsPerDegree; 


        bool limitReached =
            CurrentAngle >= MaximumAngle ||
            CurrentAngle <= MinimumAngle;


        carryHammer.AttachedPin.updateTuning(  //for the cent-base tuning system
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
} */



/*using UnityEngine;

public class RotateHarpsichordTuningHammer
    : LimitedRotatableObject
{
    [Header("OLD Tuning Test")]

    [Tooltip(
        "Old cents-per-degree model. " +
        "Kept for reference but not currently used."
    )]
    public float CentsPerDegree = 1f;


    [Header("Testing")]

    public bool ShowDebugMessages = true;


    private CarryHarpsichordTuningHammer
        carryHammer;


    public override void Start()
    {
        base.Start();

        carryHammer =
            GetComponent<
                CarryHarpsichordTuningHammer>();
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
        OLD CENTS MODEL

        float cents =
            CurrentAngle *
            CentsPerDegree;

        carryHammer.AttachedPin.updateTuning(
            cents,
            limitReached
        );
        */


/*
NEW PHYSICAL MODEL

The hammer now sends its actual
rotation angle to the tuning pin.

*/

/*  carryHammer.AttachedPin                //thisone
      .updateTuningFromAngle(
          CurrentAngle,
          limitReached
      );
}


private void checkLimits()
{
  if (CurrentAngle >= MaximumAngle)
  {
      Debug.Log(
          "Maximum tuning limit reached - " +
          "string would break."
      );
  }

  else if (
      CurrentAngle <= MinimumAngle)
  {
      Debug.Log(
          "Minimum tuning limit reached - " +
          "string is too loose."
      );
  }
}
}  */

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


    [Header("OLD Tuning Test")]

    [Tooltip(
        "Old cents-per-degree model. " +
        "Kept for reference but not currently used."
    )]
    public float CentsPerDegree = 1f;


    [Header("Testing")]

    public bool ShowDebugMessages = true;


    private CarryHarpsichordTuningHammer carryHammer;


    public override void Start()
    {
        base.Start();


        carryHammer =
            GetComponent<
                CarryHarpsichordTuningHammer>();


        /*
        Calculate the mechanical rotation
        limits from the same physical model
        that is used to calculate frequency.

        With sensitivity = 0.001:

        -400 cents ≈ -370.04 degrees
        +200 cents ≈ +259.92 degrees
        */

        calculateRotationLimits();
    }


    private void calculateRotationLimits()
    {
        /*
        The harpsichord currently uses
        DefaultTuningSensitivity = 0.01.

        This is the same sensitivity used
        by HarpsichordTuningPin unless that
        pin has a custom sensitivity.
        */

        float tuningSensitivity = 0.01f;


        /*
        From:

        f/f0 = 2^(cents/1200)

        and:

        f/f0 = sqrt(1 + sensitivity * angle)

        therefore:

        angle =
        (2^(2*cents/1200) - 1)
        / sensitivity
        */


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


        if (ShowDebugMessages)
        {
            Debug.Log(
                "Tuning hammer limits: " +
                MinimumAngle.ToString("F2") +
                "° to " +
                MaximumAngle.ToString("F2") +
                "°"
            );
        }
    }


    //prepares the hammer for the correct angle of a detuned pin
    public void prepareForPin(
    HarpsichordTuningPin pin)
    {
        if (pin == null)
            return;


        float tuningSensitivity =
            pin.getTuningSensitivity();


        MinimumAngle =
            (
                Mathf.Pow(
                    2f,
                    (2f * MinimumTuningCents) / 1200f
                )
                - 1f
            )
            / tuningSensitivity;


        MaximumAngle =
            (
                Mathf.Pow(
                    2f,
                    (2f * MaximumTuningCents) / 1200f
                )
                - 1f
            )
            / tuningSensitivity;


        CurrentAngle =
            pin.getCurrentTuningAngle();


        CurrentAngle =
            Mathf.Clamp(
                CurrentAngle,
                MinimumAngle,
                MaximumAngle
            );


        if (ShowDebugMessages)
        {
            Debug.Log(
                "Hammer attached to "
                + pin.Note
                + " | Starting cents: "
                + pin.CurrentCents.ToString("F1")
                + " | Starting angle: "
                + CurrentAngle.ToString("F2")
                + "°"
            );
        }
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
        OLD CENTS MODEL

        float cents =
            CurrentAngle *
            CentsPerDegree;

        carryHammer.AttachedPin
            .updateTuning(
                cents,
                limitReached
            );
        */


        /*
        Current physical tuning model.

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