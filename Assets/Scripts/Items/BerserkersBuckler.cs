/*****************************************************************************
// File Name : BerserkersBuckler.cs
// Author : Lucas Fehlberg
// Creation Date : April 29, 2025
// Last Updated : April 29, 2025
//
// Brief Description : When hit, increase damage
*****************************************************************************/

public class BerserkersBuckler : Item
{
    private int attackIncrease = 0;

    /// <summary>
    /// Resets on a new floor
    /// </summary>
    public override void UpdateStats()
    {
        attackIncrease = 0;
    }

    /// <summary>
    /// Set itemName and itemDescription
    /// </summary>
    public override void SetDefaults()
    {
        itemName = "BerserkersBuckler";
        itemNameDisplay = "Berserkers Buckler";
        itemDescription = "Increases Attack when hit. \nCurrently +" + attackIncrease.ToString();

        itemRarity = 1;
    }

    /// <summary>
    /// Adds to the attack increase
    /// </summary>
    /// <param name="damageTaken"></param>
    public override void OnTakeDamage(int damageTaken)
    {
        attackIncrease += damageTaken;
        itemDescription = "Increases Attack when hit. \nCurrently +" + attackIncrease.ToString();
    }

    /// <summary>
    /// Add the added damage
    /// </summary>
    public override void OnStartTurn()
    {
        player.AttackRemaining += attackIncrease;
    }
}
