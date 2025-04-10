/*****************************************************************************
// File Name : RustyDagger.cs
// Author : Lucas Fehlberg
// Creation Date : March 30, 2025
// Last Updated : April 4, 2025
//
// Brief Description : A basic item that increases attack, and allows for 1 square of attack in all directions
*****************************************************************************/

using UnityEngine;

public class RustyDagger : Item
{
    /// <summary>
    /// Add one to movement
    /// </summary>
    public override void UpdateStats()
    {
        Stats.Attack += 1;
    }

    /// <summary>
    /// We want lateUpdate, as there will be items that completely overhaul attack, and this dagger is meant to
    /// still synergize
    /// </summary>
    public override void LateUpdateStats()
    {
        Stats.PossibleAttacks.Add(Vector2.up);
        Stats.PossibleAttacks.Add(Vector2.right);
        Stats.PossibleAttacks.Add(Vector2.down);
        Stats.PossibleAttacks.Add(Vector2.left);
        Stats.PossibleAttacks.Add(Vector2.up + Vector2.left);
        Stats.PossibleAttacks.Add(Vector2.up + Vector2.right);
        Stats.PossibleAttacks.Add(Vector2.down + Vector2.left);
        Stats.PossibleAttacks.Add(Vector2.down + Vector2.right);
    }


    /// <summary>
    /// Set item defaults
    /// </summary>
    public override void SetDefaults()
    {
        itemName = "RustyDagger";
        itemNameDisplay = "Rusty Dagger";
        itemDescription = "+1 Attack\nAllows for one square of attack in all directions";

        itemRarity = 0;
    }
}
