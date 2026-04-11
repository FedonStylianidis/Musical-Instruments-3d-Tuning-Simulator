using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class Knob : InteractableObject
{

    [HideInInspector]
    public Axes MouseMovementAxis; //Mouse movement axis - X or Y
    [HideInInspector]
    public Axes RotationAxis; //Mesh rotation axis

    [HideInInspector]
    public float da;


    [HideInInspector]
    public int u = 1; //because most knobs "open" clockwise


    // Start is called before the first frame update
    public override void Start()
    {
    }

    public override void rotate(Vector2 _dv)
    {

        int Direction;

        if (MouseMovementAxis == Axes.X_Axis)
            Direction = (int)Mathf.Sign(_dv.x);
        else
            Direction = (int)Mathf.Sign(_dv.y);

        Direction *= u;

        if (Direction != 0)
            applyRotation(Direction);

    }

    public virtual void applyRotation(int _Direction)
    {
    }


    public override void doneRotating()
    {
    }





    public virtual void setRotation(float _Angle)
    {

        if (RotationAxis == Axes.X_Axis)
            transform.Rotate(_Angle, 0F, 0F);
        else if (RotationAxis == Axes.Y_Axis)
            transform.Rotate(0F, _Angle, 0F);
        else
            transform.Rotate(0F, 0F, _Angle);

    }


    public virtual void affectOtherComponents(int _NewPosition)
    {
    }

}
