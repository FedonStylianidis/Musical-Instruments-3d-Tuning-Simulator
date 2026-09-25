using UnityEngine;

public enum RecorderNote
{
    C4,
    CSharp4,
    D4,
    DSharp4,
    E4,
    F4,
    FSharp4,
    G4,
    GSharp4,
    A4,
    ASharp4,
    B4,

    C5,
    CSharp5,
    D5,
    DSharp5,
    E5,
    F5,
    FSharp5,
    G5,
    GSharp5,
    A5,
    ASharp5,
    B5,

    C6,
    CSharp6,
    D6
}

[CreateAssetMenu(
    fileName = "RecorderDatabase",
    menuName = "Musical Instruments/Recorder Database"
)]
public class RecorderDatabase : ScriptableObject
{
    [System.Serializable]
    public class Fingering
    {
        public int[] CoveredHoles;

        public bool BackHoleHalfCovered;

        public Fingering(
            int[] coveredHoles,
            bool backHoleHalfCovered = false)
        {
            CoveredHoles = coveredHoles;
            BackHoleHalfCovered = backHoleHalfCovered;
        }
    }


    [System.Serializable]
    public class RecorderNoteData
    {
        public RecorderNote Note;

        public float Frequency;

        public Fingering[] Fingerings;

        [HideInInspector]
        public AudioClip Sound;

        public RecorderNoteData(
            RecorderNote note,
            float frequency,
            Fingering[] fingerings)
        {
            Note = note;
            Frequency = frequency;
            Fingerings = fingerings;
        }
    }


    public RecorderNoteData[] Notes;

    public RecorderNoteData getNoteData(
    RecorderNote note)
    {
        for (int i = 0;
             i < Notes.Length;
             i++)
        {
            if (Notes[i].Note == note)
            {
                return Notes[i];
            }
        }

        return null;
    }

    [Header("Recorder Sounds")]
    public AudioClip[] Sounds = new AudioClip[27];

