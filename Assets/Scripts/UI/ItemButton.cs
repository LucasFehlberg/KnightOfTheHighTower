/*****************************************************************************
// File Name : ItemButton.cs
// Author : Lucas Fehlberg
// Creation Date : April 4, 2025
// Last Updated : April 4, 2025
//
// Brief Description : Assigns a button an item, closing out the UI
*****************************************************************************/

using TMPro;
using UnityEngine;

public class ItemButton : MonoBehaviour
{
    private Item item;
    [SerializeField] private GameController controller;

    [SerializeField] private ItemBox itemBox;
    [SerializeField] private TMP_Text malText;

    /// <summary>
    /// Add an item when this button is pressed
    /// </summary>
    public void ButtonPressed()
    {
        Stats.HeldItems.Add(item);
        controller.SetupNextFloor();
        GameObject.FindGameObjectWithTag("ItemHolder").GetComponent<ItemHolder>().UpdateUI();
        GameObject.FindGameObjectWithTag("ItemTextbox").GetComponent<ItemDescriptionController>().Hide();
    }

    public void SetItem(Item item, Modifier modifier = null)
    {
        this.item = item;
        itemBox.ButtonUI(item);

        if(modifier != null)
        {
            malText.text = modifier.ModifierName;
            malText.gameObject.SetActive(true);
        }
    }
}
