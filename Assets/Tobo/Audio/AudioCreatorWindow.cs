#if UNITY_EDITOR

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

// This is gonna be a quick and jank class
public class AudioCreatorWindow : EditorWindow
{
    bool is2D = true;
    List<AudioClip> clips = new List<AudioClip>();


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


}

#endif