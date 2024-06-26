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

    protected override void RegisterConversationCallbacks()
    {
        activateBearEnding.Callback += (conv, line) => BearEnding();
        BobKillerByBear.Callback += (conv, line) => BobKilled();
    }

    private static void BearEnding()
    {
        Character.Jessica.ChangeStatus(CharacterStatus.KilledByBear);
        Character.Player.ChangeStatus(CharacterStatus.KilledByBear);
        // Stop any advances
        DialogueGUI.Close();
        UnitySceneManager.LoadScene("Ending");

    }

    private void BobKilled()
    {
        Character.Bob.ChangeStatus(CharacterStatus.KilledByBear);
        Character.Bear.ChangeStatus(CharacterStatus.Left);
    }
}
