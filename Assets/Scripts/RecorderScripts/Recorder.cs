using UnityEngine;
using System.Collections;
using JetBrains.Annotations;

public class Recorder : Instrument
{
    

    public GameObject RecorderPickupMessage;
    public GameObject RecorderInstructionMessage;
    public GameObject RecorderTuningMessage;
    public GameObject RecorderTunedMessage;
    public MessageManager MessageManager;

    private bool WasBeingCarried ;

    [Header("Recorder Holes")]
    public GameObject[] Holes = new GameObject[10];

    [Header("Recorder Database")]
    public RecorderDatabase Database;

    private RecorderDatabase.RecorderNoteData CurrentlyPlayingNote;

    private bool NoteIsBeingHeld;

    [Header("Recorder Audio")]
    public AudioSource RecorderAudioSource;

    public float ReleaseFadeDuration = 0.15f;

    private Coroutine fadeCoroutine;

    [Header("Tuning Pose")]
    public Vector3 TuningLocalPosition = new Vector3(-0.14f, -0.035f, 0.42f);
    public Vector3 TuningLocalRotation=new Vector3(-182.59f, 348.8f, -495.3f);
    public Vector3 TuningLocalScale = new Vector3(2f,2f,2f);

    public GameObject RecorderTuningMeterCanvas;

    [Header("Tuning Display")]
    public RecorderTuningDisplay TuningDisplay;

    [Header("Recorder Fingering Chart")]
    public GameObject RecorderFingeringChart;

    [Header("Recorder View")]
    public Vector3 FrontRotation =
     new Vector3(182.59f, 348.8f, -495.3f);
    public Vector3 BackRotation;


    [Header("Recorder Tuning")]
    public Transform HeadJoint;
    private Vector3 HeadJointOriginalLocalPosition;
    public float HeadJointMaxPull = 0.008f;

    public float RandomTuningMaxPull = 0.006f;
    private float TunedHeadJointPull;
    private float StartingHeadJointPull;

    public float MinimumStartingDetuning = 0.002f;

    public float OriginalEffectiveLength = 0.35f;

    public float HeadJointMouseSensitivity = 0.0001f;

    public float HeadJointPitchSensitivity = 1.5f;

    private bool DemoIsPlaying ;



    public override void Start()
    {
        Focus_Mode = Modes.Recorder_Tuning;

        base.Start();
        HeadJointOriginalLocalPosition =
     HeadJoint.localPosition;


        TunedHeadJointPull =
            Random.Range(
                0f,
                RandomTuningMaxPull
            );
        do
        {
            StartingHeadJointPull =
                Random.Range(
                    0f,
                    RandomTuningMaxPull
                );
        }
        while (
            Mathf.Abs(
                StartingHeadJointPull -
                TunedHeadJointPull
            ) < MinimumStartingDetuning
        );

        Vector3 startingPosition =
            HeadJointOriginalLocalPosition;

        startingPosition.x +=
            StartingHeadJointPull;

        HeadJoint.localPosition =
            startingPosition; 

            RecorderPickupMessage.SetActive(false);
            RecorderInstructionMessage.SetActive(false);
            RecorderTuningMessage.SetActive(false);
            RecorderTunedMessage.SetActive(false);
            RecorderFingeringChart.SetActive(false);
      
            TuningDisplay.clearDisplay();
      
        setAllHoleColliders(false);
    }
      

    void Update()
    {
        bool IsBeingCarried =
            MouseUI.ObjectBeingCarried == gameObject;


        if (IsBeingCarried &&
            !WasBeingCarried)
        {
            MessageManager.showMessageOnce(
                RecorderPickupMessage,
                5f
            );
        }


        if ((IsBeingCarried || View == locked) &&
            Input.GetMouseButtonDown(1))
        {
            toggleTuningView();
        }


        if ((IsBeingCarried || View == locked) &&
            Input.GetKeyDown(KeyCode.P))
        {
            PlayRecorderDemo();
        }


        if (View == locked)
        {
            handleTuningInput();
        }


        WasBeingCarried =
            IsBeingCarried;
    }



