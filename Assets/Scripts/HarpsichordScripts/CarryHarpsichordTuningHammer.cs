
using UnityEngine;

public class CarryHarpsichordTuningHammer : MovableObject
{
    [HideInInspector]
    public HarpsichordTuningPin AttachedPin;


    [Header("Pin Attachment Pose")]

    [Tooltip(
        "World rotation used when the hammer " +
        "is attached to a tuning pin."
    )]
    public Vector3 PinAttachmentRotation =
        new Vector3(
            270f,
            301.31f,
            0f
        );

    [Tooltip(
        "Scale used when the hammer " +
        "is attached to a tuning pin."
    )]
    public Vector3 PinAttachmentScale =
        new Vector3(
            7f,
            7f,
            7f
        );


    private TuningToolMode hammerMode;


    public override void Start()
    {
        base.Start();

        AttachedPin = null;

        hammerMode =
            GetComponent<TuningToolMode>();
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


    public Quaternion getPinAttachmentRotation()
    {
        return Quaternion.Euler(
            PinAttachmentRotation
        );
    }


    public Vector3 getPinAttachmentScale()
    {
        return PinAttachmentScale;
    }

    
}