/*****************************************************************************
// File Name : AdrenalineShot.cs
// Author : Lucas Fehlberg
// Creation Date : April 2, 2025
// Last Updated : April 4, 2025
//
// Brief Description : A basic item that increases Attack and Movement
*****************************************************************************/

public class AdrenalineShot: Item
{
    /// <summary>
    /// Add one to movement, one to attack
    /// </summary>
    public override void UpdateStats()
    {
        Stats.Attack += 1;
        Stats.Movement += 1;
    }


    /// <summary>
    /// Set item defaults
    /// </summary>
    public override void SetDefaults()
    {
        itemName = "AdrenalineShot";
        itemNameDisplay = "Adrenaline Shot";
        itemDescription = "+1 Attack\n+1 Move";

        itemRarity = 1;
    }
}
