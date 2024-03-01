using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BeyondTheDoor;
using BeyondTheDoor.UI;

public class CharacterObject : MonoBehaviour
{
    public CharacterID character;

    public void OnMouseUp()
    {
        // Click on us (if we aren't already speaking to someone)
        if (!DialogueGUI.IsOpen)
            Character.All[character].OnSelected();
    }
}
