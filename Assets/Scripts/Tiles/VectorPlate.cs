/*****************************************************************************
// File Name : VectorPlate.cs
// Author : Lucas Fehlberg
// Creation Date : April 8, 2025
// Last Updated : April 11, 2025
//
// Brief Description : Vector Plate script
*****************************************************************************/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VectorPlate : AttatchmentBase
{
    [SerializeField] private LayerMask obstacleLayers;

    private readonly List<Vector2> movements = new()
    {
        Vector2.up,
        Vector2.down,
        Vector2.left,
        Vector2.right,
    };

    /// <summary>
    /// Moves the entity
    /// </summary>
    /// <param name="entity">Player or enemy</param>
    /// <returns></returns>
    private IEnumerator MoveEntity(GameObject entity)
    {
        List<Vector2> possibleMovements = new();

        //List 1
        foreach (Vector2 movement in movements)
        {
            Vector3 testPos = transform.position + new Vector3(movement.x, 0.5f, movement.y);

            if (Physics.CheckBox(testPos, Vector3.one * 0.45f, Quaternion.identity,
                obstacleLayers))
            {
                continue;
            }

            possibleMovements.Add(movement);
        }

        //Check 1
        if (possibleMovements.Count == 0)
        {
            yield break;
        }

        if (entity.TryGetComponent(out PlayerMovement player))
        {
            if (player.TriggeredTiles.Contains(gameObject))
            {
                yield break;
            }

            player.TileTrigger = true;
            player.TriggeredTiles.Add(gameObject);

            while(player.Percentage < 1)
            {
                yield return null;
            }

            Vector2 movement = possibleMovements[Random.Range(0, possibleMovements.Count)];

            player.OriginalPos = player.transform.position;
            player.DesiredPos = player.transform.position + new Vector3(movement.x, 0, movement.y);
            player.Percentage = 0;
            player.TileTrigger = false;
        }

        if (entity.TryGetComponent(out EnemyMovement enemy))
        {
            if (enemy.TriggeredTiles.Contains(gameObject))
            {
                yield break;
            }

            enemy.TileTrigger = true;
            enemy.TriggeredTiles.Add(gameObject);

            while (enemy.Percentage < 1)
            {
                yield return null;
            }

            Vector2 movement = possibleMovements[Random.Range(0, possibleMovements.Count)];

            enemy.OriginalPos = enemy.transform.position;
            enemy.DesiredPos = enemy.transform.position + new Vector3(movement.x, 0, movement.y);
            enemy.Percentage = 0;
            enemy.TileTrigger = false;
            enemy.Moving = true;
        }
    }

    /// <summary>
    /// When the plate is entered, move the entity if possible
    /// </summary>
    /// <param name="other"></param>
    public override void OnLandedOn(Collider other)
    {
        StartCoroutine(nameof(MoveEntity), other.gameObject);
    }
}
