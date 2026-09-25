using UnityEngine;

public class TuningToolMode : MonoBehaviour
{
    public enum ToolMode
    {
        Movable,
        Rotatable
    }


    [Header("Current Mode")]
    public ToolMode CurrentMode =
        ToolMode.Movable;


    private MouseUI mouseUI;

    private MovableObject movableObject;

    private RotatableObject rotatableObject;

    private bool MouseIsOverTool;


    void Start()
    {
        mouseUI =
            GetComponent<MouseUI>();

        movableObject =
            GetComponent<MovableObject>();

        rotatableObject =
            GetComponent<RotatableObject>();


        setMovableMode();
    }


    void Update()
    {
        if (!MouseIsOverTool)
            return;

        if (!Input.GetMouseButtonDown(1))
            return;


        // The tool can become rotatable
        // only when attached to a receptor.
        if (!movableObject.AttachedToReceptor)
            return;


        if (CurrentMode ==
            ToolMode.Movable)
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
        MouseIsOverTool = true;
    }


    void OnMouseExit()
    {
        MouseIsOverTool = false;
    }


    public void setMovableMode()
    {
        CurrentMode =
            ToolMode.Movable;


        if (movableObject != null)
            movableObject.enabled = true;


        if (rotatableObject != null)
            rotatableObject.enabled = false;


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
            ToolMode.Rotatable;


        if (movableObject != null)
            movableObject.enabled = false;


        if (rotatableObject != null)
            rotatableObject.enabled = true;


        if (mouseUI != null)
        {
            mouseUI.Movable = false;
            mouseUI.Rotatable = true;
        }
    }
}