/*****************************************************************************
// File Name : SealedClover.cs
// Author : Lucas Fehlberg
// Creation Date : April 5, 2025
// Last Updated : May 4, 2025
//
// Brief Description : Increases Luck (MinRandAlter)
*****************************************************************************/

public class SealedClover : Item
{
    /// <summary>
    /// Adds one to attack and manipulation
    /// </summary>
    public override void UpdateStats()
    {
        Stats.RandMinAlter += 1;
    }

    /// <summary>
    /// Set itemName and itemDescription
    /// </summary>
    public override void SetDefaults()
    {
        itemName = "SealedClover";
        itemNameDisplay = "Sealed Clover";
        itemDescription = "+1 Luck. Luck effects the numerator of random events happening.";

        itemRarity = -10;
    }
}
