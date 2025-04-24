/*****************************************************************************
// File Name : TitleUI.cs
// Author : Lucas Fehlberg
// Creation Date : March 30, 2025
// Last Updated : April 24, 2025
//
// Brief Description : UI Stuff for the title
*****************************************************************************/

using UnityEngine;
using UnityEngine.SceneManagement;

public class TitleUI : MonoBehaviour
{
    /// <summary>
    /// Starts the game
    /// </summary>
    public void StartGame()
    {
        Stats.ResetStats();
        //Stats.HeldItems.Add(new SidestepCharm());
        SceneManager.LoadScene(1);
        RoomManager.Floor = 0;
    }

    /// <summary>
    /// Quit button
    /// </summary>
    public void QuitGame()
    {
        Application.Quit();
    }
}
