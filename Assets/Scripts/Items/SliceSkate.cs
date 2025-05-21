/*****************************************************************************
// File Name : SliceSkate.cs
// Author : Lucas Fehlberg
// Creation Date : May 21, 2025
// Last Updated : May 21, 2025
//
// Brief Description : An item that stuns enemies on movement
*****************************************************************************/

using System.Collections.Generic;
using Unity.VisualScripting;
using Unity.VisualScripting.FullSerializer;
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
        Vector3 center = startPosition + (finalVector * 0.5f);

        Vector3 boxSize = new(0.5f, 0.5f, finalVector.magnitude);

        float angle = Mathf.Atan2(finalVector.x, finalVector.z) * Mathf.Rad2Deg;
        Quaternion rotation = Quaternion.Euler(0, angle, 0);

        Collider[] otherHits = Physics.OverlapBox(center, boxSize / 2, rotation, enemyLayers);

        foreach (Collider hit in otherHits)
        {
            hit.GetComponent<Collider>().GetComponent<EnemyBase>().TakeDamage(1, startPosition);

            foreach (Item item in Stats.HeldItems)
            {
                item.OnAttack(hit.GetComponent<Collider>().GetComponent<EnemyBase>(), 1, finalVector);
            }
        }

        //Vector3 moveDirection = endPosition - startPosition;
        //List<Vector3> evaluateAreas = new List<Vector3>
        //{
        //    startPosition
        //};

        //int addBy = 0;
        ////Prioritize the larger number
        //if (Mathf.Abs(moveDirection.x) < Mathf.Abs(moveDirection.z))
        //{
        //    if(Mathf.Abs(moveDirection.x) == moveDirection.x)
        //    {
        //        addBy = 1;
        //    } 
        //    else
        //    {
        //        addBy = -1;
        //    }

        //    for (int i = 1; i < Mathf.Abs(moveDirection.x) + 1; i++)
        //    {
        //        evaluateAreas.Add(new Vector3(evaluateAreas[^1].x + addBy, 1, evaluateAreas[^1].z));
        //    }

        //    if (Mathf.Abs(moveDirection.z) == moveDirection.z)
        //    {
        //        addBy = 1;
        //    }
        //    else
        //    {
        //        addBy = -1;
        //    }

        //    for (int i = 1; i < Mathf.Abs(moveDirection.z) + 1; i++)
        //    {
        //        evaluateAreas.Add(new Vector3(evaluateAreas[^1].x, 1, evaluateAreas[^1].z + addBy));
        //    }
        //}
        //else
        //{
        //    if (Mathf.Abs(moveDirection.z) == moveDirection.z)
        //    {
        //        addBy = 1;
        //    }
        //    else
        //    {
        //        addBy = -1;
        //    }

        //    for (int i = 1; i < Mathf.Abs(moveDirection.z) + 1; i++)
        //    {
        //        evaluateAreas.Add(new Vector3(evaluateAreas[^1].x, 1, evaluateAreas[^1].z + addBy));
        //    }

        //    if (Mathf.Abs(moveDirection.x) == moveDirection.x)
        //    {
        //        addBy = 1;
        //    }
        //    else
        //    {
        //        addBy = -1;
        //    }

        //    for (int i = 1; i < Mathf.Abs(moveDirection.x) + 1; i++)
        //    {
        //        evaluateAreas.Add(new Vector3(evaluateAreas[^1].x + addBy, 1, evaluateAreas[^1].z));
        //    }
        //}

        //foreach (Vector3 testPos in evaluateAreas)
        //{
        //    if (!Physics.CheckBox(testPos, Vector3.one * 0.45f, Quaternion.identity, enemyLayers))
        //    {
        //        continue;
        //    }

        //    EnemyBase enemy = Physics.OverlapBox(testPos, Vector3.one * 0.45f, Quaternion.identity, enemyLayers)[0]
        //        .GetComponent<EnemyBase>();

        //    enemy.TakeDamage(1, startPosition);

        //    foreach (Item item in Stats.HeldItems)
        //    {
        //        item.OnAttack(enemy, 1, testPos - startPosition);
        //    }
        //}
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
