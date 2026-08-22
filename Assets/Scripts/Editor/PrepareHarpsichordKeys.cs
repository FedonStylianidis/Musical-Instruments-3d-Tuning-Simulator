using UnityEngine;
using UnityEditor;
using System.Text.RegularExpressions;

public class PrepareHarpsichordKeys : EditorWindow
{
    private GameObject keysParent;

    private readonly HarpsichordNote[] WhiteNotes =
    {
        HarpsichordNote.F1, HarpsichordNote.G1, HarpsichordNote.A1, HarpsichordNote.B1,
        HarpsichordNote.C2, HarpsichordNote.D2, HarpsichordNote.E2, HarpsichordNote.F2, HarpsichordNote.G2, HarpsichordNote.A2, HarpsichordNote.B2,
        HarpsichordNote.C3, HarpsichordNote.D3, HarpsichordNote.E3, HarpsichordNote.F3, HarpsichordNote.G3, HarpsichordNote.A3, HarpsichordNote.B3,
        HarpsichordNote.C4, HarpsichordNote.D4, HarpsichordNote.E4, HarpsichordNote.F4, HarpsichordNote.G4, HarpsichordNote.A4, HarpsichordNote.B4,
        HarpsichordNote.C5, HarpsichordNote.D5, HarpsichordNote.E5, HarpsichordNote.F5, HarpsichordNote.G5, HarpsichordNote.A5, HarpsichordNote.B5,
        HarpsichordNote.C6, HarpsichordNote.D6, HarpsichordNote.E6
    };

    private readonly HarpsichordNote[] BlackNotes =
    {
        HarpsichordNote.FSharp1, HarpsichordNote.GSharp1, HarpsichordNote.ASharp1,
        HarpsichordNote.CSharp2, HarpsichordNote.DSharp2, HarpsichordNote.FSharp2, HarpsichordNote.GSharp2, HarpsichordNote.ASharp2,
        HarpsichordNote.CSharp3, HarpsichordNote.DSharp3, HarpsichordNote.FSharp3, HarpsichordNote.GSharp3, HarpsichordNote.ASharp3,
        HarpsichordNote.CSharp4, HarpsichordNote.DSharp4, HarpsichordNote.FSharp4, HarpsichordNote.GSharp4, HarpsichordNote.ASharp4,
        HarpsichordNote.CSharp5, HarpsichordNote.DSharp5, HarpsichordNote.FSharp5, HarpsichordNote.GSharp5, HarpsichordNote.ASharp5,
        HarpsichordNote.CSharp6, HarpsichordNote.DSharp6
    };

    [MenuItem("Tools/Harpsichord/Prepare Keys")]
    public static void ShowWindow()
    {
        GetWindow<PrepareHarpsichordKeys>("Prepare Harpsichord Keys");
    }

    private void OnGUI()
    {
        keysParent = (GameObject)EditorGUILayout.ObjectField(
            "Keys Parent",
            keysParent,
            typeof(GameObject),
            true
        );

        if (GUILayout.Button("Prepare Keys"))
            PrepareKeys();
    }

    private void PrepareKeys()
    {
        if (keysParent == null)
        {
            Debug.LogError("Assign the parent GameObject that contains all keys.");
            return;
        }

        foreach (Transform child in keysParent.GetComponentsInChildren<Transform>())
        {
            if (child == keysParent.transform)
                continue;

            GameObject keyObject = child.gameObject;
            string objectName = keyObject.name;

            bool isWhite = objectName.StartsWith("w_ivories");
            bool isBlack = objectName.StartsWith("b_ivories");

            if (!isWhite && !isBlack)
                continue;

            int index = ExtractIndex(objectName);

            if (index <= 0)
            {
                Debug.LogWarning("Could not read key index from: " + objectName);
                continue;
            }

            HarpsichordNote[] noteArray = isWhite ? WhiteNotes : BlackNotes;

            if (index > noteArray.Length)
            {
                Debug.LogWarning(objectName + " index is outside note array.");
                continue;
            }

            HarpsichordNote note = noteArray[index - 1];

            HarpsichordKey key = keyObject.GetComponent<HarpsichordKey>();
            if (key == null)
                key = Undo.AddComponent<HarpsichordKey>(keyObject);

            MouseUI mouseUI = keyObject.GetComponent<MouseUI>();
            if (mouseUI == null)
                mouseUI = Undo.AddComponent<MouseUI>(keyObject);

            AudioSource audioSource = keyObject.GetComponent<AudioSource>();
            if (audioSource == null)
                audioSource = Undo.AddComponent<AudioSource>(keyObject);

            audioSource.playOnAwake = false;
            EditorUtility.SetDirty(audioSource);

            Collider collider = keyObject.GetComponent<Collider>();
            if (collider == null)
                Undo.AddComponent<BoxCollider>(keyObject);

            Rigidbody rb = keyObject.GetComponent<Rigidbody>();
            if (rb == null)
                rb = Undo.AddComponent<Rigidbody>(keyObject);

            rb.useGravity = false;
            rb.isKinematic = true;
            rb.constraints =
                RigidbodyConstraints.FreezePositionX |
                RigidbodyConstraints.FreezePositionY |
                RigidbodyConstraints.FreezePositionZ |
                RigidbodyConstraints.FreezeRotationX |
                RigidbodyConstraints.FreezeRotationY |
                RigidbodyConstraints.FreezeRotationZ;

            EditorUtility.SetDirty(rb);

            Undo.RecordObject(key, "Assign Harpsichord Note");
            key.Note = note;
            EditorUtility.SetDirty(key);

            Undo.RecordObject(mouseUI, "Assign MouseUI Label");
            mouseUI.Label = Labels.HarpsichordKey;
            EditorUtility.SetDirty(mouseUI);

            EditorUtility.SetDirty(keyObject);
            PrefabUtility.RecordPrefabInstancePropertyModifications(keyObject);

            Debug.Log("Prepared " + objectName + " → " + note);
        }

        EditorUtility.SetDirty(keysParent);
        PrefabUtility.RecordPrefabInstancePropertyModifications(keysParent);

        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();

        Debug.Log("Finished preparing harpsichord keys.");
    }

    private int ExtractIndex(string objectName)
    {
        Match match = Regex.Match(objectName, @"\.(\d+)$");

        if (!match.Success)
            return -1;

        return int.Parse(match.Groups[1].Value);
    }
}