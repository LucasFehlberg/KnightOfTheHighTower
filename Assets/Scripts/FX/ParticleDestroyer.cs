/*****************************************************************************
// File Name : ParticleDestroyer.cs
// Author : Lucas Fehlberg
// Creation Date : April 25, 2025
// Last Updated : April 25, 2025
//
// Brief Description : Destroys particle objects after they've been spawned
*****************************************************************************/

using System.Collections;
using UnityEngine;

public class ParticleDestroyer : MonoBehaviour
{
    [SerializeField] float playtime = 7.5f;
    /// <summary>
    /// Starts the coroutine
    /// </summary>
    void Start()
    {
        StartCoroutine(DestroyParticles());
    }

    /// <summary>
    /// Ends the particle's lifespan
    /// </summary>
    /// <returns></returns>
    private IEnumerator DestroyParticles()
    {
        yield return new WaitForSeconds(playtime);
        Destroy(gameObject);
    }
}
