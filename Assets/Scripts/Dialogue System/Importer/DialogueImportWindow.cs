using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using ToBOE.Dialogue.Importer.CodeGen;

namespace ToBOE.Dialogue.Importer
{
    public class DialogueImportWindow : EditorWindow
    {
        TextAsset tsvFile;
        LineParser.RawLineCollection rawLines;

        [MenuItem("Dialogue/Import Window")]
        public static void ShowWindow()
        {
            DialogueImportWindow window = EditorWindow.GetWindow<DialogueImportWindow>();
            window.titleContent = new GUIContent("Import Dialogue");
            window.minSize = new Vector2(300, 400);
        }

        private void OnGUI()
        {
            tsvFile = EditorGUILayout.ObjectField("Lines File", tsvFile, typeof(TextAsset), false) as TextAsset;
            if (tsvFile == null)
                GUI.enabled = false;

            if (GUILayout.Button("Process TSV"))
            {
                rawLines = LineParser.ParseRawLines(tsvFile.text);
            }

            GUI.enabled = true;

            if (rawLines != null)
                EditorGUILayout.LabelField("Raw line count: " + rawLines.RawLines.Count);
        }
    }
}
