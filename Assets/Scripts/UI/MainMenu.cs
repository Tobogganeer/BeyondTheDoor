using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenu : MonoBehaviour
{
    public GameObject homePage;
    public GameObject continueButton;
    public TMPro.TMP_Text continueButtonText;
    public GameObject quitOverlay;

    [Space]
    public GameObject savesPage;

    [Space]
    public GameObject settingsPage; // TODO: Change type to be the settings page type

    private void Start()
    {
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
        homePage.SetActive(false);
        savesPage.SetActive(true);
        settingsPage.SetActive(false);
    }

    public void Settings()
    {
        homePage.SetActive(false);
        savesPage.SetActive(false);
        settingsPage.SetActive(true);
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
        homePage.SetActive(true);
        savesPage.SetActive(false);
        settingsPage.SetActive(false);
    }
}
