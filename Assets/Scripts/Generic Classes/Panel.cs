
using UnityEngine;

public class Panel : InteractableObject
{

    public GameObject ControlInstrument;

    public override void zoom()
    {
        ControlInstrument.GetComponent<Instrument>().toggleView();
    }

}
