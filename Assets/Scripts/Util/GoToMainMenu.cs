using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnitySceneManager = UnityEngine.SceneManagement.SceneManager;

public class GoToMainMenu : MonoBehaviour
{
    [Scene]
    public string mainMenuScene;

    // Used in case we start in the scene of a Stage
    void Start()
    {
        if (UnitySceneManager.GetActiveScene().name != mainMenuScene)
            UnitySceneManager.LoadScene(mainMenuScene);
    }
}
