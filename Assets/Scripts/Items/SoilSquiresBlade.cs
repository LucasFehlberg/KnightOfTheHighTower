/*****************************************************************************
// File Name : SoilSquiresBlade.cs
// Author : Lucas Fehlberg
// Creation Date : April 4, 2025
// Last Updated : April 4, 2025
//
// Brief Description : Basic item that increases the attack and manipulation stats
*****************************************************************************/

public class SoilSquiresBlade : Item
{
    /// <summary>
    /// Adds one to attack and manipulation
    /// </summary>
    public override void UpdateStats()
    {
        Stats.Attack += 1;
        Stats.Manipulation += 1;
    }

    /// <summary>
    /// Set itemName and itemDescription
    /// </summary>
    public override void SetDefaults()
    {
        itemName = "SoilSquiresBlade";
        itemNameDisplay = "Soil Squire's Sword";
        itemDescription = "+1 Attack\n+1 Manipulation";

        itemRarity = 1;
    }
}
