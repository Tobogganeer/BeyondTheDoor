using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BeyondTheDoor;

/// <summary>
/// Controls the display of characters in the scene
/// </summary>
public class CharacterGraphics : MonoBehaviour
{
    [SerializeField] private Graphic[] characters;
    
    private Dictionary<CharacterID, GameObject> graphics = new Dictionary<CharacterID, GameObject>();

    private void Start()
    {
        // Disable all characters and add them to the dict
        foreach (Graphic graphic in characters)
        {
            graphic.graphic.SetActive(false);
            graphics.Add(graphic.character, graphic.graphic);
        }

        // Enable current characters
        foreach (Character character in Cabin.CurrentPartyMembers())
        {
            if (graphics.TryGetValue(character.ID, out GameObject graphic))
                graphic.SetActive(true);
        }
    }


    [System.Serializable]
    private class Graphic
    {
        public CharacterID character;
        public GameObject graphic;
    }
}
