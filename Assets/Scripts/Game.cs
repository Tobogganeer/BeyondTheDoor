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

    public static CharacterID[] CharacterArrivalOrder { get; private set; } =
    {
        CharacterID.Test
    };
    public static int DayNumber => Day.DayNumber;
    public static Stage Stage => Day.Stage;




    /// <summary>
    /// Starts the game given the loaded <paramref name="saveState"/>.
    /// </summary>
    /// <param name="saveState"></param>
    public static void Begin(SaveState saveState)
    {
        saveState.Load();
        Level currentLevel = GameStageToLevel(Day.Stage);
        SceneManager.LoadLevel(currentLevel);
    }

    /// <summary>
    /// Advances to the next stage of the day, if possible.
    /// </summary>
    public static void Advance()
    {

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
                break;
            case Stage.RadioLoreTime:
                break;
            case Stage.DealingWithArrival:
                break;
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