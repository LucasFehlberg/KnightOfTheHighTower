/*****************************************************************************
// File Name : ItemButton.cs
// Author : Lucas Fehlberg
// Creation Date : April 4, 2025
// Last Updated : April 11, 2025
//
// Brief Description : Assigns a button an item/tile, closing out the UI
*****************************************************************************/

using TMPro;
using UnityEngine;

public class ItemButton : MonoBehaviour
{
    private Item item;
    private string tile;
    [SerializeField] private GameController controller;

    [SerializeField] private GameObject itemBox;
    [SerializeField] private TMP_Text malText;

    [SerializeField] private bool isTile = false;
    [SerializeField] private bool isInventory = false;

    /// <summary>
    /// Don't touch unless it's an inventory button
    /// </summary>
    [SerializeField] private int inventoryID = -1;

    /// <summary>
    /// Add an item/tile when this button is pressed
    /// </summary>
    public void ButtonPressed()
    {
        if (isTile)
        {
            Stats.CurrentlyHeldTile = tile;
            return;
        }

        if (isInventory)
        {
            if(Stats.CurrentlyHeldTile == null)
            {
                return;
            }

            Stats.HeldTiles[inventoryID] = Stats.CurrentlyHeldTile;

            Stats.CurrentlyHeldTile = null;

            controller.SetupNextFloor();
            GameObject.FindGameObjectWithTag("ItemHolder").GetComponent<ItemHolder>().UpdateUI();
            GameObject.FindGameObjectWithTag("ItemTextbox").GetComponent<ItemDescriptionController>().Hide();
            return;
        }

        Stats.HeldItems.Add(item);
        controller.SetupNextFloor();
        GameObject.FindGameObjectWithTag("ItemHolder").GetComponent<ItemHolder>().UpdateUI();
        GameObject.FindGameObjectWithTag("ItemTextbox").GetComponent<ItemDescriptionController>().Hide();
    }

    public void SetItem(Item item, Modifier modifier = null)
    {
        this.item = item;
        itemBox.GetComponent<ItemBox>().ButtonUI(item);

        if(modifier != null)
        {
            malText.text = modifier.ModifierName;
            malText.gameObject.SetActive(true);
        }
    }

    public void SetTile(string tileName)
    {
        tile = tileName;
        if(tileName == null)
        {
            itemBox.SetActive(false);
            return;
        }

        string description = Attatchment.AttatchmentDescriptions[tileName];
        Sprite sprite = Attatchment.AttatchmentSprites[tileName];
        itemBox.GetComponent<TileBox>().ButtonUI(description, tileName, sprite);
    }
}
