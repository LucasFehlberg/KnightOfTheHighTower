/*****************************************************************************
// File Name : ItemBox.cs
// Author : Lucas Fehlberg
// Creation Date : March 31, 2025
// Last Updated : April 29, 2025
//
// Brief Description : The individual item UI box
*****************************************************************************/

using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ItemBox : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    private int id = -1;
    private Item displayedItem;

    [SerializeField] private Image texture;

    /// <summary>
    /// ID of the box
    /// </summary>
    public int ID { get => id; set => id = value; }

    /// <summary>
    /// Resets the UI of the item box
    /// </summary>
    public void ResetUI()
    {
        displayedItem = Stats.HeldItems[id];
        string path = "Items/" + displayedItem.ItemName;

        Sprite sprite = Resources.Load<Sprite>(path);

        if(sprite == null)
        {
            sprite = Resources.Load<Sprite>("Items/ItemPlaceholder");
        }
        texture.sprite = sprite;
        displayedItem.Holder = this;
    }

    /// <summary>
    /// When the mouse enters the UI
    /// </summary>
    /// <param name="eventData"></param>
    public void OnPointerEnter(PointerEventData eventData)
    {
        if(id != -1)
        {
            GameObject.FindGameObjectWithTag("ItemTextbox").GetComponent<ItemDescriptionController>()
                .Show(displayedItem);
            return;
        }

        GameObject.FindGameObjectWithTag("ItemTextbox").GetComponent<ItemDescriptionController>()
                .Show(displayedItem);
    }

    /// <summary>
    /// When the mouse exits the UI element
    /// </summary>
    /// <param name="eventData"></param>
    public void OnPointerExit(PointerEventData eventData)
    {
        GameObject.FindGameObjectWithTag("ItemTextbox").GetComponent<ItemDescriptionController>().Hide();
    }

    public void ButtonUI(Item item)
    {
        string path = "Items/" + item.ItemName;

        Sprite sprite = Resources.Load<Sprite>(path);

        if (sprite == null)
        {
            sprite = Resources.Load<Sprite>("Items/ItemPlaceholder");
        }
        texture.sprite = sprite;
        displayedItem = item;
    }
}
