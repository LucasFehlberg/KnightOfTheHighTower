/*****************************************************************************
// File Name : Maledictus.cs
// Author : Lucas Fehlberg
// Creation Date : April 5, 2025
// Last Updated : April 8, 2025
//
// Brief Description : When this enemy is killed, all other enemies take 1 damage
*****************************************************************************/

using UnityEngine;

public class Maledictus : Modifier
{
    /// <summary>
    /// Sets the name of a modifier
    /// </summary>
    public override void SetDefaults()
    {
        modifierName = "Maledictus";
        modifierDifficulty = -1;
    }

    /// <summary>
    /// When the enemy is killed, deal 1 damage
    /// </summary>
    public override void OnKill()
    {
        foreach(GameObject enemy in GameObject.FindGameObjectsWithTag("Enemy"))
        {
            if(enemy == this.enemy)
            {
                return;
            }
            enemy.GetComponent<EnemyBase>().TakeDamage(1);
        }
    }
}
