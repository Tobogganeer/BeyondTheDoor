using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BeyondTheDoor;

public class CharacterObject : MonoBehaviour, IInteractable
{
    public CharacterID character;

    public void OnClicked()
    {
        // Click on us
        Character.All[character].OnSelected();
    }
}
