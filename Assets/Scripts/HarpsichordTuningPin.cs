using UnityEngine;
using System.Threading.Tasks;

public class HarpsichordTuningPin : InteractableObject
{
    [Header("Controlled Note")]

    public HarpsichordNote Note;


    [Header("Hammer Attachment")]

    [Tooltip("Position offset of the hammer relative to the pin.")]
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


    [HideInInspector]      //tuning parameters
    public float TargetFrequency;

    [HideInInspector]
    public float CurrentFrequency;

    [HideInInspector]
    public float CurrentCents;

    public override void Start()
    {
        base.Start();

        harpsichord =
            GetComponentInParent<Harpsichord>();

        TargetFrequency =     //initialization of the tuning frequencies
    calculateFrequency(Note);

        CurrentFrequency =
            TargetFrequency;

        CurrentCents =
            0f;
    }

    private float calculateFrequency(    //method to calculate the correct frequency of the target note
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


        int semitone = 0;


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


    /*public void updateTuning(   //method for the hammer to updae the pin
    float cents,
    bool rotationLimitReached)
    {
        CurrentCents =
            cents;


        CurrentFrequency =
            TargetFrequency *
            Mathf.Pow(
                2f,
                cents / 1200f
            );


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
            rotationLimitReached
        );
    }  */

    public void updateTuning(
    float cents,
    bool rotationLimitReached)
    {
        CurrentCents =
            cents;


        CurrentFrequency =
            TargetFrequency *
            Mathf.Pow(
                2f,
                cents / 1200f
            );


        float pitchRatio =
            CurrentFrequency /
            TargetFrequency;


        HarpsichordKey key =
            getControlledKey();


        if (key != null)
        {
            key.setTuningPitch(
                pitchRatio
            );
        }


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
            rotationLimitReached
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
        if (AttachedHammer == null)
            return;


        HarpsichordKey key =
            getControlledKey();


        if (key == null)
            return;


        if (Input.GetKeyDown(KeyCode.M))
        {
            key.startTuningSound();
        }


        if (Input.GetKeyUp(KeyCode.M))
        {
            key.stopTuningSound();
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


        Transform hammerTransform =
            hammer.transform;


        hammerTransform.SetParent(
            null,
            true
        );


        hammerTransform.position =
            transform.position +
            HammerPositionOffset;


        hammerTransform.rotation =
            hammer.OriginalWorldRotation;


        hammerTransform.localScale =
            hammer.getOriginalScale();


        hammerTransform.SetParent(
            transform,
            true
        );

        updateTuning(  //extra code for tuning initialization and update
    0f,
    false
);
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