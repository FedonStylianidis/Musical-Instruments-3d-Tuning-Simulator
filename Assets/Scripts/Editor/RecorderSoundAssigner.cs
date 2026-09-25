using UnityEngine;
using UnityEditor;

public class RecorderSoundAssigner : EditorWindow
{
    private RecorderDatabase Database;

    [MenuItem("Tools/Recorder/Assign Sounds")]
    public static void ShowWindow()
    {
        GetWindow<RecorderSoundAssigner>(
            "Recorder Sound Assigner"
        );
    }

    private void OnGUI()
    {
        GUILayout.Label(
            "Recorder Sound Assignment",
            EditorStyles.boldLabel
        );

        EditorGUILayout.Space();

        Database =
            (RecorderDatabase)EditorGUILayout.ObjectField(
                "Recorder Database",
                Database,
                typeof(RecorderDatabase),
                false
            );

        EditorGUILayout.Space();

        if (Database == null)
        {
            EditorGUILayout.HelpBox(
                "Select a Recorder Database.",
                MessageType.Info
            );

            return;
        }

        if (GUILayout.Button("Assign Recorder Sounds"))
        {
            assignSounds();
        }
    }

    private void assignSounds()
    {
        string folder =
            "Assets/Audio/Recorder_Sounds_Tomplay/";

        string[] fileNames =
        {
            "C4",
            "C4di",
            "D4",
            "D4di",
            "E4",
            "F4",
            "F4di",
            "G4",
            "G4di",
            "A4",
            "A4di",
            "B4",

            "C5",
            "C5di",
            "D5",
            "D5di",
            "E5",
            "F5",
            "F5di",
            "G5",
            "G5di",
            "A5",
            "A5di",
            "B5",

            "C6",
            "C6di",
            "D6"
        };

        Database.Sounds =
            new AudioClip[fileNames.Length];

        for (int i = 0; i < fileNames.Length; i++)
        {
            string path =
                folder +
                fileNames[i] +
                ".mp3";

            Database.Sounds[i] =
                AssetDatabase.LoadAssetAtPath<AudioClip>(
                    path
                );

            if (Database.Sounds[i] == null)
            {
                Debug.LogWarning(
                    "Recorder sound not found: " +
                    path
                );
            }
        }

        EditorUtility.SetDirty(Database);

        AssetDatabase.SaveAssets();

        Debug.Log(
            "Recorder sounds assigned."
        );
    }
}