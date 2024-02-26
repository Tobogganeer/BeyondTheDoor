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
    public SavesMenu savesPage;

    [Space]
    public SettingsMenu settingsPage;

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
    }
}
