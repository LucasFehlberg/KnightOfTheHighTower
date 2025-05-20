/*****************************************************************************
// File Name : GravediggersShovel.cs
// Author : Lucas Fehlberg
// Creation Date : May 20, 2025
// Last Updated : May 20, 2025
//
// Brief Description : Removing a non-wall and non-floor tile grants an extra manipulation
*****************************************************************************/

using UnityEngine;
using System.Collections.Generic;
using Unity.VisualScripting.FullSerializer;

public class GravediggersShovel : Item
{
    //Man this is complicated for a common item
    private static List<GravediggersShovel> allShovels = new();
    private List<GravediggersShovel> lastCheckedList = new();

    private static List<Vector3> positions = new();

    /// <summary>
    /// Set itemName and itemDescription
    /// </summary>
    public override void SetDefaults()
    {
        itemName = "GravediggersShovel";
        itemNameDisplay = "Gravediggers Shovel";
        itemDescription = "+1 Manipulation\n-0.5 Terrain Range\nCan use manipulation to remove the floor beneath enemies once per turn.";

        itemRarity = 1;
    }

    /// <summary>
    /// Add one to manipulation
    /// </summary>
    public override void UpdateStats()
    {
        Stats.Manipulation += 1;
        allShovels.Clear();
        lastCheckedList.Clear();
    }

    /// <summary>
    /// Adds everything neccessary to the list
    /// </summary>
    public override void OnStartTurn()
    {
        allShovels.Add(this);
        Stats.TerrainRange -= 0.5f;

        foreach(Item item in Stats.HeldItems)
        {
            if(item.GetType() == typeof(GravediggersShovel))
            {
                lastCheckedList.Add(item as GravediggersShovel);
            }
        }
    }

    /// <summary>
    /// Clears the last checked list and all of that so no bugs
    /// </summary>
    public override void OnEndTurn()
    {
        allShovels.Clear();
        lastCheckedList.Clear();
    }

    /// <summary>
    /// Clears all positions
    /// </summary>
    public override void TerrainDisabled()
    {
        positions.Clear();
    }

    /// <summary>
    /// Validates invalid enemy tiles
    /// </summary>
    /// <param name="type">The type of manipulation</param>
    /// <param name="causeForDisable">Why the tile is normally invalid, usually enemy</param>
    /// <param name="position">The position of the tile</param>
    /// <returns></returns>
    public override bool CheckValidManipulation(string type, string causeForDisable, Vector3 position)
    {
        if (!allShovels.Contains(this))
        {
            return false;
        }

        if(type != "Remove")
        {
            return false;
        }

        if(causeForDisable != "Enemy")
        {
            return false;
        }

        Vector3 testPos = new(position.x, 1, position.z);

        EnemyBase enemy = Physics.OverlapBox(testPos, Vector3.one * 0.45f, Quaternion.identity, enemyLayers)[0]
            .GetComponent<EnemyBase>();

        if (enemy == null || enemy.HealthRemaining > player.ManipulationRemaining)
        {
            return false;
        }

        if (!positions.Contains(position))
        {
            positions.Add(position);
        }
        return true;
    }

    public override void OnTerrainManipulation(Vector3 position, string type, bool consumeTerrain)
    {
        if (allShovels.Contains(this) && lastCheckedList.Count == allShovels.Count)
        {
            if (!positions.Contains(position))
            {
                return;
            }

            Vector3 testPos = new(position.x, 1, position.z);

            EnemyBase enemy = Physics.OverlapBox(testPos, Vector3.one * 0.45f, Quaternion.identity, enemyLayers)[0]
                .GetComponent<EnemyBase>();

            Debug.Log(enemy.HealthRemaining);
            player.ManipulationRemaining -= (enemy.HealthRemaining - 1);

            enemy.KillEnemyFunny(false);


            allShovels.Remove(this);
            lastCheckedList.Remove(this);
        } 
        else if(lastCheckedList.Count != allShovels.Count)
        {
            lastCheckedList.Clear();

            foreach (GravediggersShovel item in allShovels)
            {
                lastCheckedList.Add(item);
            }

            return;
        }
    }
}
