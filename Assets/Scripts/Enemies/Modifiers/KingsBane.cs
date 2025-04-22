/*****************************************************************************
// File Name : KingsBane.cs
// Author : Lucas Fehlberg
// Creation Date : April 17, 2025
// Last Updated : April 17, 2025
//
// Brief Description : Prevents the player from utilizing one of their stats each turn
*****************************************************************************/

using System.Collections.Generic;
using Unity.VisualScripting.FullSerializer;
using UnityEngine;

public class KingsBane : Modifier
{
    private readonly List<string> stats = new List<string>()
    {
        "Movement",
        "Attack",
        "Manipulation"
    };

    /// <summary>
    /// Sets default stuff
    /// </summary>
    public override void SetDefaults()
    {
        modifierDifficulty = 0;
        modifierName = "King's Bane";
        modifierDescription = "Sets one stat to 1 each turn. Stat is chosen at random";
    }

    /// <summary>
    /// Sets one stat to 1 each turn
    /// </summary>
    public override void OnPlayerStartTurn(PlayerBase player)
    {
        string stat = stats[Random.Range(0, stats.Count)];

        switch (stat)
        {
            case "Movement":
                player.MovementRemaining = 1;
                break;
            case "Attack":
                player.AttackRemaining = 1;
                break;
            case "Manipulation":
                player.ManipulationRemaining = 1;
                break;
        }
        player.UpdateStats();
    }
}
