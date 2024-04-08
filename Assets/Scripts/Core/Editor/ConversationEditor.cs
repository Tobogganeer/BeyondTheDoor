#if UNITY_EDITOR

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System;

namespace BeyondTheDoor.Editor
{
    [CustomEditor(typeof(Conversation))]
    public class ConversationEditor : UnityEditor.Editor
    {
        GUIStyle style;

        public override void OnInspectorGUI()
        {
            if (style == null)
            {
                style = new GUIStyle(EditorStyles.textArea);
                style.richText = true;
            }

            Conversation con = (Conversation)target;
            serializedObject.Update();

            EditorGUILayout.PropertyField(serializedObject.FindProperty(nameof(Conversation.onStarted)));
            EditorGUILayout.PropertyField(serializedObject.FindProperty(nameof(Conversation.elements)), true);
            DrawElementAddButtons();

            EditorGUILayout.PropertyField(serializedObject.FindProperty(nameof(Conversation.onFinished)));
            //EditorGUILayout.PropertyField(serializedObject.FindProperty(nameof(Conversation.nextConversation)));

            // Grey out choices if this conversation just moves on
            //if (con.nextConversation != null)
            //    GUI.enabled = false;
            EditorGUILayout.PropertyField(serializedObject.FindProperty(nameof(Conversation.choices)), true);
            //GUI.enabled = true;

            serializedObject.ApplyModifiedProperties();

            //base.OnInspectorGUI();

            EditorGUILayout.Space(10);
            EditorGUILayout.LabelField("======= CONVERSATION START =======", EditorStyles.largeLabel);
            GUI.enabled = false;

            if (con.onStarted != null)
            {
                string text = EditorColours.Text("Calls ");
                text += EditorColours.Callback(con.onStarted.name);
                EditorGUILayout.TextArea(text, style);
            }

            if (con.elements != null)
            {
                if (con.elements.Count > 0)
                {
                    EditorGUILayout.LabelField("=== Conversation Elements ===", EditorStyles.boldLabel);
                    foreach (IConversationElement element in con.elements)
                    {
                        DisplayElement(element);
                    }
                }
            }

            /*
            if (con.nextConversation != null)
            {
                string text = "";

                text += $"<color={EditorColours.TextColour}>Start Conversation</color> " +
                        $"'<color={EditorColours.ConversationNameColour}>" +
                        //$"<a href=\"{AssetDatabase.GetAssetPath(choice.nextConversation)}\">" +
                        con.nextConversation.name + "</color>'";
                if (con.nextConversation.lines != null && con.nextConversation.lines.Count > 0)
                {
                    Line line = Line.Get(con.nextConversation.lines[0]);
                    text += $"\n  -> {FormatCharacterMessage(line)}";
                    text += "\n  ... (cont'd)";
                }
                else
                {
                    text += "\n  (no lines)";
                }

                EditorGUILayout.TextArea(text, style);
            }
            */
            //else
            //{
            if (con.choices != null && con.choices.Count > 0)
            {
                EditorGUILayout.LabelField("=== Player Choices ===", EditorStyles.boldLabel);
                foreach (ConversationChoice choice in con.choices)
                {
                    DisplayChoice(choice);
                }
            }
            //}

            if (con.onFinished != null)
            {
                string text = EditorColours.Text("Calls ");
                text += EditorColours.Callback(con.onFinished.name);
                EditorGUILayout.TextArea(text, style);
                if (con.choices != null && con.choices.Count > 0)
                    text += EditorColours.Text(" (Once choice is selected)");
                EditorGUILayout.TextArea(text, style);
            }

            GUI.enabled = true;
            EditorGUILayout.LabelField("======= CONVERSATION END =======", EditorStyles.largeLabel);
        }

        private void DisplayElement(IConversationElement element)
        {
            if (element == null)
                EditorGUILayout.TextArea("null (codegen?)");
            else
            {
                if (element is DialogueElement dialogue)
                    DisplayLine(Line.Get(dialogue.lineID));
                else if (element is IfElement _if)
                    DisplayConditional("IF " + _if.condition + ":");
                else if (element is ElifElement _elif)
                    DisplayConditional("ELIF " + _elif.condition + ":");
                else if (element is ElseElement)
                    DisplayConditional("ELSE:");
                else if (element is EndIfElement)
                    DisplayConditional("ENDIF");
                else if (element is GotoElement _goto)
                    DisplayGoto(_goto);

                EditorGUILayout.Space(3f);
            }
        }

        private void DrawElementAddButtons()
        {
            throw new NotImplementedException();
        }

        void DisplayLine(Line line)
        {
            if (line == null)
                EditorGUILayout.TextArea("null (codegen?)");
            else
                EditorGUILayout.TextArea(FormatCharacterMessage(line), style);
        }

        void DisplayConditional(string text)
        {
            EditorGUILayout.TextArea(EditorColours.Conditional(text), style);
        }

        void DisplayGoto(GotoElement _goto)
        {
            string text = EditorColours.Goto("GOTO -> ") + "'" +
                EditorColours.ConversationName(_goto.conversation.name) + "'";
            EditorGUILayout.TextArea(text, style);
        }

        void DisplayChoice(ConversationChoice choice)
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
                string text = EditorColours.ChoicePrompt(prompt.text);
                if (choice.callback != null)
                {
                    text += "\n -> ";
                    text += EditorColours.Text("Calls ");
                    text += EditorColours.Callback(choice.callback.name);
                }

                text += "\n -> ";
                if (choice.nextConversation == null)
                    text += EditorColours.Text("Conversation Ends");
                else
                {
                    text += EditorColours.Text("Start Conversation") + "'" +
                        EditorColours.ConversationName(choice.nextConversation.name) + "'";
                    if (choice.nextConversation.elements != null && choice.nextConversation.elements.Count > 0)
                    {
                        text += $"\n    -> ";
                        DisplayElement(choice.nextConversation.elements[0]);
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
