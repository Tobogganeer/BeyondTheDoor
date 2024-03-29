using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnitySceneManager = UnityEngine.SceneManagement.SceneManager;
using Scene = UnityEngine.SceneManagement.Scene;

public class SceneManager : MonoBehaviour
{
    public static Level CurrentLevel { get; private set; } = Level.MainMenu;

    private void Awake()
    {
        scenes.Clear();

        foreach (InspectorLevel level in levels)
            scenes.Add(level.level, level.scene);
    }

    [SerializeField] private InspectorLevel[] levels;

    private static Dictionary<Level, string> scenes = new Dictionary<Level, string>();

    public static void LoadLevel(Level level)
    {
        /*
        if (CurrentLevel == level)
        {
            Debug.Log($"Skipping level change to {level} as that is the current level");
            return;
        }
        */

        if (!scenes.TryGetValue(level, out string scene))
        {
            Debug.LogError($"Tried to load {level}, but there is not scene assigned to that level!");
            return;
        }

        CurrentLevel = level;
        UnitySceneManager.LoadScene(scene);
    }

    public static void ReloadCurrentLevel()
    {
        UnitySceneManager.LoadScene(UnitySceneManager.GetActiveScene().buildIndex);
    }


    private void OnValidate()
    {
        if (levels == null || levels.Length == 0) return;

        foreach (InspectorLevel level in levels)
        {
            level.name = level.scene ?? "Unset";
        }
    }
}

[System.Serializable]
public class InspectorLevel
{
    // Used to assign scenes to the enums in the inspector
    [HideInInspector]
    public string name; // Just for inspector
    public Level level; // The level enum
    [Scene] public string scene; // The actual scene
}
