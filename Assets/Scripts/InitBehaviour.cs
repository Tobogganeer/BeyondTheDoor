using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Used to initialize pieces of the dialogue system.
/// </summary>
/// <typeparam name="T"></typeparam>
/// <remarks>Basically wraps <seealso cref="Game"/> callbacks</remarks>
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

        AddGameCallbacks();
    }

    private void AddGameCallbacks()
    {
        Game.OnInitialize.AddListener(Initialize);
        Game.OnStageChanged.AddListener(StageChanged);
        Game.OnStageLoaded.AddListener(StageLoaded);
        Game.OnNewDayStarted.AddListener(NewDayStarted);
        Game.OnGameExit.AddListener(GameExit);
        Game.OnDoorOpened.AddListener(DoorOpened);
        Game.OnDoorLeftClosed.AddListener(DoorLeftClosed);
    }


    /// <summary>
    /// Called only once when application is started.
    /// </summary>
    /// <remarks>Use this to subscribe to persistent events (like ConversationCallbacks)</remarks>
    protected virtual void RegisterConversationCallbacks() { }

    /// <summary>
    /// Called after a save file is loaded but before the stage is loaded.
    /// </summary>
    /// <remarks>Initialize lines and character callbacks here.</remarks>
    protected abstract void Initialize();
    /// <summary>
    /// Called after the game's stage is changed but before the next stage is loaded.
    /// </summary>
    protected virtual void StageChanged() { }
    /// <summary>
    /// Called after the game's stage is changed and loaded.
    /// </summary>
    protected virtual void StageLoaded() { }
    /// <summary>
    /// Called when a new day is started but before it is loaded.
    /// </summary>
    protected virtual void NewDayStarted() { }
    /// <summary>
    /// Called when returning to the main menu but before the game is unloaded.
    /// </summary>
    protected virtual void GameExit() { }
    /// <summary>
    /// Called when the door is opened, after the character's state is changed.
    /// </summary>
    protected virtual void DoorOpened() { }
    /// <summary>
    /// Called when the door is kept closed, after the character's state is changed.
    /// </summary>
    protected virtual void DoorLeftClosed() { }

}
