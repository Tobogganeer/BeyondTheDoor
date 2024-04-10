using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BeyondTheDoor;

public class Radio : MorningInteractable
{
    protected override Conversation Conversation => DayBehaviour.Current.radio;
}
