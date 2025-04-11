/*****************************************************************************
// File Name : UrsaMaledictus.cs
// Author : Lucas Fehlberg
// Creation Date : March 30, 2025
// Last Updated : April 4, 2025
//
// Brief Description : An Item that, when the floor is started, gives a random enemy a modifier that,
                       when the enemy is killed, deal one damage to all other enemies
*****************************************************************************/

using UnityEngine;

public class UrsaMaledictus : Item
{
    /// <summary>
    /// Sets name
    /// </summary>
    public override void SetDefaults()
    {
        itemName = "UrsaMaledictus";
        itemDescription = "Apply Maledictus to a random enemy\nWhen that enemy is killed, deal 1 damage to all other "
            + "enemies";
        itemNameDisplay = "Ursa Maledictus";

        itemRarity = 3;
    }
    /// <summary>
    /// Add the modifier
    /// </summary>
    public override void UpdateStats()
    {
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
        EnemyBase enemy = enemies[Random.Range(0, enemies.Length)].GetComponent<EnemyBase>();

        enemy.Modifiers.Add(new Maledictus());
    }
}
