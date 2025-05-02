/*****************************************************************************
// File Name : SaveSystem.cs
// Author : Lucas Fehlberg
// Creation Date : May 1, 2025
// Last Updated : May 1, 2025
//
// Brief Description : Used for saving and loading things. Right now, just whether or not you beat
                       The tutorial
                       Contains both the SaveData class and the SaveSystem
*****************************************************************************/

using System.IO;
using UnityEngine;

public class SaveData
{
    //Everything in here has to be public, but not static
    public bool DoneTutorial = false;

    /// <summary>
    /// Loads the actual data into the game in their respective variables
    /// Not really required right now because we're not loading stuff needed for other scripts
    /// </summary>
    //public void LoadData()
    //{
    //
    //}

    /// <summary>
    /// Resets all data
    /// </summary>
    public void ResetAllData()
    {
        DoneTutorial = false;
        //LoadData();
    }
}

public static class SaveSystem
{
    private static SaveData data = new();

    public static SaveData Data { get => data; set => data = value; }

    /// <summary>
    /// Saves the game
    /// </summary>
    public static void SaveGame()
    {
        string saveFilePath = Application.persistentDataPath + "/PlayerData.json";

        string savedData = JsonUtility.ToJson(data);
        File.WriteAllText(saveFilePath, savedData);
    }

    /// <summary>
    /// Loads the game
    /// </summary>
    public static void LoadGame() 
    {
        string saveFilePath = Application.persistentDataPath + "/PlayerData.json";

        if (File.Exists(saveFilePath))
        {
            string loadedData = File.ReadAllText(saveFilePath);
            data = JsonUtility.FromJson<SaveData>(loadedData);

            //data.LoadData();
        }
    }

    /// <summary>
    /// Deletes the file
    /// </summary>
    public static void DeleteFile()
    {
        string saveFilePath = Application.persistentDataPath + "/PlayerData.json";

        if (File.Exists(saveFilePath))
        {
            File.Delete(saveFilePath);
        }

        data.ResetAllData();
    }
}
