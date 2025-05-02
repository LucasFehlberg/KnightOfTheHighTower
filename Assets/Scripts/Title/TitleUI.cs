/*****************************************************************************
// File Name : TitleUI.cs
// Author : Lucas Fehlberg
// Creation Date : March 30, 2025
// Last Updated : May 2, 2025
//
// Brief Description : UI Stuff for the title
*****************************************************************************/

using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TitleUI : MonoBehaviour
{
    [SerializeField] private RectTransform Settings;
    [SerializeField] private RectTransform MainScreen;

    private readonly Vector2 basePos = Vector2.zero;
    private Vector2 hidePos;

    private RectTransform desiredScreen;

    /// <summary>
    /// Loads the game upon booting
    /// </summary>
    private void Start()
    {
        hidePos = Settings.anchoredPosition;
        SaveSystem.LoadGame();
    }

    /// <summary>
    /// Starts the game
    /// </summary>
    public void StartGame()
    {
        Stats.ResetStats();
        //Stats.HeldItems.Add(new TrustyTrowel());
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

    /// <summary>
    /// Starts moving to the settings screen
    /// </summary>
    public void GoToSettings()
    {
        desiredScreen = Settings;
        StartCoroutine(GoingToScreen(MainScreen, desiredScreen));
    }

    /// <summary>
    /// Goes to the screen
    /// </summary>
    /// <returns></returns>
    private IEnumerator GoingToScreen(RectTransform from, RectTransform to)
    {
        float timer = 0;
        while (timer < 1)
        {
            timer += 0.01f;
            from.anchoredPosition = Vector2.Lerp(basePos, hidePos, timer);
            to.anchoredPosition = Vector2.Lerp(hidePos, basePos, timer);
            yield return null;
        }
    }

    /// <summary>
    /// Returns to main screen
    /// </summary>
    public void GoToMain()
    {
        StartCoroutine(GoingToScreen(desiredScreen, MainScreen));
    }
}
