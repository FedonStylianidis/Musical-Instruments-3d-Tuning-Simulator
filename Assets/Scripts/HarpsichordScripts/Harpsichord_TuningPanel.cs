using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Harpsichord_TuningPanel : Panel
{
    public override void zoom()
    {
        // Do not enter harpsichord tuning mode
        // while carrying the recorder.
        if (MouseUI.ObjectBeingCarried != null)
        {
            MouseUI carriedMouseUI =
                MouseUI.ObjectBeingCarried.GetComponent<MouseUI>();

            if (carriedMouseUI != null &&
                carriedMouseUI.Label == Labels.Recorder)
            {
                return;
            }
        }

        base.zoom();

        Harpsichord harpsichord =
            GetComponentInParent<Harpsichord>();

        if (harpsichord != null)
        {
            if (harpsichord.View == Instrument.locked &&
                  harpsichord.TuningDisplay != null)
            {
                harpsichord.TuningDisplay.clearDisplay();
            }
            harpsichord.showTuningCameraInstruction();
        }
    }
}