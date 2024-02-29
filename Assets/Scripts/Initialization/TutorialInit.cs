using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BeyondTheDoor;

public class TutorialInit : InitBehaviour<TutorialInit>
{
    public Conversation wakeUpConvo;

    protected override void Initialize()
    {
        Character.Tutorial_Mom.SpokenToDay0 += Tutorial_Mom_OnSpokenToDay0;

        if (Day.DayNumber == Day.TutorialDay && Day.Stage == Stage.SpeakingWithParty)
        {
            // We just started the tutorial - add the mom and dad to the cabin
            Character.Tutorial_Mom.ChangeStatus(CharacterStatus.InsideCabin);
            Character.Tutorial_Dad.ChangeStatus(CharacterStatus.InsideCabin);
        }
    }

    private void Tutorial_Mom_OnSpokenToDay0(Character mom)
    {
        wakeUpConvo.Start();
    }
}
