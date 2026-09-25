using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

public class Harpsichord : Instrument
{
    public HarpsichordDatabase Database;
    public AudioSource DemoAudioSource;

    private Dictionary<HarpsichordNote, HarpsichordKey> KeysByNote =
        new Dictionary<HarpsichordNote, HarpsichordKey>();

    private MessageManager MessageManager;

    [Header("Player Distance")]

    public Transform PlayerNearReferencePoint;
    public float PlayerNearDistance = 2.0f;

    private bool PlayerIsNear = false;

    [Header("Demo")]

    public GameObject PlayDemoMessage;

    public float DemoMessageDuration = 2.5f;
    public GameObject HarpsichordOutOfTuneMessage;
    public float OutOfTuneMessageDuration = 3f;

   
    private bool DemoIsPlaying = false;

    [Header("Tuning Camera Instruction")]
    public GameObject TuningCameraInstructionMessage;
    public float TuningCameraInstructionDuration = 3f;

  


    [Header("Hammer Instruction")]
    public GameObject HammerInstructionMessage;
    public float HammerInstructionDuration = 10f;



    [Header("Tuning Display")]
    public HarpsichordTuningDisplay TuningDisplay;


    [Header("Tuning Camera Movement")]

    public Transform TuningCameraStartPoint;

    public Transform TuningCameraEndPoint;

    public float TuningCameraMoveSpeed = 0.25f;

    public float MouseWheelMoveSpeed = 0.1f;

    private float TuningCameraPosition = 0f;


    [Header("Tuning Physics")]

    [Tooltip(
        "Default effective fractional tension " +
        "change per degree of tuning pin rotation."
    )]
    public float DefaultTuningSensitivity = 0.001f;


    [Header("Random Initial Tuning")]
    public bool RandomizeInitialTuning = true;

    public int MinimumRandomOutOfTuneStrings = 10;
    public int MaximumRandomOutOfTuneStrings = 20;

    public float MinimumInitialTuningErrorCents = 20f;
    public float MaximumInitialTuningErrorCents = 200f;

    [Header("Tuning Completion")]
    public float CorrectTuningToleranceCents = 5f;
    public GameObject HarpsichordTunedMessage;

    private bool HarpsichordHasBeenTuned = false;
    #region Editor
#if UNITY_EDITOR

    [CustomEditor(typeof(Harpsichord)), CanEditMultipleObjects]
    public class Harpsichord_Editor : Instrument_Editor
    {
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            Harpsichord harpsichord =
                (Harpsichord)target;

            harpsichord.ControlPanel =
                EditorGUILayout.ObjectField(
                    "Control Tuning Panel",
                    harpsichord.ControlPanel,
                    typeof(GameObject),
                    true
                ) as GameObject;

            harpsichord.ControlExtraUI =
                EditorGUILayout.ObjectField(
                    "Control Harpsichord UI",
                    harpsichord.ControlExtraUI,
                    typeof(GameObject),
                    true
                ) as GameObject;

            base.showLocation();
        }
    }

