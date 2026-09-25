using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Runtime.InteropServices;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Rendering;
using UnityEngine.SceneManagement;

//using UnityEngine.UIElements;

public enum Modes
{
    Navigation,
    Harpsichord_Tuning,
    SnareDrum_Tuning,
    Recorder_Tuning,
    Microscoping

}


public class EgoController : MonoBehaviour
{

    [DllImport("user32.dll")]
    public static extern bool SetCursorPos(int X, int Y);

    //[DllImport ("user32.dll")]
    //public static extern bool GetCursorPos (out Point pos);

    Transform EmbeddedCamera;
    public CharacterController controller;
    public float speed = 12f;
    public float mouseSensitivity = 100f;
    public float gravity = -9.81f * 2;
    public float jumpHeight = 3f;

    public Transform groundCheck;
    public float groundDistance = 0.4f;
    public LayerMask groundMask;

    Vector3 velocity;

    bool isGrounded;

    float xRotation = 0f;
    float YRotation = 0f;
    // Rigidbody rb;

    const int FurnitureLayer = 11;
    const int WallsLayer = 12;

   // public float LinearHorizontalSpeed;
   // public float LinearVerticalSpeed;
   // public float AngularHorizontalSpeed;
   // public float AngularVerticalSpeed;

    const float DefaultFieldOfView = 60F;

   // const float MaximumXAngle = 70F;
   // const float MinimumXAngle = -70F;

    //  public const float Height = 3.603F;

    float move_vertically;
    float move_horizontally;

    float rotate_vertically;
    float rotate_horizontally;

    [HideInInspector]
    public Modes Mode;

    [HideInInspector]
    public GameObject InstrumentOnFocus;

    //public for Save
    [HideInInspector]
    public float Saved_X_position;

    //public for Save
    [HideInInspector]
    public float Saved_Z_position;

    //public for Save
    [HideInInspector]
    public float Saved_Y_angle;

    //public for Save
    [HideInInspector]
    public float Saved_Camera_X_Angle;

 //   bool Halt;

    public static bool CursorFrozen;


    [Header("Pause Menu")]
    public GameObject StartMenuCanvas;

    private bool PauseMenuOpen = false;
    private bool CursorWasFrozen;
    private StartMenu startMenu;




    // Start is called before the first frame update
    void Start()
    {

        Mode = Modes.Navigation;

     //   Halt = false;

        CursorFrozen = true;

        EmbeddedCamera = transform.GetChild(0);
       
        
        
        // Start rotation from the rotation
        // already set in the Unity scene.
        YRotation =
            transform.eulerAngles.y;

        xRotation =
            EmbeddedCamera.localEulerAngles.x;

        //  rb = GetComponent<Rigidbody>();

#if !UNITY_EDITOR
        Cursor.visible = false;
#endif

        if (!StartMenu.GameHasStarted)
        {
            // FIRST LAUNCH:
            // remember Ego's normal cursor state.
            CursorWasFrozen =
                CursorFrozen;

            PauseMenuOpen = true;

            StartMenuCanvas.SetActive(true);

            Time.timeScale = 0f;

            CursorFrozen = false;

            Cursor.visible = true;

            Cursor.lockState =
                CursorLockMode.None;
        }
        else
        {
            // RESTART:
            // GameHasStarted is already true,
            // so enter gameplay immediately.
            PauseMenuOpen = false;

            StartMenuCanvas.SetActive(false);

            Time.timeScale = 1f;
        }

        startMenu =
    StartMenuCanvas.GetComponent<StartMenu>();

    }


    void OnGUI()
    {
        MouseUI.callOnGUI();
    }


  /*  public void setMode(Modes _Mode)
    { //for navigation

        CursorFrozen = true;

#if !UNITY_EDITOR
        Cursor.visible = false;
#endif

        InstrumentOnFocus = null;
        Mode = _Mode;

        restoreSavedPresence();
        controller.enabled = true;  //<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<


    }  */

