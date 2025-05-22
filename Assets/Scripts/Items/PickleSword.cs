/*****************************************************************************
// File Name : PickleSword.cs
// Author : Lucas Fehlberg
// Creation Date : May 16, 2025
// Last Updated : May 17, 2025
//
// Brief Description : Upon killing an enemy, gain +1 attack
*****************************************************************************/

using UnityEngine;

public class PickleSword: Item
{
    private bool used = false;
    /// <summary>
    /// Set item defaults
    /// </summary>
    public override void SetDefaults()
    {
        itemName = "PickleSword";
        itemNameDisplay = "Pickle Sword";

        itemDescription = "The first enemy you kill each turn grants +1 attack.";

        itemRarity = 2;
    }

    /// <summary>
    /// When killing an enemy, add one to attack 
    /// </summary>
    /// <param name="position">Unused here</param>
    public override void OnKillEnemy(Vector3 position, bool voidKill = false)
    {
        if (used)
        {
            return;
        }
        if (voidKill)
        {
            return;
        }

        player.AttackRemaining += 1;
        player.UpdateStats();

        used = true;
    }

    /// <summary>
    /// Reset
    /// </summary>
    public override void OnStartTurn()
    {
        used = false;
    }
}
