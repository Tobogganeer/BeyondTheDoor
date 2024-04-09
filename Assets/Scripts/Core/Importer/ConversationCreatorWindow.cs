using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using BeyondTheDoor.Importer.CodeGen;
using System.IO;

namespace BeyondTheDoor.Importer
{
    public class ConversationCreatorWindow : EditorWindow
    {
        TSVData tsvData;

        [MenuItem("Dialogue/Conversation Importer")]
        public static void ShowWindow()
        {
            ConversationCreatorWindow window = EditorWindow.GetWindow<ConversationCreatorWindow>();
            window.titleContent = new GUIContent("Import Conversations");
            window.minSize = new Vector2(350, 150);
        }

        private void OnGUI()
        {
            if (!FilePaths.ExcelFileExists)
            {
                EditorGUILayout.LabelField($"Assets/{FilePaths.ExcelLinesFileName} not found. Please create said file.");
                return;
            }

            ProcessTSVButtons();
            if (tsvData != null)
            {
                EditorGUILayout.LabelField(tsvData.Count + " TSV entries loaded.");
            }

            GUI.enabled = true;
        }

        void ProcessTSVButtons()
        {
            if (!FilePaths.ExcelFileExists)
                GUI.enabled = false;

            if (TSVData.SavedDataExists())
            {
                GUI.enabled = true;
                if (GUILayout.Button("Load cached TSV Data"))
                {
                    tsvData = TSVData.Load();
                    return;
                }

                // Disable GUI if we can't load the lines
                if (!FilePaths.ExcelFileExists)
                    GUI.enabled = false;

                if (GUILayout.Button("Overwrite fresh data from Excel lines"))
                    tsvData = LineParser.ParseLines(File.ReadAllText(FilePaths.ExcelLinesFilePath));
            }
            else if (GUILayout.Button("Process Excel lines"))
            {
                tsvData = LineParser.ParseLines(File.ReadAllText(FilePaths.ExcelLinesFilePath));
            }

            if (tsvData != null)
                tsvData.Save();
        }
    }
}
