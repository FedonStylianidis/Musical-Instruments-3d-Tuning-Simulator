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
using UnityEngine;

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
}