    public void setMode(Modes _Mode)      ///gia to harpsichord
    {
        CursorFrozen = (_Mode == Modes.Navigation);

        InstrumentOnFocus = null;
        Mode = _Mode;

        if (_Mode == Modes.Navigation)
            restoreSavedPresence();
    }

    public void setMode(Modes _Mode, GameObject _InstrumentOnFocus, bool _RestorePresence)   //new overload for set mode
    {
        CursorFrozen = (_Mode == Modes.Navigation);

#if !UNITY_EDITOR
    Cursor.visible = !CursorFrozen;
#endif

        InstrumentOnFocus = _InstrumentOnFocus;
        Mode = _Mode;

        if (_RestorePresence)
            restoreSavedPresence();
    }

    /* public void setMode(Modes _Mode, GameObject _InstrumentOnFocus, float _X_position,
         float _Z_position, float _Y_angle, float _X_camera_angle, float _FieldOfView)
     {

         CursorFrozen = false;

 #if !UNITY_EDITOR
         Cursor.visible = true;
 #endif

         if (Mode == Modes.Navigation)
             saveCurrentPresence();

         InstrumentOnFocus = _InstrumentOnFocus;
         Mode = _Mode;

         transform.position = new Vector3(_X_position, transform.position.y, _Z_position);
         transform.eulerAngles = new Vector3(0F, _Y_angle, 0F);
         EmbeddedCamera.localEulerAngles = new Vector3(_X_camera_angle, 0F, 0F);
         EmbeddedCamera.GetComponent<Camera>().fieldOfView = _FieldOfView;

     }*/

    public void setMode(                       
    Modes _Mode,
    GameObject _InstrumentOnFocus,
    float _X_position,
    float _Y_position,
    float _Z_position,
    float _Y_angle,
    float _X_camera_angle,
    float _FieldOfView)
    {
        CursorFrozen = false;

        if (Mode == Modes.Navigation)
            saveCurrentPresence();

        InstrumentOnFocus = _InstrumentOnFocus;
        Mode = _Mode;
        controller.enabled = false;  //  <<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<
        transform.position = new Vector3(_X_position, _Y_position, _Z_position);
        transform.eulerAngles = new Vector3(0F, _Y_angle, 0F);
        EmbeddedCamera.localEulerAngles = new Vector3(_X_camera_angle, 0F, 0F);

        YRotation = _Y_angle;
        xRotation = _X_camera_angle;         //sync the variables so they don't use weird angles


        EmbeddedCamera.GetComponent<Camera>().fieldOfView = _FieldOfView;
    }
    void saveCurrentPresence()
    {

        Saved_X_position = transform.position.x;
        Saved_Z_position = transform.position.z;

        Saved_Y_angle = transform.eulerAngles.y;

        Saved_Camera_X_Angle = EmbeddedCamera.localEulerAngles.x;

    }



    void restoreSavedPresence()
    {

        transform.position = new Vector3(Saved_X_position, transform.localPosition.y, Saved_Z_position);
        transform.eulerAngles = new Vector3(0F, Saved_Y_angle, 0F);

        EmbeddedCamera.eulerAngles = new Vector3(Saved_Camera_X_Angle, 0F, 0F);

        YRotation = Saved_Y_angle;
        xRotation = Saved_Camera_X_Angle;

        EmbeddedCamera.GetComponent<Camera>().fieldOfView = DefaultFieldOfView;

    }



    public void startGameplay()
    {
        PauseMenuOpen = false;

        StartMenuCanvas.SetActive(false);

        Time.timeScale = 1f;


        // Restore the original EgoController
        // cursor behavior.
        CursorFrozen =
            CursorWasFrozen;


        if (CursorFrozen)
        {
            // EgoController itself keeps the mouse
            // in the centre using SetCursorPos().
            Cursor.lockState =
                CursorLockMode.None;

#if !UNITY_EDITOR
        Cursor.visible = false;
#endif
        }
        else
        {
            Cursor.lockState =
                CursorLockMode.None;

            Cursor.visible = true;
        }
    }




