/*****************************************************************************
// File Name : TileBox.cs
// Author : Lucas Fehlberg
// Creation Date : March 31, 2025
// Last Updated : May 1, 2025
//
// Brief Description : The individual tile UI box
*****************************************************************************/

using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class TileBox : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] private Image texture;
    [SerializeField] private bool isDefault;

    [SerializeField] private string tileDescription;
    [SerializeField] private string tileName;

    /// <summary>
    /// When the mouse enters the UI
    /// </summary>
    /// <param name="eventData"></param>
    public void OnPointerEnter(PointerEventData eventData)
    {
        GameObject.FindGameObjectWithTag("ItemTextbox").GetComponent<ItemDescriptionController>()
            .ShowTile(tileName, tileDescription);
    }

    /// <summary>
    /// When the mouse exits the UI element
    /// </summary>
    /// <param name="eventData"></param>
    public void OnPointerExit(PointerEventData eventData)
    {
        GameObject.FindGameObjectWithTag("ItemTextbox").GetComponent<ItemDescriptionController>().Hide();
    }

    /// <summary>
    /// Resets the tile
    /// </summary>
    /// <param name="description"></param>
    /// <param name="name"></param>
    /// <param name="sprite"></param>
    public void ButtonUI(string description, string name, Sprite sprite = null)
    {
        if (isDefault)
        {
            return;
        }

        if (sprite == null)
        {
            sprite = Resources.Load<Sprite>("Items/ItemPlaceholder");
        }
        texture.sprite = sprite;
        tileName = name;
        tileDescription = description;
    }
}
