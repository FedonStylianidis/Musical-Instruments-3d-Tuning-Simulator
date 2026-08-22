using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif
public class Harpsichord : Instrument {
  
    public HarpsichordDatabase Database;
    public AudioSource DemoAudioSource;   
    private Dictionary<HarpsichordNote, HarpsichordKey> KeysByNote =   // used for the demo melody
      new Dictionary<HarpsichordNote, HarpsichordKey>();
    public GameObject DemoButton;                                  //used for the demo button
    public float DemoButtonVisibleDistance = 10f;

   public HarpsichordKey KeyForButton;    //key used to measure distance for demo button to appear  

    [Header("Tuning Display")]  //reference for the tuning canvas

    public HarpsichordTuningDisplay TuningDisplay;

    [Header("Tuning Camera Movement")]        //the following fields are for the movement of tuning camera across the pins

    public Transform TuningCameraStartPoint;

    public Transform TuningCameraEndPoint;

    public float TuningCameraMoveSpeed = 0.25f;

    private float TuningCameraPosition = 0f;


    #region Editor
#if UNITY_EDITOR

    [CustomEditor(typeof(Harpsichord)), CanEditMultipleObjects]
    public class Harpsichord_Editor : Instrument_Editor
    {

        public override void OnInspectorGUI()
        {

            base.OnInspectorGUI();

            Harpsichord harpsichord = (Harpsichord)target;

           
            harpsichord.ControlPanel = EditorGUILayout.ObjectField("Control Tuning Panel", harpsichord.ControlPanel, typeof(GameObject), true) as GameObject;
            harpsichord.ControlExtraUI = EditorGUILayout.ObjectField("Control Harpsichord UI", harpsichord.ControlExtraUI, typeof(GameObject), true) as GameObject;

            base.showLocation();

        }

    }

#endif
    #endregion


    public override void Start()
    {
        Focus_Mode = Modes.Harpsichord_Tuning;

        /*Focus_PosX_Offset = 0;     //if tuning camera not in use and needs main camera parameters
        Focus_PosZ_Offset = 0;

        Focus_Camera_RotX = 89F;

        Focus_Field_of_View = 45F;
        Focus_Theta = 0F; */

        base.Start();
        buildKeyDictionary();
        Ego = GameObject.Find("Ego");   //inherited from instrument


        if (DemoButton != null)
            DemoButton.SetActive(false);


        if (TuningCameraStartPoint != null &&    //initialization of moving camera position and rotation
             FocusCamera != null)
        {
            FocusCamera.transform.localPosition =
                TuningCameraStartPoint.localPosition;

            FocusCamera.transform.localRotation =
                TuningCameraStartPoint.localRotation;

            TuningCameraPosition = 0f;
        }


    }

     /*public void ShowDemoButton()
     {
         if (DemoButton != null)
             DemoButton.SetActive(true);
     } 

     public void HideDemoButton()
     {
         if (DemoButton != null)
             DemoButton.SetActive(false);
     } */
    void Update()   //needed for demobutton activation and movement of camera 
    {
        if (DemoButton != null &&
            Ego != null &&
            KeyForButton != null)
        {
            float distance =
                Vector3.Distance(
                    Ego.transform.position,
                    KeyForButton.transform.position
                );

            DemoButton.SetActive(
                distance <= DemoButtonVisibleDistance
            );
        }

        moveTuningCamera();
    }

