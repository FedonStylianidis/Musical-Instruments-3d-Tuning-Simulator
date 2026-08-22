using UnityEngine;

public class HarpsichordTuningHammerMode : MonoBehaviour
{
    public enum HammerMode
    {
        Movable,
        Rotatable
    }


    [Header("Current Mode")]
    public HammerMode CurrentMode =
        HammerMode.Movable;


    private MouseUI mouseUI;

    private CarryHarpsichordTuningHammer carryHammer;

    private RotateHarpsichordTuningHammer rotateHammer;

    private bool MouseIsOverHammer;


    void Start()
    {
        mouseUI =
            GetComponent<MouseUI>();

        carryHammer =
            GetComponent<
                CarryHarpsichordTuningHammer>();

        rotateHammer =
            GetComponent<
                RotateHarpsichordTuningHammer>();


        setMovableMode();
    }


    void Update()
    {
        if (!MouseIsOverHammer)
            return;

        if (!Input.GetMouseButtonDown(1))
            return;


        // Allow switching to rotation
        // only while the hammer is on a pin.
        if (carryHammer.AttachedPin == null)
            return;


        if (CurrentMode ==
            HammerMode.Movable)
        {
            setRotatableMode();
        }
        else
        {
            setMovableMode();
        }
    }


    void OnMouseOver()
    {
        MouseIsOverHammer = true;
    }


    void OnMouseExit()
    {
        MouseIsOverHammer = false;
    }


    public void setMovableMode()
    {
        CurrentMode =
            HammerMode.Movable;


        if (carryHammer != null)
            carryHammer.enabled = true;


        if (rotateHammer != null)
            rotateHammer.enabled = false;


        if (mouseUI != null)
        {
            mouseUI.Movable = true;
            mouseUI.Rotatable = false;
        }


        MouseUI.Rotating = false;
    }


    public void setRotatableMode()
    {
        CurrentMode =
            HammerMode.Rotatable;


        if (carryHammer != null)
            carryHammer.enabled = false;


        if (rotateHammer != null)
            rotateHammer.enabled = true;


        if (mouseUI != null)
        {
            mouseUI.Movable = false;
            mouseUI.Rotatable = true;
        }
    }
}