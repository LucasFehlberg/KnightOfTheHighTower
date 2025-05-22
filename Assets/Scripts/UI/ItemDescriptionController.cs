/*****************************************************************************
// File Name : ItemDescriptionController.cs
// Author : Lucas Fehlberg
// Creation Date : March 31, 2025
// Last Updated : May 1, 2025
//
// Brief Description : Controls item descriptions
*****************************************************************************/

using TMPro;
using UnityEngine;
using UnityEngine.UI;
public class ItemDescriptionController : MonoBehaviour
{
    [SerializeField] private GameObject box;
    [SerializeField] private TMP_Text itemName;
    [SerializeField] private TMP_Text itemDescription;

    private RectTransform rectTransform;
    [SerializeField] private Canvas canvas;

    [SerializeField] private Image rarityBackgroundColor;
    [SerializeField] private Sprite starterBG;
    [SerializeField] private Sprite commonBG;
    [SerializeField] private Sprite uncommonBG;
    [SerializeField] private Sprite rareBG;

    [SerializeField] private TMP_Text rarityText;

    private Item visibleItem = null;

    /// <summary>
    /// Gets the rectTransform
    /// </summary>
    private void Start()
    {
        rectTransform = GetComponent<RectTransform>();
        box.SetActive(false);
    }

    /// <summary>
    /// Moves the box
    /// </summary>
    private void FixedUpdate()
    {
        RectTransformUtility.ScreenPointToLocalPointInRectangle(canvas.transform as RectTransform, Input.mousePosition, 
            canvas.worldCamera, out Vector2 newTransform);
        rectTransform.anchoredPosition = newTransform;
        if(rectTransform.anchoredPosition.x > 350)
        {
            box.GetComponent<RectTransform>().anchoredPosition = new Vector3(-190, -10);
        } else
        {
            box.GetComponent<RectTransform>().anchoredPosition = new Vector3(10, -10);
        }

        if (visibleItem != null)
        {
            itemDescription.text = visibleItem.ItemDescription;
        }
    }

    /// <summary>
    /// Hide the UI
    /// </summary>
    public void Hide()
    {
        box.SetActive(false);
        visibleItem = null;
    }

    /// <summary>
    /// Shows the UI
    /// </summary>
    /// <param name="name"></param>
    /// <param name="item"></param>
    public void Show(Item item)
    {
        if(Time.timeScale == 0)
        {
            return;
        }
        rarityBackgroundColor.gameObject.SetActive(true);
        switch (item.ItemRarity)
        {
            case 0:
                rarityBackgroundColor.sprite = starterBG;
                rarityText.text = "Starter";
                break;
            case 1:
                rarityBackgroundColor.sprite = commonBG;
                rarityText.text = "Common";
                break;
            case 2:
                rarityBackgroundColor.sprite = uncommonBG;
                rarityText.text = "Uncommon";
                break;
            case 3:
                rarityBackgroundColor.sprite = rareBG;
                rarityText.text = "Rare";
                break;
        }
        itemName.text = item.ItemNameDisplay;
        visibleItem = item;
        box.SetActive(true);
    }

    /// <summary>
    /// Shows the tile UI
    /// </summary>
    /// <param name="name"></param>
    /// <param name="tileBio"></param>
    public void ShowTile(string name, string tileBio)
    {
        rarityBackgroundColor.gameObject.SetActive(false);
        itemName.text = name;
        itemDescription.text = tileBio;
        box.SetActive(true);
    }
}
