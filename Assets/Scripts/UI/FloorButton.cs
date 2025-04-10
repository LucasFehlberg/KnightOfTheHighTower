/*****************************************************************************
// File Name : FloorButton.cs
// Author : Lucas Fehlberg
// Creation Date : April 4, 2025
// Last Updated : April 4, 2025
//
// Brief Description : Assigns a button an item, closing out the UI
*****************************************************************************/

using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class FloorButton : MonoBehaviour
{
    [SerializeField] private GameController controller;

    private string reward;
    private List<char> layout;

    [SerializeField] private TMP_Text text;

    /// <summary>
    /// Add an item when this button is pressed
    /// </summary>
    public void ButtonPressed()
    {
        RoomManager.Instance.Reward = reward;
        RoomManager.Instance.FloorLayout = layout;

        controller.NextFloor();
    }

    public void SetFloor(string reward, List<char> floor)
    {
        this.reward = reward;

        layout = floor;

        text.text = reward;
    }
}