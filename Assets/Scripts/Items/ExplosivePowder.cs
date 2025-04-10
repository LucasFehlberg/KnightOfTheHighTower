/*****************************************************************************
// File Name : ExplosivePowder.cs
// Author : Lucas Fehlberg
// Creation Date : April 5, 2025
// Last Updated : April 5, 2025
//
// Brief Description : 1 tile Radius AOE on hit
*****************************************************************************/

using System.Collections.Generic;
using UnityEngine;

public class ExplosivePowder : Item
{
    private const int enemyLayers = 128; //Enemy Layers

    /// <summary>
    /// Set informational stuff up
    /// </summary>
    public override void SetDefaults()
    {
        itemName = "ExplosivePowder";
        itemDescription = "On Attack, deal 1 damage to every enemy adjacent orthagonally";
        ItemNameDisplay = "Explosive Powder";

        itemRarity = 2;
    }

    /// <summary>
    /// Deal damage to the next enemy
    /// </summary>
    /// <param name="enemy"></param>
    /// <param name="damage"></param>
    /// <param name="direction"></param>
    public override void OnAttack(EnemyBase enemy, int damage, Vector2 direction)
    {
        List<Vector2> pos = new()
        {
            Vector2.up,
            Vector2.down,
            Vector2.left,
            Vector2.right,
        };

        foreach (Vector2 vector in pos)
        {
            Vector3 testPos = enemy.transform.position + new Vector3(vector.x, 0, vector.y);

            if (!Physics.CheckBox(testPos, Vector3.one * 0.45f, Quaternion.identity, enemyLayers))
            {
                continue;
            }

            EnemyBase newEnemy = Physics.OverlapBox(testPos, Vector3.one * 0.45f, Quaternion.identity, enemyLayers)[0]
            .GetComponent<EnemyBase>();

            newEnemy.TakeDamage(1);
        }
    }
}
