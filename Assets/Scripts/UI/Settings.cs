/*****************************************************************************
// File Name : Settings.cs
// Author : Lucas Fehlberg
// Creation Date : May 2, 2025
// Last Updated : May 2, 2025
//
// Brief Description : Settings script, controls all settings variables
                       Saved Seperately from SaveData
*****************************************************************************/

using UnityEngine;
using System.IO;

//This class is for actually holding the data
public class SettingsData
{
    public float MusicVolume = 1;
    public float SFXVolume = 1;
}

//This class is for saving/loading settings
public static class Settings
{
    private static SettingsData data = new();

    public static SettingsData Data { get => data; set => data = value; }

    /// <summary>
    /// Saves the game
    /// </summary>
    public static void SaveSettings()
    {
        string saveFilePath = Application.persistentDataPath + "/PlayerSettings.json";

        string savedData = JsonUtility.ToJson(data);
        File.WriteAllText(saveFilePath, savedData);
    }

    /// <summary>
    /// Loads the game
    /// </summary>
    public static void LoadGame()
    {
        string saveFilePath = Application.persistentDataPath + "/PlayerSettings.json";

        if (File.Exists(saveFilePath))
        {
            string loadedData = File.ReadAllText(saveFilePath);
            data = JsonUtility.FromJson<SettingsData>(loadedData);

            //data.LoadData();
        }
    }
}
