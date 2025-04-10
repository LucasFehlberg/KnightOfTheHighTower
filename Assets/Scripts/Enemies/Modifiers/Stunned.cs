/*****************************************************************************
// File Name : Stunned.cs
// Author : Lucas Fehlberg
// Creation Date : April 2, 2025
// Last Updated : April 2, 2025
//
// Brief Description : Prevents actions on this turn when stunned
*****************************************************************************/

using System.Collections;
using UnityEngine;

public class Stunned : Modifier
{
    private int timer = 0;
    public Stunned()
    {
        timer = 1;
    }

    public Stunned(int time)
    {
        timer = time;
    }

    public override void SetDefaults()
    {
        modifierDifficulty = -1;
        modifierName = "Stunned";
    }

    /// <summary>
    /// Stun the enemy
    /// </summary>
    public override void OnStartTurn()
    {
        timer--;
        enemy.AttackRemaining = 0;
        enemy.MovementRemaining = 0;
        if (timer <= 0)
        {
            enemy.StartCoroutine(RemoveMe());
        }
    }

    private IEnumerator RemoveMe()
    {
        yield return new WaitForEndOfFrame();
        enemy.Modifiers.Remove(this);
    }
}
