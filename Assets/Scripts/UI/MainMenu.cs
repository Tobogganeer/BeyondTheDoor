using BeyondTheDoor.SaveSystem;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenu : MonoBehaviour
{
    public Canvas homePage;
    public GameObject continueButton;
    public TMPro.TMP_Text continueText;
    public TMPro.TMP_Text continueButtonText;
    public GameObject quitOverlay;

    [Space]
    public SavesMenu savesPage;

    [Space]
    public SettingsMenu settingsPage;

    bool canContinue;
    uint lastPlayedSaveSlot;

    private void Start()
    {
        // Disable other menus
        ReturnToHome();
        quitOverlay.SetActive(false);
    }

    public void Continue()
    {
        if (!canContinue)
        {
            // How did we click this? Try resetting...
            ReturnToHome();
            return;
        }

        SaveState save = SaveSystem.Load(lastPlayedSaveSlot);
        Game.Begin(save, lastPlayedSaveSlot);
    }

    public void Play()
    {
        homePage.enabled = false;
        savesPage.Open();
        settingsPage.Close();
    }

    public void Settings()
    {
        homePage.enabled = false;
        savesPage.Close();
        settingsPage.Open();
    }

    public void Quit()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }

    public void ReturnToHome()
    {
        homePage.enabled = true;
        savesPage.Close();
        settingsPage.Close();

        canContinue = SaveSystem.TryGetLastPlayedSaveSlot(out lastPlayedSaveSlot);
        continueButton.SetActive(canContinue);
        if (canContinue)
            continueText.text = $"Continue (Day {lastPlayedSaveSlot})";
    }
}
