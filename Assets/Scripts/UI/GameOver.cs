/*****************************************************************************
// File Name : GameOver.cs
// Author : Lucas Fehlberg
// Creation Date : May 8, 2025
// Last Updated : May 9, 2025
//
// Brief Description : Moves the player to the title, or resets the run
*****************************************************************************/

using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOver : MonoBehaviour
{
    /// <summary>
    /// Resets the run
    /// </summary>
    public void ResetRun()
    {
        Stats.ResetStats();
        RoomManager.Floor = 0;
        SceneManager.LoadScene(1);
    }

    /// <summary>
    /// Loads the main menu scene
    /// </summary>
    public void MainMenu()
    {
        SceneManager.LoadScene(0);
    }
}
