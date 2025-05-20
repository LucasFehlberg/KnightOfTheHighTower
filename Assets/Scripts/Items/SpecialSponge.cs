/*****************************************************************************
// File Name : SpecialSponge.cs
// Author : Lucas Fehlberg
// Creation Date : May 19, 2025
// Last Updated : May 19, 2025
//
// Brief Description : Removing a non-wall and non-floor tile grants an extra manipulation
*****************************************************************************/

using UnityEngine;

public class SpecialSponge : Item
{
    private bool notUsed = true;
    //Man this is complicated for a common item
    //private static List<TrustyTrowel> allTrowels = new();
    //private List<TrustyTrowel> lastCheckedList = new();

    /// <summary>
    /// Set itemName and itemDescription
    /// </summary>
    public override void SetDefaults()
    {
        itemName = "SpecialSponge";
        itemNameDisplay = "Special Sponge";
        itemDescription = "Removing a non-floor and non-wall tile grants you +1 manipulation.\nWorks once per sponge.";

        itemRarity = 1;
    }

    ///// <summary>
    ///// When a wall is added
    ///// </summary>
    ///// <param name="position"></param>
    ///// <param name="type"></param>
    //public override void OnTerrainManipulation(Vector3 position, string type)
    //{
    //    //Super complicated math here
    //    if (type == "Wall" && allTrowels.Contains(this) && allTrowels.Count == lastCheckedList.Count)
    //    {
    //        allTrowels.Remove(this);
    //        player.ManipulationRemaining += 1;
    //        lastCheckedList.Remove(this);
    //    }
    //    else if (type == "Wall" & allTrowels.Contains(this))
    //    {
    //        lastCheckedList.Clear();
    //        foreach (TrustyTrowel trowel in allTrowels)
    //        {
    //            lastCheckedList.Add(trowel);
    //        }
    //    }
    //}

    /// <summary>
    /// Doesn't consume terrain if sponge is active and conditions are met
    /// </summary>
    /// <param name="resource"></param>
    /// <returns></returns>
    public override bool ConsumeTerrain(Vector3 position, string type, Tile tile)
    {
        if (!notUsed)
        {
            return true;
        }

        if(type != "Remove")
        {
            return true;
        }

        if (!tile.BuiltUpon)
        {
            return true;
        }

        if (tile.Wall.activeSelf)
        {
            return true;
        }

        player.GetComponent<PlayerTerrain>().FreebiesType.Remove("Remove");
        player.ManipulationRemaining++;
        player.UpdateStats();
        notUsed = false;
        return false;
    }

    /// <summary>
    /// Re-add the freebie and reset
    /// </summary>
    public override void OnStartTurn()
    {
        notUsed = true;
        player.GetComponent<PlayerTerrain>().FreebiesType.Add("Remove");
        //allTrowels.Add(this);
        //foreach(Item item in Stats.HeldItems)
        //{
        //    if(item.GetType() == typeof(TrustyTrowel))
        //    {
        //        lastCheckedList.Add(item as TrustyTrowel);
        //    }
        //}
    }

    /// <summary>
    /// Resets to 0 at the end of the turn to prevent infinite stacking
    /// </summary>
    public override void OnEndTurn()
    {
        //lastCheckedList.Clear();
        //allTrowels.Clear();
    }

    /// <summary>
    /// Fixes a bug(?) when not using trowel at the end of a round, carrying them over to the new round
    /// </summary>
    public override void UpdateStats()
    {
        //lastCheckedList.Clear();
        //allTrowels.Clear();
    }
}
