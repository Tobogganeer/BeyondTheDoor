using BeyondTheDoor;
using BeyondTheDoor.UI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnitySceneManager = UnityEngine.SceneManagement.SceneManager;

public class Day3Init : DayBehaviour
{
    public ConversationCallback activateBearEnding;

    protected override int GetDay() => 3;

    protected override void Initialize()
    {
        activateBearEnding.Callback += (conv, line) => BearEnding();
    }

    private static void BearEnding()
    {
        // Stop any advances

        DialogueGUI.ClearQueue();
        UnitySceneManager.LoadScene("Ending");
    }
}
