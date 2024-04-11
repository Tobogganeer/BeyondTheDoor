using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BeyondTheDoor;
using UnitySceneManager = UnityEngine.SceneManagement.SceneManager;

public class TutorialInit : DayBehaviour
{
    public ConversationCallback jokeEnding;
    public ConversationCallback bringParentsInside;

    protected override int GetDay() => 0;

    protected override void RegisterConversationCallbacks()
    {
        jokeEnding.Callback += ActivateJokeEnding;
        bringParentsInside.Callback += (conv, line) => DoorOpened();
    }

    protected override void Initialize()
    {
        if (Stage == Stage.MorningSupplies)
        {
            // We just started the tutorial - add the mom and dad to the cabin
            Character.Tutorial_Mom.ChangeStatus(CharacterStatus.InsideCabin);
            Character.Tutorial_Dad.ChangeStatus(CharacterStatus.InsideCabin);
        }
        // Skip this stage
        else if (Stage == Stage.SpeakingWithParty)
            Game.Advance();

        Character.Player.ChangeStatus(CharacterStatus.InsideCabin);
    }

    protected override void DoorOpened()
    {
        // The mom will automatically have the status set, the dad will not
        Character.Tutorial_Mom.ChangeStatus(CharacterStatus.InsideCabin);
        Character.Tutorial_Dad.ChangeStatus(CharacterStatus.InsideCabin);
    }

    protected override void DoorLeftClosed()
    {
        // The mom will automatically have the status set, the dad will not
        Character.Tutorial_Mom.ChangeStatus(CharacterStatus.LeftOutside);
        Character.Tutorial_Dad.ChangeStatus(CharacterStatus.LeftOutside);
    }

    private void ActivateJokeEnding(Conversation convo, LineID line)
    {
        // Set them outside
        DoorLeftClosed();
        UnitySceneManager.LoadScene("Ending");
    }
}
