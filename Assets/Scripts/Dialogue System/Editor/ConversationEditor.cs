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

            EditorGUILayout.Space();
            EditorGUILayout.LabelField("=== TEXT TREE ===", EditorStyles.boldLabel);
            foreach (Line line in con.Lines)
            {
                DisplayLine(line);
            }
        }

        void DisplayLine(Line line)
        {
            EditorGUILayout.SelectableLabel($"{line.character}: {line.text}");
        }
    }
}

#endif
