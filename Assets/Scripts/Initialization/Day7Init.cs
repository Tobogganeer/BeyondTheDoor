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

    protected override void RegisterConversationCallbacks()
    {
        endOfDay.Callback += (conv, line) => EndGame();
        KilledByRaiders.Callback += (conv, line) => GroupKilledByRaiders();
    }

    private void EndGame()
    {
        // Stop any advances
        DialogueGUI.Close();
        CalculateEndStates();
        UnitySceneManager.LoadScene("Ending");
    }

    private void CalculateEndStates()
    {
        foreach (Character character in Cabin.CurrentPartyMembers())
        {
            character.ChangeStatus(CharacterStatus.AliveAtBorder);
        }

        if (Cabin.NumCurrentPartyMembers() > 0)
            // They made it with the group
            Character.Player.ChangeStatus(CharacterStatus.AliveAtBorder);
        else if (Character.Dad.Status == CharacterStatus.LeftWithShotgun)
        {
            // Saved by the Dad
            Character.Player.ChangeStatus(CharacterStatus.AliveAtBorder);
            Character.Dad.ChangeStatus(CharacterStatus.AliveAtBorder);
        }
        else
            // Womp womp
            Character.Player.ChangeStatus(CharacterStatus.DeadOnWayToBorder);
    }


    private void GroupKilledByRaiders()
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
