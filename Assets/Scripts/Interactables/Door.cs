using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BeyondTheDoor;

public class Door : MonoBehaviour, IInteractable
{
    public Conversation doorOptions;

    public void OnClicked()
    {
        if (Day.Stage == Stage.DealingWithArrival && Day.DayNumber != Day.TutorialDay)
        {
            doorOptions.Start();
        }
    }
}