    public void togglePauseMenu()
    {
        if (!PauseMenuOpen)
        {
            // Remember whether the cursor was
            // frozen before opening the menu.
            CursorWasFrozen =
                CursorFrozen;


            PauseMenuOpen = true;


            // Show the complete menu.
            StartMenuCanvas.SetActive(true);


            // Whenever the menu is opened,
            // make sure we start on the main
            // menu and NOT Instructions.
            if (startMenu != null)
            {
                startMenu.hideInstructions();
            }


            // Pause gameplay.
            Time.timeScale = 0f;


            // Free the mouse for the UI.
            CursorFrozen = false;

            Cursor.lockState =
                CursorLockMode.None;

            Cursor.visible = true;
        }
        else
        {
            startGameplay();
        }
    }

    // Update is called once per frame
    void Update()
        {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            // If we are already in the pause menu
            // and Instructions are showing,
            // ESC closes only Instructions.
            if (PauseMenuOpen &&
                startMenu != null &&
                startMenu.instructionsAreOpen())
            {
                startMenu.hideInstructions();
            }

            // Otherwise ESC opens/closes
            // the complete pause menu.
            else
            {
                togglePauseMenu();
            }
        }


        if (PauseMenuOpen)
        {
            return;
        }




#if UNITY_EDITOR
        if (Input.GetKeyUp(KeyCode.Space))
                CursorFrozen = !CursorFrozen;
#endif

            if (CursorFrozen)
                //GetCursorPos (out Point CursorPos);
                //print (CursorPos.X + " , " + CursorPos.Y);
                SetCursorPos(Screen.width / 2, Screen.height / 2);


            if (Mode == Modes.Navigation)
            {
                isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask);

                if (isGrounded && velocity.y < 0)
                {
                    velocity.y = -2f;
                }
                //  if (!Halt)
                //   {

                move_horizontally = Input.GetAxis("Horizontal");
                move_vertically = Input.GetAxis("Vertical");
                //  }

                rotate_horizontally = Input.GetAxis("Mouse X");
                rotate_vertically = Input.GetAxis("Mouse Y");


                Vector3 move = transform.right * move_horizontally + transform.forward * move_vertically;
                controller.Move(move * speed * Time.deltaTime);
                //check if the player is on the ground so he can jump
                if (Input.GetButtonDown("Jump") && isGrounded)
                {
                    //the equation for jumping
                    velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
                }

                velocity.y += gravity * Time.deltaTime;

                controller.Move(velocity * Time.deltaTime);
                /* if (move_vertically != 0F)
                {

                    rb.MovePosition(transform.position + transform.forward * LinearVerticalSpeed * move_vertically * Time.deltaTime);

                    move_vertically = 0F;

                }

                if (move_horizontally != 0F)
                {

                    rb.MovePosition(transform.position + transform.right * LinearHorizontalSpeed * move_horizontally * Time.deltaTime);

                    move_horizontally = 0F;

                } */

