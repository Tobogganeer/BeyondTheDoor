using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using ToBOE.Dialogue.Importer.CodeGen;
using System.Security.Cryptography;

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
            {
                EditorGUILayout.LabelField("Raw lines loaded: " + rawLines.RawLines.Count);

                if (rawLines.IsValid)
                    LinesValid();
                else
                    LinesInvalid();
            }
        }

        void LinesValid()
        {

        }

        void LinesInvalid()
        {
            EditorGUILayout.LabelField("Lines invalid: " + rawLines.InvalidLines.Count);

            bool charactersInvalid = rawLines.InvalidElements.HasFlag(LineParser.LineDataType.Character);
            bool lineIDsInvalid = rawLines.InvalidElements.HasFlag(LineParser.LineDataType.LineID);
            bool lineStatusesInvalid = rawLines.InvalidElements.HasFlag(LineParser.LineDataType.LineStatus) ||
                rawLines.InvalidElements.HasFlag(LineParser.LineDataType.VoiceStatus);

            if (charactersInvalid || lineIDsInvalid || lineStatusesInvalid)
            {
                EditorGUILayout.LabelField("Enums may be invalid.");
                if (GUILayout.Button("Generate Enums"))
                {
                    GenerateEnums(charactersInvalid, lineIDsInvalid, lineStatusesInvalid);
                }
            }
        }

        void GenerateEnums(bool chars, bool ids, bool stats)
        {
            if (chars)
                GenerateCharacterEnum();
            if (ids)
                GenerateLineIDEnum();
            if (stats)
                GenerateLineStatusEnum();
        }

        void GenerateCharacterEnum()
        {

        }

        void GenerateLineIDEnum()
        {

        }

        void GenerateLineStatusEnum()
        {

        }
    }
}
