using UnityEngine;

public enum RecorderHoleState
{
    Open,
    Covered,
    HalfCovered
}

public class RecorderHole : InteractableObject
{
    [Header("Hole")]
    public int HoleNumber;

    public bool AllowsHalfCover;  //default is false

    [HideInInspector]
    public RecorderHoleState State = RecorderHoleState.Open;
   
    
    [Header("Hole Visual")]
    public Material CoveredMaterial;

    private MeshRenderer CoverRenderer;
    private MeshRenderer HalfCoverRenderer;


    public override void Start()
    {
        base.Start();

        State = RecorderHoleState.Open;

        Transform coverVisual =
     transform.Find("CoverVisual");

      
            CoverRenderer =
                coverVisual.GetComponent<MeshRenderer>();

                CoverRenderer.material =
                    CoveredMaterial;

                CoverRenderer.enabled = false;
            
        

        Transform halfCoverVisual =
            transform.Find("HalfCoverVisual");

        if (halfCoverVisual != null)
        {
            HalfCoverRenderer =
                halfCoverVisual.GetComponent<MeshRenderer>();

            
                HalfCoverRenderer.material =
                    CoveredMaterial;

                HalfCoverRenderer.enabled = false;
            
        }
    }

    private void updateVisual()
    {
        if (CoverRenderer != null)
        {
            CoverRenderer.enabled =
                State == RecorderHoleState.Covered;
        }

        if (HalfCoverRenderer != null)
        {
            HalfCoverRenderer.enabled =
                State == RecorderHoleState.HalfCovered;
        }
    
    }
    public override void press()
    {
        if (AllowsHalfCover)
        {
            if (State == RecorderHoleState.Open)
                State = RecorderHoleState.Covered;

            else if (State == RecorderHoleState.Covered)
                State = RecorderHoleState.HalfCovered;

            else
                State = RecorderHoleState.Open;
        }
        else
        {
            if (State == RecorderHoleState.Open)
                State = RecorderHoleState.Covered;
            else
                State = RecorderHoleState.Open;
        }

     
        updateVisual();
    }

    //  sets a new recorder hole state so it will show visually in the demo
    public void setState(
    RecorderHoleState newState)
    {
        State = newState;

        updateVisual();
    }
    public void resetHole()
    {
        State = RecorderHoleState.Open;

        updateVisual();
    }

    public override string getTooltipName()
    {
        switch (HoleNumber)
        {
            case 1:
                return "LEFT HAND HOLE 1";

            case 2:
                return "LEFT HAND HOLE 2";

            case 3:
                return "LEFT HAND HOLE 3";

            case 4:
                return "RIGHT HAND HOLE 1";

            case 5:
                return "RIGHT HAND HOLE 2";

            case 6:
                return "RIGHT HAND HOLE 3";

            case 7:
                return "RIGHT HAND HOLE 4";

            case 8:
                return "SMALL HOLE 1";

            case 9:
                return "SMALL HOLE 2";

            case 10:
                return "THUMB HOLE";
        }

        return "";
    }
}