                if (!MouseUI.Rotating)
                {
                    float mouseX = rotate_horizontally * mouseSensitivity * Time.deltaTime;
                    float mouseY = rotate_vertically * mouseSensitivity * Time.deltaTime;

                    //control rotation around x axis (Look up and down)
                    xRotation -= mouseY;

                    //we clamp the rotation so we cant Over-rotate (like in real life)
                    xRotation = Mathf.Clamp(xRotation, -45f, 45f);

                    //control rotation around y axis (Look up and down)
                    YRotation += mouseX;

                    //applying both rotations
                    transform.localRotation = Quaternion.Euler(xRotation, YRotation, 0f);
                }
                /* if (rotate_vertically != 0F)
                 {

                     if (withinVerticalRotationLimits())
                         //we rotate the child Camera, not Ego GameOject
                         EmbeddedCamera.localEulerAngles = new Vector3(EmbeddedCamera.localEulerAngles.x - AngularVerticalSpeed * rotate_vertically * Time.deltaTime, 0F, 0F);

                     rotate_vertically = 0F;

                 }

                 if (rotate_horizontally != 0F)
                 {

                     //we rotate Ego GameObject, not the child Camera
                     transform.localEulerAngles = new Vector3(0F, transform.localEulerAngles.y + AngularHorizontalSpeed * rotate_horizontally * Time.deltaTime, 0F);

                     rotate_horizontally = 0F;

                 }  */

            }

            //<- if (Input.GetKeyUp(KeyCode.Escape))
            //  Application.Quit();
            if (MouseUI.ObjectBeingCarried != null &&
         !CursorFrozen)
            {
                MovableObject movable =
                    MouseUI.ObjectBeingCarried
                        .GetComponent<MovableObject>();

                if (movable != null)
                {
                    Camera activeCamera = null;

                    Camera[] cameras = Camera.allCameras;

                    foreach (Camera cam in cameras)
                    {
                        if (cam.enabled &&
                            cam.gameObject.activeInHierarchy)
                        {
                            activeCamera = cam;
                            break;
                        }
                    }

                    movable.followMouse(activeCamera);
                }
            }

        }
    


    /*  bool withinVerticalRotationLimits()
    {

      return reduced(EmbeddedCamera.localEulerAngles.x) -
                  AngularVerticalSpeed * (rotate_vertically - 1) >= MinimumXAngle &&
                  reduced(EmbeddedCamera.localEulerAngles.x) -
                  AngularVerticalSpeed * (rotate_vertically + 1) <= MaximumXAngle;

    }


    float reduced(float _Angle)
    {

      float NewAngle = _Angle;

      if (NewAngle > 270F)
          NewAngle -= 360F;

      return NewAngle;

    }


    void OnCollisionEnter(Collision _collision)
    {

      if (_collision.gameObject.layer == FurnitureLayer || _collision.gameObject.layer == WallsLayer)
      {

          Halt = true;

          rb.linearVelocity = Vector3.zero;

          StartCoroutine(unHalting());

      }

    }


    void OnCollisionExit(Collision _collision)
    {

      if (_collision.gameObject.layer == FurnitureLayer || _collision.gameObject.layer == WallsLayer)
      {

          Halt = false;

      }

    }


    IEnumerator unHalting()
    {

      yield return new WaitUntil(() => Input.GetKeyUp(KeyCode.DownArrow)
      || Input.GetKeyUp(KeyCode.UpArrow) || Input.GetKeyUp(KeyCode.LeftArrow) || Input.GetKeyUp(KeyCode.RightArrow) || Input.GetKeyUp(KeyCode.W)
      || Input.GetKeyUp(KeyCode.S) || Input.GetKeyUp(KeyCode.A) || Input.GetKeyUp(KeyCode.D));

      Halt = false;

    } */

    /* public void attach(GameObject _object)      
      {
          //RestoreUprightPosition ();
          _object.transform.parent = EmbeddedCamera;
          _object.transform.localPosition = new Vector3(0F, _object.GetComponent<MovableObject>().Y_Offset_for_Carrying, 0.5F);
          _object.GetComponent<MovableObject>().rotateObject_for_Carrying();
      }
    */
    /*public void attach(GameObject _object)
    {
        _object.transform.parent = EmbeddedCamera;

        _object
            .GetComponent<MovableObject>()
            .setCarryingPose();
    } */
    
    public void attach(GameObject _object)
    {
        Camera activeCamera = getActiveCamera();

        if (activeCamera == null)
        {
            Debug.LogWarning("No active camera found.");
            return;
        }

     //   _object.transform.SetParent(activeCamera.transform);
       _object.transform.SetParent( activeCamera.transform, true);

       _object.GetComponent<MovableObject>().setCarryingPose();


    }


   
    private Camera getActiveCamera()
    {
        Camera[] cameras = Camera.allCameras;

        foreach (Camera cam in cameras)
        {
            if (cam.enabled && cam.gameObject.activeInHierarchy)
                return cam;
        }

        return null;
    }

    public static Dictionary<Modes, bool> PermittingCollection_of_Objects = new Dictionary<Modes, bool>() {
      {Modes.Navigation,true},
      {Modes.Microscoping,false},
      {Modes.Harpsichord_Tuning, true},
      {Modes.SnareDrum_Tuning, true},
      {Modes.Recorder_Tuning, true}
  };

}   


