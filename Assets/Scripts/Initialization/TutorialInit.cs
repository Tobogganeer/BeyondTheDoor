using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BeyondTheDoor;

public class TutorialInit : DayBehaviour<TutorialInit>
{
    public Conversation wakeUpConvo;
    public Conversation cyaLaterConvo;
    public Conversation arrivingConvo;
    public ConversationCallback activateJokeEnding;


    protected override int GetDay() => 0;

    protected override void RegisterConversationCallbacks()
    {
        activateJokeEnding.Callback += ActivateJokeEnding;
    }

    protected override void Initialize()
    {
        // If you talk to them they'll just ask you where the keys are (does nothing)
        Character.Tutorial_Mom.SpokenTo += (mom) => Line.tutmom_0_looking_for_keys.Open();
        Character.Tutorial_Dad.SpokenTo += (dad) => Line.tutdad_0_looking_for_keys.Open();

        // Clicking either character has the same result
        Character.Tutorial_Mom.SendingToScavenge += SendToScavenge;
        Character.Tutorial_Dad.SendingToScavenge += SendToScavenge;

        Character.Tutorial_Mom.ArrivingAtDoor += ArrivingAtDoor;

        if (Stage == Stage.SpeakingWithParty)
        {
            // We just started the tutorial - add the mom and dad to the cabin
            Character.Tutorial_Mom.ChangeStatus(CharacterStatus.InsideCabin);
            Character.Tutorial_Dad.ChangeStatus(CharacterStatus.InsideCabin);
            // Start the game essentially
            wakeUpConvo.Start();
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

    private void SendToScavenge(Character momOrDad)
    {
        cyaLaterConvo.Start();
    }

    private void ArrivingAtDoor(Character mom)
    {
        arrivingConvo.Start();
    }


    private void ActivateJokeEnding(Conversation convo, LineID line)
    {
        // TODO: Actually activate ending
        Debug.Log("Joke ending silly");
        Game.ExitToMenu();
    }
}
