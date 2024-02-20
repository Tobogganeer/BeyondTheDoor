#if UNITY_EDITOR

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace BeyondTheDoor.Editor
{
    [CustomEditor(typeof(Conversation))]
    public class ConversationEditor : UnityEditor.Editor
    {
        GUIStyle style;

        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            if (style == null)
            {
                style = new GUIStyle(EditorStyles.textArea);
                style.richText = true;
            }

            Conversation con = (Conversation)target;

            EditorGUILayout.Space(10);
            EditorGUILayout.LabelField("======= CONVERSATION START =======", EditorStyles.largeLabel);
            GUI.enabled = false;

            if (con.lines != null)
            {
                if (con.lines.Count > 0)
                {
                    EditorGUILayout.LabelField("=== Character Lines ===", EditorStyles.boldLabel);
                    foreach (LineID line in con.lines)
                    {
                        DisplayLine(Line.Get(line));
                        EditorGUILayout.Space(3f);
                    }
                }
            }

            if (con.choices != null && con.choices.Count > 0)
            {
                EditorGUILayout.LabelField("=== Player Choices ===", EditorStyles.boldLabel);
                foreach (Conversation.ConversationChoice choice in con.choices)
                {
                    DisplayChoice(choice);
                }
            }

            GUI.enabled = true;
            EditorGUILayout.LabelField("======= CONVERSATION END =======", EditorStyles.largeLabel);
        }

        void DisplayLine(Line line)
        {
            if (line == null)
                EditorGUILayout.TextArea("null (codegen?)");
            else
                EditorGUILayout.TextArea(FormatCharacterMessage(line), style);
        }

        void DisplayChoice(Conversation.ConversationChoice choice)
        {
            if (choice == null)
                EditorGUILayout.TextArea("null choice");
            else
            {
                Line prompt = Line.Get(choice.prompt);
                if (prompt == null)
                {
                    EditorGUILayout.TextArea("Invalid Choice");
                    return;
                }
                string text = $"<color={EditorColours.ChoicePromptColour}>{prompt.text}</color>";
                if (choice.callback != null)
                {
                    text += "\n -> ";
                    text += $"<color={EditorColours.TextColour}>Calls </color>";
                    text += $"<color={EditorColours.CallbackColour}>{choice.callback.name}</color>";
                }

                text += "\n -> ";
                if (choice.nextConversation == null)
                    text += $"<color={EditorColours.TextColour}>Conversation Ends</color>";
                else
                {
                    text += $"<color={EditorColours.TextColour}>Start Conversation</color> " +
                        $"'<color={EditorColours.ConversationNameColour}>" +
                        //$"<a href=\"{AssetDatabase.GetAssetPath(choice.nextConversation)}\">" +
                        choice.nextConversation.name + "</color>'";
                    if (choice.nextConversation.lines != null && choice.nextConversation.lines.Count > 0)
                    {
                        Line line = Line.Get(choice.nextConversation.lines[0]);
                        text += $"\n    -> {FormatCharacterMessage(line)}";
                        text += "\n    ... (cont'd)";
                    }
                    else
                    {
                        text += "\n    (no lines)";
                    }
                }

                EditorGUILayout.TextArea(text, style);
            }
        }

        string FormatCharacterMessage(Line line)
        {
            return $"<color={EditorColours.CharacterNameColour}>{line.character}:</color> " +
                $"<color={EditorColours.TextColour}>{line.text}</color>";
        }
    }
}

#endif
