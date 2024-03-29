using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BeyondTheDoor;
using BeyondTheDoor.SaveSystem;
using UnityEngine.Events;

public class Game : MonoBehaviour
{
    private static Game instance;
    private void Awake()
    {
        instance = this;
        Settings.Load();
    }


    [Header("Input")]
    [SerializeField] private ConversationCallback advanceCallback;
    [SerializeField] private ConversationCallback openDoorCallback;
    [SerializeField] private ConversationCallback leaveDoorClosedCallback;

    [Header("Output")]
    [Tooltip("Called after a save file is loaded but before the stage is loaded.")]
    [SerializeField] private UnityEvent onInitialize;

    [Space]
    [Tooltip("Called after the game's stage is changed but before the next stage is loaded.")]
    [SerializeField] private UnityEvent onStageChanged;
    [Tooltip("Called after the game's stage is changed and loaded.")]
    [SerializeField] private UnityEvent onStageLoaded;

    [Space]
    [Tooltip("Called when a new day is started but before it is loaded.")]
    [SerializeField] private UnityEvent onNewDayStarted;

    [Space]
    [Tooltip("Called when returning to the main menu but before the game is unloaded.")]
    [SerializeField] private UnityEvent onGameExit;

    [Space]
    [SerializeField] private UnityEvent onDoorOpened;
    [SerializeField] private UnityEvent onDoorLeftClosed;


    public static Character[] CharacterArrivalOrder { get; private set; } =
    {
        Character.Tutorial_Mom,
        Character.Jessica,
        Character.Bob,
        Character.Bear,
        Character.Violet,
        Character.Dad,
        Character.Hal,
        Character.Raiders
    };
    /// <summary>
    /// What character is arriving today?
    /// </summary>
    public static Character ArrivingCharacter => CharacterArrivalOrder[DayNumber];
    public static int DayNumber => Day.DayNumber;
    public static Stage Stage => Day.Stage;

    /// <summary>
    /// Called after a save file is loaded but before the stage is loaded.
    /// </summary>
    public static UnityEvent OnInitialize => instance.onInitialize;
    /// <summary>
    /// Called after the game's stage is changed but before the next stage is loaded.
    /// </summary>
    public static UnityEvent OnStageChanged => instance.onStageChanged;
    /// <summary>
    /// Called after the game's stage is changed and loaded.
    /// </summary>
    public static UnityEvent OnStageLoaded => instance.onStageLoaded;
    /// <summary>
    /// Called when a new day is started but before it is loaded.
    /// </summary>
    public static UnityEvent OnNewDayStarted => instance.onNewDayStarted;
    /// <summary>
    /// Called when returning to the main menu but before the game is unloaded.
    /// </summary>
    public static UnityEvent OnGameExit => instance.onGameExit;
    /// <summary>
    /// Called when the door is opened, after the character's state is changed.
    /// </summary>
    public static UnityEvent OnDoorOpened => instance.onDoorOpened;
    /// <summary>
    /// Called when the door is kept closed, after the character's state is changed.
    /// </summary>
    public static UnityEvent OnDoorLeftClosed => instance.onDoorLeftClosed;


    private static uint currentSaveSlot;



    private void Start()
    {
        // Advance when a conversation wants to
        if (advanceCallback != null)
            advanceCallback.Callback += (conv, line) => Advance();
        if (openDoorCallback != null)
            openDoorCallback.Callback += (conv, line) => OpenDoor();
        if (leaveDoorClosedCallback != null)
            leaveDoorClosedCallback.Callback += (conv, line) => LeaveDoorClosed();
    }



    /// <summary>
    /// Starts the game given the loaded <paramref name="saveState"/>.
    /// </summary>
    /// <param name="saveState"></param>
    public static void Begin(SaveState saveState, uint saveSlot)
    {
        // Load the save
        saveState.Load();
        currentSaveSlot = saveSlot;

        OnInitialize?.Invoke();
        // Change scenes
        LoadStage(Day.Stage);
    }

    /// <summary>
    /// Advances to the next stage of the day, if possible. Saves progress.
    /// </summary>
    public static void Advance()
    {
        if (CanAdvance())
        {
            Day.Stage++;
            // Wrap around if we went too far
            if (Day.Stage > Stage.DealingWithArrival)
            {
                Day.StartDay(Day.DayNumber + 1);
                OnNewDayStarted?.Invoke();
            }
            else if (Day.Stage == Stage.DealingWithArrival)
            {
                // Make the character "arrive"
                ArrivingCharacter.ChangeStatus(CharacterStatus.AtDoor);
            }

            // Save the state after we switch stages (but before changing scenes)
            SaveState currentState = new SaveState();
            currentState.SaveCurrentState();
            ByteBuffer savedBuf = SaveSystem.Save(currentState, currentSaveSlot);
            savedBuf.Read<byte>(); // Skip over the version number

            // Load the new day (resets state)
            Begin(new SaveState(savedBuf), currentSaveSlot);
            //LoadStage(Day.Stage);
        }
    }

    /// <summary>
    /// Can we advance to the next stage currently?
    /// </summary>
    /// <returns></returns>
    public static bool CanAdvance()
    {
        switch (Stage)
        {
            case Stage.SpeakingWithParty:
                // You can always advance from speaking
                return true;
            case Stage.SendingScavengers:
                // You can always advance without sending anyone
                return true;
            case Stage.FixingOvercrowding:
                // You can advance after you fix the overcrowding
                return !Cabin.IsOvercrowded();
            case Stage.RadioLoreTime:
                // This stage is mainly just lore anyways
                return true;
            case Stage.DealingWithArrival:
                // You can advance once you've decided what to do with today's character
                return CharacterArrivalOrder[DayNumber].DoorDecisionMade;
        }

        throw new InvalidProgramException("Tried to check advancement on some goofy state that doesn't exist?");
    }

    private static void LoadStage(Stage stage)
    {
        OnStageChanged?.Invoke();
        Level level = GameStageToLevel(stage);
        SceneManager.LoadLevel(level);
        OnStageLoaded?.Invoke();
    }


    public static void OpenDoor()
    {
        ArrivingCharacter.ChangeStatus(CharacterStatus.InsideCabin);
        OnDoorOpened?.Invoke();
    }

    public static void LeaveDoorClosed()
    {
        ArrivingCharacter.ChangeStatus(CharacterStatus.LeftOutside);
        OnDoorLeftClosed?.Invoke();
    }


    public static void ExitToMenu()
    {
        OnGameExit?.Invoke();
        BeyondTheDoor.UI.DialogueGUI.Close(); // Turn off the GUI if it's playing a line
        Character.ResetAll(); // Reset them (doesn't really matter but meh)

        // Game is saved automatically
        SceneManager.LoadLevel(Level.MainMenu);
    }

    public static Level GameStageToLevel(Stage stage) => stage switch
    {
        Stage.SpeakingWithParty => Level.Morning,
        Stage.SendingScavengers => Level.Noon,
        Stage.FixingOvercrowding => Level.Afternoon,
        Stage.RadioLoreTime => Level.Evening,
        Stage.DealingWithArrival => Level.Door,
        _ => throw new NotImplementedException("Invalid stage: " + stage)
    };
}