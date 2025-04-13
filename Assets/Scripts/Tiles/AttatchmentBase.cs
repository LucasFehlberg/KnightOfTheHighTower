/*****************************************************************************
// File Name : AttatchmentBase.cs
// Author : Lucas Fehlberg
// Creation Date : April 8, 2025
// Last Updated : April 11, 2025
//
// Brief Description : Base Script for Attatchments
*****************************************************************************/

using UnityEngine;

public class AttatchmentBase : MonoBehaviour
{
    /// <summary>
    /// Runs OnLandedOn. Use that instead
    /// </summary>
    /// <param name="other"></param>
    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent(out PlayerBase player))
        {
            player.CurrentTile = this;
        }

        if (other.TryGetComponent(out EnemyBase enemy))
        {
            enemy.CurrentTile = this;
        }

        OnLandedOn(other);
    }

    /// <summary>
    /// When this plate is landed on
    /// </summary>
    /// <param name="other"></param>
    public virtual void OnLandedOn(Collider other)
    {

    }

    /// <summary>
    /// Runs OnExited. Use that instead
    /// </summary>
    /// <param name="other"></param>
    private void OnTriggerExit(Collider other)
    {
        if(other.TryGetComponent(out PlayerBase player))
        {
            player.CurrentTile = null;
        }

        if (other.TryGetComponent(out EnemyBase enemy))
        {
            enemy.CurrentTile = null;
        }

        OnExited(other);
    }

    /// <summary>
    /// Runs when a piece steps off of the plate
    /// </summary>
    /// <param name="other"></param>
    public virtual void OnExited(Collider other)
    {

    }

    /// <summary>
    /// When a piece is still on this plate at the end of a turn
    /// </summary>
    public virtual void OnEndTurnActive(GameObject go)
    {

    }
}