    private void OnEnable()
    {
        Notes = new RecorderNoteData[]
        {
            // C4
            new RecorderNoteData(
                RecorderNote.C4,
                261.63f,
                new Fingering[]
                {
                    new Fingering(
                        new int[] {1,2,3,4,5,6,7,8,9,10})
                }),

            // C#4
            new RecorderNoteData(
                RecorderNote.CSharp4,
                277.18f,
                new Fingering[]
                {
                    new Fingering(
                        new int[] {1,2,3,4,5,6,7,8,10})
                }),

            // D4
            new RecorderNoteData(
                RecorderNote.D4,
                293.66f,
                new Fingering[]
                {
                    new Fingering(
                        new int[] {1,2,3,4,5,6,8,10})
                }),

            // D#4
            new RecorderNoteData(
                RecorderNote.DSharp4,
                311.13f,
                new Fingering[]
                {
                    new Fingering(
                        new int[] {1,2,3,4,5,6,10})
                }),

            // E4
            new RecorderNoteData(
                RecorderNote.E4,
                329.63f,
                new Fingering[]
                {
                    new Fingering(
                        new int[] {1,2,3,4,5,10})
                }),

            // F4
            new RecorderNoteData(
                RecorderNote.F4,
                349.23f,
                new Fingering[]
                {
                    new Fingering(
                        new int[] {1,2,3,4,6,7,8,9,10})
                }),

            // F#4
            new RecorderNoteData(
                RecorderNote.FSharp4,
                369.99f,
                new Fingering[]
                {
                    new Fingering(
                        new int[] {1,2,3,5,6,8,10})
                }),

            // G4
            new RecorderNoteData(
                RecorderNote.G4,
                392.00f,
                new Fingering[]
                {
                    new Fingering(
                        new int[] {1,2,3,10})
                }),

            // G#4
            new RecorderNoteData(
                RecorderNote.GSharp4,
                415.30f,
                new Fingering[]
                {
                    new Fingering(
                        new int[] {1,2,4,5,6,8,10})
                }),

            // A4
            new RecorderNoteData(
                RecorderNote.A4,
                440.00f,
                new Fingering[]
                {
                    new Fingering(
                        new int[] {1,2,10})
                }),

            // A#4
            new RecorderNoteData(
                RecorderNote.ASharp4,
                466.16f,
                new Fingering[]
                {
                    new Fingering(
                        new int[] {1,3,4,10}),

                    new Fingering(
                        new int[] {2,3,4,10})
                }),

            // B4
            new RecorderNoteData(
                RecorderNote.B4,
                493.88f,
                new Fingering[]
                {
                    new Fingering(
                        new int[] {1,10}),

                    new Fingering(
                        new int[] {2,3,10})
                }),

            // C5
            new RecorderNoteData(
                RecorderNote.C5,
                523.25f,
                new Fingering[]
                {
                    new Fingering(
                        new int[] {2,10})
                }),

            // C#5
            new RecorderNoteData(
                RecorderNote.CSharp5,
                554.37f,
                new Fingering[]
                {
                    new Fingering(
                        new int[] {1,2}),

                    new Fingering(
                        new int[] {1,3,4})
                }),

            // D5
            new RecorderNoteData(
                RecorderNote.D5,
                587.33f,
                new Fingering[]
                {
                    new Fingering(
                        new int[] {2})
                }),

            // D#5
            new RecorderNoteData(
                RecorderNote.DSharp5,
                622.25f,
                new Fingering[]
                {
                    new Fingering(
                        new int[] {2,3,4,5,6,8})
                }),

            // E5
            new RecorderNoteData(
                RecorderNote.E5,
                659.26f,
                new Fingering[]
                {
                    // Hole 10 half-covered
                    new Fingering(
                        new int[] {1,2,3,4,5},
                        true),

                    // Alternate fingering
                    new Fingering(
                        new int[] {2,3,4,5})
                }),

            // F5
            new RecorderNoteData(
                RecorderNote.F5,
                698.46f,
                new Fingering[]
                {
                    new Fingering(
                        new int[] {1,2,3,4,6,8},
                        true)
                }),

            // F#5
            new RecorderNoteData(
                RecorderNote.FSharp5,
                739.99f,
                new Fingering[]
                {
                    new Fingering(
                        new int[] {1,2,3,5,7,9},
                        true),

                    new Fingering(
                        new int[] {1,2,3,6},
                        true)
                }),

            // G5
            new RecorderNoteData(
                RecorderNote.G5,
                783.99f,
                new Fingering[]
                {
                    new Fingering(
                        new int[] {1,2,3},
                        true)
                }),

            // G#5
            new RecorderNoteData(
                RecorderNote.GSharp5,
                830.61f,
                new Fingering[]
                {
                    new Fingering(
                        new int[] {1,2,4},
                        true)
                }),

            // A5
            new RecorderNoteData(
                RecorderNote.A5,
                880.00f,
                new Fingering[]
                {
                    new Fingering(
                        new int[] {1,2},
                        true)
                }),

            // A#5
            new RecorderNoteData(
                RecorderNote.ASharp5,
                932.33f,
                new Fingering[]
                {
                    new Fingering(
                        new int[] {1,2,4,5,6,8},
                        true)
                }),

            // B5
            new RecorderNoteData(
                RecorderNote.B5,
                987.77f,
                new Fingering[]
                {
                    new Fingering(
                        new int[] {1,2,4,5},
                        true)
                }),

            // C6
            new RecorderNoteData(
                RecorderNote.C6,
                1046.50f,
                new Fingering[]
                {
                    new Fingering(
                        new int[] {1,4,5},
                        true)
                }),

            // C#6
            new RecorderNoteData(
                RecorderNote.CSharp6,
                1108.73f,
                new Fingering[]
                {
                    new Fingering(
                        new int[] {1,3,4,5,7,9},
                        true)
                }),

            // D6
            new RecorderNoteData(
                RecorderNote.D6,
                1174.66f,
                new Fingering[]
                {
                    new Fingering(
                        new int[] {1,3,4,6,7,8,9},
                        true)
                })
        };
        for (int i = 0; i < Notes.Length; i++)
        {
            if (Sounds != null && i < Sounds.Length)
                Notes[i].Sound = Sounds[i];
        }
    }
    public RecorderNoteData getNoteForFingering(
    RecorderHole[] holes)
    {
        foreach (RecorderNoteData note in Notes)
        {
            foreach (Fingering fingering in note.Fingerings)
            {
                bool matches = true;

                for (int i = 0; i < holes.Length; i++)
                {
                    int holeNumber = i + 1;

                    RecorderHoleState requiredState =
                        RecorderHoleState.Open;

                    // Check whether this hole should be covered.
                    for (int j = 0;
                         j < fingering.CoveredHoles.Length;
                         j++)
                    {
                        if (fingering.CoveredHoles[j] ==
                            holeNumber)
                        {
                            requiredState =
                                RecorderHoleState.Covered;

                            break;
                        }
                    }

                    // Hole 10 may instead need to be half-covered.
                    if (holeNumber == 10 &&
                        fingering.BackHoleHalfCovered)
                    {
                        requiredState =
                            RecorderHoleState.HalfCovered;
                    }

                    if (holes[i].State != requiredState)
                    {
                        matches = false;
                        break;
                    }
                }

                if (matches)
                    return note;
            }
        }

        // No valid recorder fingering.
        return null;
    }
}