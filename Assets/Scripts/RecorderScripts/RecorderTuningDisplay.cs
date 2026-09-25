using UnityEngine;
using TMPro;

public class RecorderTuningDisplay : MonoBehaviour
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


    public void updateDisplay(
        string targetNote,
        float currentFrequency,
        float centsDifference)
    {
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


        if (
            Mathf.Abs(centsDifference)
            <= InTuneToleranceCents)
        {
            StatusText.text =
                "IN TUNE";

            setColor(
                Color.green
            );
        }

        else if (centsDifference < 0f)
        {
            StatusText.text =
                "FLAT";

            setColor(
                Color.red
            );
        }

        else
        {
            StatusText.text =
                "SHARP";

            setColor(
                Color.red
            );
        }
    }


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


        int absoluteNote =
            octave * 12
            + noteIndex;


        absoluteNote +=
            semitoneChange;


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