/*****************************************************************************
// File Name : NormalEnemyMovement.cs
// Author : Lucas Fehlberg
// Creation Date : April 1, 2025
// Last Updated : April 10, 2025
//
// Brief Description : Controls all normal enemy movement. Special movement will have their own scripts
*****************************************************************************/

using System.Collections.Generic;
using UnityEngine;

public class NormalEnemyMovement : EnemyMovement
{
    //[SerializeField] private LayerMask groundLayers;
    [SerializeField] private LayerMask obstacleLayers;
    [SerializeField] private LayerMask nodeLayers;

    /// <summary>
    /// Moves the knight. Probably one of the more complex movements
    /// </summary>
    public override void MoveEnemy()
    {
        if (moving)
        {
            return;
        }

        Transform player = GameObject.FindGameObjectWithTag("Player").transform;
        originalPos = transform.position;

        desiredPosition = Vector3.zero;

        moving = true;

        Node start = Physics.OverlapBox(transform.position, Vector3.one * 0.5f, Quaternion.identity, nodeLayers)[0]
            .GetComponent<Node>();

        if (tileTrigger)
        {
            enemy.Target = null;
            moving = false;
            return;
        }

        //This is for enemies with multiple per-turn movements. If they already have a target locked, then just
        //Continue using this target
        if (enemy.Target != null)
        {
            List<Node> path = GeneratePath(start, enemy.Target, enemy.PossibleNormalMovements, 
                enemy.PossibleInfiniteMovements, enemy.CanJumpHoles, enemy.CanJumpWalls);

            if (path == null || path.Count <= 1)
            {
                enemy.MovementRemaining = 0;
                moving = false;
                return;
            }

            desiredPos = path[1].transform.position;
            Percentage = 0;
            enemy.MovementRemaining--;

            return;
        }

        //List<Vector3> possibleMovements = new();

        ////Step 1: Calculate all possible positions
        //foreach (Vector2 movement in enemy.PossibleNormalMovements)
        //{
        //    Vector3 testPos = transform.position + new Vector3(movement.x, 0, movement.y);

        //    //Check to see if there is ground
        //    if (!Physics.CheckBox(testPos + Vector3.down, Vector3.one * 0.45f, Quaternion.identity, groundLayers))
        //    {
        //        continue;
        //    }

        //    //Next, we check for obstacles
        //    if (Physics.CheckBox(testPos, Vector3.one * 0.45f, Quaternion.identity, obstacleLayers))
        //    {
        //        continue;
        //    }

        //    possibleMovements.Add(testPos);
        //}

        //foreach (Vector2 movement in enemy.PossibleInfiniteMovements)
        //{
        //    possibleMovements.AddRange(CheckDirection(transform.position, movement));
        //}

        //Step 2: Calculate Optimal positions (1 step of attack away from player)

        List<Vector3> optimalPositions = new();

        foreach (Vector2 movement in enemy.PossibleNormalAttacks)
        {
            Vector3 testPos = player.position + new Vector3(movement.x, 0, movement.y);

            //Check to see if there is ground
            if (!Physics.CheckBox(testPos + Vector3.down, Vector3.one * 0.45f, Quaternion.identity, groundLayers))
            {
                continue;
            }

            //Next, we check for obstacles
            if (Physics.CheckBox(testPos, Vector3.one * 0.45f, Quaternion.identity, obstacleLayers))
            {
                continue;
            }

            optimalPositions.Add(testPos);
        }


        foreach (Vector2 movement in enemy.PossibleInfiniteAttacks)
        {
            optimalPositions.AddRange(CheckDirection(player.position, movement));
        }

        Vector3 closestPosition = player.position;

        List<Node> finalPath = null;
        List<Node> nullPoints = new();

        foreach (Vector3 pos in optimalPositions)
        {
            if(!Physics.OverlapBox(pos, Vector3.one * 0.5f, Quaternion.identity, nodeLayers)[0]
                .TryGetComponent<Node>(out var tempEnd))
            {
                continue;
            }

            List<Node> path = GeneratePath(start, tempEnd, enemy.PossibleNormalMovements, enemy.PossibleInfiniteMovements,
            enemy.CanJumpHoles, enemy.CanJumpWalls);

            if (path == null || path[^1] != tempEnd)
            {
                nullPoints.Add(tempEnd);
                continue;
            }
            //Check to see if the final path is shorter than the current path. Don't use distance - Knight's don't like
            //Distance and prefer to dance
            if (finalPath == null || path.Count < finalPath.Count || closestPosition == player.position)
            {
                closestPosition = pos;
                finalPath = path;
                enemy.Target = tempEnd;
            }
        }

        if(finalPath == null)
        {
            foreach(Node node in nullPoints)
            {
                List<Node> path = GeneratePath(start, node, enemy.PossibleNormalMovements, 
                    enemy.PossibleInfiniteMovements, enemy.CanJumpHoles, enemy.CanJumpWalls);

                if(path == null)
                {
                    continue;
                }

                if (Vector3.Distance(transform.position, node.transform.position) < 
                    Vector3.Distance(transform.position, closestPosition) || closestPosition == player.position)
                {
                    closestPosition = node.transform.position;
                    finalPath = path;
                    enemy.Target = node;
                }
            }
        }

        //If it's ourself, don't move. Might change this later to become smarter
        if (closestPosition == transform.position)
        {
            enemy.MovementRemaining = 0;
            moving = false;
            return;
        }

        ////Get the closest of all of these, defaulting to the player
        //foreach (Vector3 movement in optimalPositions)
        //{
        //    if(Vector3.Distance(transform.position, movement) < Vector3.Distance(transform.position, closestPosition)
        //        || closestPosition == player.position)
        //    {
        //        closestPosition = movement;
        //    }
        //}



        //Node end = Physics.OverlapBox(closestPosition, Vector3.one * 0.5f, Quaternion.identity, nodeLayers)[0]
        //    .GetComponent<Node>();

        //List<Node> nodes = GeneratePath(start, end, enemy.PossibleNormalMovements, enemy.PossibleInfiniteMovements, 
        //    enemy.CanJumpHoles, enemy.CanJumpWalls);

        if (finalPath == null || finalPath.Count <= 1)
        {
            enemy.MovementRemaining = 0;
            moving = false;
            return;
        }
        enemy.MovementRemaining--;
        desiredPos = finalPath[1].transform.position;
        Percentage = 0;

        ////Step 2.1: If we can reach an optimal position, go there, assuming we have the Attack remaining for it
        //if (enemy.AttackRemaining > 0)
        //{
        //    List<Vector3> possibleFinalOptions = new();
        //    foreach (Vector3 movement in possibleMovements)
        //    {
        //        if (optimalPositions.Contains(movement))
        //        {
        //            possibleFinalOptions.Add(movement);
        //        }
        //    }

        //    if (possibleFinalOptions.Count > 0)
        //    {
        //        desiredPos = possibleFinalOptions[Random.Range(0, possibleFinalOptions.Count)];
        //        Percentage = 0;
        //        return;
        //    }
        //}

        ////Step 3: Find position 2, which is one step away from position 1
        //List<Vector3> secondaryPositions = new();
        //List<Vector3> alreadyEvaluated = new();
        ////We'll do the infinite moves first, as those will have more than the normal moves

        //foreach(Vector3 startPos in optimalPositions)
        //{
        //    foreach(Vector2 direction in enemy.PossibleInfiniteMovements)
        //    {
        //        Vector3 testPos = startPos + new Vector3(direction.x, 0, direction.y);
        //        secondaryPositions.AddRange(CheckDirection(testPos, direction));
        //    }
        //}

        ////While we're at it, we'll clean up the list
        //List<int> cleanup = new();

        ////Adds positions to alreadyevaluated, or to cleanup if they're already evaluated
        //for (int i = 0; i < secondaryPositions.Count; i++)
        //{
        //    if (!alreadyEvaluated.Contains(secondaryPositions[i]))
        //    {
        //        alreadyEvaluated.Add(secondaryPositions[i]);
        //    } 
        //    else
        //    {
        //        cleanup.Add(i);
        //    }
        //}

        ////Reverse the list so we don't get issues later
        //cleanup.Reverse();

        ////Destroy any duplicate positions
        //foreach (int i in cleanup)
        //{
        //    secondaryPositions.RemoveAt(i);
        //}

        ////Now we handle all the normal movements
        //foreach (Vector3 startPos in optimalPositions)
        //{
        //    foreach(Vector2 movement in enemy.PossibleNormalMovements)
        //    {
        //        Vector3 testPos = startPos + new Vector3(movement.x, 0, movement.y);

        //        if (alreadyEvaluated.Contains(testPos))
        //        {
        //            continue;
        //        }

        //        //Check to see if there is ground
        //        if (!Physics.CheckBox(testPos + Vector3.down, Vector3.one * 0.45f, Quaternion.identity, groundLayers))
        //        {
        //            continue;
        //        }

        //        //Next, we check for obstacles
        //        if (Physics.CheckBox(testPos, Vector3.one * 0.45f, Quaternion.identity, obstacleLayers))
        //        {
        //            continue;
        //        }

        //        secondaryPositions.Add(testPos);
        //    }
        //}

        ////If we can reach any of these positions, go to them
        //List<Vector3> possibleFinalOptionsAgain = new();
        //foreach (Vector3 movement in possibleMovements)
        //{
        //    if (secondaryPositions.Contains(movement))
        //    {
        //        possibleFinalOptionsAgain.Add(movement);
        //    }
        //}

        //if (possibleFinalOptionsAgain.Count > 0)
        //{
        //    desiredPos = possibleFinalOptionsAgain[Random.Range(0, possibleFinalOptionsAgain.Count)];
        //    Percentage = 0;
        //    return;
        //}

        ////If that doesn't work, then we look into getting closer to the player
        //Vector3 closestPosition = transform.position;
        //foreach (Vector3 possible in possibleMovements)
        //{
        //    if (Vector3.Distance(player.position, possible) < Vector3.Distance(player.position, closestPosition))
        //    {
        //        closestPosition = possible;
        //    }
        //}

        ////Go to the closest position
        //if (closestPosition != transform.position)
        //{
        //    desiredPos = closestPosition;
        //    Percentage = 0;
        //    return;
        //}

        //If absolutely nothing can be done, skip the turn
        //enemy.MovementRemaining = 0;
        //moving = false;
    }

