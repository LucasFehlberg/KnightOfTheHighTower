/*****************************************************************************
// File Name : TrustyTrowel.cs
// Author : Lucas Fehlberg
// Creation Date : April 29, 2025
// Last Updated : April 29, 2025
//
// Brief Description : Allow for more walls to be placed per-turn
*****************************************************************************/

using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class TrustyTrowel : Item
{
    //Man this is complicated for a common item
    private static List<TrustyTrowel> allTrowels = new();
    private List<TrustyTrowel> lastCheckedList = new();

    /// <summary>
    /// Set itemName and itemDescription
    /// </summary>
    public override void SetDefaults()
    {
        itemName = "TrustyTrowel";
        itemNameDisplay = "Trusty Trowel";
        itemDescription = "Allows you to place a wall or floor without consuming any manipulation";

        itemRarity = 1;
    }

    /// <summary>
    /// When a wall is added
    /// </summary>
    /// <param name="position"></param>
    /// <param name="type"></param>
    public override void OnTerrainManipulation(Vector3 position, string type)
    {
        //Super complicated math here
        if(type == "Wall" && allTrowels.Contains(this) && allTrowels.Count == lastCheckedList.Count)
        {
            allTrowels.Remove(this);
            player.ManipulationRemaining += 1;
            lastCheckedList.Remove(this);
        } 
        else if(allTrowels.Contains(this))
        {
            lastCheckedList.Clear();
            foreach (TrustyTrowel trowel in allTrowels)
            {
                lastCheckedList.Add(trowel);
            }
        }
    }

    /// <summary>
    /// Add the added damage
    /// </summary>
    public override void OnStartTurn()
    {
        allTrowels.Add(this);
        foreach(Item item in Stats.HeldItems)
        {
            if(item.GetType() == typeof(TrustyTrowel))
            {
                lastCheckedList.Add(item as TrustyTrowel);
            }
        }
    }

    /// <summary>
    /// Resets to 0 at the end of the turn to prevent infinite stacking
    /// </summary>
    public override void OnEndTurn()
    {
        lastCheckedList.Clear();
        allTrowels.Clear();
    }

    /// <summary>
    /// Fixes a bug(?) when not using trowel at the end of a round, carrying them over to the new round
    /// </summary>
    public override void UpdateStats()
    {
        lastCheckedList.Clear();
        allTrowels.Clear();
    }
}
