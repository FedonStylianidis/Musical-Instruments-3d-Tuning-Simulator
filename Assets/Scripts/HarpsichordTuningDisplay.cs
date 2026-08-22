/*using UnityEngine;
using TMPro;

public class HarpsichordTuningDisplay : MonoBehaviour
{
    [Header("Text Fields")]

    public TMP_Text NoteText;
    public TMP_Text FrequencyText;
    public TMP_Text StatusText;


    [Header("Tuning Tolerance")]

    public float InTuneToleranceCents = 5f;


    public void updateDisplay(
        string noteName,
        float targetFrequency,
        float currentFrequency,
        float centsDifference,
        bool rotationLimitReached)
    {
        NoteText.text =
            noteName;

        FrequencyText.text =
            currentFrequency.ToString("F2") +
            " Hz";


        if (rotationLimitReached)
        {
            FrequencyText.color =
                Color.red;

            StatusText.color =
                Color.red;

            StatusText.text =
                "ROTATION LIMIT";

            return;
        }


        if (Mathf.Abs(centsDifference) <=
            InTuneToleranceCents)
        {
            FrequencyText.color =
                Color.green;

            StatusText.color =
                Color.green;

            StatusText.text =
                "IN TUNE";
        }
        else
        {
            FrequencyText.color =
                Color.red;

            StatusText.color =
                Color.red;


            if (centsDifference < 0f)
                StatusText.text = "FLAT";
            else
                StatusText.text = "SHARP";
        }
    }
} */


