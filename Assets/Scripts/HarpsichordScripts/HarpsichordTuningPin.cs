
using UnityEngine;
using System.Threading.Tasks;

public class HarpsichordTuningPin : InteractableObject
{
    [Header("Controlled Note")]

    public HarpsichordNote Note;


    [Header("Hammer Attachment")]

    [Tooltip(
        "Position offset of the hammer relative to the pin."
    )]
    public Vector3 HammerPositionOffset =
        new Vector3(
            0.00016785f,
            0.00177228f,
           -0.00051975f
        );


    [HideInInspector]
    public CarryHarpsichordTuningHammer AttachedHammer;


    private Harpsichord harpsichord;

    private HarpsichordKey ControlledKey;


    [HideInInspector]
    public float TargetFrequency;

    [HideInInspector]
    public float CurrentFrequency;

    [HideInInspector]
    public float CurrentCents;


    [Header("Tuning Physics")]

    [Tooltip(
        "If false, this pin uses the common " +
        "Harpsichord tuning sensitivity."
    )]
    public bool UseCustomTuningSensitivity =
        false;


    [Tooltip(
        "Used only when " +
        "Use Custom Tuning Sensitivity is enabled."
    )]
    public float CustomTuningSensitivity =
        0.001f;


    /*
    True while the tuning note is being
    played with the M key.

    The tuner display is updated only
    while this value is true.
    */
    private bool TuningSoundIsPlaying =
        false;


    /*
    Remembers whether the hammer has reached
    one of its allowed rotation limits.

    The hammer already calculates this and
    sends it to updateTuningFromAngle().
    */
    private bool RotationLimitReached =
        false;


    public float getTuningSensitivity()
    {
        if (UseCustomTuningSensitivity)
        {
            return CustomTuningSensitivity;
        }


        if (harpsichord != null)
        {
            return
                harpsichord
                    .DefaultTuningSensitivity;
        }


        return 0.001f;
    }


    public override void Start()
    {
        base.Start();


        harpsichord =
            GetComponentInParent<Harpsichord>();


        TargetFrequency =
            calculateFrequency(Note);


        CurrentFrequency =
            TargetFrequency;


        CurrentCents =
            0f;


        TuningSoundIsPlaying =
            false;


        RotationLimitReached =
            false;
    }


    private float calculateFrequency(
        HarpsichordNote note)
    {
        string noteText =
            note.ToString();


        int octave =
            int.Parse(
                noteText.Substring(
                    noteText.Length - 1,
                    1
                )
            );


        string pitchName =
            noteText.Substring(
                0,
                noteText.Length - 1
            );


        int semitone =
            0;


        switch (pitchName)
        {
            case "C":
                semitone = 0;
                break;

            case "CSharp":
                semitone = 1;
                break;

            case "D":
                semitone = 2;
                break;

            case "DSharp":
                semitone = 3;
                break;

            case "E":
                semitone = 4;
                break;

            case "F":
                semitone = 5;
                break;

            case "FSharp":
                semitone = 6;
                break;

            case "G":
                semitone = 7;
                break;

            case "GSharp":
                semitone = 8;
                break;

            case "A":
                semitone = 9;
                break;

            case "ASharp":
                semitone = 10;
                break;

            case "B":
                semitone = 11;
                break;
        }


        int midiNote =
            12 * (octave + 1) +
            semitone;


        return 440f *
            Mathf.Pow(
                2f,
                (midiNote - 69) / 12f
            );
    }


  
    // method to set the initial detuning of the strings
    public void setInitialTuning(float cents)
    {
        CurrentCents =
            cents;


        float frequencyRatio =
            Mathf.Pow(
                2f,
                CurrentCents / 1200f
            );


        CurrentFrequency =
            TargetFrequency *
            frequencyRatio;


        RotationLimitReached =
            false;


        HarpsichordKey key =
            getControlledKey();


        if (key != null)
        {
            key.setTuningPitch(
                frequencyRatio
            );
        }
    }

    //method to calculate the angle of the pin for the detuning, 0 corresponts to the correct position
    public float getCurrentTuningAngle()
    {
        float tuningSensitivity =
            getTuningSensitivity();


        float frequencyRatio =
            CurrentFrequency /
            TargetFrequency;


        float tensionRatio =
            frequencyRatio *
            frequencyRatio;


        float angle =
            (tensionRatio - 1f) /
            tuningSensitivity;


        return angle;
    }

    /*
    PHYSICAL TUNING METHOD.

    Called by the tuning hammer whenever
    its angle changes.

    This changes the real simulated string
    frequency and its sound pitch.

    It does NOT directly update the tuner
    display.
    */
    public void updateTuningFromAngle(
        float angle,
        bool rotationLimitReached)
    {
        /*
        Remember the limit state supplied
        by the tuning hammer.
        */

        RotationLimitReached =
            rotationLimitReached;


        float tuningSensitivity =
            getTuningSensitivity();


        /*
        Simplified tension model:

            T / T0 =
                1 + sensitivity * angle
        */

        float tensionRatio =
            1f +
            tuningSensitivity *
            angle;


        /*
        Tension must remain positive.
        */

        tensionRatio =
            Mathf.Max(
                tensionRatio,
                0.01f
            );


        /*
        String frequency law:

            f / f0 =
                sqrt(T / T0)
        */

        float frequencyRatio =
            Mathf.Sqrt(
                tensionRatio
            );


        CurrentFrequency =
            TargetFrequency *
            frequencyRatio;


        /*
        Calculate cents from the resulting
        physical frequency.
        */

        CurrentCents =
            1200f *
            Mathf.Log(
                CurrentFrequency /
                TargetFrequency,
                2f
            );


        /*
        Change the actual sound pitch.

        If the sound is currently playing,
        changing AudioSource.pitch causes
        the heard frequency to change
        immediately.
        */

        HarpsichordKey key =
            getControlledKey();


        if (key != null)
        {
            key.setTuningPitch(
                frequencyRatio
            );
        }


        /*
        There is deliberately NO tuner
        display update here.

        Turning the hammer changes the
        physical state of the string, but
        the tuner only detects that state
        while the string is sounding.
        */
        if (harpsichord != null)
        {
            harpsichord.checkTuningCompletion();
        }
    }


