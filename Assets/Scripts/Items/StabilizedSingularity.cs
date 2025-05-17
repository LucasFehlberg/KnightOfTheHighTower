/*****************************************************************************
// File Name : PickleSword.cs
// Author : Lucas Fehlberg
// Creation Date : May 17, 2025
// Last Updated : May 17, 2025
//
// Brief Description : Sending enemies to the void stores their matter as manipulation
*****************************************************************************/

using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class StabilizedSingularity : Item
{
    private int bonus = 0;
    private static bool bonusUsed = false;
    private static bool checkPlayerManip = false;
    private static List<StabilizedSingularity> singularities = new();

    private static int originalPlayerManipulation;
    /// <summary>
    /// Set item defaults
    /// </summary>
    public override void SetDefaults()
    {
        itemName = "StabilizedSingularity";
        itemNameDisplay = "Stabilized Singularity";

        itemDescription = "Enemies dying to the void get stored as manipulation. Using this manipulation drains this" +
            " reserve.\n Currently +" + bonus.ToString() + "."; 

        itemRarity = 3;
    }

    /// <summary>
    /// Adds this to singularities
    /// </summary>
    public override void UpdateStats()
    {
        checkPlayerManip = false;
        bonusUsed = false;
        if (!singularities.Contains(this))
        {
            singularities.Add(this);
        }
    }

    /// <summary>
    /// When the void kills an enemy, increase bonus
    /// </summary>
    /// <param name="position"></param>
    /// <param name="voidKill"></param>
    public override void OnKillEnemy(Vector3 position, bool voidKill = false)
    {
        if (!voidKill)
        {
            return;
        }

        bonus++;
        itemDescription = "Enemies dying to the void get stored as manipulation. Using this manipulation drains this" +
            " reserve.\n Currently +" + bonus.ToString() + ".";
    }

    public override void OnStartTurn()
    {
        if (!checkPlayerManip)
        {
            originalPlayerManipulation = player.ManipulationRemaining;
            checkPlayerManip = true;
        }
        player.ManipulationRemaining += bonus;
    }

    /// <summary>
    /// Lower only ONE bonus on manipulation
    /// </summary>
    /// <param name="position"></param>
    /// <param name="type"></param>
    public override void OnTerrainManipulation(Vector3 position, string type, bool consumeTerrain)
    {
        if (!consumeTerrain)
        {
            return;
        }
        if (bonus > 0 && !bonusUsed)
        {
            bonusUsed = true;
            if (originalPlayerManipulation > 0)
            {
                originalPlayerManipulation--;
                return;
            }
            bonus--;
            itemDescription = "Enemies dying to the void get stored as manipulation. Using this manipulation drains this" +
                " reserve.\n Currently +" + bonus.ToString() + ".";
        }

        if (this == singularities[^1])
        {
            bonusUsed = false;
        }
    }

    public override void OnEndTurn()
    {
        checkPlayerManip = false;
    }
}
