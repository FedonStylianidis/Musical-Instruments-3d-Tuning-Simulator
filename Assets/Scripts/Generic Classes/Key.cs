
using UnityEngine;


public class Key : InteractableObject
{

    [HideInInspector]
    public Axes RotationAxis;

    [HideInInspector]
    public float Angle_for_Pressed;

    [HideInInspector]
    public float Angle_for_Released;

    [HideInInspector]
    public bool Pressed;

    public override void Start()
    {
        Pressed = false;
        setAngle(Angle_for_Released);
    }

    public override void press()
    {
        Pressed = true;
        setAngle(Angle_for_Pressed);
    }

    public override void release()
    {
        Pressed = false;
        setAngle(Angle_for_Released);
    }

    void setAngle(float _NewAngle)
    {
        if (RotationAxis == Axes.X_Axis)
            transform.localEulerAngles = new Vector3(_NewAngle, transform.localEulerAngles.y, transform.localEulerAngles.z);
        else if (RotationAxis == Axes.Y_Axis)
            transform.localEulerAngles = new Vector3(transform.localEulerAngles.x, _NewAngle, transform.localEulerAngles.z);
        else
            transform.localEulerAngles = new Vector3(transform.localEulerAngles.x, transform.localEulerAngles.y, _NewAngle);
    }
}