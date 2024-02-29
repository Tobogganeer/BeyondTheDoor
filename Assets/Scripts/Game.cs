using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BeyondTheDoor;
using BeyondTheDoor.SaveSystem;

public class Game : MonoBehaviour
{
    private static Game instance;
    private void Awake()
    {
        instance = this;
        Settings.Load();
    }

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
    public static int DayNumber => Day.DayNumber;
    public static Stage Stage => Day.Stage;

    private static uint currentSaveSlot;




    /// <summary>
    /// Starts the game given the loaded <paramref name="saveState"/>.
    /// </summary>
    /// <param name="saveState"></param>
    public static void Begin(SaveState saveState, uint saveSlot)
    {
        // Load the save
        saveState.Load();
        currentSaveSlot = saveSlot;
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
                Day.Stage = Stage.SpeakingWithParty;

            // Save the state after we switch stages (but before changing scenes)
            SaveState currentState = new SaveState();
            currentState.SaveCurrentState();
            SaveSystem.Save(currentState, currentSaveSlot);

            // Load the new day
            LoadStage(Day.Stage);
        }
    }

    private static void LoadStage(Stage stage)
    {
        Level level = GameStageToLevel(stage);
        SceneManager.LoadLevel(level);
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