using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class SavesMenu : MonoBehaviour
{
    public int numSaves = 3;
    public GameObject[] selectedSlotHighlights;

    [Space]
    public TMP_Text saveSlotText;

    [Space]
    public GameObject occupiedSlotInfo;
    public TMP_Text saveDateText;
    public TMP_Text dayText;

    [Space]
    public GameObject emptySlotInfo;

    int currentSlot = -1;

    private void Start()
    {
        SelectSlot(1); // Default slot
    }

    public void SelectSlot(int slot)
    {
        for(int i = 0; i < selectedSlotHighlights.Length; i++)
        {
            selectedSlotHighlights[i].SetActive(i == slot);
        }

        //occupiedSlotInfo.SetActive(false);
        //emptySlotInfo.SetActive(true);

        saveSlotText.text = "Slot " + (slot + 1);
    }

    public void StartNewGame()
    {

    }

    public void LoadCurrentSave()
    {

    }

    public void DeleteCurrentSave()
    {

    }
}
