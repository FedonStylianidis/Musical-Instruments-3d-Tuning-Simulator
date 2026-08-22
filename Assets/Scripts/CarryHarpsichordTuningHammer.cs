
/*using UnityEngine;

public class CarryHarpsichordTuningHammer : MovableObject
{
    [HideInInspector]
    public HarpsichordTuningPin AttachedPin;


    public override void Start()
    {
        base.Start();

        AttachedPin = null;
    }


    public void attachToPin(
        HarpsichordTuningPin pin)
    {
        AttachedPin = pin;

        prepareForAttachment();
    }


    public void detachFromPin()
    {
        AttachedPin = null;

        AttachedToReceptor = false;
    }
} */
using UnityEngine;

public class CarryHarpsichordTuningHammer : MovableObject
{
    [HideInInspector]
    public HarpsichordTuningPin AttachedPin;


    private HarpsichordTuningHammerMode hammerMode;


    public override void Start()
    {
        base.Start();

        AttachedPin =
            null;

        hammerMode =
            GetComponent<
                HarpsichordTuningHammerMode>();

        if (hammerMode != null)
        {
            hammerMode.setMovableMode();
        }
    }


    public void attachToPin(
        HarpsichordTuningPin pin)
    {
        AttachedPin =
            pin;

        prepareForAttachment();


        // Every time the hammer is first
        // attached, it starts as Movable.
        if (hammerMode != null)
        {
            hammerMode.setMovableMode();
        }
    }


    public void detachFromPin()
    {
        AttachedPin =
            null;

        AttachedToReceptor =
            false;


        if (hammerMode != null)
        {
            hammerMode.setMovableMode();
        }
    }
}