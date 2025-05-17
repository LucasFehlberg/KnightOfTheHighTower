/*****************************************************************************
// File Name : Glass.cs
// Author : Lucas Fehlberg
// Creation Date : May 17, 2025
// Last Updated : May 17, 2025
//
// Brief Description : Glass Script
*****************************************************************************/

using UnityEngine;

public class Glass : AttatchmentBase
{
    private Tile tileOn;
    [SerializeField] private Mesh cracked;
    private bool isCracked = false;

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
        if (!isCracked)
        {
            isCracked = true;
            GetComponent<MeshFilter>().mesh = cracked;
            return;
        }

        tileOn.BuiltUpon = false;
        tileOn.Destroy();
        Destroy(gameObject);
    }
}
