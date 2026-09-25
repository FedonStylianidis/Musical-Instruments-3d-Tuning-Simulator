/*using UnityEngine;

public class CarrySnareDrumTuningKey : MovableObject
{
    [HideInInspector]
    public SnareDrumLug AttachedLug;
    [Header("Lug Attachment Pose")]

    public Vector3 LugAttachmentRotation =
    Vector3.zero;

    public Vector3 LugAttachmentScale =
        Vector3.one;

    public Quaternion getLugAttachmentRotation()
    {
        return Quaternion.Euler(
            LugAttachmentRotation
        );
    }

    public Vector3 getLugAttachmentScale()
    {
        return LugAttachmentScale;
    }

    public override void Start()
    {
        base.Start();

        AttachedLug = null;
    }

    public void attachToLug(
        SnareDrumLug lug)
    {
        AttachedLug = lug;

        prepareForAttachment();
    }

    public void detachFromLug()
    {
        AttachedLug = null;

        AttachedToReceptor = false;
    }
}*/
/*using UnityEngine;

public class CarrySnareDrumTuningKey : MovableObject
{
    [HideInInspector]
    public SnareDrumLug AttachedLug;

    private TuningToolMode keyMode;


    public override void Start()
    {
        base.Start();

        AttachedLug = null;

        keyMode =
            GetComponent<TuningToolMode>();

        if (keyMode != null)
            keyMode.setMovableMode();
    }


    public void attachToLug(
        SnareDrumLug lug)
    {
        AttachedLug = lug;

        prepareForAttachment();

        if (keyMode != null)
            keyMode.setMovableMode();
    }


    public void detachFromLug()
    {
        AttachedLug = null;

        AttachedToReceptor = false;

        if (keyMode != null)
            keyMode.setMovableMode();
    }
}*/

using UnityEngine;

public class CarrySnareDrumTuningKey : MovableObject
{
    [HideInInspector]
    public SnareDrumLug AttachedLug;


    [Header("Lug Attachment Pose")]

    public Vector3 LugAttachmentRotation =
        Vector3.zero;

    public Vector3 LugAttachmentScale =
        Vector3.one;


    private TuningToolMode keyMode;


    public Quaternion getLugAttachmentRotation()
    {
        return Quaternion.Euler(
            LugAttachmentRotation
        );
    }


    public Vector3 getLugAttachmentScale()
    {
        return LugAttachmentScale;
    }


    public override void Start()
    {
        base.Start();

        AttachedLug = null;

        keyMode =
            GetComponent<TuningToolMode>();

        if (keyMode != null)
            keyMode.setMovableMode();


     
    }


    public void attachToLug(
        SnareDrumLug lug)
    {
        AttachedLug = lug;

        prepareForAttachment();

        if (keyMode != null)
            keyMode.setMovableMode();
    }


    public void detachFromLug()
    {
        AttachedLug = null;

        AttachedToReceptor = false;

        if (keyMode != null)
            keyMode.setMovableMode();
    }
   
}