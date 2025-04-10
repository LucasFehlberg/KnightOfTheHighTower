/*****************************************************************************
// File Name : SidestepCharm.cs
// Author : Lucas Fehlberg
// Creation Date : March 29, 2025
// Last Updated : April 4, 2025
//
// Brief Description : A basic item that increases movement slightly and adds the ability to move one space in any
//      Direction
*****************************************************************************/

using UnityEngine;

public class SidestepCharm : Item
{
    /// <summary>
    /// Add one to movement
    /// </summary>
    public override void UpdateStats()
    {
        Stats.Movement += 1;
    }

    /// <summary>
    /// We want lateUpdate, as there will be items that completely overhaul movement, and this charm is meant to
    /// still synergize
    /// </summary>
    public override void LateUpdateStats()
    {
        Stats.PossibleMovements.Add(Vector2.up);
        Stats.PossibleMovements.Add(Vector2.right);
        Stats.PossibleMovements.Add(Vector2.down);
        Stats.PossibleMovements.Add(Vector2.left);
        Stats.PossibleMovements.Add(Vector2.up + Vector2.left);
        Stats.PossibleMovements.Add(Vector2.up + Vector2.right);
        Stats.PossibleMovements.Add(Vector2.down + Vector2.left);
        Stats.PossibleMovements.Add(Vector2.down + Vector2.right);
    }

    /// <summary>
    /// Set itemName and itemDescription
    /// </summary>
    public override void SetDefaults()
    {
        itemName = "SidestepCharm";
        itemNameDisplay = "Sidestep Charm";
        itemDescription = "+1 Movement\nAllows for one square of movement in all directions";

        itemRarity = 0;
    }
}
