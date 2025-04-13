/*****************************************************************************
// File Name : HotPlate.cs
// Author : Lucas Fehlberg
// Creation Date : April 12, 2025
// Last Updated : April 12, 2025
//
// Brief Description : Hot Plate Script
*****************************************************************************/

using UnityEngine;

public class HotPlate : AttatchmentBase
{
    /// <summary>
    /// Deal 1 damage to the entity
    /// </summary>
    /// <param name="go">Entity</param>
    public override void OnEndTurnActive(GameObject go)
    {
        if(go.TryGetComponent(out PlayerBase player))
        {
            player.TakeDamage(1);
        }

        if (go.TryGetComponent(out EnemyBase enemy))
        {
            enemy.TakeDamage(1);
        }
    }

}
