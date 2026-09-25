using UnityEngine;

public class MovableObject : InteractableObject
{
    [Header("Resting Pose")]

    [Tooltip("Rotation used when the object is placed back on a normal surface.")]
    public Vector3 RestingRotation;

    [Tooltip("Vertical offset when the object is placed on a normal surface.")]
    public float Y_Offset_for_Relocation = 0f;


    [Header("Carrying Pose")]

    [Tooltip("Position relative to the camera while being carried.")]
    public Vector3 CarryingLocalPosition;

    [Tooltip("Rotation relative to the camera while being carried.")]
    public Vector3 CarryingLocalRotation;
   

    [Tooltip("Scale while being carried.")]
    public Vector3 CarryingLocalScale;
     


    [Header("Free Mouse Carrying")]

    public float MouseCarryDistance = 0.003f;

    public Vector2 MouseCarryOffset =
        Vector2.zero;


    [HideInInspector]
    public bool AttachedToReceptor = false;

    private Vector3 OriginalScale;
    private int OriginalLayer;


    // Exact world pose of the object
    // when the application starts.
    // This is used for ToolsStand.
    private Vector3 OriginalRestingPosition;

    private Quaternion OriginalRestingRotation;

    private Vector3 OriginalRestingScale;


    public override void Start()
    {
        base.Start();


        // Save original scale and layer.
        OriginalScale =
            transform.localScale;

        OriginalLayer =
            gameObject.layer;


        // Save the exact starting pose.
        // For tools, this is their position
        // on ToolsStand.
        OriginalRestingPosition =
            transform.position;

        OriginalRestingRotation =
            transform.rotation;

        OriginalRestingScale =
            transform.localScale;


        AttachedToReceptor =
            false;
    }


    public virtual void prepareForCarrying()
    {
        gameObject.layer =
            LayerMask.NameToLayer(
                "Ignore Raycast"
            );

        AttachedToReceptor =
            false;
    }


    public virtual void prepareForRelocation()
    {
        gameObject.layer =
            OriginalLayer;

        transform.localScale =
            OriginalScale;

        AttachedToReceptor =
            false;
    }


    public virtual void restoreUprightRotation()
    {
        transform.localEulerAngles =
            RestingRotation;
    }
 
   

    public virtual void setCarryingPose()
    {
        transform.localPosition =
            CarryingLocalPosition;

        transform.localEulerAngles =
            CarryingLocalRotation;

        transform.localScale =
            CarryingLocalScale;
    }



    public virtual void followMouse(
         Camera activeCamera)
     {
         if (activeCamera == null)
             return;

         if (AttachedToReceptor)
             return;


         Ray mouseRay =
             activeCamera.ScreenPointToRay(
                 Input.mousePosition
             );


         Vector3 position =
             mouseRay.GetPoint(
                 MouseCarryDistance
             );


         position +=
             activeCamera.transform.right *
             MouseCarryOffset.x;

         position +=
             activeCamera.transform.up *
             MouseCarryOffset.y;


         transform.position =
             position;
     }  


    public virtual void prepareForAttachment()
    {
        gameObject.layer =
            OriginalLayer;

        transform.localScale =
            OriginalScale;

        AttachedToReceptor =
            true;
    }

    public virtual void returnToOriginalRestingPose()
    {
        // Remove the object from the camera,
        // pin, or any other current parent.
        transform.SetParent(
            null,
            true
        );


        // Restore the exact world position
        // it had when the scene started.
        transform.position =
            OriginalRestingPosition;


        // Restore the exact world rotation
        // it had when the scene started.
        transform.rotation =
            OriginalRestingRotation;


        // Restore its original scale.
        transform.localScale =
            OriginalRestingScale;


        // Restore its original layer.
        gameObject.layer =
            OriginalLayer;


        AttachedToReceptor =
            false;
    }
}