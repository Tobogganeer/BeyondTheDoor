using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using BeyondTheDoor.UI;

public static class Settings
{
    private static SettingsValues values;
    private static readonly string FilePath = Path.Combine(Application.persistentDataPath, "settings.json");
    const float MinTextSpeed = 15f;
    const float MaxTextSpeed = 50f;

    public static float MasterVolume
    {
        get => values.masterVolume;
        set
        {
            values.masterVolume = Mathf.Clamp01(value);
            AudioMaster.SetMasterVolume(values.masterVolume);
        }
    }
    public static float SfxVolume
    {
        get => values.sfxVolume;
        set
        {
            values.sfxVolume = Mathf.Clamp01(value);
            AudioMaster.SetSFXVolume(values.sfxVolume);
        }
    }
    public static float AmbientVolume
    {
        get => values.ambientVolume;
        set
        {
            values.ambientVolume = Mathf.Clamp01(value);
            AudioMaster.SetAmbientVolume(values.ambientVolume);
        }
    }
    public static float MusicVolume
    {
        get => values.musicVolume;
        set
        {
            values.musicVolume = Mathf.Clamp01(value);
            AudioMaster.SetMusicVolume(values.musicVolume);
        }
    }
    public static float DialogueVolume
    {
        get => values.dialogueVolume;
        set
        {
            values.dialogueVolume = Mathf.Clamp01(value);
            AudioMaster.SetDialogueVolume(values.dialogueVolume);
        }
    }
    public static float TextSpeed
    {
        get => values.textSpeed;
        set
        {
            values.textSpeed = Mathf.Clamp(value, MinTextSpeed, MaxTextSpeed);
            DialogueGUI.RevealedCharactersPerSecond = values.textSpeed;
        }
    }


    public static void Save()
    {
        // Save that sucker
        File.WriteAllText(FilePath, JsonUtility.ToJson(values, true));
    }

    public static void Load()
    {
        // Set defaults in case the file doesn't exist (and as a fallback in case of error)
        values = SettingsValues.Default;

        // Try to read our settings file
        try
        {
            if (File.Exists(FilePath))
                values = JsonUtility.FromJson<SettingsValues>(File.ReadAllText(FilePath));
        }
        catch (System.Exception ex)
        {
            Debug.LogWarning("Error reading settings (using defaults): " + ex);
        }
    }


    [System.Serializable]
    private struct SettingsValues
    {
        public float masterVolume;
        public float sfxVolume;
        public float ambientVolume;
        public float musicVolume;
        public float dialogueVolume;
        public float textSpeed;

        public static SettingsValues Default => new SettingsValues(0.75f, 1.0f, 1.0f, 1.0f, 1.0f, 25.0f);

        public SettingsValues(float masterVolume, float sfxVolume, float ambientVolume, float musicVolume, float dialogueVolume, float textSpeed)
        {
            this.masterVolume = masterVolume;
            this.sfxVolume = sfxVolume;
            this.ambientVolume = ambientVolume;
            this.musicVolume = musicVolume;
            this.dialogueVolume = dialogueVolume;
            this.textSpeed = textSpeed;
        }
    }
}
