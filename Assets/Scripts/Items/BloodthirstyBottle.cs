/*****************************************************************************
// File Name : BloodthirstyBottle.cs
// Author : Lucas Fehlberg
// Creation Date : April 5, 2025
// Last Updated : April 5, 2025
//
// Brief Description : An item that, onKill, grants a 50% chance (affected by luck stats) to regain health
*****************************************************************************/

using UnityEngine;

public class BloodthirstyBottle: Item
{
    /// <summary>
    /// Set item defaults
    /// </summary>
    public override void SetDefaults()
    {
        itemName = "BloodthirstyBottle";
        itemNameDisplay = "Bloodthirsty Bottle";
        itemDescription = "Grants a 50% chance (affected by luck stats) to regain +1 health (cannot go over max)";

        itemRarity = 2;
    }

    /// <summary>
    /// When the enemy is killed, regain health
    /// </summary>
    /// <param name="position">Unused here</param>
    public override void OnKillEnemy(Vector3 position)
    {
        if (player.HealthRemaining >= Stats.Health)
        {
            return;
        }

        if (RandomEffect(1, 2))
        {
            player.HealthRemaining += 1;
            player.UpdateStats();
        }
    }
}
