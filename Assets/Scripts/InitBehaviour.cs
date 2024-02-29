using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Used to initialize a piece of the dialogue system.
/// </summary>
/// <typeparam name="T"></typeparam>
public abstract class InitBehaviour<T> : MonoBehaviour
{
    private static InitBehaviour<T> instance;

    private void Awake()
    {
        // Make sure there is only one
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
            return;
        }
    }

    private void Start()
    {
        RegisterConversationCallbacks();

        Game.OnInitialize.AddListener(GameStart);
    }


    /// <summary>
    /// Called only once when application is started.
    /// </summary>
    /// <remarks>Use this to subscribe to persistent events (like ConversationCallbacks)</remarks>
    protected virtual void RegisterConversationCallbacks() { }
    /// <summary>
    /// Called when the game is loaded.
    /// </summary>
    /// <remarks>Initialize lines and character callbacks here.</remarks>
    protected abstract void GameStart();
}
