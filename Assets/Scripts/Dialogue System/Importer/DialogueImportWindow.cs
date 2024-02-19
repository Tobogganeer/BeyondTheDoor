using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using BeyondTheDoor.Importer.CodeGen;

namespace BeyondTheDoor.Importer
{
    public class DialogueImportWindow : EditorWindow
    {
        TextAsset tsvFile;
        LineParser.RawLineCollection rawLines;
        bool clearLinesOnEnumGeneration;

        [MenuItem("Dialogue/Import Window")]
        public static void ShowWindow()
        {
            DialogueImportWindow window = EditorWindow.GetWindow<DialogueImportWindow>();
            window.titleContent = new GUIContent("Import Dialogue");
            window.minSize = new Vector2(350, 150);
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
            EditorGUILayout.LabelField("All lines passed validation.");

            if (GUILayout.Button("Generate Lines"))
            {
                LineGenerator.GenerateLinesFile(rawLines);
            }
        }

        void LinesInvalid()
        {
            EditorGUILayout.LabelField("Lines invalid: " + rawLines.InvalidLines.Count +
                ". Reason(s): " + rawLines.InvalidElements);

            if (GUILayout.Button("Log Invalid lines"))
            {
                Debug.Log("Invalid lines:");
                foreach (LineParser.RawLineData data in rawLines.InvalidLines)
                    Debug.Log("- " + data.GetInvalidElementsString());
            }

            bool charactersInvalid = rawLines.InvalidElements.HasFlag(Line.Element.CharacterID);
            bool lineIDsInvalid = rawLines.InvalidElements.HasFlag(Line.Element.LineID);
            bool lineStatusesInvalid = rawLines.InvalidElements.HasFlag(Line.Element.LineStatus);
            bool voiceStatusesInvalid = rawLines.InvalidElements.HasFlag(Line.Element.VoiceStatus);

            if (charactersInvalid || lineIDsInvalid || lineStatusesInvalid || voiceStatusesInvalid)
            {
                EditorGUILayout.LabelField("Enums may be invalid.");
                if (GUILayout.Button("Generate Enums"))
                {
                    if (clearLinesOnEnumGeneration)
                        LineGenerator.ClearLinesFile();

                    GenerateEnums(charactersInvalid, lineIDsInvalid, lineStatusesInvalid, voiceStatusesInvalid);
                }
                clearLinesOnEnumGeneration = EditorGUILayout.Toggle("Clear all Lines?", clearLinesOnEnumGeneration);
            }
            else
            {
                EditorGUILayout.LabelField("All Enums appear valid.");
                if (GUILayout.Button("Regenerate Enums anyways"))
                {
                    GenerateEnums(true, true, true, true);
                }
            }
        }

        void GenerateEnums(bool chars, bool ids, bool stats, bool voices)
        {
            if (chars)
                GenerateEnum(nameof(CharacterID), Line.Element.CharacterID);
            if (ids)
                GenerateEnum(nameof(LineID), Line.Element.LineID);
            if (stats)
                GenerateEnum(nameof(LineStatus), Line.Element.LineStatus);
            if (voices)
                GenerateEnum(nameof(VoiceStatus), Line.Element.VoiceStatus);
        }

        void GenerateEnum(string enumName, Line.Element dataType)
        {
            EnumGenerator gen = new EnumGenerator(enumName, FilePaths.Autogenerated, FilePaths.DialogueNamespace);
            foreach (string data in rawLines.GetAllData(dataType))
                gen.Add(data, data.GetHashCode());
            gen.Generate();
        }
    }
}
