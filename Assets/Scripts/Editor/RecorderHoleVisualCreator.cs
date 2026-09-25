using UnityEngine;
using UnityEditor;

public class RecorderHoleVisualCreator : EditorWindow
{
    private Recorder Recorder;

    [MenuItem("Tools/Recorder/Create Hole Visuals")]
    public static void ShowWindow()
    {
        GetWindow<RecorderHoleVisualCreator>(
            "Recorder Hole Visuals"
        );
    }

    private void OnGUI()
    {
        GUILayout.Label(
            "Recorder Hole Visual Creator",
            EditorStyles.boldLabel
        );

        EditorGUILayout.Space();

        Recorder =
            (Recorder)EditorGUILayout.ObjectField(
                "Recorder",
                Recorder,
                typeof(Recorder),
                true
            );

        EditorGUILayout.Space();

        if (Recorder == null)
        {
            EditorGUILayout.HelpBox(
                "Select the Recorder.",
                MessageType.Info
            );

            return;
        }

        if (GUILayout.Button("Create / Update Hole Visuals"))
        {
            createHoleVisuals();
        }
    }

    private void createHoleVisuals()
    {
        for (int i = 0; i < Recorder.Holes.Length; i++)
        {
            GameObject hole = Recorder.Holes[i];

            if (hole == null)
                continue;

            SphereCollider holeCollider =
                hole.GetComponent<SphereCollider>();

            if (holeCollider == null)
            {
                Debug.LogWarning(
                    hole.name +
                    " has no SphereCollider."
                );

                continue;
            }

            GameObject coverVisual =
                getOrCreateCoverVisual(
                    hole,
                    holeCollider
                );

            updateCoverVisual(
                coverVisual,
                holeCollider,
                i
            );

            // Element 9 = Hole 10.
            if (i == 9)
            {
                createOrUpdateHalfCoverVisual(
                    hole,
                    holeCollider
                );
            }
        }

        Debug.Log(
            "Recorder hole visuals created / updated."
        );
    }

    private GameObject getOrCreateCoverVisual(
        GameObject hole,
        SphereCollider holeCollider)
    {
        Transform existingVisual =
            hole.transform.Find("CoverVisual");

        if (existingVisual != null)
            return existingVisual.gameObject;

        GameObject visual =
            GameObject.CreatePrimitive(
                PrimitiveType.Sphere
            );

        Undo.RegisterCreatedObjectUndo(
            visual,
            "Create Recorder Hole Visual"
        );

        visual.name = "CoverVisual";

        visual.transform.SetParent(
            hole.transform,
            false
        );

        Collider visualCollider =
            visual.GetComponent<Collider>();

        if (visualCollider != null)
        {
            DestroyImmediate(
                visualCollider
            );
        }

        return visual;
    }

    private void updateCoverVisual(
        GameObject visual,
        SphereCollider holeCollider,
        int holeIndex)
    {
        visual.transform.localPosition =
            holeCollider.center;

        visual.transform.localRotation =
            Quaternion.identity;

        float diameter =
            holeCollider.radius * 2f;

        float visualDiameter;

        // Holes 1-7 and Hole 10.
        if (holeIndex <= 6 || holeIndex == 9)
        {
            visualDiameter = 0.006f;
        }
        else
        {
            // Holes 8-9.
            visualDiameter = diameter;
        }

        visual.transform.localScale =
            new Vector3(
                visualDiameter,
                visualDiameter,
                diameter * 0.15f
            );
    }

    private void createOrUpdateHalfCoverVisual(
        GameObject hole,
        SphereCollider holeCollider)
    {
        Transform existingHalf =
            hole.transform.Find("HalfCoverVisual");

        GameObject halfVisual;

        if (existingHalf == null)
        {
            halfVisual =
                new GameObject(
                    "HalfCoverVisual"
                );

            Undo.RegisterCreatedObjectUndo(
                halfVisual,
                "Create Recorder Half Cover Visual"
            );

            halfVisual.transform.SetParent(
                hole.transform,
                false
            );

            halfVisual.AddComponent<MeshFilter>();
            halfVisual.AddComponent<MeshRenderer>();
        }
        else
        {
            halfVisual =
                existingHalf.gameObject;
        }

        halfVisual.transform.localPosition =
            holeCollider.center;

        halfVisual.transform.localRotation =
            Quaternion.identity;

        halfVisual.transform.localScale =
            Vector3.one;

        MeshFilter meshFilter =
            halfVisual.GetComponent<MeshFilter>();

        meshFilter.sharedMesh =
            createHalfCircleMesh(
                0.003f,
                32
            );
    }

    private Mesh createHalfCircleMesh(
        float radius,
        int segments)
    {
        Mesh mesh = new Mesh();

        Vector3[] vertices =
            new Vector3[segments + 2];

        int[] triangles =
            new int[segments * 3];

        vertices[0] =
            Vector3.zero;

        for (int i = 0; i <= segments; i++)
        {
            float angle =
                Mathf.PI *
                i /
                segments;

            vertices[i + 1] =
                new Vector3(
                    Mathf.Cos(angle) * radius,
                    Mathf.Sin(angle) * radius,
                    0f
                );
        }

        for (int i = 0; i < segments; i++)
        {
            int triangleIndex =
                i * 3;

            triangles[triangleIndex] = 0;
            triangles[triangleIndex + 1] =
                i + 2;
            triangles[triangleIndex + 2] =
                i + 1;
        }

        mesh.vertices = vertices;
        mesh.triangles = triangles;

        mesh.RecalculateNormals();
        mesh.RecalculateBounds();

        return mesh;
    }
}