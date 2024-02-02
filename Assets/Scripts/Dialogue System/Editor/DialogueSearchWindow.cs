#if UNITY_EDITOR

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEditor.UIElements;

namespace ToBOE.Dialogue
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
