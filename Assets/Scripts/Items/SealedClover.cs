/*****************************************************************************
// File Name : SealedClover.cs
// Author : Lucas Fehlberg
// Creation Date : April 5, 2025
// Last Updated : May 13, 2025
//
// Brief Description : Increases terrain range
*****************************************************************************/

using UnityEngine;

public class SealedClover : Item
{
    private int tileBonus = 1;

    /// <summary>
    /// Set itemName and itemDescription
    /// </summary>
    public override void SetDefaults()
    {
        itemName = "SealedClover";
        itemNameDisplay = "Sealed Clover";
        itemDescription = "+" + tileBonus.ToString() + " Tile Manipulation Radius \nMoving removes this bonus" +
            "\nResets at the end of your turn";

        itemRarity = 1;
    }

    /// <summary>
    /// Applys bounus
    /// </summary>
    public override void OnStartTurn()
    {
        Stats.TerrainRange += tileBonus;
    }

    /// <summary>
    /// Resets the description and name
    /// </summary>
    public override void OnEndTurn()
    {
        itemNameDisplay = "Sealed Clover";
        tileBonus = 1;
        itemDescription = "+" + tileBonus.ToString() + " tile radius \nMoving removes this bonus\nResets " +
            "at the end of your turn";
    }

    /// <summary>
    /// When attacking, disable the bonus
    /// </summary>
    /// <param name="position"></param>
    public override void OnAttack(EnemyBase enemy, int damage, Vector2 direction)
    {
        Stats.TerrainRange -= tileBonus;

        itemNameDisplay = "Wilted Clover";
        tileBonus = 0;
        itemDescription = "+" + tileBonus.ToString() + " tile radius \nMoving removes this bonus\nResets " +
            "at the end of your turn";
    }
}
