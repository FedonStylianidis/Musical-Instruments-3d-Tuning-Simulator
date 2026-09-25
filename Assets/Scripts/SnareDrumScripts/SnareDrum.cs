using UnityEngine;
using System.Collections;

public class SnareDrum : Instrument
{
    public SnareDrumLug[] Lugs;
    public SnareDrumSlice[] Slices;
    public Transform TuningCameraPivot;
    public float TuningCameraRotationSpeed = 40f;
    public float MouseWheelRotationSpeed = 40f;

    [Header("Initial Detuning")]

    public float MinimumInitialTuningErrorCents = 10f;
    public float MaximumInitialTuningErrorCents = 25f;

    
    [Header("Tuning Feedback")]
    public GameObject CorrectPitchMessage;
    public GameObject SoundsTunedMessage;

    public GameObject SnareDrumTunedMessage;
    public GameObject WrongTuningMessage;

    public GameObject SnareDrumInstructionMessage;

    [Header("Snare Drum Tuning Instructions")]
    public GameObject SnareDrumTuningInstructions1;
    public GameObject SnareDrumTuningInstructions2;

    private bool FirstSliceHasBeenTuned ;

    private MessageManager MessageManager;

    private SnareTuningAlgorithm TuningAlgorithm;

    [Header("Drumsticks")]
    public GameObject Drumsticks;

    private bool DemoIsPlaying ;

    public override void Start()
    {
        Focus_Mode =
            Modes.SnareDrum_Tuning;

        base.Start();
        Lugs = GetComponentsInChildren<SnareDrumLug>();

        Slices = GetComponentsInChildren<SnareDrumSlice>();

        TuningAlgorithm = GetComponent<SnareTuningAlgorithm>();

        randomizeInitialTuning();

        MessageManager = FindFirstObjectByType<MessageManager>();
       
            CorrectPitchMessage.SetActive(false);
            SoundsTunedMessage.SetActive(false);
            SnareDrumTunedMessage.SetActive(false);
            WrongTuningMessage.SetActive(false);
            SnareDrumTuningInstructions1.SetActive(false);
            SnareDrumTuningInstructions2.SetActive(false);
            SnareDrumInstructionMessage.SetActive(false);
        
        setTuningColliders(false);

    }

    

    void Update()

    {
        if (View == locked &&
    Input.GetKeyDown(KeyCode.F))
        {
                SnareDrumTuningInstructions1.SetActive(
                    !SnareDrumTuningInstructions1.activeSelf
                );
                SnareDrumTuningInstructions2.SetActive(
                    !SnareDrumTuningInstructions2.activeSelf
                    );
        }
        moveTuningCamera();

        if (View == locked &&
    Input.GetKeyDown(KeyCode.P))
        {
            PlaySnareDemo();
        }
    }

    private void setTuningColliders(bool enabled)
    {
        foreach (SnareDrumLug lug in Lugs)
        {
            Collider collider =
                lug.GetComponent<Collider>();

                collider.enabled = enabled;
        }

        foreach (SnareDrumSlice slice in Slices)
        {
            Collider collider =
                slice.GetComponent<Collider>();

                collider.enabled = enabled;
        }
    }
    public override void toggleView()
    {
        // When ENTERING Snare tuning mode:
        // allow the Snare tuning key,
        // but block every other carried object.
        if (!View &&
            MouseUI.ObjectBeingCarried != null &&
            MouseUI.ObjectBeingCarried.GetComponent<
                CarrySnareDrumTuningKey>() == null)
        {
            return;
        }

        base.toggleView();

        setTuningColliders(View);


        // Entering Snare Drum tuning mode.
        if (View)
        {
            // Hide the drumsticks.
    
                Drumsticks.SetActive(false);
            


            // Show the introductory instructions once.
          
                MessageManager.showMessageOnce(
                    SnareDrumInstructionMessage,
                    10f
                );
            
        }

        // Leaving Snare Drum tuning mode.
        else
        {
            // Show the drumsticks again.
        
                Drumsticks.SetActive(true);
            
        }
    }
   

    private void moveTuningCamera()
    {
        // Arrow keys.
        float movement = 0f;

        if (Input.GetKey(KeyCode.LeftArrow))
            movement = 1f;

        else if (Input.GetKey(KeyCode.RightArrow))
            movement = -1f;


        TuningCameraPivot.Rotate(
            0f,
            movement *
            TuningCameraRotationSpeed *
            Time.deltaTime,
            0f
        );


        // Mouse wheel.
        float mouseWheel =
            Input.GetAxis("Mouse ScrollWheel");

        TuningCameraPivot.Rotate(
            0f,
            mouseWheel *
            MouseWheelRotationSpeed,
            0f
        );
    }

