using BeyondTheDoor.UI;
using BeyondTheDoor;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Day7Init : DayBehaviour
{
    public ConversationCallback endOfDay;

    protected override int GetDay() => 3;

    protected override void Initialize()
    {
        endOfDay.Callback += (conv, line) => EndGame();
    }

    private static void EndGame()
    {
        // Stop any advances
        DialogueGUI.ClearQueue();
    }
}
