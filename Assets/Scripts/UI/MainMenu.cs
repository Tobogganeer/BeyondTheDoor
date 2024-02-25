using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenu : MonoBehaviour
{
    public Canvas homePage;
    public GameObject continueButton;
    public TMPro.TMP_Text continueButtonText;
    public GameObject quitOverlay;

    [Space]
    public Canvas savesPage;

    [Space]
    public Canvas settingsPage; // TODO: Change type to be the settings page type

    private void Start()
    {
        // Disable other menus
        ReturnToHome();
        quitOverlay.SetActive(false);

        // TODO: Check if we can continue
        continueButton.SetActive(false);
    }

    public void Continue()
    {
        throw new System.NotImplementedException();
    }

    public void Play()
    {
        homePage.enabled = false;
        savesPage.enabled = true;
        settingsPage.enabled = false;
    }

    public void Settings()
    {
        homePage.enabled = false;
        savesPage.enabled = false;
        settingsPage.enabled = true;
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
        savesPage.enabled = false;
        settingsPage.enabled = false;
    }
}
