using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BeyondTheDoor;

/// <summary>
/// Controls the display of characters in the scene
/// </summary>
public class CharacterGraphics : MonoBehaviour
{
    private static CharacterGraphics current;

    [SerializeField] private CharacterObject[] characters;
    
    private Dictionary<CharacterID, CharacterObject> graphics = new Dictionary<CharacterID, CharacterObject>();

    private void OnEnable()
    {
        current = this;
    }

    private void Start()
    {
        // Add all characters to the dict
        foreach (CharacterObject character in characters)
            graphics.Add(character.character, character);

        UpdateGraphics();
    }

    public void UpdateGraphics()
    {
        // Disable them all
        foreach (CharacterObject graphic in characters)
            graphic.gameObject.SetActive(false);

        // Enable current characters
        foreach (Character character in Cabin.CurrentPartyMembers())
        {
            if (graphics.TryGetValue(character.ID, out CharacterObject graphic))
                graphic.gameObject.SetActive(true);
        }
    }

    public static void UpdateCurrent()
    {
        if (current != null)
            current.UpdateGraphics();
    }
}
