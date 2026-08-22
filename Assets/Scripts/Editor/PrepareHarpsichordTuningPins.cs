using UnityEngine;
using UnityEditor;
using System;

public class PrepareHarpsichordTuningPins : EditorWindow
{
    [MenuItem("Tools/Harpsichord/Prepare Tuning Pins")]
    public static void ShowWindow()
    {
        GetWindow<PrepareHarpsichordTuningPins>(
            "Prepare Tuning Pins"
        );
    }


    private void OnGUI()
    {
        GUILayout.Space(10);

        GUILayout.Label(
            "Harpsichord Tuning Pins",
            EditorStyles.boldLabel
        );

        GUILayout.Space(10);

        GUILayout.Label(
            "This tool prepares regulators_pivot_001 through regulators_pivot_054 and assigns notes from G1 through C6."
        );

        GUILayout.Space(20);


        if (GUILayout.Button(
            "Prepare Tuning Pins",
            GUILayout.Height(35)
        ))
        {
            PreparePins();
        }
    }


    private void PreparePins()
    {
        Harpsichord harpsichord =
            UnityEngine.Object.FindFirstObjectByType<Harpsichord>();


        if (harpsichord == null)
        {
            EditorUtility.DisplayDialog(
                "Prepare Tuning Pins",
                "No Harpsichord object was found in the scene.",
                "OK"
            );

            return;
        }


        string[] noteNames =
        {
            "G1",
            "GSharp1",
            "A1",
            "ASharp1",
            "B1",

            "C2",
            "CSharp2",
            "D2",
            "DSharp2",
            "E2",
            "F2",
            "FSharp2",
            "G2",
            "GSharp2",
            "A2",
            "ASharp2",
            "B2",

            "C3",
            "CSharp3",
            "D3",
            "DSharp3",
            "E3",
            "F3",
            "FSharp3",
            "G3",
            "GSharp3",
            "A3",
            "ASharp3",
            "B3",

            "C4",
            "CSharp4",
            "D4",
            "DSharp4",
            "E4",
            "F4",
            "FSharp4",
            "G4",
            "GSharp4",
            "A4",
            "ASharp4",
            "B4",

            "C5",
            "CSharp5",
            "D5",
            "DSharp5",
            "E5",
            "F5",
            "FSharp5",
            "G5",
            "GSharp5",
            "A5",
            "ASharp5",
            "B5",

            "C6"
        };


        if (noteNames.Length != 54)
        {
            EditorUtility.DisplayDialog(
                "Prepare Tuning Pins",
                "The note list does not contain exactly 54 notes.",
                "OK"
            );

            return;
        }


        Transform[] allTransforms =
            harpsichord.GetComponentsInChildren<Transform>(
                true
            );


        int preparedPins = 0;


        for (int i = 0; i < 54; i++)
        {
            string pinName =
                "regulators_pivot_" +
                (i + 1).ToString("000");


            Transform pinTransform =
                FindTransformByName(
                    allTransforms,
                    pinName
                );


            if (pinTransform == null)
            {
                Debug.LogWarning(
                    "Could not find pin: " +
                    pinName
                );

                continue;
            }


            if (!Enum.TryParse(
                    noteNames[i],
                    out HarpsichordNote note))
            {
                Debug.LogError(
                    "HarpsichordNote does not contain: " +
                    noteNames[i]
                );

                continue;
            }


            GameObject pinObject =
                pinTransform.gameObject;


            HarpsichordTuningPin tuningPin =
                pinObject.GetComponent<
                    HarpsichordTuningPin>();


            if (tuningPin == null)
            {
                tuningPin =
                    Undo.AddComponent<
                        HarpsichordTuningPin>(
                        pinObject
                    );
            }


            MouseUI mouseUI =
                pinObject.GetComponent<MouseUI>();


            if (mouseUI == null)
            {
                mouseUI =
                    Undo.AddComponent<MouseUI>(
                        pinObject
                    );
            }


            BoxCollider boxCollider =
                pinObject.GetComponent<BoxCollider>();


            if (boxCollider == null)
            {
                boxCollider =
                    Undo.AddComponent<BoxCollider>(
                        pinObject
                    );
            }


            Undo.RecordObject(
                tuningPin,
                "Prepare Harpsichord Tuning Pin"
            );

            Undo.RecordObject(
                mouseUI,
                "Prepare Harpsichord Tuning Pin MouseUI"
            );

            Undo.RecordObject(
                boxCollider,
                "Prepare Harpsichord Tuning Pin Collider"
            );


            tuningPin.Note =
                note;


            mouseUI.Label =
                Labels.HarpsichordRegulatorPin;


            EditorUtility.SetDirty(
                tuningPin
            );

            EditorUtility.SetDirty(
                mouseUI
            );

            EditorUtility.SetDirty(
                boxCollider
            );


            preparedPins++;


            string displayedNote =
                note.ToString().Replace(
                    "Sharp",
                    "#"
                );


            Debug.Log(
                pinName +
                " -> " +
                displayedNote
            );
        }


        EditorUtility.DisplayDialog(
            "Prepare Tuning Pins",
            preparedPins +
            " of 54 tuning pins were prepared successfully.",
            "OK"
        );
    }


    private Transform FindTransformByName(
        Transform[] transforms,
        string objectName)
    {
        foreach (
            Transform currentTransform
            in transforms)
        {
            if (currentTransform.name ==
                objectName)
            {
                return currentTransform;
            }
        }


        return null;
    }
}