/*using UnityEngine;
using TMPro;

public class HarpsichordTuningDisplay : MonoBehaviour
{
    [Header("Text Fields")]

    public TMP_Text NoteText;
    public TMP_Text FrequencyText;
    public TMP_Text StatusText;


    [Header("Tuning Tolerance")]

    [Tooltip("Maximum deviation in cents that is considered in tune.")]
    public float InTuneToleranceCents = 5f;


    [Header("Displayed Note Change")]

    [Tooltip(
        "When the deviation passes this value, " +
        "the displayed note changes to the neighbouring note."
    )]
    public float NoteChangeThresholdCents = 50f;


    private readonly string[] NoteNames =
    {
        "C",
        "C#",
        "D",
        "D#",
        "E",
        "F",
        "F#",
        "G",
        "G#",
        "A",
        "A#",
        "B"
    };


    public void updateDisplay(
        string targetNote,
        float targetFrequency,
        float currentFrequency,
        float centsDifference,
        bool rotationLimitReached)
    {
        string displayedNote =
            getDisplayedNote(
                targetNote,
                centsDifference
            );


        NoteText.text =
            displayedNote;


        FrequencyText.text =
            currentFrequency.ToString("F2") +
            " Hz";


        // Hammer has reached one of its
        // mechanical rotation limits.
        if (rotationLimitReached)
        {
            setRed();

            StatusText.text =
                "ROTATION LIMIT";

            return;
        }


        // Correct tuning range.
        if (Mathf.Abs(centsDifference) <=
            InTuneToleranceCents)
        {
            setGreen();

            StatusText.text =
                "IN TUNE";

            return;
        }


        // Any incorrect tuning is shown in red.
        setRed();


        if (centsDifference < 0f)
        {
            StatusText.text =
                "FLAT";
        }
        else
        {
            StatusText.text =
                "SHARP";
        }
    }


    private void setGreen()
    {
        Color32 green =
            new Color32(
                0,
                255,
                0,
                255
            );


        NoteText.color =
            green;

        FrequencyText.color =
            green;

        StatusText.color =
            green;
    }


    private void setRed()
    {
        Color32 red =
            new Color32(
                255,
                0,
                0,
                255
            );


        NoteText.color =
            red;

        FrequencyText.color =
            red;

        StatusText.color =
            red;
    }


    private string getDisplayedNote(
        string targetNote,
        float centsDifference)
    {
        // Still closer to the intended note.
        if (Mathf.Abs(centsDifference) <
            NoteChangeThresholdCents)
        {
            return targetNote;
        }


        // More than +50 cents:
        // display the next chromatic note.
        if (centsDifference >=
            NoteChangeThresholdCents)
        {
            return shiftNote(
                targetNote,
                1
            );
        }


        // More than -50 cents:
        // display the previous chromatic note.
        return shiftNote(
            targetNote,
            -1
        );
    }


    private string shiftNote(
        string noteName,
        int semitoneChange)
    {
        int octave =
            int.Parse(
                noteName.Substring(
                    noteName.Length - 1
                )
            );


        string pitchName =
            noteName.Substring(
                0,
                noteName.Length - 1
            );


        int noteIndex =
            0;


        for (int i = 0;
             i < NoteNames.Length;
             i++)
        {
            if (NoteNames[i] ==
                pitchName)
            {
                noteIndex =
                    i;

                break;
            }
        }


        int newIndex =
            noteIndex +
            semitoneChange;


        if (newIndex < 0)
        {
            newIndex =
                11;

            octave--;
        }


        else if (newIndex > 11)
        {
            newIndex =
                0;

            octave++;
        }


        return NoteNames[newIndex] +
               octave;
    }
} */
/*using UnityEngine;
using TMPro;

public class HarpsichordTuningDisplay : MonoBehaviour
{
    [Header("Text Fields")]

    public TMP_Text NoteText;
    public TMP_Text FrequencyText;
    public TMP_Text StatusText;


    [Header("Tuning Tolerance")]

    public float InTuneToleranceCents = 5f;


    [Header("Displayed Note Change")]

    public float NoteChangeThresholdCents = 50f;


    private readonly string[] NoteNames =
    {
        "C",
        "C#",
        "D",
        "D#",
        "E",
        "F",
        "F#",
        "G",
        "G#",
        "A",
        "A#",
        "B"
    };


    /* void Start()
     {
         // Keep the TMP component itself white.
         NoteText.color = Color.white;
         FrequencyText.color = Color.white;
         StatusText.color = Color.white;

     }
    void Start()
    {
        NoteText.text = "TEST NOTE";
        FrequencyText.text = "123.45 Hz";
        StatusText.text = "TEST";

        NoteText.color = Color.green;
        FrequencyText.color = Color.red;
        StatusText.color = Color.red;
    }


    public void updateDisplay(
        string targetNote,
        float targetFrequency,
        float currentFrequency,
        float centsDifference,
        bool rotationLimitReached)
    {
        string displayedNote =
            getDisplayedNote(
                targetNote,
                centsDifference
            );


        string frequency =
            currentFrequency.ToString("F2") +
            " Hz";


        if (rotationLimitReached)
        {
            showRed(
                displayedNote,
                frequency,
                "ROTATION LIMIT"
            );

            return;
        }


        if (Mathf.Abs(centsDifference) <=
            InTuneToleranceCents)
        {
            showGreen(
                displayedNote,
                frequency,
                "IN TUNE"
            );

            return;
        }


        if (centsDifference < 0f)
        {
            showRed(
                displayedNote,
                frequency,
                "FLAT"
            );
        }
        else
        {
            showRed(
                displayedNote,
                frequency,
                "SHARP"
            );
        }
    }


    private void showGreen(
        string note,
        string frequency,
        string status)
    {
        NoteText.text =
            "<color=#00FF00>" +
            note +
            "</color>";

        FrequencyText.text =
            "<color=#00FF00>" +
            frequency +
            "</color>";

        StatusText.text =
            "<color=#00FF00>" +
            status +
            "</color>";
    }


    private void showRed(
        string note,
        string frequency,
        string status)
    {
        NoteText.text =
            "<color=#FF0000>" +
            note +
            "</color>";

        FrequencyText.text =
            "<color=#FF0000>" +
            frequency +
            "</color>";

        StatusText.text =
            "<color=#FF0000>" +
            status +
            "</color>";


        Debug.Log(
            "RED DISPLAY | " +
            note +
            " | " +
            frequency +
            " | " +
            status
        );
    }


    private string getDisplayedNote(
        string targetNote,
        float centsDifference)
    {
        if (Mathf.Abs(centsDifference) <
            NoteChangeThresholdCents)
        {
            return targetNote;
        }


        if (centsDifference >=
            NoteChangeThresholdCents)
        {
            return shiftNote(
                targetNote,
                1
            );
        }


        return shiftNote(
            targetNote,
            -1
        );
    }


    private string shiftNote(
        string noteName,
        int semitoneChange)
    {
        int octave =
            int.Parse(
                noteName.Substring(
                    noteName.Length - 1
                )
            );


        string pitchName =
            noteName.Substring(
                0,
                noteName.Length - 1
            );


        int noteIndex = 0;


        for (int i = 0;
             i < NoteNames.Length;
             i++)
        {
            if (NoteNames[i] ==
                pitchName)
            {
                noteIndex = i;
                break;
            }
        }


        int newIndex =
            noteIndex +
            semitoneChange;


        if (newIndex < 0)
        {
            newIndex = 11;
            octave--;
        }
        else if (newIndex > 11)
        {
            newIndex = 0;
            octave++;
        }


        return NoteNames[newIndex] +
               octave;
    }
}*/
using UnityEngine;
using TMPro;

