using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using ToBOE.Dialogue.Importer.CodeGen;

namespace ToBOE.Dialogue.Importer
{
    public class DialogueImportWindow : EditorWindow
    {
        TextAsset tsvFile;
        //bool generateCharactersEnum;
        //bool generateStatusEnum;
        //bool generateIDEnum = true;

        [MenuItem("Dialogue/Import Window")]
        public static void ShowWindow()
        {
            DialogueImportWindow window = EditorWindow.GetWindow<DialogueImportWindow>();
            window.titleContent = new GUIContent("Import Dialogue");
            window.minSize = new Vector2(300, 400);
        }

        private void OnGUI()
        {
            tsvFile = EditorGUILayout.ObjectField("Lines File", tsvFile, typeof(TextAsset), false) as TextAsset;
            //generateCharactersEnum = EditorGUILayout.Toggle("Generate Characters Enum", generateCharactersEnum);
            //generateStatusEnum = EditorGUILayout.Toggle("Generate Status Enum", generateStatusEnum);
            //generateIDEnum = EditorGUILayout.Toggle("Generate LineID Enum", generateIDEnum);
        }
    }
}