    /// <summary>
    /// Checks the direction of enemy movement
    /// </summary>
    /// <param name="origin">Origin of the movement</param>
    /// <param name="direction">Direction of the movement</param>
    /// <returns>List of all possible infinite directions</returns>
    private List<Vector3> CheckDirection(Vector3 origin, Vector2 direction)
    {
        List<Vector3> list = new();
        Vector3 currentPos = origin;
        bool stillRunning = true;

        //I don't need this but I'm stupid so I'm keeping it in
        if (direction == Vector2.zero)
        {
            Debug.Log("You have a movement direction set to 0, 0 somewhere. Fix it");
            stillRunning = false;
        }

        while (stillRunning)
        {
            currentPos += new Vector3(direction.x, 0, direction.y);

            //Check for floor
            if (!Physics.CheckBox(currentPos + Vector3.down, Vector3.one * 0.45f, Quaternion.identity, groundLayers))
            {
                if (!enemy.CanJumpHoles)
                {
                    stillRunning = false;
                }
                continue;
            }

            //Next, we check for obstacles
            if (Physics.CheckBox(currentPos, Vector3.one * 0.45f, Quaternion.identity, obstacleLayers))
            {
                if (!enemy.CanJumpWalls)
                {
                    stillRunning = false;
                }
                continue;
            }

            //Check if the position is still inside the board
            if(currentPos.x < -3.5f || currentPos.x > 3.5f || currentPos.z < 0f || currentPos.z > 7f)
            {
                stillRunning = false;
            }

            list.Add(currentPos);
        }

        return list;
    }
}