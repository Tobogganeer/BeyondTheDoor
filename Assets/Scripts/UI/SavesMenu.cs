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

    [Space]
    public GameObject deleteConfirmWindow;

    int currentSlot = -1;
    Canvas canvas;

    private void Awake()
    {
        canvas = GetComponent<Canvas>();
    }

    public void Open()
    {
        canvas.enabled = true;
        // Close this if it was opened
        deleteConfirmWindow.SetActive(false);
    }

    public void Close()
    {
        canvas.enabled = false;
    }

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
