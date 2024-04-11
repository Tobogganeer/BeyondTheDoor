using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BeyondTheDoor;

public class KeyObject : MonoBehaviour, IInteractable
{
    public void OnClicked()
    {
        // Move forward once we find the tutorial key (onto scavenging)
        if (Day.DayNumber == Day.TutorialDay && Day.Stage == Stage.MorningSupplies)
            Game.Advance();
    }
}
