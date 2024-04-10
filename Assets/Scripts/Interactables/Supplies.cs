using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BeyondTheDoor;

public class Supplies : MorningInteractable
{
    protected override Conversation Conversation => DayBehaviour.Current.checkSupplies;
}
