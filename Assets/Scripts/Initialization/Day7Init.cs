using BeyondTheDoor.UI;
using BeyondTheDoor;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnitySceneManager = UnityEngine.SceneManagement.SceneManager;

public class Day7Init : DayBehaviour
{
    public ConversationCallback endOfDay;
    public ConversationCallback KilledByRaiders;

    protected override int GetDay() => 7;

    protected override void Initialize()
    {
        endOfDay.Callback += (conv, line) => EndGame();
        KilledByRaiders.Callback += (conv, line) => KilledRaiders();
    }

    private static void EndGame()
    {
        // Stop any advances
        DialogueGUI.ClearQueue();
        UnitySceneManager.LoadScene("Ending");
    }

    private void KilledRaiders()
    {
        foreach (var item in Cabin.CurrentPartyMembers())
        {
            if (item == Character.Raiders)
            {
                Character.Raiders.ChangeStatus(CharacterStatus.Left);
            }
            else
            {
                item.ChangeStatus(CharacterStatus.Kidnapped);
            }
        };
    }
}
