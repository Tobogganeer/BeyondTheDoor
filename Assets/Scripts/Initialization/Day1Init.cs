using BeyondTheDoor;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Day1Init : DayBehaviour
{
    protected override int GetDay() => 1;

    protected override void Initialize()
    {
        // Make sure the parents are outta here
        Character.Tutorial_Dad.ChangeStatus(CharacterStatus.NotMet, true);
        Character.Tutorial_Mom.ChangeStatus(CharacterStatus.NotMet, true);
        Character.Jessica.NameKnown = false;
        Character.Player.ChangeStatus(CharacterStatus.InsideCabin);
    }



}
