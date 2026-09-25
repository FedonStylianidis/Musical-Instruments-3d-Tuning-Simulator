using UnityEngine;
using UnityEditor;
using System.Collections.Generic;

public class PopulateHarpsichordDatabase : EditorWindow
{
    private HarpsichordDatabase database;
    private DefaultAsset audioFolder;

    [MenuItem("Tools/Harpsichord/Populate Database")]
    public static void ShowWindow()
    {
        GetWindow<PopulateHarpsichordDatabase>("Populate Harpsichord Database");
    }

    private void OnGUI()
    {
        database = (HarpsichordDatabase)EditorGUILayout.ObjectField(
            "Database",
            database,
            typeof(HarpsichordDatabase),
            false
        );

        audioFolder = (DefaultAsset)EditorGUILayout.ObjectField(
            "Audio Folder",
            audioFolder,
            typeof(DefaultAsset),
            false
        );

        if (GUILayout.Button("Populate Database"))
        {
            Populate();
        }
    }

    private void Populate()
    {
        if (database == null)
        {
            Debug.LogError("Assign the HarpsichordDatabase asset first.");
            return;
        }

        if (audioFolder == null)
        {
            Debug.LogError("Assign the audio folder first.");
            return;
        }

        string folderPath = AssetDatabase.GetAssetPath(audioFolder);

        database.Keys = new List<KeyData>();

        Add(folderPath, HarpsichordNote.F1, "F1", 43.65f);
        Add(folderPath, HarpsichordNote.FSharp1, "F#1", 46.25f);
        Add(folderPath, HarpsichordNote.G1, "G1", 49.00f);
        Add(folderPath, HarpsichordNote.GSharp1, "G#1", 51.91f);
        Add(folderPath, HarpsichordNote.A1, "A1", 55.00f);
        Add(folderPath, HarpsichordNote.ASharp1, "A#1", 58.27f);
        Add(folderPath, HarpsichordNote.B1, "B1", 61.74f);

        Add(folderPath, HarpsichordNote.C2, "C2", 65.41f);
        Add(folderPath, HarpsichordNote.CSharp2, "C#2", 69.30f);
        Add(folderPath, HarpsichordNote.D2, "D2", 73.42f);
        Add(folderPath, HarpsichordNote.DSharp2, "D#2", 77.78f);
        Add(folderPath, HarpsichordNote.E2, "E2", 82.41f);
        Add(folderPath, HarpsichordNote.F2, "F2", 87.31f);
        Add(folderPath, HarpsichordNote.FSharp2, "F#2", 92.50f);
        Add(folderPath, HarpsichordNote.G2, "G2", 98.00f);
        Add(folderPath, HarpsichordNote.GSharp2, "G#2", 103.83f);
        Add(folderPath, HarpsichordNote.A2, "A2", 110.00f);
        Add(folderPath, HarpsichordNote.ASharp2, "A#2", 116.54f);
        Add(folderPath, HarpsichordNote.B2, "B2", 123.47f);

        Add(folderPath, HarpsichordNote.C3, "C3", 130.81f);
        Add(folderPath, HarpsichordNote.CSharp3, "C#3", 138.59f);
        Add(folderPath, HarpsichordNote.D3, "D3", 146.83f);
        Add(folderPath, HarpsichordNote.DSharp3, "D#3", 155.56f);
        Add(folderPath, HarpsichordNote.E3, "E3", 164.81f);
        Add(folderPath, HarpsichordNote.F3, "F3", 174.61f);
        Add(folderPath, HarpsichordNote.FSharp3, "F#3", 185.00f);
        Add(folderPath, HarpsichordNote.G3, "G3", 196.00f);
        Add(folderPath, HarpsichordNote.GSharp3, "G#3", 207.65f);
        Add(folderPath, HarpsichordNote.A3, "A3", 220.00f);
        Add(folderPath, HarpsichordNote.ASharp3, "A#3", 233.08f);
        Add(folderPath, HarpsichordNote.B3, "B3", 246.94f);

        Add(folderPath, HarpsichordNote.C4, "C4", 261.63f);
        Add(folderPath, HarpsichordNote.CSharp4, "C#4", 277.18f);
        Add(folderPath, HarpsichordNote.D4, "D4", 293.66f);
        Add(folderPath, HarpsichordNote.DSharp4, "D#4", 311.13f);
        Add(folderPath, HarpsichordNote.E4, "E4", 329.63f);
        Add(folderPath, HarpsichordNote.F4, "F4", 349.23f);
        Add(folderPath, HarpsichordNote.FSharp4, "F#4", 369.99f);
        Add(folderPath, HarpsichordNote.G4, "G4", 392.00f);
        Add(folderPath, HarpsichordNote.GSharp4, "G#4", 415.30f);
        Add(folderPath, HarpsichordNote.A4, "A4", 440.00f);
        Add(folderPath, HarpsichordNote.ASharp4, "A#4", 466.16f);
        Add(folderPath, HarpsichordNote.B4, "B4", 493.88f);

        Add(folderPath, HarpsichordNote.C5, "C5", 523.25f);
        Add(folderPath, HarpsichordNote.CSharp5, "C#5", 554.37f);
        Add(folderPath, HarpsichordNote.D5, "D5", 587.33f);
        Add(folderPath, HarpsichordNote.DSharp5, "D#5", 622.25f);
        Add(folderPath, HarpsichordNote.E5, "E5", 659.26f);
        Add(folderPath, HarpsichordNote.F5, "F5", 698.46f);
        Add(folderPath, HarpsichordNote.FSharp5, "F#5", 739.99f);
        Add(folderPath, HarpsichordNote.G5, "G5", 783.99f);
        Add(folderPath, HarpsichordNote.GSharp5, "G#5", 830.61f);
        Add(folderPath, HarpsichordNote.A5, "A5", 880.00f);
        Add(folderPath, HarpsichordNote.ASharp5, "A#5", 932.33f);
        Add(folderPath, HarpsichordNote.B5, "B5", 987.77f);

        Add(folderPath, HarpsichordNote.C6, "C6", 1046.50f);
        Add(folderPath, HarpsichordNote.CSharp6, "C#6", 1108.73f);
        Add(folderPath, HarpsichordNote.D6, "D6", 1174.66f);
        Add(folderPath, HarpsichordNote.DSharp6, "D#6", 1244.51f);
        Add(folderPath, HarpsichordNote.E6, "E6", 1318.51f);

        EditorUtility.SetDirty(database);
        AssetDatabase.SaveAssets();

        Debug.Log("Harpsichord database populated.");
    }

    private void Add(string folderPath, HarpsichordNote note, string displayName, float frequency)
    {
        AudioClip clip = FindClip(folderPath, displayName);

        KeyData data = new KeyData
        {
            Note = note,
            DisplayName = displayName,
            TargetFrequency = frequency,
            Sound = clip
        };

        database.Keys.Add(data);
    }

    private AudioClip FindClip(string folderPath, string displayName)
    {
        string fileName = displayName
            .ToLower();

        string[] guids = AssetDatabase.FindAssets("harpsichord_" + fileName + " t:AudioClip", new[] { folderPath });

        if (guids.Length == 0)
        {
            Debug.LogWarning("Missing clip: harpsichord_" + fileName);
            return null;
        }

        string path = AssetDatabase.GUIDToAssetPath(guids[0]);
        return AssetDatabase.LoadAssetAtPath<AudioClip>(path);
    }
}