    private void toggleTuningView()
    {
        FocusCamera.transform.position =
            MainCamera.transform.position;

        FocusCamera.transform.rotation =
            MainCamera.transform.rotation;


        toggleView();


        if (View == locked)
        {
            hideFrontHoleColliders(false);

            TuningDisplay.clearDisplay();

            MessageManager.showMessageOnce(
                RecorderInstructionMessage,
                10f
            );


            transform.localPosition =
                TuningLocalPosition;

            transform.localEulerAngles =
                TuningLocalRotation;

            transform.localScale =
                TuningLocalScale;


            MouseUI.ObjectBeingCarried =
                null;

            MouseUI.switchCursor(
                MouseUI.Wedge
            );


            GetComponent<MovableObject>().enabled =
                false;

            RecorderTuningMeterCanvas.SetActive(
                true
            );
        }
        else
        {
            resetAllHoles();

            setAllHoleColliders(false);


            GetComponent<MovableObject>().enabled =
                true;

            MouseUI.ObjectBeingCarried =
                gameObject;


            Ego.GetComponent<EgoController>()
                .attach(gameObject);

            GetComponent<MovableObject>()
                .setCarryingPose();


            MouseUI.hideCursor();

            RecorderTuningMeterCanvas.SetActive(
                false
            );

            RecorderFingeringChart.SetActive(
                false
            );
        }
    }

    private void handleTuningInput()
    {
        if (Input.GetKeyDown(KeyCode.F))
        {
            RecorderFingeringChart.SetActive(
                !RecorderFingeringChart.activeSelf
            );
        }


        if (Input.GetKeyDown(KeyCode.M))
        {
            playCurrentNote();
        }


        if (NoteIsBeingHeld)
        {
            updatePlayingNotePitch();
            updateTuningDisplay();
        }


        if (Input.GetKeyUp(KeyCode.M))
        {
            stopCurrentNote();
        }


        float mouseWheel =
            Input.GetAxis("Mouse ScrollWheel");


        if (Input.GetKeyDown(KeyCode.DownArrow) ||
            mouseWheel < 0f)
        {
            transform.localEulerAngles =
                BackRotation;

            hideFrontHoleColliders(true);
        }


        if (Input.GetKeyDown(KeyCode.UpArrow) ||
            mouseWheel > 0f)
        {
            transform.localEulerAngles =
                FrontRotation;

            hideFrontHoleColliders(false);
        }


        float HeadJointMovement =
            0.00001f;


        if (Input.GetKey(KeyCode.LeftArrow))
        {
            moveHeadJoint(
                HeadJointMovement
            );
        }


        if (Input.GetKey(KeyCode.RightArrow))
        {
            moveHeadJoint(
                -HeadJointMovement
            );
        }


        if (Input.GetMouseButton(0) &&
            NoteIsBeingHeld)
        {
            float mouseMovement =
                -Input.GetAxis("Mouse X") *
                HeadJointMouseSensitivity;

            moveHeadJoint(
                mouseMovement
            );
        }
    }

    private void moveHeadJoint(float movement)
    {
        Vector3 position =
            HeadJoint.localPosition;

        position.x +=
            movement;

        position.x =
            Mathf.Clamp(
                position.x,
                HeadJointOriginalLocalPosition.x,
                HeadJointOriginalLocalPosition.x +
                HeadJointMaxPull
            );

        HeadJoint.localPosition =
            position;
    }
    private void setAllHoleColliders(bool enabled)
    {
        for (int i = 0; i < Holes.Length; i++)
        {
            Holes[i]
                .GetComponent<Collider>()
                .enabled = enabled;
        }
    }

    private void hideFrontHoleColliders(bool hide)
    {
        for (int i = 0; i < Holes.Length; i++)
        {
            Collider holeCollider =
                Holes[i].GetComponent<Collider>();

          
                if (i == 9)
                {
                    // Hole 10 is the back/thumb hole.
                    holeCollider.enabled = hide;
                }
                else
                {
                    // Holes 1-9 are the front holes.
                    holeCollider.enabled = !hide;
                }
            
        }
    }

    // applies visual fingering when the demo plays a note
    private void applyFingering(
    RecorderDatabase.Fingering fingering)
    {
        for (int i = 0;
             i < Holes.Length;
             i++)
        {
            RecorderHole hole =
                Holes[i].GetComponent<RecorderHole>();

            if (hole == null)
                continue;


            int holeNumber =
                i + 1;


            RecorderHoleState newState =
                RecorderHoleState.Open;


            for (int j = 0;
                 j < fingering.CoveredHoles.Length;
                 j++)
            {
                if (fingering.CoveredHoles[j] ==
                    holeNumber)
                {
                    newState =
                        RecorderHoleState.Covered;

                    break;
                }
            }


            if (holeNumber == 10 &&
                fingering.BackHoleHalfCovered)
            {
                newState =
                    RecorderHoleState.HalfCovered;
            }


            hole.setState(
                newState
            );
        }
    }
    private void resetAllHoles()
    {
        for (int i = 0; i < Holes.Length; i++)
        {
            RecorderHole hole =
                Holes[i].GetComponent<RecorderHole>();
           
                hole.resetHole();
           
        }
    }
    // Gets note from state of holes
    public RecorderDatabase.RecorderNoteData getCurrentNote()
    {
        RecorderHole[] recorderHoles =
            new RecorderHole[Holes.Length];

        for (int i = 0; i < Holes.Length; i++)
        {
            recorderHoles[i] =
                Holes[i].GetComponent<RecorderHole>();
        }

        return Database.getNoteForFingering(
            recorderHoles
        );
    }

