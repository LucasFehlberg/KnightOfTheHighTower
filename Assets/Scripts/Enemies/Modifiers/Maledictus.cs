/*****************************************************************************
// File Name : Maledictus.cs
// Author : Lucas Fehlberg
// Creation Date : April 5, 2025
// Last Updated : April 26, 2025
//
// Brief Description : When this enemy is killed, all other enemies take 1 damage
*****************************************************************************/

using System.Collections.Generic;
using UnityEngine;

public class Maledictus : Modifier
{
    private List<EnemyBase> stopTheLoops = new List<EnemyBase>();
    /// <summary>
    /// Sets the name of a modifier
    /// </summary>
    public override void SetDefaults()
    {
        modifierName = "Maledictus";
        modifierDescription = "When this enemy is killed, deal 1 damage to all other enemies on the board";
        modifierDifficulty = -1;
    }

    /// <summary>
    /// When the enemy is killed, deal 1 damage
    /// </summary>
    public override void OnKill()
    {
        foreach(GameObject enemy in GameObject.FindGameObjectsWithTag("Enemy"))
        {
            if(enemy == this.enemy.gameObject)
            {
                continue;
            }
            if (stopTheLoops.Contains(enemy.GetComponent<EnemyBase>()))
            {
                continue;
            }
            enemy.GetComponent<EnemyBase>().TakeDamage(1);
            stopTheLoops.Add(enemy.GetComponent<EnemyBase>());
        }
    }

    /// <summary>
    /// Loads in the particle effect
    /// </summary>
    public override void OnLoad()
    {
        Object.Instantiate(Resources.Load("ParticleFX/MaledictusParticles"), enemy.transform);
    }
}
