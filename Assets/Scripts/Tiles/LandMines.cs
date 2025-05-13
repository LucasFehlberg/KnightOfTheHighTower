/*****************************************************************************
// File Name : LandMines.cs
// Author : Lucas Fehlberg
// Creation Date : May 13, 2025
// Last Updated : May 13, 2025
//
// Brief Description : LandMines script
*****************************************************************************/

using UnityEngine;

public class LandMines : AttatchmentBase
{
    private Tile tileOn;

    /// <summary>
    /// Get the tile this mine is on
    /// </summary>
    private void Start()
    {
        tileOn = transform.parent.parent.GetComponent<Tile>();
    }

    /// <summary>
    /// When the plate is entered, move the entity if possible
    /// </summary>
    /// <param name="other"></param>
    public override void OnLandedOn(Collider other)
    {
        if(other.TryGetComponent(out PlayerBase player))
        {
            player.TakeDamage(2);
            player.CurrentTile = null;
        }
        else if (other.TryGetComponent(out EnemyBase enemy))
        {
            enemy.TakeDamage(2);
            enemy.CurrentTile = null;
        }

        tileOn.BuiltUpon = false;
        Destroy(gameObject);
    }
}
