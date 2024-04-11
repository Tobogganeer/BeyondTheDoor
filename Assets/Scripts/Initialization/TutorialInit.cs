using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BeyondTheDoor;

public class TutorialInit : DayBehaviour
{
    public ConversationCallback jokeEnding;


    protected override int GetDay() => 0;

    protected override void RegisterConversationCallbacks()
    {
        jokeEnding.Callback += ActivateJokeEnding;
    }

    protected override void Initialize()
    {
        if (Stage == Stage.SpeakingWithParty)
        {
            // We just started the tutorial - add the mom and dad to the cabin
            Character.Tutorial_Mom.ChangeStatus(CharacterStatus.InsideCabin);
            Character.Tutorial_Dad.ChangeStatus(CharacterStatus.InsideCabin);
        }
    }

    protected override void DoorOpened()
    {
        // The mom will automatically have the status set, the dad will not
        Character.Tutorial_Dad.ChangeStatus(CharacterStatus.InsideCabin);
    }

    protected override void DoorLeftClosed()
    {
        // The mom will automatically have the status set, the dad will not
        Character.Tutorial_Dad.ChangeStatus(CharacterStatus.LeftOutside);
    }

    private void ActivateJokeEnding(Conversation convo, LineID line)
    {
        // TODO: Actually activate ending
        Debug.Log("Joke ending silly");
        Game.ExitToMenu();
    }
}
