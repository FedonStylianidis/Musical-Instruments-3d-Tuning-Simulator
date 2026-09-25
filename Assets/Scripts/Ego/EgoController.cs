
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;


public enum Modes
{
    Navigation,
    Harpsichord_Tuning,
    SnareDrum_Tuning,
    Recorder_Tuning
}

public class EgoController : MonoBehaviour
{

    [DllImport("user32.dll")]
    public static extern bool SetCursorPos(int X, int Y);


    Transform EmbeddedCamera;
    public CharacterController controller;
    public float speed = 12f;
    public float mouseSensitivity = 100f;
    public float gravity = -9.81f * 2;

    public Transform groundCheck;
    public float groundDistance = 0.4f;
    public LayerMask groundMask;

    Vector3 velocity;

    bool isGrounded;

    float xRotation = 0f;
    float YRotation = 0f;


   
    float move_vertically;
    float move_horizontally;

    float rotate_vertically;
    float rotate_horizontally;

    [HideInInspector]
    public Modes Mode;

    [HideInInspector]
    public GameObject InstrumentOnFocus;

    public static bool CursorFrozen;


    [Header("Pause Menu")]
    public GameObject StartMenuCanvas;

    private bool PauseMenuOpen = false;
    private bool CursorWasFrozen;
    private StartMenu startMenu;


    void Start()
    {

        Mode = Modes.Navigation;

        CursorFrozen = true;

        EmbeddedCamera = transform.GetChild(0);


        // Start rotation from the rotation
        // already set in the Unity scene.
        YRotation = transform.eulerAngles.y;

        xRotation = transform.eulerAngles.x;


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


    public void setMode(
    Modes _Mode,
    GameObject _InstrumentOnFocus)
    {
        CursorFrozen =
            (_Mode == Modes.Navigation);

#if !UNITY_EDITOR
    Cursor.visible =
        !CursorFrozen;
#endif

        InstrumentOnFocus =
            _InstrumentOnFocus;

        Mode =
            _Mode;
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

                SetCursorPos(Screen.width / 2, Screen.height / 2);


            if (Mode == Modes.Navigation)
            {
                isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask);

                if (isGrounded && velocity.y < 0)
                {
                    velocity.y = -2f;
                }
              

                move_horizontally = Input.GetAxis("Horizontal");
                move_vertically = Input.GetAxis("Vertical");
            

                rotate_horizontally = Input.GetAxis("Mouse X");
                rotate_vertically = Input.GetAxis("Mouse Y");


                Vector3 move = transform.right * move_horizontally + transform.forward * move_vertically;
                controller.Move(move * speed * Time.deltaTime);

                velocity.y += gravity * Time.deltaTime;

                controller.Move(velocity * Time.deltaTime);
              

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
             

            }

   
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
    
   
    public void attach(GameObject _object)
    {
        Camera activeCamera = getActiveCamera();

        if (activeCamera == null)
        {
            Debug.LogWarning("No active camera found.");
            return;
        }

   
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
      {Modes.Harpsichord_Tuning, true},
      {Modes.SnareDrum_Tuning, true},
      {Modes.Recorder_Tuning, true}
  };

}   

