/*****************************************************************************
// File Name : ProfessorsDice.cs
// Author : Lucas Fehlberg
// Creation Date : April 5, 2025
// Last Updated : April 5, 2025
//
// Brief Description : Reroll +1
*****************************************************************************/

public class ProfessorsDice : Item
{
    /// <summary>
    /// Adds one to attack and manipulation
    /// </summary>
    public override void UpdateStats()
    {
        Stats.Rerolls += 1;
    }

    /// <summary>
    /// Set itemName and itemDescription
    /// </summary>
    public override void SetDefaults()
    {
        itemName = "ProfessorsDice";
        itemNameDisplay = "Professor's Dice";
        itemDescription = "Rerolls random events for a positive outcome";

        itemRarity = 2;
    }
}
