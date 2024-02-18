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
    AudioCategory category = AudioCategory.Dialogue;

    bool batchMode = true;

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
        {
            EditorGUILayout.LabelField("Please choose at least 1 audio clip.");
            return;
        }

        // Make them choose a name and batch mode setting
        if (clips.Count > 1)
        {
            batchMode = EditorGUILayout.Toggle("Batch/Multiple sounds", batchMode);
            if (!batchMode)
            {
                EditorGUILayout.LabelField("All clips will be part of one sound. Name it:");
                fileName = EditorGUILayout.TextField("Sound ID", fileName);
            }
        }

        if (clips.Count > 0 && clips[0] != null)
            DisplaySaveGUI();
    }

    void DisplaySaveGUI()
    {
        // They need a filename if there are multiple sounds in one file
        if (clips.Count > 1 && !batchMode && string.IsNullOrEmpty(fileName))
        {
            EditorGUILayout.LabelField("Please choose a valid name");
            return;
        }

        string path = Combine(BasePath, pathExtension ?? string.Empty);
        if (clips.Count == 1)
            fileName = clips[0].name;
        if (clips.Count == 1 || !batchMode)
            DisabledLabel("Asset Path", Combine(path, fileName));
        else
            DisabledLabel("Asset Path", Combine(path, "(multiple)"));

        EditorGUILayout.Space();
        is2D = EditorGUILayout.Toggle("Sound is 2D", is2D);
        category = (AudioCategory)EditorGUILayout.EnumPopup("Audio Category", category);
        if (clips.Count == 1 || !batchMode)
        {
            if (GUILayout.Button("Save sound"))
                Save(path, fileName);
        }
        else if (GUILayout.Button($"Save {clips.Count} sounds"))
            SaveBatch(path);
    }

    void Save(string path, string fileName)
    {
        fileName += ".asset";

        Sound s = Sound.CreateInternal(clips, is2D, category);
        AssetDatabase.CreateAsset(s, Combine(path, fileName));
        AssetDatabase.Refresh();
        FillSounds();
    }

    void SaveBatch(string path)
    {
        for (int i = 0; i < clips.Count; i++)
        {
            List<AudioClip> _new = new List<AudioClip>();
            _new.Add(clips[i]);
            Sound s = Sound.CreateInternal(_new, is2D, category);
            AssetDatabase.CreateAsset(s, Combine(path, clips[i].name + ".asset"));
        }
        AssetDatabase.Refresh();
        FillSounds();
    }

    void DisabledLabel(string label, string text)
    {
        GUI.enabled = false;
        EditorGUILayout.TextField(label, text);
        GUI.enabled = true;
    }
}

#endif