/*****************************************************************************
// File Name : FallenHalo.cs
// Author : Lucas Fehlberg
// Creation Date : May 14, 2025
// Last Updated : May 14, 2025
//
// Brief Description : Revives when the player dies
*****************************************************************************/

using UnityEngine;

public class FallenHalo : Item
{
    private bool used = false;
    /// <summary>
    /// Sets name
    /// </summary>
    public override void SetDefaults()
    {
        itemName = "FallenHalo";
        itemDescription = "Grants a singular revive to 1/3 health. Does not reset.";
        itemNameDisplay = "Fallen Halo";

        itemRarity = 3;
    }

    /// <summary>
    /// Revives the player
    /// </summary>
    public override void OnDeath()
    {
        if (!used)
        {
            used = true;
            player.HealthRemaining = (int)Mathf.Ceil(Stats.Health / 3f);
            player.UpdateStats();
            itemDescription = "A trinket of former glory.";
            itemNameDisplay = "Tainted Halo";
        }
    }
}
