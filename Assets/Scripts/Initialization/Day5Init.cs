using BeyondTheDoor.UI;
using BeyondTheDoor;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnitySceneManager = UnityEngine.SceneManagement.SceneManager;

public class Day5Init : DayBehaviour
{
    public ConversationCallback endOfDay;

    protected override int GetDay() => 5;

    protected override void Initialize()
    {
        endOfDay.Callback += (conv, line) => EndGame();
    }

    private static void EndGame()
    {
        // Stop any advances
        DialogueGUI.ClearQueue();
        UnitySceneManager.LoadScene("Ending");
    }
}
