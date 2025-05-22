/*****************************************************************************
// File Name : Stunned.cs
// Author : Lucas Fehlberg
// Creation Date : April 2, 2025
// Last Updated : April 13, 2025
//
// Brief Description : Prevents actions on this turn when stunned
*****************************************************************************/

using System.Collections;
using Unity.VisualScripting.FullSerializer;
using UnityEngine;

public class Stunned : Modifier
{
    private int timer = 0;

    private GameObject particles;

    /// <summary>
    /// Constructor
    /// </summary>
    public Stunned()
    {
        timer = 1;
    }

    /// <summary>
    /// Alternate constructor
    /// </summary>
    /// <param name="time"></param>
    public Stunned(int time)
    {
        timer = time;
    }

    /// <summary>
    /// 
    /// </summary>
    public override void SetDefaults()
    {
        modifierDifficulty = -1;
        modifierName = "Stunned";
        modifierDescription = "This enemy cannot take actions this turn";
    }

    /// <summary>
    /// Stun the enemy
    /// </summary>
    public override void OnStartTurn()
    {
        if(timer > 0)
        {
            timer--;
            enemy.AttackRemaining = 0;
            enemy.MovementRemaining = 0;
        }
        else if (!particles.GetComponent<ParticleSystem>().isStopped)
        {
            particles.GetComponent<ParticleSystem>().Stop();
        }
        //if (timer <= 0)
        //{
        //    enemy.StartCoroutine(RemoveMe());
        //}
    }

    /// <summary>
    /// Deletes this
    /// </summary>
    /// <returns></returns>
    private IEnumerator RemoveMe()
    {
        yield return new WaitForEndOfFrame();
        enemy.Modifiers.Remove(this);
    }

    /// <summary>
    /// Loads in the particle effect
    /// </summary>
    public override void OnLoad()
    {
        particles = (GameObject)Object.Instantiate(Resources.Load("ParticleFX/StunnedParticles"), enemy.transform);
    }
}
