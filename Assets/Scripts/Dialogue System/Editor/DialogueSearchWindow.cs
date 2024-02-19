#if UNITY_EDITOR

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace BeyondTheDoor.Editor
{
    public class DialogueSearchWindow : EditorWindow
    {
        Line.Element searchElements = Line.Element.CharacterID;

        // Could use a line to hold this but idc
        CharacterID character;
        string text;
        string context;
        int day;
        LineStatus lineStatus;
        VoiceStatus voiceStatus;
        string extraData;

        List<Line> lines;
        List<bool> foldouts;

        GUIStyle foldoutStyleRich;
        GUIStyle textFieldRich;

        /*
            CharacterID = 1 << 0,
            Text = 1 << 1,
            Context = 1 << 2,
            Day = 1 << 3,
            // LineID = 1 << 4,
            LineStatus = 1 << 5,
            VoiceStatus = 1 << 6,
            ExtraData = 1 << 7
        */

        [MenuItem("Dialogue/Search Lines")]
        public static void ShowWindow()
        {
            DialogueSearchWindow window = EditorWindow.GetWindow<DialogueSearchWindow>();
            window.titleContent = new GUIContent("Search Lines");
            window.minSize = new Vector2(300, 450);
        }

        private void OnGUI()
        {
            if (foldoutStyleRich == null || textFieldRich == null)
            {
                foldoutStyleRich = new GUIStyle(EditorStyles.foldout);
                foldoutStyleRich.richText = true;

                textFieldRich = new GUIStyle(EditorStyles.textField);
                textFieldRich.richText = true;
            }

            searchElements = (Line.Element)EditorGUILayout.EnumFlagsField("Search Filters", searchElements);

            if (searchElements == Line.Element.None)
            {
                EditorGUILayout.LabelField("Please select a filter to search.");
                GUI.enabled = false;
            }
            else
            {
                DisplayFilterFields();
            }

            if (GUILayout.Button("Search"))
            {
                lines = new List<Line>(LineSearch.Filter(Line.All.Values, searchElements,
                    character, text, context, day, lineStatus, voiceStatus, extraData));

                // Make all lines collapsed by default
                foldouts = new List<bool>(lines.Count);
                for (int i = 0; i < lines.Count; i++)
                    foldouts.Add(false);
            }

            GUI.enabled = false;

            if (lines != null)
            {
                DisplayLines();
            }

            GUI.enabled = true;
        }

        void DisplayFilterFields()
        {
            if (searchElements.HasFlag(Line.Element.CharacterID))
                character = (CharacterID)EditorGUILayout.EnumPopup("Character", character);
            if (searchElements.HasFlag(Line.Element.Text))
                text = EditorGUILayout.TextField("Text", text);
            if (searchElements.HasFlag(Line.Element.Context))
                context = EditorGUILayout.TextField("Context", context);
            if (searchElements.HasFlag(Line.Element.Day))
                day = EditorGUILayout.IntSlider("Day", day, 0, 8);
            if (searchElements.HasFlag(Line.Element.LineStatus))
                lineStatus = (LineStatus)EditorGUILayout.EnumPopup("Line Status", lineStatus);
            if (searchElements.HasFlag(Line.Element.VoiceStatus))
                voiceStatus = (VoiceStatus)EditorGUILayout.EnumPopup("Voice Status", voiceStatus);
            if (searchElements.HasFlag(Line.Element.ExtraData))
                extraData = EditorGUILayout.TextField("Extra Data", extraData);
        }

        void DisplayLines()
        {
            if (lines.Count == 0)
            {
                EditorGUILayout.TextField("No matching lines found.");
            }
            else
            {
                for (int i = 0; i < lines.Count; i++)
                    DisplayLine(i);
            }
        }

        void DisplayLine(int index)
        {
            Line line = lines[index];
            string foldoutText = White($"{line.character} (day {line.day}) - {line.id}");
            foldouts[index] = EditorGUILayout.Foldout(foldouts[index], foldoutText, foldoutStyleRich);
            // Draw the rest of the owl
            if (foldouts[index])
            {
                EditorGUI.indentLevel++;
                /*
                CharacterID character;
                string text;
                string context;
                int day;
                LineID id;
                LineStatus lineStatus;
                VoiceStatus voiceStatus;
                string extraData;
                */
                EditorGUILayout.TextField(DetailedEntry("Character", line.character.ToString()), textFieldRich);
                EditorGUILayout.TextField(DetailedEntry("Text", line.text), textFieldRich);
                EditorGUILayout.TextField(DetailedEntry("Context", line.context), textFieldRich);
                EditorGUILayout.TextField(DetailedEntry("Day", line.day.ToString()), textFieldRich);
                EditorGUILayout.TextField(DetailedEntry("LineID", line.id.ToString()), textFieldRich);
                EditorGUILayout.TextField(DetailedEntry("LineStatus", line.lineStatus.ToString()), textFieldRich);
                EditorGUILayout.TextField(DetailedEntry("VoiceStatus", line.voiceStatus.ToString()), textFieldRich);
                EditorGUILayout.TextField(DetailedEntry("ExtraData", line.extraData), textFieldRich);
                EditorGUI.indentLevel--;
            }
        }

        string White(string text)
        {
            return $"<color={EditorColours.TextColour}>{text}</color>";
        }

        string DetailedEntry(string header, string value)
        {
            // This line sucks lol I hate it
            return $"<color={EditorColours.CharacterNameColour}>{header}:</color> " +
                $"<color={EditorColours.TextColour}>{value}</color>";
        }

        /*
            CharacterID = 1 << 0,
            Text = 1 << 1,
            Context = 1 << 2,
            Day = 1 << 3,
            // LineID = 1 << 4,
            LineStatus = 1 << 5,
            VoiceStatus = 1 << 6,
            ExtraData = 1 << 7
        */
    }
}

#endif
