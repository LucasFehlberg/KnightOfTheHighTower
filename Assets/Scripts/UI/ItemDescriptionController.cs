/*****************************************************************************
// File Name : ItemDescriptionController.cs
// Author : Lucas Fehlberg
// Creation Date : March 31, 2025
// Last Updated : March 31, 2025
//
// Brief Description : Controls item descriptions
*****************************************************************************/

using TMPro;
using UnityEngine;

public class ItemDescriptionController : MonoBehaviour
{
    [SerializeField] private GameObject box;
    [SerializeField] private TMP_Text itemName;
    [SerializeField] private TMP_Text itemDescription;

    private RectTransform rectTransform;
    [SerializeField] private Canvas canvas;

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
    }

    /// <summary>
    /// Hide the UI
    /// </summary>
    public void Hide()
    {
        box.SetActive(false);
    }

    public void Show(string name, string description)
    {
        itemName.text = name;
        itemDescription.text = description;
        box.SetActive(true);
    }
}