    private void moveTuningCamera()
    {
        if (FocusCamera == null)
            return;

        if (!FocusCamera.enabled)
            return;

        if (TuningCameraStartPoint == null ||
            TuningCameraEndPoint == null)
            return;


        float movement = 0f;

        if (Input.GetKey(KeyCode.DownArrow))
            movement = 1f;

        else if (Input.GetKey(KeyCode.UpArrow))
            movement = -1f;


        if (movement == 0f)
            return;


        TuningCameraPosition +=
            movement *
            TuningCameraMoveSpeed *
            Time.deltaTime;


        TuningCameraPosition =
            Mathf.Clamp01(TuningCameraPosition);


        // Position
        FocusCamera.transform.localPosition =
            Vector3.Lerp(
                TuningCameraStartPoint.localPosition,
                TuningCameraEndPoint.localPosition,
                TuningCameraPosition
            );


        // Rotation
        FocusCamera.transform.localRotation =
            Quaternion.Lerp(
                TuningCameraStartPoint.localRotation,
                TuningCameraEndPoint.localRotation,
                TuningCameraPosition
            );
    }
    private void buildKeyDictionary()     // builds the key dictionary where each  harpsichord key corresponds to a note
    {
        KeysByNote.Clear();

        HarpsichordKey[] keys =
            GetComponentsInChildren<HarpsichordKey>();

        foreach (HarpsichordKey key in keys)
        {
            if (!KeysByNote.ContainsKey(key.Note))
                KeysByNote.Add(key.Note, key);
        }
    }

    public HarpsichordKey GetKey(     //for  the pin of a certain note to find the matching key without assigning it in the inspector
    HarpsichordNote note)
    {
        if (KeysByNote.TryGetValue(
            note,
            out HarpsichordKey key))
        {
            return key;
        }

        return null;
    }

    public void PlayToccataDemo()
    {
        StartCoroutine(playToccataDemo());
    }
    private IEnumerator playToccataDemo()
    {
        yield return PlayDemoNote(HarpsichordNote.A2, 0.2f);
        yield return PlayDemoNote(HarpsichordNote.G2, 0.2f);
        yield return PlayDemoNote(HarpsichordNote.A2, 1.2f);

        yield return PlayDemoNote(HarpsichordNote.G2, 0.2f);
        yield return PlayDemoNote(HarpsichordNote.F2, 0.2f);
        yield return PlayDemoNote(HarpsichordNote.E2, 0.2f);
        yield return PlayDemoNote(HarpsichordNote.D2, 0.2f);
        yield return PlayDemoNote(HarpsichordNote.CSharp2, 0.8f);

        yield return PlayDemoNote(HarpsichordNote.D2, 1.2f);
    }
    private IEnumerator PlayDemoNote(HarpsichordNote note, float duration)
{
    if (!KeysByNote.ContainsKey(note))
    {
        Debug.LogWarning("No key found for " + note);
        yield break;
    }

    HarpsichordKey key = KeysByNote[note];

    key.press();
    key.release(duration);

    yield return new WaitForSeconds(duration);
}
   /* private IEnumerator PlayDemoNote(HarpsichordNote note, float duration)
    {
        if (!KeysByNote.ContainsKey(note))
        {
            Debug.LogWarning("No key found for " + note);
            yield break;
        }

        HarpsichordKey key = KeysByNote[note];

        key.press();

        yield return new WaitForSeconds(duration);

        key.release();
    }   */

    /*  IEnumerator playToccataDemo()
      {
          PlayNote(HarpsichordNote.A2);
          yield return new WaitForSeconds(0.2f);

          PlayNote(HarpsichordNote.G2);
          yield return new WaitForSeconds(0.2f);

          PlayNote(HarpsichordNote.A2);
          yield return new WaitForSeconds(1.2f);

          PlayNote(HarpsichordNote.G2);
          yield return new WaitForSeconds(0.2f);

          PlayNote(HarpsichordNote.F2);
          yield return new WaitForSeconds(0.2f);

          PlayNote(HarpsichordNote.E2);
          yield return new WaitForSeconds(0.2f);

          PlayNote(HarpsichordNote.D2);
          yield return new WaitForSeconds(0.2f);

          PlayNote(HarpsichordNote.CSharp2);
          yield return new WaitForSeconds(0.8f);
          PlayNote(HarpsichordNote.D2);
      }

       public void PlayNote(HarpsichordNote note)
       {
           KeyData data = Database.GetKeyData(note);

           if (data == null || data.Sound == null)
           {
               Debug.LogWarning("Missing sound for " + note);
               return;
           }

           DemoAudioSource.PlayOneShot(data.Sound);
       }  */
}