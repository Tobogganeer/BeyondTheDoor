using BeyondTheDoor;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Day6Init : DayBehaviour
{
    protected override int GetDay() => 6;
    public ConversationCallback dadWithShotgun;
    public ConversationCallback dadWithoutShotgun;


    protected override void Initialize()
    {
        dadWithShotgun.Callback += (conv, line) => dadShotgun();
        dadWithoutShotgun.Callback += (conv, line) => dadNoShotgun();

    }

    private void dadShotgun()
    {
        Character.Dad.ChangeStatus(CharacterStatus.LeftWithShotgun, true);
    }
    private void dadNoShotgun()
    {
        Character.Dad.ChangeStatus(CharacterStatus.Left, true);
    }

}
