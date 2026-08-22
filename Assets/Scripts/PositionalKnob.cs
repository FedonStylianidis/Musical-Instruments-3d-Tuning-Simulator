using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PositionalKnob : Knob
{

    [HideInInspector]
    public int MaxPosition;
    [HideInInspector]
    public int MinPosition;
    [HideInInspector]
    public int Position;

    [HideInInspector]
    public int Step;

    // Start is called before the first frame update
    public override void Start()
    {
    }

    public override void applyRotation(int _Direction)
    {

        if (_Direction > 0 && Position + Step <= MaxPosition)
        {

            setRotation(da);

            Position += Step;

            affectOtherComponents(Position);

        }
        else if (_Direction < 0 && Position - Step >= MinPosition)
        {

            setRotation(-da);

            Position -= Step;

            affectOtherComponents(Position);

        }


    }

}
