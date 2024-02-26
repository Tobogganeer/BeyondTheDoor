using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using BeyondTheDoor.SaveSystem;
using BeyondTheDoor;

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
    SaveState[] saves;
    SaveState currentSave => saves[currentSlot];
    Canvas canvas;

    private void Awake()
    {
        canvas = GetComponent<Canvas>();
        saves = new SaveState[numSaves];
        LoadSaves();
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
        currentSlot = Mathf.Clamp(slot, 0, numSaves - 1);
        saveSlotText.text = "Slot " + (currentSlot + 1);

        // Highlight the current save
        for (int i = 0; i < selectedSlotHighlights.Length; i++)
            selectedSlotHighlights[i].SetActive(i == currentSlot);

        if (currentSave == null)
            LoadEmptySlotInfo();
        else
            LoadSlotInfo(currentSave);
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

    private void LoadEmptySlotInfo()
    {
        occupiedSlotInfo.SetActive(false);
        emptySlotInfo.SetActive(true);
    }

    private void LoadSlotInfo(SaveState state)
    {
        occupiedSlotInfo.SetActive(true);
        emptySlotInfo.SetActive(false);
        saveDateText.text = state.SaveTime.ToString();
        dayText.text = "Day " + state.GameDay + " - " + state.GameStage.ToTimeString();
    }

    private void LoadSaves()
    {
        for (int i = 0; i < saves.Length; i++)
        {
            if (SaveSystem.SaveExists(i))
                saves[i] = SaveSystem.Load(i);
        }
    }
}
