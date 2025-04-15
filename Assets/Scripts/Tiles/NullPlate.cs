/*****************************************************************************
// File Name : NullPlate.cs
// Author : Lucas Fehlberg
// Creation Date : April 13, 2025
// Last Updated : April 13, 2025
//
// Brief Description : Null Plate Script
*****************************************************************************/

using UnityEngine;

public class NullPlate : AttatchmentBase
{
    /// <summary>
    /// Freeze entity
    /// </summary>
    /// <param name="other">Entity</param>
    public override void OnLandedOn(Collider other)
    {
        if(other.TryGetComponent(out PlayerBase player))
        {
            player.AttackRemaining = 0;
            player.ManipulationRemaining = 0;
            player.MovementRemaining = 0;
            player.UpdateStats();
            return;
        }

        if (other.TryGetComponent(out EnemyBase enemy))
        {
            enemy.AttackRemaining = 0;
            enemy.MovementRemaining = 0;
        }
    }
}
