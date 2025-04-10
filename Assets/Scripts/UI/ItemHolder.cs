/*****************************************************************************
// File Name : ItemHolder.cs
// Author : Lucas Fehlberg
// Creation Date : March 31, 2025
// Last Updated : April 2, 2025
//
// Brief Description : Handles item UI
*****************************************************************************/

using System.Collections.Generic;
using UnityEngine;

public class ItemHolder : MonoBehaviour
{
    [SerializeField] private GameObject itemPrefab;

    private List<GameObject> itemUI = new();

    /// <summary>
    /// Updates the UI
    /// </summary>
    public void UpdateUI()
    {
        for (int i = 0; i < Stats.HeldItems.Count; i++)
        {
            if (itemUI.Count > i)
            {
                continue;
            }

            GameObject newUI = Instantiate(itemPrefab, transform);

            newUI.GetComponent<ItemBox>().ID = i;
            newUI.GetComponent<RectTransform>().anchoredPosition = new((i % 20 * 30) + 5, -1 * (Mathf.Floor(i / 20f) 
                * 30) - 5);

            itemUI.Add(newUI);
        }

        for (int i = 0; i < itemUI.Count; i++)
        {
            if(i >= Stats.HeldItems.Count)
            {
                Destroy(itemUI[i]);
                itemUI.RemoveAt(i);
            }
        }

        foreach (GameObject item in itemUI)
        {
            item.GetComponent<ItemBox>().ResetUI();
        }
    }
}
