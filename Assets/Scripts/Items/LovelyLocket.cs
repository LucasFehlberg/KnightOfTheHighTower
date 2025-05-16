/*****************************************************************************
// File Name : LovelyLocket.cs
// Author : Lucas Fehlberg
// Creation Date : May 16, 2025
// Last Updated : May 16, 2025
//
// Brief Description : +1 max HP
*****************************************************************************/

public class LovelyLocket : Item
{
    /// <summary>
    /// Increases player life by one
    /// </summary>
    public override void UpdateStats()
    {
        Stats.Health += 1;
    }

    /// <summary>
    /// Set itemName and itemDescription
    /// </summary>
    public override void SetDefaults()
    {
        itemName = "LovelyLocket";
        itemNameDisplay = "Lovely Locket";
        itemDescription = "+1 Max HP";

        itemRarity = 1;
    }
}
