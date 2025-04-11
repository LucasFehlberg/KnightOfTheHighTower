/*****************************************************************************
// File Name : TileBox.cs
// Author : Lucas Fehlberg
// Creation Date : March 31, 2025
// Last Updated : April 4, 2025
//
// Brief Description : The individual tile UI box
*****************************************************************************/

using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class TileBox : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] private Image texture;

    private string tileDescription;
    private string tileName;

    /// <summary>
    /// When the mouse enters the UI
    /// </summary>
    /// <param name="eventData"></param>
    public void OnPointerEnter(PointerEventData eventData)
    {
        GameObject.FindGameObjectWithTag("ItemTextbox").GetComponent<ItemDescriptionController>()
            .Show(tileName, tileDescription);
    }

    /// <summary>
    /// When the mouse exits the UI element
    /// </summary>
    /// <param name="eventData"></param>
    public void OnPointerExit(PointerEventData eventData)
    {
        GameObject.FindGameObjectWithTag("ItemTextbox").GetComponent<ItemDescriptionController>().Hide();
    }

    public void ButtonUI(string description, string name, Sprite sprite = null)
    {
        if (sprite == null)
        {
            sprite = Resources.Load<Sprite>("Items/ItemPlaceholder");
        }
        texture.sprite = sprite;
        tileName = name;
        tileDescription = description;
    }
}
