using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BeyondTheDoor;

public class TutorialInit : InitBehaviour<TutorialInit>
{
    public Conversation wakeUpConvo;
    public LineID momLookingForKeys;
    public LineID dadLookingForKeys;

    protected override void Initialize()
    {
        // If you talk to them they'll just ask you where the keys are
        Character.Tutorial_Mom.SpokenTo += (mom) => Line.Get(momLookingForKeys).Open();
        Character.Tutorial_Dad.SpokenTo += (mom) => Line.Get(dadLookingForKeys).Open();

        Character.Tutorial_Mom.SendingToScavenge += SendToScavenge;

        if (Day.DayNumber == Day.TutorialDay && Day.Stage == Stage.SpeakingWithParty)
        {
            // We just started the tutorial - add the mom and dad to the cabin
            Character.Tutorial_Mom.ChangeStatus(CharacterStatus.InsideCabin);
            Character.Tutorial_Dad.ChangeStatus(CharacterStatus.InsideCabin);
            // Start the game essentially
            wakeUpConvo.Start();
        }
    }

    private void SendToScavenge(Character momOrDad)
    {
        throw new System.NotImplementedException();
    }
}
