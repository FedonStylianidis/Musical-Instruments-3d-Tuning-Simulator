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

    public void clearDisplay()
    {
        NoteText.text =
            "___";

        FrequencyText.text =
            "___ Hz";

        StatusText.text =
            "___";

        setColor(
            Color.white
        );
    }
    public void updateDisplay(
        string targetNote,
        float targetFrequency,
        float currentFrequency,
        float centsDifference,
        bool rotationLimitReached)
    {
        /*
        Determine which musical note is
        actually closest to the currently
        sounding frequency.
        */

        string displayedNote =
            getDisplayedNote(
                targetNote,
                centsDifference
            );


        NoteText.text =
            displayedNote;


        FrequencyText.text =
            currentFrequency.ToString("F2")
            + " Hz";


        /*
        Mechanical tuning limits.
        */

        if (rotationLimitReached)
        {
            if (centsDifference < 0f)
            {
                StatusText.text =
                    "TOO FLAT";
            }
            else
            {
                StatusText.text =
                    "TOO SHARP";
            }


            setColor(
                Color.red
            );
        }

        /*
        The target note is correctly tuned.
        */

        else if (
            Mathf.Abs(centsDifference)
            <= InTuneToleranceCents)
        {
            StatusText.text =
                "IN TUNE";


            setColor(
                Color.green
            );
        }

        /*
        The string is below the target
        frequency.
        */

        else if (centsDifference < 0f)
        {
            StatusText.text =
                "FLAT";


            setColor(
                Color.red
            );
        }

        /*
        The string is above the target
        frequency.
        */

        else
        {
            StatusText.text =
                "SHARP";


            setColor(
                Color.red
            );
        }
    }


    private void setColor(
        Color new_color)
    {
        NoteText.color =
            new_color;

        FrequencyText.color =
            new_color;

        StatusText.color =
            new_color;
    }


    /*
    Calculates the note that is closest
    to the currently sounding frequency.

    Every 100 cents corresponds to one
    semitone.

    The boundaries between neighbouring
    notes are therefore at:

    +/- 50 cents
    +/- 150 cents
    +/- 250 cents
    +/- 350 cents
    etc.
    */
    private string getDisplayedNote(
        string targetNote,
        float centsDifference)
    {
        int semitoneChange =
            Mathf.FloorToInt(
                (
                    centsDifference
                    + 50f
                )
                / 100f
            );


        return shiftNote(
            targetNote,
            semitoneChange
        );
    }


    /*
    Moves a note by any number of
    semitones.

    This also correctly handles crossing
    from one octave into another.
    */
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


        for (
            int i = 0;
            i < NoteNames.Length;
            i++)
        {
            if (
                NoteNames[i]
                == pitchName)
            {
                noteIndex =
                    i;

                break;
            }
        }


        /*
        Convert the note into one continuous
        semitone number.

        For example:

        C4 = 48
        C#4 = 49
        B4 = 59
        C5 = 60
        */

        int absoluteNote =
            octave * 12
            + noteIndex;


        absoluteNote +=
            semitoneChange;


        /*
        Convert the continuous semitone
        number back into note + octave.
        */

        int newOctave =
            Mathf.FloorToInt(
                absoluteNote / 12f
            );


        int newNoteIndex =
            absoluteNote
            - newOctave * 12;


        return
            NoteNames[newNoteIndex]
            + newOctave;
    }
}