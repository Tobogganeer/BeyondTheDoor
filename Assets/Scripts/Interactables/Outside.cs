using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BeyondTheDoor;

public class Outside : MorningInteractable
{
    protected override Conversation Conversation => DayBehaviour.Current.checkOutside;
}
