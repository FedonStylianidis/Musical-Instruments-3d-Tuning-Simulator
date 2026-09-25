/*using UnityEngine;

public class MovableObject : InteractableObject
{
    [Header("Resting Pose")]

    [Tooltip("Rotation used when the object is placed back on a surface.")]
    public Vector3 RestingRotation;

    [Tooltip("Vertical offset when the object is placed on a surface.")]
    public float Y_Offset_for_Relocation = 0f;


    [Header("Carrying Pose")]

    [Tooltip("Position relative to the camera while being carried.")]
    
   public Vector3 CarryingLocalPosition = new Vector3(-0.003f, -0.003f, 0.03f);
    [Tooltip("Rotation relative to the camera while being carried.")]
    public Vector3 CarryingLocalRotation =
        Vector3.zero;
    [Tooltip("Scale relative to the camera while being carried.")]
    public Vector3 CarryingLocalScale =
    new Vector3(0.5f, 0.5f, 0.5f);

    [Header("Free Mouse Carrying")]

    public float MouseCarryDistance = 0.003f;

    public Vector2 MouseCarryOffset = Vector2.zero;

    [HideInInspector]
    public bool AttachedToReceptor = false;

    [Header("Attachment Pose")]

    [Tooltip("Local position used when attached to another object.")]
    public Vector3 AttachmentLocalPosition =
        Vector3.zero;

    [Tooltip("Local rotation used when attached to another object.")]
    public Vector3 AttachmentLocalRotation =
        Vector3.zero;

    private Vector3 OriginalScale; // it keeps the original scale of the carried object in order to restore it later
    private int OriginalLayer;
    public override void Start()
    {
        base.Start();
        OriginalScale = transform.localScale;  //stores the orignal scale of the object when starting
        /*
         * If you want the object's scene rotation to be
         * automatically used as its resting rotation,
         * uncomment this:
         *
         * RestingRotation = transform.localEulerAngles;
         
    }

  

    public void prepareForCarrying()
    {
        OriginalLayer = gameObject.layer;
        gameObject.layer = LayerMask.NameToLayer("Ignore Raycast");
    }

    public void prepareForRelocation()
    {
        gameObject.layer = OriginalLayer;
        transform.localScale = OriginalScale;
    }
    public virtual void restoreUprightRotation()
    {
        transform.localEulerAngles = RestingRotation;
    }


    public virtual void setCarryingPose()
    {
        transform.localPosition =
            CarryingLocalPosition;

        transform.localEulerAngles =
            CarryingLocalRotation;

        transform.localScale = CarryingLocalScale;
    }
    public virtual void followMouse(Camera activeCamera)
    {
        if (activeCamera == null)
            return;

        if (AttachedToReceptor)
            return;

        Ray mouseRay =
            activeCamera.ScreenPointToRay(Input.mousePosition);

        Vector3 position =
            mouseRay.GetPoint(MouseCarryDistance);

        position +=
            activeCamera.transform.right * MouseCarryOffset.x;

        position +=
            activeCamera.transform.up * MouseCarryOffset.y;

        transform.position = position;
    }

    public virtual void setAttachmentPose(
        Transform attachmentPoint)
    {
        transform.SetParent(attachmentPoint);

        transform.localPosition =
            AttachmentLocalPosition;

        transform.localEulerAngles =
            AttachmentLocalRotation;
    }
    public virtual void prepareForAttachment()
    {
        gameObject.layer = OriginalLayer;

        transform.localScale = OriginalScale;

        AttachedToReceptor = true;
    }
} */