    private void randomizeInitialTuning()
    {
        foreach (SnareDrumSlice slice in Slices)
        {
            // Choose a random tuning error
            // between 10 and 25 cents.
            float cents =
                Random.Range(
                    MinimumInitialTuningErrorCents,
                    MaximumInitialTuningErrorCents
                );


            // Randomly make the slice
            // either sharp or flat.
            if (Random.value < 0.5f)
            {
                cents = -cents;
            }


            // Convert cents to frequency ratio.
            float pitchRatio =
                Mathf.Pow(
                    2f,
                    cents / 1200f
                );


            // For a membrane:
            //
            // f / f0 = sqrt(T / T0)
            //
            // Therefore:
            //
            // T / T0 = (f / f0)^2
            float tensionRatio =
                pitchRatio *
                pitchRatio;


            slice.CurrentTension =
                slice.TargetTension *
                tensionRatio;


            slice.CurrentTension =
                Mathf.Clamp(
                    slice.CurrentTension,
                    slice.MinimumTension,
                    slice.MaximumTension
                );

            slice.updatePitch();

        }
    }
    public void showTunedMessage()
    {
     

        // The first successfully tuned slice
        // receives the reference-pitch message.
        if (!FirstSliceHasBeenTuned)
        {
            FirstSliceHasBeenTuned = true;

           
                MessageManager.showMessage(
                    CorrectPitchMessage,
                    5f
                );
            

            return;
        }


        // Every later successfully tuned slice
        // receives the shorter feedback message.
      
            MessageManager.showMessage(
                SoundsTunedMessage,
                5f
            );
        
    }

    public void showSnareDrumTunedMessage()
    {
        MessageManager.showMessage(
            SnareDrumTunedMessage,
            5f
        );
    }


    public void showWrongTuningMessage()
    {

        MessageManager.showMessage(
            WrongTuningMessage,
            5f
        );
    }
    public void registerTunedLug(
    int lugNumber)
    {
        // Register the lug that has just
        // entered the correct tuning range.
        TuningAlgorithm.registerTunedLug(
            lugNumber
        );


        // If all 10 lugs have now been tuned
        // according to the algorithm, the
        // tuning exercise is complete.
        if (TuningAlgorithm.isComplete())
        {
            showSnareDrumTunedMessage();

            FindFirstObjectByType<AllInstrumentsTunedManager>()
        .snareDrumCompleted();
            
        }

    }

    public bool isValidNextLug(
    int lugNumber)
    {
        return TuningAlgorithm.isValidNextLug(
            lugNumber
        );
    }

    public void resetTuningExercise()
    {
        // Forget all previously tuned lugs
        // and restart the tuning sequence.
      
            TuningAlgorithm.resetAlgorithm();
        


        // The next successfully tuned lug
        // becomes the new reference lug.
        FirstSliceHasBeenTuned = false;


        // Give all slices new random
        // starting tuning errors.
        randomizeInitialTuning();
    }

    public void PlaySnareDemo()
    {
        if (DemoIsPlaying)
            return;

        StartCoroutine(playSnareDemo());
    }


    private IEnumerator playSnareDemo()
    {
        DemoIsPlaying = true;

        yield return PlayDemoSlice(Slices[0], 1f);
        yield return new WaitForSeconds(0.2f);
        yield return PlayDemoSlice(Slices[1], 0.2f);
        yield return PlayDemoSlice(Slices[1], 0.2f);
        yield return PlayDemoSlice(Slices[2], 1.2f);
        yield return PlayDemoSlice(Slices[3], 0.4f);
       // yield return PlayDemoSlice(Slices[4], 0.2f);
        yield return PlayDemoSlice(Slices[5], 0.2f);
        yield return PlayDemoSlice(Slices[6], 0.2f);
        yield return PlayDemoSlice(Slices[7], 0.8f);
        yield return PlayDemoSlice(Slices[8], 1.2f);

        DemoIsPlaying = false;
    }


    private IEnumerator PlayDemoSlice(
        SnareDrumSlice slice,
        float duration)
    {
        slice.playSound();

        yield return new WaitForSeconds(
            duration
        );

        slice.stopSound();
    }

} 