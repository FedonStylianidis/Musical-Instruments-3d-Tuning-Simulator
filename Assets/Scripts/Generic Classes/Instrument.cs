
using UnityEngine;


public class Instrument : MonoBehaviour
{
    public Camera MainCamera; 
    public Camera FocusCamera;


    public const bool locked = true;
     public const bool unlocked = false;

    public GameObject ControlExtraUI;


  

    public static GameObject Ego;

    [HideInInspector]
    public Modes Focus_Mode;

    [HideInInspector]
    public bool View;

    public virtual void Start()
    {

        Ego = GameObject.Find("Ego");

        View = unlocked;
    }

    public virtual void toggleView()
    {

        if (View == unlocked)
            lockView();
        else
            unlockView();

        View = !View;

        if (ControlExtraUI != null)
            ControlExtraUI.GetComponent<ExtraUI>().setActivationStatus(View);

    }

    public void lockView()
    {
        Ego.GetComponent<EgoController>().setMode(Focus_Mode,gameObject);
        
        MainCamera.enabled = false;
        FocusCamera.enabled = true;

        if (MouseUI.ObjectBeingCarried != null)
        {

            // Attach it to the FocusCamera.
            Ego.GetComponent<EgoController>()
                .attach(
                    MouseUI.ObjectBeingCarried
                );
        }
      
    }
   
    public void unlockView()
    {
        FocusCamera.enabled = false;
        MainCamera.enabled = true;

        if (MouseUI.ObjectBeingCarried != null)   //reattaches the object being carried to the previous camera
        {
            Ego.GetComponent<EgoController>()
               .attach(MouseUI.ObjectBeingCarried);
        }

        Ego.GetComponent<EgoController>().setMode(Modes.Navigation, null);

    }

}
