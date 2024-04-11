using BeyondTheDoor.UI;
using BeyondTheDoor;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnitySceneManager = UnityEngine.SceneManagement.SceneManager;

public class Day5Init : DayBehaviour
{
    public ConversationCallback afterScavenging;

    public ConversationCallback bobChoppingBlock;
    public ConversationCallback jessicaChoppingBlock;
    public ConversationCallback violetChoppingBlock;
    public ConversationCallback halChoppingBlock;
    public ConversationCallback salChoppingBlock;

    protected override int GetDay() => 5;

    protected override void RegisterConversationCallbacks()
    {
        afterScavenging.Callback += (conv, line) => CheckForStarvation();

        bobChoppingBlock.Callback += (conv, line) => Character.ChoppingBlock = Character.Bob;
        jessicaChoppingBlock.Callback += (conv, line) => Character.ChoppingBlock = Character.Jessica;
        violetChoppingBlock.Callback += (conv, line) => Character.ChoppingBlock = Character.Violet;
        halChoppingBlock.Callback += (conv, line) => Character.ChoppingBlock = Character.Hal;
        salChoppingBlock.Callback += (conv, line) => Character.ChoppingBlock = Character.Sal;
    }

    private static void CheckForStarvation()
    {
        // Stop any advances
        DialogueGUI.Close();
        if (!Cabin.HasScavengedSuccessfully)
            UnitySceneManager.LoadScene("Ending");
    }
}
