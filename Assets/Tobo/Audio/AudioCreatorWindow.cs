#if UNITY_EDITOR

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using static System.IO.Path;

// This is gonna be a quick and jank class
public class AudioCreatorWindow : EditorWindow
{
    static readonly string BasePath = "Assets/Sounds/";
    string pathExtension = string.Empty;

    [SerializeField]
    List<AudioClip> clips = new List<AudioClip>();
    string fileName;
    bool is2D = true;

    SerializedObject target;


    [MenuItem("Audio/Fill Sounds")]
    public static void FillSounds()
    {
        LibraryUtil.FillLibrary<SoundLibrary, Sound>(nameof(SoundLibrary.sounds));
    }

    [MenuItem("Audio/Sound Creator")]
    public static void ShowWindow()
    {
        AudioCreatorWindow window = EditorWindow.GetWindow<AudioCreatorWindow>();
        window.titleContent = new GUIContent("Sound Creator");
        window.minSize = new Vector2(300, 450);
    }

    private void OnEnable()
    {
        target = new SerializedObject(this);
    }

    private void OnGUI()
    {
        DisabledLabel("Base path:", BasePath);
        // If users want to put this into a sub-folder
        pathExtension = EditorGUILayout.TextField("Path Extension", pathExtension);
        DisabledLabel("Folder path:", Combine(BasePath, pathExtension ?? string.Empty));

        // Edit clips list
        target.Update();
        SerializedProperty prop = target.FindProperty(nameof(clips));
        EditorGUILayout.PropertyField(prop, true);
        target.ApplyModifiedProperties();

        if (clips.Count == 0)
            EditorGUILayout.LabelField("Please choose at least 1 audio clip.");
        else if (clips.Count == 1)
        {
            // Use the clip name
        }
        else
        {
            // Make them choose a name
        }
    }

    void DisplaySaveGUI()
    {

    }

    void DisabledLabel(string label, string text)
    {
        GUI.enabled = false;
        EditorGUILayout.TextField(label, text);
        GUI.enabled = true;
    }
}

#endif