using BeyondTheDoor;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

[RequireComponent(typeof(CharacterObject))]
public class CharacterNameText : MonoBehaviour
{
    public TMP_Text text;

    private void Start()
    {
        text.text = Character.All[GetComponent<CharacterObject>().character].Name;
    }
}
