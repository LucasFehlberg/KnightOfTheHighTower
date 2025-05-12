/*****************************************************************************
// File Name : SettingsController.cs
// Author : Lucas Fehlberg
// Creation Date : May 2, 2025
// Last Updated : May 11, 2025
//
// Brief Description : Settings Controller Script
*****************************************************************************/

using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SettingsController : MonoBehaviour
{
    [SerializeField] private GameObject fileDeletedText;
    [SerializeField] private GameObject confirmation;

    [SerializeField] private bool ingameMenu = false;

    /// <summary>
    /// Sets stuff to false on boot
    /// </summary>
    private void Awake()
    {
        if (!ingameMenu)
        {
            Settings.LoadSettings();
            fileDeletedText.SetActive(false);
            confirmation.SetActive(false);
            return;
        }

        gameObject.SetActive(false);
        Time.timeScale = 1f;
    }

    /// <summary>
    /// Erases save file
    /// </summary>
    public void EraseData()
    {
        confirmation.SetActive(false);
        fileDeletedText.SetActive(true);
        StartCoroutine(HideDeletion());
        SaveSystem.DeleteFile();
    }

    /// <summary>
    /// Opens up the failsafe prompt
    /// </summary>
    public void PromptAreYouSure()
    {
        if (!confirmation.activeSelf)
        {
            confirmation.SetActive(true);
        }
    }

    /// <summary>
    /// Cancels file deletion
    /// </summary>
    public void Cancel()
    {
        confirmation.SetActive(false);
    }

    /// <summary>
    /// Hides the deletion of the file
    /// </summary>
    /// <returns></returns>
    private IEnumerator HideDeletion()
    {
        yield return new WaitForSeconds(2f);
        fileDeletedText.SetActive(false);
    }

    /// <summary>
    /// Saves the settings
    /// </summary>
    public void SaveSettings()
    {
        Settings.SaveSettings();
    }

    /// <summary>
    /// Returns the game to the main menu
    /// </summary>
    public void ReturnToMainMenu()
    {
        SceneManager.LoadScene(0);
    }

    /// <summary>
    /// Resets the game
    /// </summary>
    public void ResetGame()
    {
        Stats.ResetStats();
        SceneManager.LoadScene(1);
    }

    /// <summary>
    /// Quits the game
    /// </summary>
    public void QuitToDesktop()
    {
        Application.Quit();
    }

    /// <summary>
    /// Resumes the game
    /// </summary>
    public void Resume()
    {
        Time.timeScale = 1;
        gameObject.SetActive(false);
    }

    /// <summary>
    /// Pauses the game
    /// </summary>
    public void PauseGame()
    {
        Time.timeScale = 0;
        gameObject.SetActive(true);
    }
}
