/*****************************************************************************
// File Name : BloodthirstyBottle.cs
// Author : Lucas Fehlberg
// Creation Date : April 5, 2025
// Last Updated : May 13, 2025
//
// Brief Description : An item that, onKill, grants a 25% chance (affected by luck stats) to regain health
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

        string chance = GetEffectChance(1, 4);

        itemDescription = "Grants a " + chance + 
            "% chance to regain +1 health on enemy kill (cannot go over max)";

        itemRarity = 2;
    }

    /// <summary>
    /// Changes the description when a new room is entered
    /// </summary>
    public override void OnStartTurn()
    {
        string chance = GetEffectChance(1, 4);

        itemDescription = "Grants a " + chance +
            "% chance to regain +1 health on enemy kill (cannot go over max)";
    }

    /// <summary>
    /// When the enemy is killed, regain health
    /// </summary>
    /// <param name="position">Unused here</param>
    public override void OnKillEnemy(Vector3 position, bool voidKill = false)
    {
        if (player.HealthRemaining >= Stats.Health)
        {
            return;
        }

        //Nerfed to 1 out of 10 because 1 out of 2 was OP especially when collecting luck items
        if (RandomEffect(1, 4))
        {
            player.HealthRemaining += 1;
            player.UpdateStats();
        }
    }
}