    public void playCurrentNote()
    {
        RecorderDatabase.RecorderNoteData note =
            getCurrentNote();

        if (note == null)
        {
            RecorderAudioSource.Stop();

            return;
        }

        if (note.Sound == null)
        {
            return;
        }

        CurrentlyPlayingNote = note;

        NoteIsBeingHeld = true;

      
            MessageManager.showMessageOnce(
                RecorderTuningMessage,
                5f
            );
        

        float tunedFrequency =
            getTunedFrequency(
                note.Frequency
            );

        if (fadeCoroutine != null)
        {
            StopCoroutine(fadeCoroutine);

            fadeCoroutine = null;
        }

        RecorderAudioSource.volume = 1f;

        RecorderAudioSource.clip =
            note.Sound;

        RecorderAudioSource.pitch =
            tunedFrequency /
            note.Frequency;

        RecorderAudioSource.Play();

        if (View == locked && TuningDisplay != null)
        {
            updateTuningDisplay();
        }

    }
    public void stopCurrentNote()
    {
        NoteIsBeingHeld = false;

        FadeOutAndStop(
            ReleaseFadeDuration
        );
    }
    private void FadeOutAndStop(
    float fadeDuration)
    {
        if (fadeCoroutine != null)
        {
            StopCoroutine(
                fadeCoroutine
            );
        }

        fadeCoroutine =
            StartCoroutine(
                fadeOutAndStop(
                    fadeDuration
                )
            );
    }

    private IEnumerator fadeOutAndStop(
    float fadeDuration)
    {
        float startVolume =
            RecorderAudioSource.volume;

        float timer =
            0f;

        while (timer < fadeDuration)
        {
            timer +=
                Time.deltaTime;

            float t =
                timer /
                fadeDuration;

            RecorderAudioSource.volume =
                startVolume *
                Mathf.Sqrt(
                    1f - t
                );

            yield return null;
        }

        RecorderAudioSource.Stop();

        RecorderAudioSource.volume =
            startVolume;

        fadeCoroutine =
            null;
    }


    private void updatePlayingNotePitch()
    {
        if (CurrentlyPlayingNote == null)
            return;

        if (!RecorderAudioSource.isPlaying)
            return;

        float tunedFrequency =
            getTunedFrequency(
                CurrentlyPlayingNote.Frequency
            );

        RecorderAudioSource.pitch =
            tunedFrequency /
            CurrentlyPlayingNote.Frequency;
    }
    private void updateTuningDisplay()
    {
        RecorderDatabase.RecorderNoteData note =
            getCurrentNote();

        if (note == null)
        {
            TuningDisplay.clearDisplay();

            return;
        }


        float targetFrequency =
            note.Frequency;


        float currentFrequency =
            getTunedFrequency(
                targetFrequency
            );


        float centsDifference =
            1200f *
            Mathf.Log(
                currentFrequency /
                targetFrequency,
                2f
            );


        TuningDisplay.updateDisplay(
            note.Note.ToString(),
         
            currentFrequency,
            centsDifference
        );
        if (Mathf.Abs(centsDifference) <= 5f)
        {
       
                MessageManager.showMessageOnce(
                    RecorderTunedMessage,
                    3f
                );

                FindFirstObjectByType<AllInstrumentsTunedManager>().recorderCompleted();

            
        }
    }

