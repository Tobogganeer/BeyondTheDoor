using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BeyondTheDoor;

public class CharacterObject : MonoBehaviour
{
    public CharacterID character;

    public void OnMouseUp()
    {
        Character.All[character].OnSelected();
    }
}