#endif
    #endregion
    public HarpsichordTuningPin[] TuningPins;

    public override void Start()
    {
        Focus_Mode = Modes.Harpsichord_Tuning;

        /*Focus_PosX_Offset = 0;
        Focus_PosZ_Offset = 0;

        Focus_Camera_RotX = 89F;

        Focus_Field_of_View = 45F;
        Focus_Theta = 0F; */

        base.Start();

        buildKeyDictionary();

        TuningPins = GetComponentsInChildren<HarpsichordTuningPin>();

        Ego = GameObject.Find("Ego");

        MessageManager = FindFirstObjectByType<MessageManager>();


        if (TuningCameraStartPoint != null &&
            FocusCamera != null)
        {
            FocusCamera.transform.localPosition =
                TuningCameraStartPoint.localPosition;

            FocusCamera.transform.localRotation =
                TuningCameraStartPoint.localRotation;

            TuningCameraPosition = 0f;
        }

        if (PlayDemoMessage != null)
        {
            PlayDemoMessage.SetActive(false);
        }
        if (HarpsichordTunedMessage != null)
        {
            HarpsichordTunedMessage.SetActive(false);
        }
        if (HarpsichordOutOfTuneMessage != null)
        {
            HarpsichordOutOfTuneMessage.SetActive(false);
        }
        if (TuningCameraInstructionMessage != null)
        {
            TuningCameraInstructionMessage.SetActive(false);
        }
        if (HammerInstructionMessage != null)
        {
            HammerInstructionMessage.SetActive(false);
        }


        // Coroutine that ensures the randomization of the tuning happens after the initilization of the pins
        StartCoroutine(randomizeInitialTuningAfterStart());

        setTuningPinColliders(false);
    }

    private void setTuningPinColliders(bool enabled)
    {
        foreach (HarpsichordTuningPin pin in TuningPins)
        {
            Collider collider =
                pin.GetComponent<Collider>();

            if (collider != null)
                collider.enabled = enabled;
        }
    }

    public override void toggleView()
    {
        // Do not enter harpsichord tuning mode
     // while carrying the snare tuning key.
        if (!View &&
            MouseUI.ObjectBeingCarried != null &&
            MouseUI.ObjectBeingCarried.GetComponent<
                CarrySnareDrumTuningKey>() != null)
        {
            return;
        }

        base.toggleView();

        setTuningPinColliders(View);
    }
    private IEnumerator randomizeInitialTuningAfterStart()
    {
        yield return null;

        randomizeInitialTuning();
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


    void Update()
    {
        if (Ego != null &&
            PlayerNearReferencePoint != null)
        {
            float distance =
                Vector3.Distance(
                    Ego.transform.position,
                    PlayerNearReferencePoint.position
                );


            PlayerIsNear =
                distance <= PlayerNearDistance;
        }
        else
        {
            PlayerIsNear = false;
        }


        if (PlayDemoMessage != null)
        {
            if (PlayerIsNear &&
                  MessageManager != null)
            {
                MessageManager.showMessageOnce(
                    PlayDemoMessage,
                    DemoMessageDuration
                );
            }

            if (MessageManager != null)
            {
                if (HarpsichordHasBeenTuned &&
                    PlayerIsNear)
                {
                    MessageManager.showMessage(
                        HarpsichordTunedMessage
                    );
                }
                else
                {
                    MessageManager.hideMessage(
                        HarpsichordTunedMessage
                    );
                }
            }

            if (PlayerIsNear &&
                Input.GetKeyDown(KeyCode.P))
            {
                PlayToccataDemo();
            }
        }


        moveTuningCamera();
    }



    public void showTuningCameraInstruction()
    {
        if (MessageManager != null)
        {
            if (MouseUI.ObjectBeingCarried != null &&
        MouseUI.ObjectBeingCarried.GetComponent<
            CarrySnareDrumTuningKey>() != null)
            {
                return;
            }

            MessageManager.showMessageOnce(
                TuningCameraInstructionMessage,
                TuningCameraInstructionDuration
            );
        }
    }

  public void showHammerInstruction()
{

    if (MessageManager != null)
    {
        MessageManager.showMessageOnce(
            HammerInstructionMessage,
            HammerInstructionDuration
        );
    }
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


        // Arrow keys.
        float movement = 0f;

        if (Input.GetKey(KeyCode.DownArrow))
            movement = 1f;

        else if (Input.GetKey(KeyCode.UpArrow))
            movement = -1f;


        TuningCameraPosition +=
            movement *
            TuningCameraMoveSpeed *
            Time.deltaTime;


        // Mouse wheel.
        float mouseWheel =
            Input.GetAxis("Mouse ScrollWheel");

        TuningCameraPosition +=
           - mouseWheel *
            MouseWheelMoveSpeed;


        TuningCameraPosition =
            Mathf.Clamp01(TuningCameraPosition);


        FocusCamera.transform.localPosition =
            Vector3.Lerp(
                TuningCameraStartPoint.localPosition,
                TuningCameraEndPoint.localPosition,
                TuningCameraPosition
            );


        FocusCamera.transform.localRotation =
            Quaternion.Lerp(
                TuningCameraStartPoint.localRotation,
                TuningCameraEndPoint.localRotation,
                TuningCameraPosition
            );
    }



    private void buildKeyDictionary()
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


    public HarpsichordKey GetKey(
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
        if (DemoIsPlaying)
            return;

        StartCoroutine(
            playToccataDemo()
        );
    }


    private IEnumerator playToccataDemo()
    {
        DemoIsPlaying = true;

      
        yield return PlayDemoNote(
            HarpsichordNote.A2,
            0.2f
        );

        yield return PlayDemoNote(
            HarpsichordNote.G2,
            0.2f
        );

        yield return PlayDemoNote(
            HarpsichordNote.A2,
            1.2f
        );

        yield return PlayDemoNote(
            HarpsichordNote.G2,
            0.2f
        );

        yield return PlayDemoNote(
            HarpsichordNote.F2,
            0.2f
        );

        yield return PlayDemoNote(
            HarpsichordNote.E2,
            0.2f
        );

        yield return PlayDemoNote(
            HarpsichordNote.D2,
            0.2f
        );

        yield return PlayDemoNote(
            HarpsichordNote.CSharp2,
            0.8f
        );

        yield return PlayDemoNote(
            HarpsichordNote.D2,
            1.2f
        );

        if (MessageManager != null)
        {
            MessageManager.showMessageOnce(
                HarpsichordOutOfTuneMessage,
                OutOfTuneMessageDuration
            );
        }

        DemoIsPlaying = false;
    }

    public void showKeyOnTuner(
    HarpsichordKey key)
    {
        if (key == null)
            return;


        // Only show the tuner when the
        // harpsichord tuning UI is active.
        if (ControlExtraUI == null ||
            !ControlExtraUI.activeInHierarchy)
        {
            return;
        }


        if (TuningDisplay == null)
            return;


        KeyData data =
            Database.GetKeyData(
                key.Note
            );


        if (data == null)
            return;


        float targetFrequency =
            data.TargetFrequency;


        /*
        The key's current tuning pitch already
        contains the effect of tuning its string.
        */
        float currentFrequency =
            targetFrequency *
            key.getTuningPitch();


        float centsDifference =
            1200f *
            Mathf.Log(
                currentFrequency /
                targetFrequency,
                2f
            );


        TuningDisplay.updateDisplay(
            data.DisplayName,
            targetFrequency,
            currentFrequency,
            centsDifference,
            false
        );
    }
    private IEnumerator PlayDemoNote(
        HarpsichordNote note,
        float duration)
    {
        if (!KeysByNote.ContainsKey(note))
        {
            Debug.LogWarning(
                "No key found for " + note
            );

            yield break;
        }


        HarpsichordKey key =
            KeysByNote[note];


        key.press();

        key.release(duration);


        yield return new WaitForSeconds(
            duration
        );
    }


    public bool areAllStringsCorrectlyTuned()
    {
        HarpsichordTuningPin[] pins =
            GetComponentsInChildren<HarpsichordTuningPin>();


        if (pins.Length == 0)
            return false;


        foreach (HarpsichordTuningPin pin in pins)
        {
            if (Mathf.Abs(pin.CurrentCents) >
                CorrectTuningToleranceCents)
            {
                return false;
            }
        }


        return true;
    }

    public void checkTuningCompletion()
    {
        // Already completed.
        if (HarpsichordHasBeenTuned)
            return;


        // Not all strings are correctly tuned yet.
        if (!areAllStringsCorrectlyTuned())
            return;


        // The harpsichord is now completely tuned.
        HarpsichordHasBeenTuned = true;


        // Show the completion message.
        if (MessageManager != null &&
            HarpsichordTunedMessage != null)
        {
            MessageManager.showMessage(
                HarpsichordTunedMessage,
                5f
            );
        }


        // Tell the final manager that
        // the harpsichord is complete.
        AllInstrumentsTunedManager manager =
            FindFirstObjectByType<
                AllInstrumentsTunedManager>();

        if (manager != null)
        {
            manager.harpsichordCompleted();
        }
    }
    /*public void checkTuningCompletion()
    {
        if (HarpsichordHasBeenTuned)

            MessageManager.showMessage(
           HarpsichordTunedMessage,
           5f
       );


        return;


        if (!areAllStringsCorrectlyTuned())
            return;


        HarpsichordHasBeenTuned = true;

        Debug.Log(
            "Harpsichord tuned correctly to equal temperament."
        );
    }*/

    private void randomizeInitialTuning()
    {
        if (!RandomizeInitialTuning)
            return;


        HarpsichordTuningPin[] pins =
            GetComponentsInChildren<HarpsichordTuningPin>();


        if (pins.Length == 0)
            return;


        /*
        First set every tunable string
        to its correct tuning.
        */
        foreach (HarpsichordTuningPin pin in pins)
        {
            pin.setInitialTuning(0f);
        }


        int minimum =
            Mathf.Clamp(
                MinimumRandomOutOfTuneStrings,
                3,
                pins.Length
            );


        int maximum =
            Mathf.Clamp(
                MaximumRandomOutOfTuneStrings,
                minimum,
                pins.Length
            );


        int numberOfMistunedStrings =
            Random.Range(
                minimum,
                maximum + 1
            );


        /*
        The six different notes that occur
        in the demonstration melody.
        */
        List<HarpsichordNote> demoNotes =
            new List<HarpsichordNote>()
            {
                HarpsichordNote.A2,
                HarpsichordNote.G2,
                HarpsichordNote.F2,
                HarpsichordNote.E2,
                HarpsichordNote.D2,
                HarpsichordNote.CSharp2
            };


        /*
        Shuffle the demo-note list.
        The first three will be deliberately
        placed out of tune.
        */
        for (int i = 0; i < demoNotes.Count; i++)
        {
            int randomIndex =
                Random.Range(
                    i,
                    demoNotes.Count
                );


            HarpsichordNote temporary =
                demoNotes[i];


            demoNotes[i] =
                demoNotes[randomIndex];


            demoNotes[randomIndex] =
                temporary;
        }


        List<HarpsichordTuningPin> selectedPins =
            new List<HarpsichordTuningPin>();


        /*
        Find the tuning pins belonging to
        the first three randomized demo notes.
        */
        for (int i = 0; i < 3; i++)
        {
            foreach (
                HarpsichordTuningPin pin
                in pins)
            {
                if (pin.Note == demoNotes[i])
                {
                    selectedPins.Add(pin);

                    break;
                }
            }
        }


        /*
        Build a list containing all remaining
        pins that have not already been selected.
        */
        List<HarpsichordTuningPin> remainingPins =
            new List<HarpsichordTuningPin>();


        foreach (HarpsichordTuningPin pin in pins)
        {
            if (!selectedPins.Contains(pin))
            {
                remainingPins.Add(pin);
            }
        }


        /*
        Shuffle the remaining tuning pins.
        */
        for (
            int i = 0;
            i < remainingPins.Count;
            i++)
        {
            int randomIndex =
                Random.Range(
                    i,
                    remainingPins.Count
                );


            HarpsichordTuningPin temporary =
                remainingPins[i];


            remainingPins[i] =
                remainingPins[randomIndex];


            remainingPins[randomIndex] =
                temporary;
        }


        /*
        We already selected three demo notes.

        Add enough other strings to reach the
        randomly selected total of 10-20.
        */
        int additionalStringsNeeded =
            numberOfMistunedStrings -
            selectedPins.Count;


        for (
            int i = 0;
            i < additionalStringsNeeded;
            i++)
        {
            selectedPins.Add(
                remainingPins[i]
            );
        }


        /*
        Give every selected string a random
        tuning error.

        Each selected string is either:
            20-200 cents flat
        or:
            20-200 cents sharp.
        */
        foreach (
            HarpsichordTuningPin pin
            in selectedPins)
        {
            bool makeSharp =
                Random.value >= 0.5f;


            float tuningError =
                Random.Range(
                    MinimumInitialTuningErrorCents,
                    MaximumInitialTuningErrorCents
                );


            if (!makeSharp)
            {
                tuningError =
                    -tuningError;
            }


            pin.setInitialTuning(
                tuningError
            );


            Debug.Log(
                "Initial tuning: "
                + pin.Note
                + " = "
                + tuningError.ToString("F1")
                + " cents"
            );
        }


        /*
        Show which three notes of the demo
        melody were deliberately detuned.
        */
        Debug.Log(
            "Detuned demo notes: "
            + demoNotes[0]
            + ", "
            + demoNotes[1]
            + ", "
            + demoNotes[2]
        );


        Debug.Log(
            "Random tuning exercise created with "
            + numberOfMistunedStrings
            + " mistuned strings."
        );
    }

}