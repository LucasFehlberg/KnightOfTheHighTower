/*****************************************************************************
// File Name : SentryTower.cs
// Author : Lucas Fehlberg
// Creation Date : April 13, 2025
// Last Updated : April 13, 2025
//
// Brief Description : Sentry Tower Script
*****************************************************************************/

using System.Collections.Generic;
using UnityEngine;

public class SentryTower : AttatchmentBase
{
    private List<Vector2> attackRange = new()
    {
        new Vector2(0, 2),
        new Vector2(0, 3),
        new Vector2(2, 2),
        new Vector2(0, -2),
        new Vector2(0, -3),
        new Vector2(-2, -2),
        new Vector2(2, 0),
        new Vector2(3, 0),
        new Vector2(-3, 0),
        new Vector2(-2, 0),
        new Vector2(-2, 2),
        new Vector2(2, -2),
    };

    /// <summary>
    /// When the tile is entered, give the entity increased attack range and one extra attack
    /// </summary>
    /// <param name="other"></param>
    public override void OnLandedOn(Collider other)
    {
        if(other.TryGetComponent(out PlayerBase player))
        {
            player.MovementRemaining = 0;
            player.AttackRemaining++;
            player.UpdateStats();

            Stats.AdditionalAttacks.AddRange(attackRange);
            player.UpdateMovementOptions();
        }

        if (other.TryGetComponent(out EnemyBase enemy))
        {
            enemy.MovementRemaining = 0;
            enemy.AttackRemaining++;

            enemy.AdditionalAttackOptions.AddRange(attackRange);
            enemy.UpdateMovementOptions();
        }
    }
}
