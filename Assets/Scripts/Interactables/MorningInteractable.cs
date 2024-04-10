using BeyondTheDoor;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class MorningInteractable : MonoBehaviour, IInteractable
{
    protected abstract Conversation Conversation { get; }

    public void OnClicked()
    {
        if (Day.Stage == Stage.SpeakingWithParty && Conversation != null)
            Conversation.Start();
    }
}
