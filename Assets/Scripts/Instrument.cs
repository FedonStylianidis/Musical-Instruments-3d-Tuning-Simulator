using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.InputSystem.Controls.AxisControl;
using static UnityEngine.Rendering.DebugUI;



#if UNITY_EDITOR
using UnityEditor;
#endif

public class Instrument : MonoBehaviour
{
    public Camera MainCamera; //
    public Camera FocusCamera;//  new camera for the harpsichord, assigned in inspector


    public const bool locked = true;
     public const bool unlocked = false;

    //public const bool ON = true;
    //public const bool OFF = false;

   //public const bool connected = true;
   // public const bool disconnected = false;

    [HideInInspector]
    public GameObject ControlPanel, ControlExtraUI;

    [HideInInspector]
    public GameObject Location;

   // [HideInInspector]
   // public bool State;

   // bool PowerController_State;

   // [HideInInspector]
   // public bool Cable_State; //needs to be public so as to be checked externally

    public static GameObject Ego;

    [HideInInspector]
    public Modes Focus_Mode;

    [HideInInspector]
    public float Focus_PosX_Offset, Focus_PosZ_Offset, Focus_Camera_RotX, Focus_Field_of_View, Focus_Theta;

    float Y_Angle;
    float dx, dz;

    #region Editor
#if UNITY_EDITOR

    [CustomEditor(typeof(Instrument)), CanEditMultipleObjects]
    public class Instrument_Editor : Editor
    {

        public override void OnInspectorGUI()
        {

            base.OnInspectorGUI();

        }

        public void showLocation()
        {

            Instrument instrument = (Instrument)target;

            EditorGUILayout.Space();

            instrument.Location = EditorGUILayout.ObjectField("Location", instrument.Location, typeof(GameObject), true) as GameObject;

        }

    }

#endif    
    #endregion

    [HideInInspector]
    public bool View;

    // Start is called before the first frame update
    public virtual void Start()
    {

        Ego = GameObject.Find("Ego");

        View = unlocked;

        if (Location != null)
            setFocusValues();

    }


    void setFocusValues()
    {

        float Orientation = Location.transform.localEulerAngles.y;

        Y_Angle = Orientation + Focus_Theta;

        if (Orientation == 0F)
        {
            dx = Focus_PosX_Offset;
            dz = Focus_PosZ_Offset;
        }
        else if (Orientation == 90F)
        {
            dx = Focus_PosZ_Offset;
            dz = -Focus_PosX_Offset;
        }
        else if (Orientation == 180F)
        {
            dx = -Focus_PosX_Offset;
            dz = -Focus_PosZ_Offset;
        }
        else
        {
            dx = -Focus_PosZ_Offset;
            dz = Focus_PosX_Offset;
        }

    }


    public void toggleView()
    {

        if (View == unlocked)
            lockView();
        else
            unlockView();

        View = !View;

        if (ControlExtraUI != null)
            ControlExtraUI.GetComponent<ExtraUI>().setActivationStatus(View);

    }


    /* public void lockView()
      {
          Ego.GetComponent<EgoController>().setMode(Focus_Mode, gameObject, transform.position.x + dx, transform.position.z + dz, Y_Angle, Focus_Camera_RotX, Focus_Field_of_View);
      }  
      public virtual void lockView()
      {
          Ego.GetComponent<EgoController>().setMode(
              Focus_Mode,
              gameObject,
              transform.position.x + dx,
              transform.position.y + 50F,
              transform.position.z + dz,
              Y_Angle,
              80F,
              Focus_Field_of_View
          );
      }

       public void lockView()
       {
           Ego.GetComponent<EgoController>().setMode(
               Focus_Mode,
               gameObject,
               Location.transform.position.x,
               Location.transform.position.z,
               Location.transform.eulerAngles.y + Focus_Theta,
               Focus_Camera_RotX,
               Focus_Field_of_View
           ); 
       } */
   /* public  void lockView()
    {
        Ego.GetComponent<EgoController>().setMode(
            Focus_Mode,
            gameObject,
            Location.transform.position.x - 0.5F,
            Location.transform.position.y,
            Location.transform.position.z,
            Location.transform.eulerAngles.y,
            Location.transform.eulerAngles.x,
            Focus_Field_of_View
        );
    }  */
    public void lockView()
    {
        Ego.GetComponent<EgoController>().setMode(Focus_Mode,gameObject,false);
        
        MainCamera.enabled = false;
        FocusCamera.enabled = true;
        if (MouseUI.ObjectBeingCarried != null)    //reattaches the object being carried to the new camera
        {
            Ego.GetComponent<EgoController>()
               .attach(MouseUI.ObjectBeingCarried);
        }
    }
    /*public void unlockView()
    {
        Ego.GetComponent<EgoController>().setMode(Modes.Navigation);
    }
    */

    public void unlockView()
    {
        FocusCamera.enabled = false;
        MainCamera.enabled = true;

        if (MouseUI.ObjectBeingCarried != null)   //reattaches the object being carried to the previous camera
        {
            Ego.GetComponent<EgoController>()
               .attach(MouseUI.ObjectBeingCarried);
        }

        Ego.GetComponent<EgoController>().setMode(Modes.Navigation, null, false);

    }

    /* public void updateFeedback_from_PowerController(bool _New_PowerController_State)
    {

        PowerController_State = _New_PowerController_State;

        updateState();

    } */

    /*public virtual void updateFeedback_from_Cable(bool _New_Cable_State)
    {

        Cable_State = _New_Cable_State;

        updateState();

        if (ControlPanel != null)
            ControlPanel.GetComponent<Panel>().updateZoomability(_New_Cable_State);

    } */



   /* void updateState()
    {

        State = Cable_State && PowerController_State;

        if (ControlLamp != null)
            ControlLamp.GetComponent<Lamp>().updateState(State);

    }  */
    

    //public Vector3 Offset;
    //public float XAngle;
    //public float FoV;


    //public void Update () {
    //    Offset = Ego.transform.localPosition - transform.localPosition;
    //	XAngle = Ego.transform.localEulerAngles.x;
    //	FoV = Ego.GetComponent<Camera> ().fieldOfView;
    //}

}