/*using UnityEngine;

public class MovableObject : InteractableObject
{
    [Header("Resting Pose")]

    [Tooltip("Rotation used when the object is placed back on a surface.")]
    public Vector3 RestingRotation;

    [Tooltip("Vertical offset when the object is placed on a surface.")]
    public float Y_Offset_for_Relocation = 0f;


    [Header("Carrying Pose")]

    [Tooltip("Position relative to the camera while being carried.")]
    public Vector3 CarryingLocalPosition =
        new Vector3(-0.003f, -0.003f, 0.03f);

    [Tooltip("Rotation relative to the camera while being carried.")]
    public Vector3 CarryingLocalRotation =
        Vector3.zero;

    [Tooltip("Scale while being carried.")]
    public Vector3 CarryingLocalScale =
        new Vector3(0.5f, 0.5f, 0.5f);


    [Header("Free Mouse Carrying")]

    public float MouseCarryDistance = 0.003f;

    public Vector2 MouseCarryOffset =
        Vector2.zero;


    [HideInInspector]
    public bool AttachedToReceptor = false;


    [Header("Attachment Pose")]

    [Tooltip("Local position used when attached to another object.")]
    public Vector3 AttachmentLocalPosition =
        Vector3.zero;

    [Tooltip("Local rotation used when attached to another object.")]
    public Vector3 AttachmentLocalRotation =
        Vector3.zero;


    private Vector3 OriginalScale;
    private int OriginalLayer;


    public override void Start()
    {
        base.Start();

        OriginalScale =
            transform.localScale;

        OriginalLayer =
            gameObject.layer;

        AttachedToReceptor =
            false;
    }


    public virtual void prepareForCarrying()
    {
        gameObject.layer =
            LayerMask.NameToLayer("Ignore Raycast");

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


    public virtual void setAttachmentPose(
        Transform attachmentPoint)
    {
        transform.SetParent(
            attachmentPoint
        );

        transform.localPosition =
            AttachmentLocalPosition;

        transform.localEulerAngles =
            AttachmentLocalRotation;
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
} */
/*using UnityEngine;

public class MovableObject : InteractableObject
{
    [Header("Resting Pose")]

    [Tooltip("Rotation used when the object is placed back on a surface.")]
    public Vector3 RestingRotation;

    [Tooltip("Vertical offset when the object is placed on a surface.")]
    public float Y_Offset_for_Relocation = 0f;


    [Header("Carrying Pose")]

    [Tooltip("Position relative to the camera while being carried.")]
    public Vector3 CarryingLocalPosition =
        new Vector3(-0.003f, -0.003f, 0.03f);

    [Tooltip("Rotation relative to the camera while being carried.")]
    public Vector3 CarryingLocalRotation =
        Vector3.zero;

    [Tooltip("Scale while being carried.")]
    public Vector3 CarryingLocalScale =
        new Vector3(0.5f, 0.5f, 0.5f);


    [Header("Free Mouse Carrying")]

    public float MouseCarryDistance = 0.003f;

    public Vector2 MouseCarryOffset =
        Vector2.zero;


    [HideInInspector]
    public bool AttachedToReceptor = false;


    private Vector3 OriginalScale;
    private int OriginalLayer;


    public override void Start()
    {
        base.Start();

        OriginalScale =
            transform.localScale;

        OriginalLayer =
            gameObject.layer;

        AttachedToReceptor =
            false;
    }


    public virtual void prepareForCarrying()
    {
        gameObject.layer =
            LayerMask.NameToLayer("Ignore Raycast");

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
} */
/*using UnityEngine;

public class MovableObject : InteractableObject
{
    [Header("Resting Pose")]

    [Tooltip("Rotation used when the object is placed back on a surface.")]
    public Vector3 RestingRotation;

    [Tooltip("Vertical offset when the object is placed on a surface.")]
    public float Y_Offset_for_Relocation = 0f;


    [Header("Carrying Pose")]

    [Tooltip("Position relative to the camera while being carried.")]
    
   public Vector3 CarryingLocalPosition = new Vector3(-0.003f, -0.003f, 0.03f);
    [Tooltip("Rotation relative to the camera while being carried.")]
    public Vector3 CarryingLocalRotation =
        Vector3.zero;
    [Tooltip("Scale relative to the camera while being carried.")]
    public Vector3 CarryingLocalScale =
    new Vector3(0.5f, 0.5f, 0.5f);

    [Header("Free Mouse Carrying")]

    public float MouseCarryDistance = 0.003f;

    public Vector2 MouseCarryOffset = Vector2.zero;

    [HideInInspector]
    public bool AttachedToReceptor = false;

    [Header("Attachment Pose")]

    [Tooltip("Local position used when attached to another object.")]
    public Vector3 AttachmentLocalPosition =
        Vector3.zero;

    [Tooltip("Local rotation used when attached to another object.")]
    public Vector3 AttachmentLocalRotation =
        Vector3.zero;

    private Vector3 OriginalScale; // it keeps the original scale of the carried object in order to restore it later
    private int OriginalLayer;
    public override void Start()
    {
        base.Start();
        OriginalScale = transform.localScale;  //stores the orignal scale of the object when starting
        /*
         * If you want the object's scene rotation to be
         * automatically used as its resting rotation,
         * uncomment this:
         *
         * RestingRotation = transform.localEulerAngles;
         
    }

  

    public void prepareForCarrying()
    {
        OriginalLayer = gameObject.layer;
        gameObject.layer = LayerMask.NameToLayer("Ignore Raycast");
    }

    public void prepareForRelocation()
    {
        gameObject.layer = OriginalLayer;
        transform.localScale = OriginalScale;
    }
    public virtual void restoreUprightRotation()
    {
        transform.localEulerAngles = RestingRotation;
    }


    public virtual void setCarryingPose()
    {
        transform.localPosition =
            CarryingLocalPosition;

        transform.localEulerAngles =
            CarryingLocalRotation;

        transform.localScale = CarryingLocalScale;
    }
    public virtual void followMouse(Camera activeCamera)
    {
        if (activeCamera == null)
            return;

        if (AttachedToReceptor)
            return;

        Ray mouseRay =
            activeCamera.ScreenPointToRay(Input.mousePosition);

        Vector3 position =
            mouseRay.GetPoint(MouseCarryDistance);

        position +=
            activeCamera.transform.right * MouseCarryOffset.x;

        position +=
            activeCamera.transform.up * MouseCarryOffset.y;

        transform.position = position;
    }

    public virtual void setAttachmentPose(
        Transform attachmentPoint)
    {
        transform.SetParent(attachmentPoint);

        transform.localPosition =
            AttachmentLocalPosition;

        transform.localEulerAngles =
            AttachmentLocalRotation;
    }
    public virtual void prepareForAttachment()
    {
        gameObject.layer = OriginalLayer;

        transform.localScale = OriginalScale;

        AttachedToReceptor = true;
    }
} */

