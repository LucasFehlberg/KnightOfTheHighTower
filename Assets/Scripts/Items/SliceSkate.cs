/*****************************************************************************
// File Name : SliceSkate.cs
// Author : Lucas Fehlberg
// Creation Date : May 21, 2025
// Last Updated : May 21, 2025
//
// Brief Description : An item that stuns enemies on movement
*****************************************************************************/

using UnityEngine;

public class SliceSkate : Item
{
    /// <summary>
    /// Stun nearby enemies
    /// </summary>
    /// <param name="endPosition">Position of stun</param>
    public override void OnMove(Vector3 startPosition, Vector3 endPosition)
    {
        Vector3 finalVector = endPosition - startPosition;

        RaycastHit[] hits = Physics.RaycastAll(startPosition, finalVector.normalized, finalVector.magnitude, 
            enemyLayers);

        foreach (RaycastHit hit in hits)
        {
            hit.collider.GetComponent<EnemyBase>().TakeDamage(1, startPosition);

            foreach(Item item in Stats.HeldItems)
            {
                item.OnAttack(hit.collider.GetComponent<EnemyBase>(), 1, finalVector);
            }
        }
    }

    /// <summary>
    /// Set itemName and itemDescription
    /// </summary>
    public override void SetDefaults()
    {
        itemName = "SliceSkate";
        itemNameDisplay = "Slice Skate";
        itemDescription = "Allows you to move in a 3 spaces orthagonally.\n-1 Attack\nEnemies in your movement path" +
            "take 1 damage.";
        itemRarity = 2;
    }

    /// <summary>
    /// Lowers attack
    /// </summary>
    public override void UpdateStats()
    {
        Stats.Attack -= 1;
    }

    /// <summary>
    /// Lateupdate stats
    /// </summary>
    public override void LateUpdateStats()
    {
        Stats.PossibleMovements.Add(Vector2.up * 3);
        Stats.PossibleMovements.Add(Vector2.down * 3);
        Stats.PossibleMovements.Add(Vector2.left * 3);
        Stats.PossibleMovements.Add(Vector2.right * 3);
    }
}
