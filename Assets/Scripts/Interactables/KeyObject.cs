using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeyObject : MonoBehaviour, IInteractable
{
    public void OnClicked()
    {
        // Move forward once we find the key (onto scavenging)
        Game.Advance();
    }
}
