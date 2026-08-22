using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Panel : InteractableObject
{

    public GameObject ControlInstrument;

    public override void zoom()
    {
        ControlInstrument.GetComponent<Instrument>().toggleView();
    }

    public virtual void updateZoomability(bool _NewZoomability)
    {
        GetComponent<MouseUI>().Zoomable = _NewZoomability;
    }

}
