/*****************************************************************************
// File Name : Node.cs
// Author : Lucas Fehlberg
// Creation Date : April 6, 2025
// Last Updated : April 6, 2025
//
// Brief Description : Node used for A* pathfinding
*****************************************************************************/

using System.Collections.Generic;
using UnityEngine;

public class Node : MonoBehaviour
{
    private float gCost;
    private float hCost;

    [SerializeField] private LayerMask nodeLayer;
    [SerializeField] private LayerMask groundLayers;
    [SerializeField] private LayerMask obstacleLayers;

    private List<Node> nodeConnections = new();
    private Node cameFrom;

    public float FCost {  get { return gCost + hCost; } }

    public float GCost { get => gCost; set => gCost = value; }
    public float HCost { get => hCost; set => hCost = value; }
    public List<Node> NodeConnections { get => nodeConnections; set => nodeConnections = value; }
    public Node CameFrom { get => cameFrom; set => cameFrom = value; }

    /// <summary>
    /// Sets up connections to all neighbors
    /// </summary>
    /// <param name="movements"></param>
    /// <param name="infMovements"></param>
    /// <param name="canJumpHoles"></param>
    /// <param name="canJumpWalls"></param>
    public void SetupConnections(List<Vector2> movements, List<Vector2> infMovements, bool canJumpHoles, 
        bool canJumpWalls)
    {
        nodeConnections.Clear();

        foreach (Vector2 movement in infMovements)
        {
            nodeConnections.AddRange(CheckDirection(transform.position, movement, canJumpHoles, canJumpWalls));
        }

        foreach (Vector2 movement in movements)
        {
            Vector3 testPos = transform.position + new Vector3(movement.x, 0, movement.y);

            //Next, we check for obstacles
            if (Physics.CheckBox(testPos, Vector3.one * 0.45f, Quaternion.identity, obstacleLayers))
            {
                continue;
            }

            if (Physics.CheckBox(testPos, Vector3.one * 0.45f, Quaternion.identity, nodeLayer))
            {
                Node node = Physics.OverlapBox(testPos, Vector3.one * 0.5f, Quaternion.identity, nodeLayer)[0]
                    .GetComponent<Node>();

                if (!nodeConnections.Contains(node))
                {
                    nodeConnections.Add(node);
                }
            }
        }
    }

    /// <summary>
    /// Checks the direction of enemy movement
    /// </summary>
    /// <param name="origin">Origin of the movement</param>
    /// <param name="direction">Direction of the movement</param>
    /// <returns>List of all possible infinite directions</returns>
    private List<Node> CheckDirection(Vector3 origin, Vector2 direction, bool canJumpHoles, bool canJumpWalls)
    {
        List<Node> list = new();
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
                if (!canJumpHoles)
                {
                    stillRunning = false;
                }
                continue;
            }

            //Next, we check for obstacles
            if (Physics.CheckBox(currentPos, Vector3.one * 0.45f, Quaternion.identity, obstacleLayers))
            {
                if (!canJumpWalls)
                {
                    stillRunning = false;
                }
                continue;
            }

            //Check if the position is still inside the board
            if (currentPos.x < -3.5f || currentPos.x > 3.5f || currentPos.z < 0f || currentPos.z > 7f)
            {
                stillRunning = false;
            }

            Node node = Physics.OverlapBox(currentPos, Vector3.one * 0.5f, Quaternion.identity, nodeLayer)[0]
                .GetComponent<Node>();


            if (!nodeConnections.Contains(node))
            {
                list.Add(node);
            }
        }

        return list;
    }
}
