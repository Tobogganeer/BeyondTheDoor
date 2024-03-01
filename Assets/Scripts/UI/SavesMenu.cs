using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using BeyondTheDoor.SaveSystem;
using BeyondTheDoor;
using UnityEngine.UI;

public class SavesMenu : MonoBehaviour
{
    public int numSaves = 3;
    public GameObject[] selectedSlotHighlights;
    public TMP_Text[] slotNameTexts;

    [Space]
    public TMP_Text saveSlotText;
    public TMP_Text deleteSaveSlotText;

    [Space]
    public GameObject occupiedSlotInfo;
    public TMP_Text saveDateText;
    public TMP_Text dayText;

    [Space]
    public GameObject emptySlotInfo;
    public Toggle playTutorialToggle;

    [Space]
    public GameObject deleteConfirmWindow;

    uint currentSlot = 0;
    SaveState[] saves;
    SaveState currentSave => saves[currentSlot];
    Canvas canvas;

    private void Awake()
    {
        canvas = GetComponent<Canvas>();
        saves = new SaveState[numSaves];
    }

    public void Open()
    {
        canvas.enabled = true;
        // Close this if it was opened
        deleteConfirmWindow.SetActive(false);
        LoadSaves();
        SelectSlot((int)currentSlot);
    }

    public void Close()
    {
        canvas.enabled = false;
    }

    public void SelectSlot(int slot)
    {
        currentSlot = (uint)Mathf.Clamp(slot, 0, numSaves - 1);
        string slotText = "Slot " + (currentSlot + 1);
        saveSlotText.text = slotText;
        deleteSaveSlotText.text = "Delete " + slotText + " ? ";

        // Highlight the current save
        for (int i = 0; i < selectedSlotHighlights.Length; i++)
            selectedSlotHighlights[i].SetActive(i == currentSlot);

        if (currentSave == null)
            LoadEmptySlotInfo();
        else
            LoadSlotInfo(currentSave);
    }

    public void LoadCurrentSave()
    {
        // Loads the current save or starts a new one
        SaveState save = SaveSystem.Load(currentSlot, playTutorialToggle.isOn);

        // This is the last slot we've played
        SaveSystem.SaveLastPlayedSaveSlot(currentSlot);
        Game.Begin(save, currentSlot);
    }

    public void DeleteCurrentSave()
    {
        SaveSystem.Delete(currentSlot);
        saves[currentSlot] = null;
        RefreshSaveSlots();
        SelectSlot((int)currentSlot);
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
        for (uint i = 0; i < saves.Length; i++)
        {
            if (SaveSystem.SaveExists(i))
                saves[i] = SaveSystem.Load(i);
        }

        RefreshSaveSlots();
    }

    private void RefreshSaveSlots()
    {
        for (int i = 0; i < saves.Length; i++)
        {
            if (saves[i] != null)
            {
                slotNameTexts[i].text = "Save " + (i + 1);
            }
            else
            {
                slotNameTexts[i].text = "<Empty>";
            }
        }
    }
}
