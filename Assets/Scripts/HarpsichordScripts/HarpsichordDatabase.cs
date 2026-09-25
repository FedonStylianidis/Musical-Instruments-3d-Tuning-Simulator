using System.Collections.Generic;
using UnityEngine;

public enum HarpsichordNote
{
   
    F1, FSharp1, G1, GSharp1, A1, ASharp1, B1,  // 1st Octave 7 notes)

    C2, CSharp2, D2, DSharp2, E2, F2, FSharp2, G2, GSharp2, A2, ASharp2, B2, // 2nd Octave 12 notes)

    C3, CSharp3, D3, DSharp3, E3, F3, FSharp3, G3, GSharp3, A3, ASharp3, B3, // 3rd Octave 12 notes)

    C4, CSharp4, D4, DSharp4, E4, F4, FSharp4, G4, GSharp4, A4, ASharp4, B4, // 4th Octave 12 notes)

    C5, CSharp5, D5, DSharp5, E5, F5, FSharp5, G5, GSharp5, A5, ASharp5, B5, // 5th Octave 12 notes)

    C6, CSharp6, D6, DSharp6, E6  // 6th Octave 5 notes)
}



[System.Serializable]
public class KeyData
{
    public HarpsichordNote Note;

    public string DisplayName;

    public float TargetFrequency;

    public AudioClip Sound;
}

[CreateAssetMenu(fileName = "HarpsichordDatabase",
                 menuName = "Harpsichord/Database")]
public class HarpsichordDatabase : ScriptableObject
{
    public List<KeyData> Keys = new();

    public KeyData GetKeyData(HarpsichordNote note)
    {
        return Keys.Find(k => k.Note == note);
    }
}