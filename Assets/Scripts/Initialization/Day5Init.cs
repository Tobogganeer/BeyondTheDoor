using BeyondTheDoor.UI;
using BeyondTheDoor;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnitySceneManager = UnityEngine.SceneManagement.SceneManager;

public class Day5Init : DayBehaviour
{
    public ConversationCallback afterScavenging;

    protected override int GetDay() => 5;

    protected override void RegisterConversationCallbacks()
    {
        afterScavenging.Callback += (conv, line) => CheckForStarvation();
    }

    private static void CheckForStarvation()
    {
        // Stop any advances
        DialogueGUI.Close();
        if (!Cabin.HasScavengedSuccessfully)
            UnitySceneManager.LoadScene("Ending");
    }
}
