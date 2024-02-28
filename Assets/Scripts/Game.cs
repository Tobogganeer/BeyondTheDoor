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
    }

    /// <summary>
    /// Starts the game given the loaded <paramref name="gameState"/>.
    /// </summary>
    /// <param name="gameState"></param>
    public static void Begin(SaveState gameState)
    {
        gameState.Load();
        Level currentLevel = GameStageToLevel(Day.Stage);
        SceneManager.LoadLevel(currentLevel);
    }

    public static CharacterID[] CharacterArrivalOrder { get; private set; } =
    {
        CharacterID.Test
    };

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