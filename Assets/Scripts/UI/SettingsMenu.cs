using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SettingsMenu : MonoBehaviour
{
    public MenuSlider masterVolumeSlider;
    public MenuSlider sfxVolumeSlider;
    public MenuSlider ambientVolumeSlider;
    public MenuSlider musicVolumeSlider;
    public MenuSlider dialogueVolumeSlider;
    public MenuSlider textSpeedSlider;

    Canvas canvas;

    private void Awake()
    {
        canvas = GetComponent<Canvas>();

        // Register callbacks to change the values in the settings
        masterVolumeSlider.ValueChanged += (value) => Settings.MasterVolume = value;
        sfxVolumeSlider.ValueChanged += (value) => Settings.SfxVolume = value;
        ambientVolumeSlider.ValueChanged += (value) => Settings.AmbientVolume = value;
        musicVolumeSlider.ValueChanged += (value) => Settings.MusicVolume = value;
        dialogueVolumeSlider.ValueChanged += (value) => Settings.DialogueVolume = value;
        textSpeedSlider.ValueChanged += (value) => Settings.TextSpeed = value;
    }

    public void Open()
    {
        canvas.enabled = true;

        // Set the appropriate values of our sliders
        masterVolumeSlider.value = Settings.MasterVolume;
        sfxVolumeSlider.value = Settings.SfxVolume;
        ambientVolumeSlider.value = Settings.AmbientVolume;
        musicVolumeSlider.value = Settings.MusicVolume;
        dialogueVolumeSlider.value = Settings.DialogueVolume;
        textSpeedSlider.value = Settings.TextSpeed;
    }

    public void Close()
    {
        canvas.enabled = false;
        // Save our settings to disk
        Settings.Save();
    }
}