    /*
    Represents the tuner measuring the
    currently sounding string.
    */
    private void updateTuningDisplay()
    {
        if (harpsichord == null)
            return;


        if (harpsichord.TuningDisplay == null)
            return;


        string displayedNote =
            Note.ToString().Replace(
                "Sharp",
                "#"
            );


        harpsichord.TuningDisplay.updateDisplay(
            displayedNote,
            TargetFrequency,
            CurrentFrequency,
            CurrentCents,
            RotationLimitReached
        );
    }


    private HarpsichordKey getControlledKey()
    {
        if (ControlledKey != null)
            return ControlledKey;


        if (harpsichord == null)
        {
            harpsichord =
                GetComponentInParent<Harpsichord>();
        }


        if (harpsichord == null)
            return null;


        ControlledKey =
            harpsichord.GetKey(Note);


        return ControlledKey;
    }


    void Update()
    {
        /*
       The M key is available only while
      the harpsichord is in tuning mode.
            */

        if (harpsichord == null ||
            harpsichord.View != Instrument.locked)
        {
            return;
        }
        /*
        The tuning sound is available only
        while a hammer is attached to this pin.
        */

        if (AttachedHammer == null)
            return;


        HarpsichordKey key =
            getControlledKey();


        if (key == null)
            return;


        /*
        M PRESSED

        Start playing the string at its
        current simulated frequency.
        */

        if (Input.GetKeyDown(KeyCode.M))
        {
            key.startTuningSound();

            TuningSoundIsPlaying =
                true;
        }


        /*
        M HELD

        While the note is sounding, the
        tuner continuously "listens".

        Therefore, if the hammer is rotated
        while M is held, both the sound pitch
        and the displayed frequency change
        continuously.
        */

        if (TuningSoundIsPlaying)
        {
            updateTuningDisplay();
        }


        /*
        M RELEASED

        Stop/fade the note and stop updating
        the tuner.

        The last detected frequency therefore
        remains visible.
        */

        if (Input.GetKeyUp(KeyCode.M))
        {
            key.stopTuningSound();

            TuningSoundIsPlaying =
                false;
        }
    }


    public override string getTooltipName()
    {
        string displayedNote =
            Note.ToString().Replace(
                "Sharp",
                "#"
            );


        return MouseUI.AttributedName(
            Labels.HarpsichordRegulatorPin
        )
        + " "
        + displayedNote;
    }


    public override async Task<Values_After_JointUse>
        use_with(GameObject _OtherObject)
    {
        Values_After_JointUse result =
            new Values_After_JointUse(false);


        CarryHarpsichordTuningHammer hammer =
            _OtherObject.GetComponent<
                CarryHarpsichordTuningHammer>();


        if (hammer == null)
            return result;


        if (AttachedHammer != null)
            return result;


        attachHammer(hammer);


        result =
            new Values_After_JointUse(
                true,
                gameObject,
                true,
                null,
                true
            );


        return result;
    }


    public void attachHammer(
        CarryHarpsichordTuningHammer hammer)
    {
        if (hammer == null)
            return;


        if (AttachedHammer != null)
            return;


        AttachedHammer =
            hammer;


        hammer.attachToPin(this);

       //prepares hammer for the right angle when attached 
       RotateHarpsichordTuningHammer rotateHammer =
    hammer.GetComponent<RotateHarpsichordTuningHammer>();

        if (rotateHammer != null)
        {
            rotateHammer.prepareForPin(this);
        }

        Transform hammerTransform =
            hammer.transform;


        /*
        Temporarily remove the hammer from
        its previous parent, such as the camera.
        */

        hammerTransform.SetParent(
            null,
            true
        );


        /*
        Position is determined by the
        calibrated pin offset.
        */

        hammerTransform.position =
            transform.position +
            HammerPositionOffset;


        /*
        Attachment rotation is independent
        of the hammer's resting rotation
        on the tool stand.
        */

        hammerTransform.rotation =
            hammer.getPinAttachmentRotation();


        /*
        Attachment scale is independent
        of the hammer's resting scale.
        */

        hammerTransform.localScale =
            hammer.getPinAttachmentScale();


        /*
        Parent the hammer to the tuning pin
        while preserving its world transform.
        */

        hammerTransform.SetParent(
            transform,
            true
        );


        /*
        Do NOT update the tuner here.

        Merely attaching the hammer should
        not give the player a frequency
        measurement.
        */

        if (harpsichord != null)
        {
            harpsichord.showHammerInstruction();
        }
    }


    public override void evacuate(
        GameObject _Object)
    {
        CarryHarpsichordTuningHammer hammer =
            _Object.GetComponent<
                CarryHarpsichordTuningHammer>();


        if (hammer == null)
            return;


        if (AttachedHammer != hammer)
            return;


        HarpsichordKey key =
            getControlledKey();


        if (key != null)
        {
            key.stopTuningSound();
        }


        /*
        The tuner must stop listening when
        the hammer is detached.
        */

        TuningSoundIsPlaying =
            false;


        hammer.transform.SetParent(
            null,
            true
        );


        AttachedHammer =
            null;


        hammer.detachFromPin();


        MouseUI mouseUI =
            hammer.GetComponent<MouseUI>();


        if (mouseUI != null)
        {
            mouseUI.Place =
                null;
        }
    }
}