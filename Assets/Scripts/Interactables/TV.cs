using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BeyondTheDoor;

public class TV : MorningInteractable
{
    protected override Conversation Conversation => DayBehaviour.Current.tv;
}
