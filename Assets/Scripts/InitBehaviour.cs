using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BeyondTheDoor;

/// <summary>
/// Used to initialize pieces of the dialogue system for a specific day.
/// </summary>
/// <typeparam name="T"></typeparam>
/// <remarks>Basically wraps <seealso cref="Game"/> callbacks.</remarks>
public abstract class InitBehaviour<T> : MonoBehaviour
{
    private static InitBehaviour<T> instance;

    private int day;

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

        day = GetDay();
    }

    private void Start()
    {
        RegisterConversationCallbacks();

        AddGameCallbacks();
    }

    private void AddGameCallbacks()
    {
        Game.OnInitialize.AddListener(() => { if (Day.DayNumber == day) Initialize(); });
        Game.OnStageChanged.AddListener(() => { if (Day.DayNumber == day) StageChanged(); });
        Game.OnStageLoaded.AddListener(() => { if (Day.DayNumber == day) StageLoaded(); });
        //Game.OnNewDayStarted.AddListener(NewDayStarted);
        Game.OnGameExit.AddListener(() => { if (Day.DayNumber == day) GameExit(); });
        Game.OnDoorOpened.AddListener(() => { if (Day.DayNumber == day) DoorOpened(); });
        Game.OnDoorLeftClosed.AddListener(() => { if (Day.DayNumber == day) DoorLeftClosed(); });
    }


    /// <summary>
    /// Override and return the day that this init should be used for.
    /// </summary>
    /// <returns></returns>
    protected abstract int GetDay();

    /// <summary>
    /// Called only once when application is started.
    /// </summary>
    /// <remarks>Use this to subscribe to persistent events (like ConversationCallbacks)</remarks>
    protected virtual void RegisterConversationCallbacks() { }

    /// <summary>
    /// Called for this day before each stage is loaded.
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
    /*
    /// <summary>
    /// Called when a new day is started but before it is loaded.
    /// </summary>
    protected virtual void NewDayStarted() { }
    */
    /// <summary>
    /// Called when returning to the main menu from this day, but before the game is unloaded.
    /// </summary>
    protected virtual void GameExit() { }
    /// <summary>
    /// Called when the door is opened on this day, after the character's state is changed.
    /// </summary>
    protected virtual void DoorOpened() { }
    /// <summary>
    /// Called when the door is kept closed on this day, after the character's state is changed.
    /// </summary>
    protected virtual void DoorLeftClosed() { }

}
