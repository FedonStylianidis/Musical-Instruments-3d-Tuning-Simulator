using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class MovableObject : InteractableObject
{

    [HideInInspector]
    public Vector3 UprightRotation;

    [HideInInspector]
    public Vector3 Rotation_for_Carrying;

    [HideInInspector]
    public float Y_Offset_for_Carrying;

    [HideInInspector]
    public float Y_Offset_for_Relocation;


    // Start is called before the first frame update
    public override void Start()
    {
        UprightRotation = transform.localEulerAngles;
        Y_Offset_for_Carrying = 0F;
        Y_Offset_for_Relocation = 0F;
    }

    public virtual void restoreUprightRotation()
    {
        transform.localEulerAngles = UprightRotation;
    }

    public void rotateObject_for_Carrying()
    {
        transform.localEulerAngles = Rotation_for_Carrying;
    }

}
