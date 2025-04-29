/*****************************************************************************
// File Name : TricolorClock.cs
// Author : Lucas Fehlberg
// Creation Date : April 28, 2025
// Last Updated : April 28, 2025
//
// Brief Description : Grants a stat in order
*****************************************************************************/

using System.Collections.Generic;

public class TricolorClock : Item
{
    private int index = 0;
    private readonly List<string> effects = new List<string>()
    {
        "Terrain",
        "Movement",
        "Attack"
    };
    /// <summary>
    /// Sets name and other stuff
    /// </summary>
    public override void SetDefaults()
    {
        itemName = "TricolorClock";
        itemDescription = "Grants +1 stat, alternates each round (currently: " + effects[index] + ").";
        itemNameDisplay = "Tricolor Clock";

        itemRarity = 2;
    }

    /// <summary>
    /// Add the selected stat
    /// </summary>
    public override void OnStartTurn()
    {
        //Resets description
        itemDescription = "Grants +1 stat, alternates each round (currently: " + effects[index] + ").";

        switch (effects[index])
        {
            case "Terrain":
                player.ManipulationRemaining += 1;
                break;
            case "Movement":
                player.MovementRemaining += 1;
                break;
            case "Attack":
                player.AttackRemaining += 1;
                break;
        }

        index++;
        if(index > 2)
        {
            index = 0;
        }
    }
}