/*using UnityEngine;

public class MovableObject : InteractableObject
{
    [Header("Resting Pose")]

    [Tooltip("Rotation used when the object is placed back on a surface.")]
    public Vector3 RestingRotation;

    [Tooltip("Vertical offset when the object is placed on a surface.")]
    public float Y_Offset_for_Relocation = 0f;


    [Header("Carrying Pose")]

    [Tooltip("Position relative to the camera while being carried.")]
    public Vector3 CarryingLocalPosition =
        new Vector3(-0.003f, -0.003f, 0.03f);

    [Tooltip("Rotation relative to the camera while being carried.")]
    public Vector3 CarryingLocalRotation =
        Vector3.zero;

    [Tooltip("Scale while being carried.")]
    public Vector3 CarryingLocalScale =
        new Vector3(0.5f, 0.5f, 0.5f);


    [Header("Free Mouse Carrying")]

    public float MouseCarryDistance = 0.003f;

    public Vector2 MouseCarryOffset =
        Vector2.zero;


    [HideInInspector]
    public bool AttachedToReceptor = false;


    [Header("Attachment Pose")]

    [Tooltip("Local position used when attached to another object.")]
    public Vector3 AttachmentLocalPosition =
        Vector3.zero;

    [Tooltip("Local rotation used when attached to another object.")]
    public Vector3 AttachmentLocalRotation =
        Vector3.zero;


    private Vector3 OriginalScale;
    private int OriginalLayer;


    public override void Start()
    {
        base.Start();

        OriginalScale =
            transform.localScale;

        OriginalLayer =
            gameObject.layer;

        AttachedToReceptor =
            false;
    }


    public virtual void prepareForCarrying()
    {
        gameObject.layer =
            LayerMask.NameToLayer("Ignore Raycast");

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


    public virtual void setAttachmentPose(
        Transform attachmentPoint)
    {
        transform.SetParent(
            attachmentPoint
        );

        transform.localPosition =
            AttachmentLocalPosition;

        transform.localEulerAngles =
            AttachmentLocalRotation;
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
} */
/*using UnityEngine;

public class MovableObject : InteractableObject
{
    [Header("Resting Pose")]

    [Tooltip("Rotation used when the object is placed back on a surface.")]
    public Vector3 RestingRotation;

    [Tooltip("Vertical offset when the object is placed on a surface.")]
    public float Y_Offset_for_Relocation = 0f;


    [Header("Carrying Pose")]

    [Tooltip("Position relative to the camera while being carried.")]
    public Vector3 CarryingLocalPosition =
        new Vector3(-0.003f, -0.003f, 0.03f);

    [Tooltip("Rotation relative to the camera while being carried.")]
    public Vector3 CarryingLocalRotation =
        Vector3.zero;

    [Tooltip("Scale while being carried.")]
    public Vector3 CarryingLocalScale =
        new Vector3(0.5f, 0.5f, 0.5f);


    [Header("Free Mouse Carrying")]

    public float MouseCarryDistance = 0.003f;

    public Vector2 MouseCarryOffset =
        Vector2.zero;


    [HideInInspector]
    public bool AttachedToReceptor = false;


    private Vector3 OriginalScale;
    private int OriginalLayer;


    public override void Start()
    {
        base.Start();

        OriginalScale =
            transform.localScale;

        OriginalLayer =
            gameObject.layer;

        AttachedToReceptor =
            false;
    }


    public virtual void prepareForCarrying()
    {
        gameObject.layer =
            LayerMask.NameToLayer("Ignore Raycast");

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
} */
/*using UnityEngine;

public class MovableObject : InteractableObject
{
    [Header("Resting Pose")]

    [Tooltip("Rotation used when the object is placed back on a surface.")]
    public Vector3 RestingRotation;

    [Tooltip("Vertical offset when the object is placed on a surface.")]
    public float Y_Offset_for_Relocation = 0f;


    [Header("Carrying Pose")]

    [Tooltip("Position relative to the camera while being carried.")]
    public Vector3 CarryingLocalPosition =
        new Vector3(-0.003f, -0.003f, 0.03f);

    [Tooltip("Rotation relative to the camera while being carried.")]
    public Vector3 CarryingLocalRotation =
        new Vector3(270f, 301.31f, 0f);

    [Tooltip("Scale while being carried.")]
    public Vector3 CarryingLocalScale =
        new Vector3(0.5f, 0.5f, 0.5f);


    [Header("Free Mouse Carrying")]

    public float MouseCarryDistance = 0.003f;

    public Vector2 MouseCarryOffset =
        Vector2.zero;


    [HideInInspector]
    public bool AttachedToReceptor = false;


    [HideInInspector]
    public Quaternion OriginalWorldRotation;


    private Vector3 OriginalScale;
    private int OriginalLayer;


    public override void Start()
    {
        base.Start();

        OriginalScale =
            transform.localScale;

        OriginalLayer =
            gameObject.layer;

        OriginalWorldRotation =
            transform.rotation;

        AttachedToReceptor =
            false;
    }


    public virtual void prepareForCarrying()
    {
        gameObject.layer =
            LayerMask.NameToLayer("Ignore Raycast");

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


    public Vector3 getOriginalScale()
    {
        return OriginalScale;
    }
}*/

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
    //  = new Vector3(-0.003f, -0.003f, 0.03f);

    [Tooltip("Rotation relative to the camera while being carried.")]
    public Vector3 CarryingLocalRotation;
    // = new Vector3(270f, 301.31f, 0f);

    [Tooltip("Scale while being carried.")]
    public Vector3 CarryingLocalScale;
      //  =  new Vector3(0.5f, 0.5f, 0.5f);


    [Header("Free Mouse Carrying")]

    public float MouseCarryDistance = 0.003f;

    public Vector2 MouseCarryOffset =
        Vector2.zero;


    [HideInInspector]
    public bool AttachedToReceptor = false;


    [HideInInspector]
    public Quaternion OriginalWorldRotation;


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


        // Keep this because existing code
        // may still use it.
        OriginalWorldRotation =
            transform.rotation;


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
   /* public virtual void setCarryingPose()
    {
        transform.localPosition =
            CarryingLocalPosition;

        transform.localEulerAngles =
            CarryingLocalRotation;

        Vector3 parentScale =
            transform.parent.lossyScale;

        transform.localScale =
            new Vector3(
                CarryingLocalScale.x / parentScale.x,
                CarryingLocalScale.y / parentScale.y,
                CarryingLocalScale.z / parentScale.z
            );
    }*/

   

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


    public Vector3 getOriginalScale()
    {
        return OriginalScale;
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