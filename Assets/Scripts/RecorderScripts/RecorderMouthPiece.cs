using UnityEngine;

public class RecorderMouthpiece : InteractableObject
{
    private Recorder Recorder;


    public override void Start()
    {
        base.Start();

        Recorder =
            GetComponentInParent<Recorder>();
    }


    public override void press()
    {
        if (Recorder == null)
            return;


        Recorder.playCurrentNote();
    }


    public override void release()
    {

        Recorder.stopCurrentNote();
    }
}