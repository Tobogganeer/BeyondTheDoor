using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BeyondTheDoor;
using UnityEngine.EventSystems;

public class CharacterObject : MonoBehaviour, IPointerUpHandler
{
    public CharacterID character;

    public void OnPointerUp(PointerEventData eventData)
    {
        Character.All[character].OnSelected();
    }
}
