using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using BeyondTheDoor.Importer.CodeGen;

namespace BeyondTheDoor.Importer
{
    public class DialogueImportWindow : EditorWindow
    {
        TextAsset rawExcelExport;
        TSVData tsvData;
        RawLineCollection rawLines;
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
            rawExcelExport = EditorGUILayout.ObjectField("Lines File", rawExcelExport, typeof(TextAsset), false) as TextAsset;

            ProcessTSVButtons();
            if (tsvData != null)
                EditorGUILayout.LabelField(tsvData.Count + " TSV entries loaded.");

            if (GUILayout.Button("Process TSV"))
            {
                //rawLines = LineParser.ParseRawLines(tsvFile.text);
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

        void ProcessTSVButtons()
        {
            if (rawExcelExport == null)
                GUI.enabled = false;

            if (TSVData.SavedDataExists())
            {
                GUI.enabled = true;
                if (GUILayout.Button("Load saved TSV Data"))
                {
                    tsvData = TSVData.Load();
                    return;
                }

                // Disable GUI if we can't load the lines
                if (rawExcelExport == null)
                    GUI.enabled = false;

                if (GUILayout.Button("Overwrite fresh data from Excel lines"))
                    tsvData = LineParser.ParseLines(rawExcelExport.text);
            }
            else if (GUILayout.Button("Process Excel lines"))
            {
                tsvData = LineParser.ParseLines(rawExcelExport.text);
            }

            if (tsvData != null)
                tsvData.Save();
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
                foreach (RawLineData data in rawLines.InvalidLines)
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
                        LineGenerator.ClearLinesFile(rawLines);

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
