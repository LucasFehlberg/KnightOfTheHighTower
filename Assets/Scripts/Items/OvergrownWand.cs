/*****************************************************************************
// File Name : OvergrownWand.cs
// Author : Lucas Fehlberg
// Creation Date : April 4, 2025
// Last Updated : April 4, 2025
//
// Brief Description : A basic item that increases manipulation and terrain manipulation range
*****************************************************************************/

public class OvergrownWand : Item
{
    /// <summary>
    /// Adds One to Manipulation and One to Range
    /// </summary>
    public override void UpdateStats()
    {
        Stats.Manipulation += 1;
        Stats.TerrainRange += 1;
    }

    /// <summary>
    /// Set itemName and itemDescription
    /// </summary>
    public override void SetDefaults()
    {
        itemName = "OvergrownWand";
        itemNameDisplay = "Overgrown Wand";
        itemDescription = "+1 Manipulation\nIncreases range of terrain manipulation";

        itemRarity = 0;
    }
}
