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
        Game.OnInitialize.AddListener(Initialize);
    }

    /// <summary>
    /// Called when the game is loaded.
    /// </summary>
    /// <remarks>Initialize lines here.</remarks>
    protected abstract void Initialize();
}
