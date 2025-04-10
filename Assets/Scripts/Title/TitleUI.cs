/*****************************************************************************
// File Name : TitleUI.cs
// Author : Lucas Fehlberg
// Creation Date : March 30, 2025
// Last Updated : March 30, 2025
//
// Brief Description : UI Stuff for the title
*****************************************************************************/

using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class TitleUI : MonoBehaviour
{
    private PlayerInput pInput;
    private InputAction click;

    [SerializeField] private GameObject UIA;
    [SerializeField] private GameObject UIB;

    private bool hasClicked = false;

    /// <summary>
    /// Init
    /// </summary>
    private void Start()
    {
        pInput = GetComponent<PlayerInput>();

        pInput.currentActionMap.Enable();
        click = pInput.currentActionMap.FindAction("MainAction");

        click.started += Click_started;

        UIA.SetActive(true);
        UIB.SetActive(false);
    }

    /// <summary>
    /// No Unity lag today
    /// </summary>
    private void OnDestroy()
    {
        click.started -= Click_started;
    }

    /// <summary>
    /// Get rid of the popup and go to the game
    /// </summary>
    /// <param name="obj"></param>
    private void Click_started(InputAction.CallbackContext obj)
    {
        if (!hasClicked)
        {
            UIA.SetActive(false);
            UIB.SetActive(true);
            hasClicked = true;
        }
    }

    /// <summary>
    /// Changes scene to the main Gameplay
    /// </summary>
    private void ChangeSceneToGame()
    {
        SceneManager.LoadScene(1);
    }

    /// <summary>
    /// Gives the player a SidestepCharm
    /// </summary>
    public void Charm()
    {
        Stats.HeldItems.Clear();
        //Stats.HeldItems.Add(new SidestepCharm());
        ChangeSceneToGame();
    }

    /// <summary>
    /// Gives the player a RustyDagger
    /// </summary>
    public void Knife()
    {
        Stats.HeldItems.Clear();
        Stats.HeldItems.Add(new RustyDagger());
        ChangeSceneToGame();
    }
}
