using BeyondTheDoor;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Day6Init : DayBehaviour
{
    protected override int GetDay() => 6;
    public ConversationCallback giveDadShotgun;
    public ConversationCallback dadLeaves;
    bool dadHasShotgun = false;


    protected override void RegisterConversationCallbacks()
    {
        giveDadShotgun.Callback += (conv, line) => GiveDadShotgun();
        dadLeaves.Callback += (conv, line) => DadLeaves();

    }

    private void GiveDadShotgun()
    {
        dadHasShotgun = true;
        Cabin.HasShotgun = false;
    }
    private void DadLeaves()
    {
        Character.Dad.ChangeStatus(dadHasShotgun ? CharacterStatus.LeftWithShotgun : CharacterStatus.Left);
        CharacterGraphics.UpdateCurrent();
    }

}
