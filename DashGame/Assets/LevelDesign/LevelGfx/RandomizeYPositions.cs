using System.Collections.Generic;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

public class RandomizeYPositions : MonoBehaviour
{
    public List<Transform> objects = new List<Transform>();

    [Header("Random Y Offset")]
    public float minOffset = -0.33f;
    public float maxOffset = 0.33f;

    public void RandomizeY()
    {
#if UNITY_EDITOR
        Undo.RecordObjects(objects.ToArray(), "Randomize Y Positions");
#endif

        foreach (Transform t in objects)
        {
            if (t == null)
                continue;

            Vector3 pos = t.position;
            pos.y += Random.Range(minOffset, maxOffset);
            t.position = pos;

#if UNITY_EDITOR
            EditorUtility.SetDirty(t);
#endif
        }

#if UNITY_EDITOR
        //EditorUtility.SetDirty(gameObject.scene);
#endif
    }
}

#if UNITY_EDITOR
[CustomEditor(typeof(RandomizeYPositions))]
public class RandomizeYPositionsEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        GUILayout.Space(10);

        RandomizeYPositions script = (RandomizeYPositions)target;

        if (GUILayout.Button("Randomize Y"))
        {
            script.RandomizeY();
        }
    }
}
#endif