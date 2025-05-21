/*****************************************************************************
// File Name : BigFreakingBoot.cs
// Author : Lucas Fehlberg
// Creation Date : April 2, 2025
// Last Updated : May 13, 2025
//
// Brief Description : An item that stuns enemies on movement
*****************************************************************************/

using System.Linq;
using UnityEngine;

public class BigFreakingBoot : Item
{
    /// <summary>
    /// Stun nearby enemies
    /// </summary>
    /// <param name="endPosition">Position of stun</param>
    public override void OnMove(Vector3 startPosition, Vector3 endPosition)
    {
        int numStun = 0;
        foreach(Item item in Stats.HeldItems)
        {
            if(item.GetType() == typeof(BigFreakingBoot))
            {
                numStun++;
            }
        }

        for (int x = -1; x < 2; x++)
        {
            for (int z = -1; z < 2; z++)
            {
                if(x == 0 && z == 0)
                {
                    continue;
                }

                Vector3 testPos = endPosition + new Vector3(x, 0, z);

                if (!Physics.CheckBox(testPos, Vector3.one * 0.45f, Quaternion.identity, enemyLayers))
                {
                    continue;
                }

                EnemyBase enemy = Physics.OverlapBox(testPos, Vector3.one * 0.45f, Quaternion.identity, enemyLayers)[0]
                    .GetComponent<EnemyBase>();

                if (!enemy.Modifiers.OfType<Stunned>().Any())
                {
                    enemy.AddModifier(new Stunned(numStun));
                }
            }
        }
    }

    /// <summary>
    /// Set itemName and itemDescription
    /// </summary>
    public override void SetDefaults()
    {
        itemName = "BigFreakingBoot";
        itemNameDisplay = "Big Freaking Boot";
        itemDescription = "Increases the amount of stun after moving near an enemy. Enemies can only be stunned" +
            "once per floor";
        itemRarity = 1;
    }
}
