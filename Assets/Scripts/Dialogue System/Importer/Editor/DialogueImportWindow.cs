using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class DialogueImportWindow : EditorWindow
{
    TextAsset tsvFile;

    [MenuItem("Dialogue/Import Window")]
    public static void ShowWindow()
    {
        DialogueImportWindow window = EditorWindow.GetWindow<DialogueImportWindow>();
        window.titleContent = new GUIContent("Import Dialogue");
        window.minSize = new Vector2(300, 600);
    }

    private void OnGUI()
    {
        tsvFile = EditorGUILayout.ObjectField(tsvFile, typeof(TextAsset), false) as TextAsset;
    }
}
