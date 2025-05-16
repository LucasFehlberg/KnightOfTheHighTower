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
        itemDescription = "Grants a singular revive to 2/3 health, skipping all other enemy's turns.";
        itemNameDisplay = "Fallen Halo";

        itemRarity = 3;
    }

    /// <summary>
    /// Revives the player
    /// </summary>
    public override bool OnDeath()
    {
        if (!used)
        {
            used = true;
            player.UpdateStats();
            itemDescription = "A trinket of former glory.";
            itemNameDisplay = "Tainted Halo";

            GameObject.FindGameObjectWithTag("GameController").GetComponent<GameController>().CurrentIndex = -1;

            player.StartCoroutine(player.Revive(Mathf.CeilToInt(Stats.Health * 2f / 3f)));

            return true;
        }

        return false;
    }
}
