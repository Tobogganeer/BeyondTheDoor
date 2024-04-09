using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using BeyondTheDoor.Importer.CodeGen;
using System.IO;
using System.Linq;

namespace BeyondTheDoor.Importer
{
    public class ConversationCreatorWindow : EditorWindow
    {
        TSVData tsvData;
        List<ConversationRange> tsvConversations;

        static readonly string ConversationMarker = "conversation";
        static readonly string EndMarker = "end";
        static readonly string IfMarker = "if";
        static readonly string ElifMarkerMarker = "elif";
        static readonly string ElseIfMarker = "else if";
        static readonly string ElseMarker = "else";
        static readonly string EndIfMarker = "endif";
        static readonly string End_IfMarker = "end if";
        static readonly string ChoiceMarker = "choice";
        static readonly string GotoMarker = "goto";

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
            if (tsvData == null)
                return;

            GUI.enabled = true;

            EditorGUILayout.LabelField(tsvData.Count + " TSV entries loaded.");

            if (GUILayout.Button(tsvConversations == null ?
                "Fetch Conversations from TSV" : "Refresh Conversations from TSV"))
                FetchConversations();

            if (tsvConversations == null)
                return;

            EditorGUILayout.LabelField(tsvConversations.Count + " conversations loaded.");

            if (GUILayout.Button("Create Conversation assets"))
                CreateConversations();
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

        void FetchConversations()
        {
            tsvConversations = new List<ConversationRange>();

            bool inConversation = false;
            int currentConversationIndex = 0;

            for (int i = 0; i < tsvData.Count; i++)
            {
                string marker = tsvData.CharacterIDColumn[i].ToLower();
                if (marker == ConversationMarker)
                {
                    if (inConversation)
                        Debug.LogWarning("Missing End marker for conversation '"
                            + tsvData.CharacterIDColumn[currentConversationIndex] + "'");

                    inConversation = true;
                    currentConversationIndex = i;
                }
                else if (marker == EndMarker)
                {
                    // We reached the end of this convo
                    if (inConversation)
                    {
                        inConversation = false;
                        // Get the name and day
                        bool hasDay = int.TryParse(tsvData.DayColumn[currentConversationIndex], out int day);
                        string currentName = tsvData.CharacterIDColumn[currentConversationIndex];
                        // Add the proper file name to the list
                        string fileName = RawConversationData.GetFormattedName(currentName, hasDay ? day : null, out int parsedDay);
                        if (tsvConversations.Any(conv => conv.fileName == fileName))
                            Debug.LogWarning("Duplicate conversation. Name: " + fileName);
                        else
                            tsvConversations.Add(new ConversationRange(fileName, currentName, parsedDay, currentConversationIndex, i));
                    }
                    // We aren't in a conversation?
                    else
                    {
                        Debug.LogWarning("End marker not inside any conversation (index=" + i + ")");
                    }
                }
            }
        }

        void CreateConversations()
        {
            Conversation[] existingConvos = FindAllScriptableObjectsOfType<Conversation>(FilePaths.ConversationsFolderName);
            // Store a list of names so we don't overwrite existing convos
            HashSet<string> existingConvoNames = new HashSet<string>();
            foreach (Conversation convo in existingConvos)
                existingConvoNames.Add(convo.name);

            // Store all convo names we want to have
            List<string> allTSVConvoNames = new List<string>();
            foreach (ConversationRange convoRange in tsvConversations)
                allTSVConvoNames.Add(convoRange.fileName);

            // Store invalid convos
            List<RawConversationData> invalidConvos = new List<RawConversationData>();

            foreach (ConversationRange convoRange in tsvConversations)
            {
                // Skip over any conversations that already have assets
                if (existingConvoNames.Contains(convoRange.fileName))
                    continue;

                RawConversationData data = new RawConversationData(convoRange.name, convoRange.day);
                data.Validate(allTSVConvoNames);
                if (!data.IsValid)
                {
                    invalidConvos.Add(data);
                    continue;
                }

                data.
            }
        }

        Conversation CreateConversationAsset(RawConversationData data)
        {

        }


        public static void FillScriptableObjects<T>(Object owner, string arrayName) where T : ScriptableObject
        {
            SerializedObject ob = new SerializedObject(owner);
            ob.Update();
            SerializedProperty prop = ob.FindProperty(arrayName);
            T[] scrObjects = FindAllScriptableObjectsOfType<T>();
            prop.ClearArray();
            prop.arraySize = scrObjects.Length;
            for (int i = 0; i < scrObjects.Length; i++)
            {
                prop.GetArrayElementAtIndex(i).objectReferenceValue = scrObjects[i];
            }
            ob.ApplyModifiedProperties();
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
        }

        private static T[] FindAllScriptableObjectsOfType<T>(params string[] folders)
                where T : ScriptableObject
        {
            return AssetDatabase.FindAssets($"t:{typeof(T)}", folders)
                .Select(guid => AssetDatabase.LoadAssetAtPath<ScriptableObject>(AssetDatabase.GUIDToAssetPath(guid)))
                .Cast<T>().ToArray();
        }
    }

    // A range in the massive list of TSV data
    public class ConversationRange
    {
        public string fileName;
        public string name;
        public int day;
        public int start, end;

        public ConversationRange(string fileName, string name, int day, int start, int end)
        {
            this.fileName = fileName;
            this.name = name;
            this.day = day;
            this.start = start;
            this.end = end;
        }
    }
}
