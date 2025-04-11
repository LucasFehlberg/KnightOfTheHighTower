/*****************************************************************************
// File Name : EscapePlan.cs
// Author : Lucas Fehlberg
// Creation Date : April 2, 2025
// Last Updated : April 4, 2025
//
// Brief Description : Allows for greater movement options on attack
//                     Grants +1 move for the first attack
*****************************************************************************/

using System;
using System.Collections.Generic;
using UnityEngine;

public class EscapePlan : Item
{
    private bool hasAttacked = false;

    private readonly List<Vector2> newMovements = new()
    {
        Vector2.up * 2,
        Vector2.down * 2,
        Vector2.left * 2,
        Vector2.right * 2,
        Vector2.one * 2,
        Vector2.one * -2,
        new Vector2(-2, 2),
        new Vector2(2, -2),
    };

    /// <summary>
    /// Sets hasAttacked to false
    /// </summary>
    public override void OnStartTurn()
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
            player.MovementRemaining++;

            Stats.AdditionalMovements.AddRange(newMovements);
            player.UpdateStats();
            player.UpdateMovementOptions();

            hasAttacked = true;
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
