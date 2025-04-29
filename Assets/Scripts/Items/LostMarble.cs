/*****************************************************************************
// File Name : LostMarble.cs
// Author : Lucas Fehlberg
// Creation Date : April 28, 2025
// Last Updated : April 28, 2025
//
// Brief Description : Grants +3 movement at start of floor, -1 movement per round
*****************************************************************************/

public class LostMarble : Item
{
    private int movement = 3;
    /// <summary>
    /// Sets name
    /// </summary>
    public override void SetDefaults()
    {
        itemName = "LostMarble";
        itemDescription = "Grants +" + movement.ToString() + " movement. Lose one per round (minimum: 0).";
        itemNameDisplay = "Lost Marble";

        itemRarity = 1;
    }

    /// <summary>
    /// Sets movement to the default
    /// </summary>
    public override void UpdateStats()
    {
        movement = 3;
    }

    /// <summary>
    /// Add remaining movement
    /// </summary>
    public override void OnStartTurn()
    {
        player.MovementRemaining += movement;

        itemDescription = "Grants +" + movement.ToString() + " movement. Lose one per round (minimum: 0).";

        movement -= 1;
        if(movement < 0)
        {
            movement = 0;
        }
    }
}
