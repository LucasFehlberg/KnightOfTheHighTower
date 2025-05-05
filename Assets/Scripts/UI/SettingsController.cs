/*****************************************************************************
// File Name : SettingsController.cs
// Author : Lucas Fehlberg
// Creation Date : May 2, 2025
// Last Updated : May 4, 2025
//
// Brief Description : Settings Controller Script
*****************************************************************************/

using System.Collections;
using UnityEngine;

public class SettingsController : MonoBehaviour
{
    [SerializeField] private GameObject fileDeletedText;
    [SerializeField] private GameObject confirmation;

    /// <summary>
    /// Sets stuff to false on boot
    /// </summary>
    private void Start()
    {
        fileDeletedText.SetActive(false);
        confirmation.SetActive(false);
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
}
