/*****************************************************************************
// File Name : PiercingArrow.cs
// Author : Lucas Fehlberg
// Creation Date : April 2, 2025
// Last Updated : April 4, 2025
//
// Brief Description : Attacks keep moving through enemies, only stopping when it can't find one
//                     Also triggers OnHit effects
*****************************************************************************/

using UnityEngine;

public class PiercingArrow : Item
{
    private const int enemyLayers = 128; //Enemy Layers

    /// <summary>
    /// Set informational stuff up
    /// </summary>
    public override void SetDefaults()
    {
        itemName = "PiercingArrow";
        itemDescription = "Attacks continue until there is no enemy in sight\nTriggers other Attack effects";
        ItemNameDisplay = "Piercing Arrow";

        itemRarity = 1;
    }

    /// <summary>
    /// Deal damage to the next enemy
    /// </summary>
    /// <param name="enemy"></param>
    /// <param name="damage"></param>
    /// <param name="direction"></param>
    public override void OnAttack(EnemyBase enemy, int damage, Vector2 direction)
    {
        Vector3 testPos = enemy.transform.position + new Vector3(direction.x, 0, direction.y);

        if (!Physics.CheckBox(testPos, Vector3.one * 0.45f, Quaternion.identity, enemyLayers))
        {
            return;
        }

        EnemyBase newEnemy = Physics.OverlapBox(testPos, Vector3.one * 0.45f, Quaternion.identity, enemyLayers)[0]
            .GetComponent<EnemyBase>();

        newEnemy.TakeDamage(1);
        foreach(Item item in Stats.HeldItems)
        {
            item.OnAttack(newEnemy, damage, direction);
        }
    }
}
