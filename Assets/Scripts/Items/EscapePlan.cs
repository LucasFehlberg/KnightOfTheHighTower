/*****************************************************************************
// File Name : EscapePlan.cs
// Author : Lucas Fehlberg
// Creation Date : April 2, 2025
// Last Updated : April 4, 2025
//
// Brief Description : Allows for greater movement options on attack
//                     Grants +1 move for the first attack
*****************************************************************************/

using UnityEngine;

public class EscapePlan : Item
{
    private bool hasAttacked = false;

    /// <summary>
    /// Sets 
    /// </summary>
    public override void UpdateStats()
    {
        hasAttacked = false;
    }

    /// <summary>
    /// On attack, give the player +1 movement and increased movement options
    /// </summary>
    /// <param name="enemy"></param>
    /// <param name="damage"></param>
    public override void OnAttack(EnemyBase enemy, int damage, Vector2 direction)
    {
        if (!hasAttacked)
        {

        }
    }

    /// <summary>
    /// Set item defaults
    /// </summary>
    public override void SetDefaults()
    {
        itemName = "EscapePlan";
        itemNameDisplay = "Escape Plan";
        itemDescription = "The first attack allows for +1 movement\nIncreases movement options after attacking";

        itemRarity = 1;
    }
}