public class HarpsichordTuningDisplay : MonoBehaviour
{
    [Header("Text Fields")]

    public TMP_Text NoteText;
    public TMP_Text FrequencyText;
    public TMP_Text StatusText;


    [Header("Tuning Tolerance")]

    public float InTuneToleranceCents = 5f;


    [Header("Displayed Note Change")]

    public float NoteChangeThresholdCents = 50f;


    private readonly string[] NoteNames =
    {
        "C",
        "C#",
        "D",
        "D#",
        "E",
        "F",
        "F#",
        "G",
        "G#",
        "A",
        "A#",
        "B"
    };


    public void updateDisplay(
        string targetNote,
        float targetFrequency,
        float currentFrequency,
        float centsDifference,
        bool rotationLimitReached)
    {
        string displayedNote =
            getDisplayedNote(
                targetNote,
                centsDifference
            );


        NoteText.text =
            displayedNote;


        FrequencyText.text =
            currentFrequency.ToString("F2") +
            " Hz";


        if (rotationLimitReached)
        {
            StatusText.text =
                "ROTATION LIMIT";
        }
        else if (
            Mathf.Abs(centsDifference) <=
            InTuneToleranceCents)
        {
            StatusText.text =
                "IN TUNE";
        }
        else if (centsDifference < 0f)
        {
            StatusText.text =
                "FLAT";
        }
        else
        {
            StatusText.text =
                "SHARP";
        }


        setGreen();
    }


    private void setGreen()
    {
        NoteText.color =
            Color.green;

        FrequencyText.color =
            Color.green;

        StatusText.color =
            Color.green;
    }


    private string getDisplayedNote(
        string targetNote,
        float centsDifference)
    {
        if (Mathf.Abs(centsDifference) <
            NoteChangeThresholdCents)
        {
            return targetNote;
        }


        if (centsDifference >=
            NoteChangeThresholdCents)
        {
            return shiftNote(
                targetNote,
                1
            );
        }


        return shiftNote(
            targetNote,
            -1
        );
    }


    private string shiftNote(
        string noteName,
        int semitoneChange)
    {
        int octave =
            int.Parse(
                noteName.Substring(
                    noteName.Length - 1
                )
            );


        string pitchName =
            noteName.Substring(
                0,
                noteName.Length - 1
            );


        int noteIndex = 0;


        for (int i = 0;
             i < NoteNames.Length;
             i++)
        {
            if (NoteNames[i] ==
                pitchName)
            {
                noteIndex = i;
                break;
            }
        }


        int newIndex =
            noteIndex +
            semitoneChange;


        if (newIndex < 0)
        {
            newIndex = 11;
            octave--;
        }
        else if (newIndex > 11)
        {
            newIndex = 0;
            octave++;
        }


        return NoteNames[newIndex] +
               octave;
    }
}