/*using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Runtime.InteropServices;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering;

//using UnityEngine.UIElements;

public enum Modes
{
    Navigation,
    Microscoping
}


public class EgoController : MonoBehaviour
{

    [DllImport("user32.dll")]
    public static extern bool SetCursorPos(int X, int Y);

    //[DllImport ("user32.dll")]
    //public static extern bool GetCursorPos (out Point pos);

    Transform EmbeddedCamera;

    Rigidbody rb;

    const int FurnitureLayer = 11;
    const int WallsLayer = 12;

    public float LinearHorizontalSpeed;
    public float LinearVerticalSpeed;
    public float AngularHorizontalSpeed;
    public float AngularVerticalSpeed;

    const float DefaultFieldOfView = 60F;

    const float MaximumXAngle = 70F;
    const float MinimumXAngle = -70F;

    //  public const float Height = 3.603F;

    float move_vertically;
    float move_horizontally;

    float rotate_vertically;
    float rotate_horizontally;

    [HideInInspector]
    public Modes Mode;

    [HideInInspector]
    public GameObject InstrumentOnFocus;

    //public for Save
    [HideInInspector]
    public float Saved_X_position;

    //public for Save
    [HideInInspector]
    public float Saved_Z_position;

    //public for Save
    [HideInInspector]
    public float Saved_Y_angle;

    //public for Save
    [HideInInspector]
    public float Saved_Camera_X_Angle;

    bool Halt;

    public static bool CursorFrozen;


    // Start is called before the first frame update
    void Start()
    {

        Mode = Modes.Navigation;

        Halt = false;

        CursorFrozen = true;

        EmbeddedCamera = transform.GetChild(0);

        rb = GetComponent<Rigidbody>();

#if !UNITY_EDITOR
        Cursor.visible = false;
#endif

    }


    void OnGUI()
    {
        MouseUI.callOnGUI();
    }


    public void setMode(Modes _Mode)
    { //for navigation

        CursorFrozen = true;

#if !UNITY_EDITOR
        Cursor.visible = false;
#endif

        InstrumentOnFocus = null;
        Mode = _Mode;

        restoreSavedPresence();

    }



    public void setMode(Modes _Mode, GameObject _InstrumentOnFocus, float _X_position,
        float _Z_position, float _Y_angle, float _X_camera_angle, float _FieldOfView)
    {

        CursorFrozen = false;

#if !UNITY_EDITOR
        Cursor.visible = true;
#endif

        if (Mode == Modes.Navigation)
            saveCurrentPresence();

        InstrumentOnFocus = _InstrumentOnFocus;
        Mode = _Mode;

        transform.position = new Vector3(_X_position, transform.position.y, _Z_position);
        transform.eulerAngles = new Vector3(0F, _Y_angle, 0F);
        EmbeddedCamera.localEulerAngles = new Vector3(_X_camera_angle, 0F, 0F);
        EmbeddedCamera.GetComponent<Camera>().fieldOfView = _FieldOfView;

    }  

   

        void saveCurrentPresence()
    {

        Saved_X_position = transform.position.x;
        Saved_Z_position = transform.position.z;

        Saved_Y_angle = transform.eulerAngles.y;

        Saved_Camera_X_Angle = EmbeddedCamera.localEulerAngles.x;

    }



    void restoreSavedPresence()
    {

        transform.position = new Vector3(Saved_X_position, transform.localPosition.y, Saved_Z_position);
        transform.eulerAngles = new Vector3(0F, Saved_Y_angle, 0F);

        EmbeddedCamera.eulerAngles = new Vector3(Saved_Camera_X_Angle, 0F, 0F);

        EmbeddedCamera.GetComponent<Camera>().fieldOfView = DefaultFieldOfView;

    }



    // Update is called once per frame
    void Update()
    {

#if UNITY_EDITOR
        if (Input.GetKeyUp(KeyCode.Space))
            CursorFrozen = !CursorFrozen;
#endif

        if (CursorFrozen)
            //GetCursorPos (out Point CursorPos);
            //print (CursorPos.X + " , " + CursorPos.Y);
            SetCursorPos(Screen.width / 2, Screen.height / 2);


        if (Mode == Modes.Navigation)
        {

            if (!Halt)
            {

                move_horizontally = Input.GetAxis("Horizontal");
                move_vertically = Input.GetAxis("Vertical");
            }

            rotate_horizontally = Input.GetAxis("Mouse X");
            rotate_vertically = Input.GetAxis("Mouse Y");

        }

        if (move_vertically != 0F)
        {

            rb.MovePosition(transform.position + transform.forward * LinearVerticalSpeed * move_vertically * Time.deltaTime);

            move_vertically = 0F;

        }

        if (move_horizontally != 0F)
        {

            rb.MovePosition(transform.position + transform.right * LinearHorizontalSpeed * move_horizontally * Time.deltaTime);

            move_horizontally = 0F;

        }

        if (!MouseUI.Rotating)
        {

            if (rotate_vertically != 0F)
            {

                if (withinVerticalRotationLimits())
                    //we rotate the child Camera, not Ego GameOject
                    EmbeddedCamera.localEulerAngles = new Vector3(EmbeddedCamera.localEulerAngles.x - AngularVerticalSpeed * rotate_vertically * Time.deltaTime, 0F, 0F);

                rotate_vertically = 0F;

            }

            if (rotate_horizontally != 0F)
            {

                //we rotate Ego GameObject, not the child Camera
                transform.localEulerAngles = new Vector3(0F, transform.localEulerAngles.y + AngularHorizontalSpeed * rotate_horizontally * Time.deltaTime, 0F);

                rotate_horizontally = 0F;

            }

        }

        if (Input.GetKeyUp(KeyCode.Escape))
            Application.Quit();

    }


    bool withinVerticalRotationLimits()
    {

        return reduced(EmbeddedCamera.localEulerAngles.x) -
                    AngularVerticalSpeed * (rotate_vertically - 1) >= MinimumXAngle &&
                    reduced(EmbeddedCamera.localEulerAngles.x) -
                    AngularVerticalSpeed * (rotate_vertically + 1) <= MaximumXAngle;

    }


    float reduced(float _Angle)
    {

        float NewAngle = _Angle;

        if (NewAngle > 270F)
            NewAngle -= 360F;

        return NewAngle;

    }


    void OnCollisionEnter(Collision _collision)
    {

        if (_collision.gameObject.layer == FurnitureLayer || _collision.gameObject.layer == WallsLayer)
        {

            Halt = true;

            rb.linearVelocity = Vector3.zero;

            StartCoroutine(unHalting());

        }

    }


    void OnCollisionExit(Collision _collision)
    {

        if (_collision.gameObject.layer == FurnitureLayer || _collision.gameObject.layer == WallsLayer)
        {

            Halt = false;

        }

    }


    IEnumerator unHalting()
    {

        yield return new WaitUntil(() => Input.GetKeyUp(KeyCode.DownArrow)
        || Input.GetKeyUp(KeyCode.UpArrow) || Input.GetKeyUp(KeyCode.LeftArrow) || Input.GetKeyUp(KeyCode.RightArrow) || Input.GetKeyUp(KeyCode.W)
        || Input.GetKeyUp(KeyCode.S) || Input.GetKeyUp(KeyCode.A) || Input.GetKeyUp(KeyCode.D));

        Halt = false;

    }


    public void attach(GameObject _object)
    {
        //RestoreUprightPosition ();
        _object.transform.parent = EmbeddedCamera;
        _object.transform.localPosition = new Vector3(0F, _object.GetComponent<MovableObject>().Y_Offset_for_Carrying, 0.5F);
        _object.GetComponent<MovableObject>().rotateObject_for_Carrying();
    }


    public static Dictionary<Modes, bool> PermittingCollection_of_Objects = new Dictionary<Modes, bool>() {
        {Modes.Navigation,true},
        {Modes.Microscoping,false}
    };

}
*/