#if UNITY_EDITOR

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace ToBOE.Dialogue
{
    [CustomEditor(typeof(Conversation))]
    public class ConversationEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            Conversation con = (Conversation)target;

            EditorGUILayout.Space(10);
            EditorGUILayout.LabelField("======= CONVERSATION START =======", EditorStyles.largeLabel);
            if (con.lines != null)
            {
                if (con.lines.Count > 0)
                {
                    EditorGUILayout.LabelField("=== Character Lines ===", EditorStyles.boldLabel);
                    foreach (LineID line in con.lines)
                    {
                        DisplayLine(Line.Get(line));
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

            EditorGUILayout.LabelField("======= CONVERSATION END =======", EditorStyles.largeLabel);
        }

        void DisplayLine(Line line)
        {
            if (line == null)
                EditorGUILayout.SelectableLabel("null (codegen?)");
            else
                EditorGUILayout.SelectableLabel($"{line.character}: {line.text}");
        }

        void DisplayChoice(Conversation.ConversationChoice choice)
        {
            if (choice == null)
                EditorGUILayout.SelectableLabel("null choice");
            else
            {
                Line prompt = Line.Get(choice.prompt);
                if (prompt == null)
                {
                    EditorGUILayout.SelectableLabel("Invalid Choice");
                    return;
                }
                string text = prompt.text;
                text += "\n -> ";
                if (choice.nextConversation == null)
                    text += "Conversation Ends";
                else
                {
                    text += "Start " + choice.nextConversation.name;
                    if (choice.nextConversation.lines != null && choice.nextConversation.lines.Count > 0)
                    {
                        Line line = Line.Get(choice.nextConversation.lines[0]);
                        text += $"\n    -> {line.character}: {line.text}";
                        text += "\n    ...";
                    }
                }

                EditorGUILayout.SelectableLabel(text);
            }
        }
    }
}

#endif
