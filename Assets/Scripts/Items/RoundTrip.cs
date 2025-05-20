/*****************************************************************************
// File Name : RoundTrip.cs
// Author : Lucas Fehlberg
// Creation Date : May 20, 2025
// Last Updated : May 20, 2025
//
// Brief Description : An item that lets you move to the other side of the board instantly
*****************************************************************************/

using UnityEngine;

public class RoundTrip : Item
{
    /// <summary>
    /// Add one to movement
    /// </summary>
    public override void UpdateStats()
    {
        Stats.Movement += 1;
    }

    /// <summary>
    /// We want lateUpdate, as there will be items that completely overhaul movement, and this is meant to
    /// still synergize
    /// </summary>
    public override void LateUpdateStats()
    {
        Stats.PossibleMovements.Add(Vector2.up * 7);
        Stats.PossibleMovements.Add(Vector2.right * 7);
        Stats.PossibleMovements.Add(Vector2.down * 7);
        Stats.PossibleMovements.Add(Vector2.left * 7);
        Stats.PossibleMovements.Add((Vector2.up + Vector2.left) * 7);
        Stats.PossibleMovements.Add((Vector2.up + Vector2.right) * 7);
        Stats.PossibleMovements.Add((Vector2.down + Vector2.left) * 7);
        Stats.PossibleMovements.Add((Vector2.down + Vector2.right) * 7);
    }

    /// <summary>
    /// Set itemName and itemDescription
    /// </summary>
    public override void SetDefaults()
    {
        itemName = "RoundTrip";
        itemNameDisplay = "Round Trip";
        itemDescription = "+1 Movement\nBeing on one side of the board allows you to move to the other";

        itemRarity = 3;
    }
}
