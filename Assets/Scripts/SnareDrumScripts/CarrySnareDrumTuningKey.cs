
using UnityEngine;

public class CarrySnareDrumTuningKey : MovableObject
{
    [HideInInspector]
    public SnareDrumLug AttachedLug;
    private TuningToolMode keyMode;

    public override void Start()
    {
        base.Start();


        keyMode =
            GetComponent<TuningToolMode>();

            keyMode.setMovableMode(); 
    }

    public void attachToLug(
        SnareDrumLug lug)
    {
        AttachedLug = lug;

        prepareForAttachment();

            keyMode.setMovableMode();
    }

    public void detachFromLug()
    {
        AttachedLug = null;

        AttachedToReceptor = false;

            keyMode.setMovableMode();
    }
   
}