   //Physics Implementation
    public float getTunedFrequency(float originalFrequency)
    {
        // Calculate the current pull of the Head Joint
        // from its original fully pushed-in position.
        float currentPull =
            HeadJoint.localPosition.x -
            HeadJointOriginalLocalPosition.x;

        // Limit the displacement between the fully pushed-in
        // position and the maximum allowed pull.
        currentPull = Mathf.Clamp(
            currentPull,
            0f,
            HeadJointMaxPull
        );

        /*// Effective air-column length at the randomly selected
        // correctly tuned Head Joint position:
        // L_tuned = L_0 + ΔL_tuned
        float tunedEffectiveLength =
            OriginalEffectiveLength +
            TunedHeadJointPull;

        // Effective air-column length at the current
        // Head Joint position:
        // L_current = L_0 + ΔL_current
        float currentEffectiveLength =
            OriginalEffectiveLength +
            currentPull; */


        // Effective air-column length at the randomly selected
        // correctly tuned Head Joint position.
        float tunedEffectiveLength =
            OriginalEffectiveLength +
            TunedHeadJointPull;


        // Calculate how far the current position is from
        // the correctly tuned Head Joint position.
        float pullDifference =
            currentPull -
            TunedHeadJointPull;


        // Increase the acoustic effect of the physical movement
        // for clearer tuning feedback in the simulation.
        float effectivePullDifference =
            pullDifference *
            HeadJointPitchSensitivity;


        // Effective air-column length at the current position.
        // At the tuned position, effectivePullDifference is zero.
        float currentEffectiveLength =
            tunedEffectiveLength +
            effectivePullDifference;

        // For an ideal air column:
        // f ∝ 1 / L
        //
        // Therefore, relative to the correctly tuned position:
        // f_current = f_0 * L_tuned / L_current
        //
        // If currentPull == TunedHeadJointPull,
        // L_current == L_tuned and f_current == f_0.
        //
        // Pulling farther than the tuned position increases
        // the effective length and makes the note flatter.
        //
        // Pushing inward from the tuned position decreases
        // the effective length and makes the note sharper.
        return originalFrequency *
               tunedEffectiveLength /
               currentEffectiveLength;
    }

    public void PlayRecorderDemo()
    {
        if (DemoIsPlaying)
            return;

        StartCoroutine(
            playRecorderDemo()
        );
    }


    private IEnumerator playRecorderDemo()
    {
        DemoIsPlaying = true;

        yield return PlayDemoNote(
            RecorderNote.A4,
            0.2f
        );

        yield return PlayDemoNote(
            RecorderNote.G4,
            0.2f
        );

        yield return PlayDemoNote(
            RecorderNote.A4,
            1.2f
        );

        yield return PlayDemoNote(
            RecorderNote.G4,
            0.2f
        );

        yield return PlayDemoNote(
            RecorderNote.F4,
            0.2f
        );

        yield return PlayDemoNote(
            RecorderNote.E4,
            0.2f
        );

        yield return PlayDemoNote(
            RecorderNote.D4,
            0.2f
        );

        yield return PlayDemoNote(
            RecorderNote.CSharp4,
            0.8f
        );

        yield return PlayDemoNote(
            RecorderNote.D4,
            1.2f
        );


        resetAllHoles();
        DemoIsPlaying = false;
    }

    private IEnumerator PlayDemoNote(
    RecorderNote note,
    float duration)
    {
        RecorderDatabase.RecorderNoteData noteData =
            Database.getNoteData(note);

        if (noteData == null)
        {

            yield break;
        }


        if (noteData.Sound == null)
        {

            yield break;
        }
        if (noteData.Fingerings.Length > 0)
        {
            applyFingering(
                noteData.Fingerings[0]
            );
        }

        float tunedFrequency =
            getTunedFrequency(
                noteData.Frequency
            );

        if (RecorderTuningMeterCanvas.activeSelf)
        {
            float centsDifference =
                1200f *
                Mathf.Log(
                    tunedFrequency /
                    noteData.Frequency,
                    2f
                );

            TuningDisplay.updateDisplay(
                noteData.Note.ToString(),
             
                tunedFrequency,
                centsDifference
            );
        }
        if (fadeCoroutine != null)
        {
            StopCoroutine(
                fadeCoroutine
            );

            fadeCoroutine = null;
        }


        RecorderAudioSource.volume =
            1f;

        RecorderAudioSource.clip =
            noteData.Sound;

        RecorderAudioSource.pitch =
            tunedFrequency /
            noteData.Frequency;

        RecorderAudioSource.Play();


        float playDuration =
   Mathf.Max(
       0f,
       duration - ReleaseFadeDuration
   );

        yield return new WaitForSeconds(
            playDuration
        );

        FadeOutAndStop(
            ReleaseFadeDuration
        );

        yield return new WaitForSeconds(
            ReleaseFadeDuration
        ); 
    }

}