using BeyondTheDoor;
using BeyondTheDoor.UI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnitySceneManager = UnityEngine.SceneManagement.SceneManager;

public class Day3Init : DayBehaviour
{
    public ConversationCallback activateBearEnding;
    public ConversationCallback BobKillerByBear;

    protected override int GetDay() => 3;

    protected override void Initialize()
    {
        activateBearEnding.Callback += (conv, line) => BearEnding();
        BobKillerByBear.Callback += (conv, line) => BobKilled();
    }

    private static void BearEnding()
    {
        // Stop any advances

        DialogueGUI.Close();
        UnitySceneManager.LoadScene("Ending");
    }

    private void BobKilled()
    {
        DialogueGUI.ClearQueue();
        Character.Bob.ChangeStatus(CharacterStatus.KilledByBear, true);
        Character.Bear.ChangeStatus(CharacterStatus.Left, true);

    }
}
