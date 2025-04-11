/*****************************************************************************
// File Name : EnemyMovement.cs
// Author : Lucas Fehlberg
// Creation Date : March 30, 2025
// Last Updated : April 10, 2025
//
// Brief Description : Base class for enemy movement
*****************************************************************************/

using System.Collections.Generic;
using UnityEngine;

public class EnemyMovement : MonoBehaviour
{
    protected bool tileTrigger = false;

    protected EnemyBase enemy;
    protected bool moving = false;

    protected Vector3 desiredPos;
    protected Vector3 originalPos;
    private float percentage = 1f;

    protected Vector3 desiredPosition;

    protected List<GameObject> triggeredTiles = new();

    [SerializeField] protected LayerMask groundLayers;

    public float Percentage { get => percentage; set => percentage = value; }
    public bool Moving { get => moving; set => moving = value; }
    public bool TileTrigger { get => tileTrigger; set => tileTrigger = value; }
    public List<GameObject> TriggeredTiles { get => triggeredTiles; set => triggeredTiles = value; }
    public Vector3 DesiredPos { get => desiredPos; set => desiredPos = value; }
    public Vector3 OriginalPos { get => originalPos; set => originalPos = value; }


    /// <summary>
    /// Get the enemy base
    /// </summary>
    private void Awake()
    {
        enemy = GetComponent<EnemyBase>();
    }

    /// <summary>
    /// Move the enemy
    /// </summary>
    public virtual void MoveEnemy()
    {
        if (moving)
        {
            return;
        }
    }

    /// <summary>
    /// Does the animation stuff
    /// </summary>
    private void FixedUpdate()
    {
        if (Percentage < 1f)
        {
            Percentage += 0.05f;
            if (Percentage >= 1f)
            {
                Percentage = 1f;
                moving = false;
            }

            //Very specific curve fitting here
            float yValue = (-4.5f * (800f / 324f) * (Percentage - 0.1f) * (Percentage - 1f)) + 0.75f;
            if (yValue < 0.75f)
            {
                yValue = 0.75f;
            }

            Vector3 newPos = Vector3.Lerp(originalPos, desiredPos, Percentage);
            transform.position = new(newPos.x, yValue, newPos.z);

            if(Percentage == 1f && !tileTrigger)
            {
                TriggeredTiles.Clear();

                //If there is no ground, DON'T ignore
                if (!Physics.CheckBox(transform.position + Vector3.down, Vector3.one * 0.45f, Quaternion.identity,
                    groundLayers))
                {
                    enemy.KillEnemyFunny();
                }
            }
        }
    }

    /// <summary>
    /// Uses A* to pathfind
    /// </summary>
    /// <param name="start">Starting Node</param>
    /// <param name="end">End node</param>
    /// <param name="possibleMovements">The movements this piece can make</param>
    /// <param name="possibleInfMovements">The movements this piece can make</param>
    /// <param name="possibleInfMovements">The movements this piece can make</param>
    /// <param name="canJumpHoles">If this piece can jump holes</param>
    /// <param name="canJumpWalls">If this piece can jump walls</param>
    /// <returns>List of nodes</returns>
    public List<Node> GeneratePath(Node start, Node end, List<Vector2> possibleMovements, 
        List<Vector2> possibleInfMovements, bool canJumpHoles, bool canJumpWalls)
    {
        //Start with an openSet
        List<Node> openSet = new();
        //Making a closedSet for if a path cannot be found
        List<Node> closedSet = new();

        //Set all nodes to max cost
        foreach (Node n in FindObjectsOfType<Node>())
        {
            n.GCost = float.MaxValue;
        }

        //Set the starting node's GCost and HCost
        start.GCost = 0;
        start.HCost = Vector3.Distance(start.transform.position, end.transform.position);

        openSet.Add(start);

        //Run only while the openset has more elements than 0
        while (openSet.Count > 0)
        {
            //Find a node based on FCost
            int lowestF = 0;

            for (int i = 1; i < openSet.Count; i++)
            {
                if (openSet[i].FCost < openSet[lowestF].FCost)
                {
                    lowestF = i;
                }
            }

            //Set the currently evaluated node to the node with the lowest FCost
            Node currentNode = openSet[lowestF];
            openSet.Remove(currentNode);
            closedSet.Add(currentNode);

            //Found end node, get the path taken and reverse it
            if(currentNode == end)
            {
                List<Node> path = new()
                {
                    end
                };

                //Reverse the path by getting the nodes we just came from
                while(currentNode != start)
                {
                    currentNode = currentNode.CameFrom;
                    path.Add(currentNode);
                }

                path.Reverse();

                return path;
            }

            currentNode.SetupConnections(possibleMovements, possibleInfMovements, canJumpHoles, canJumpWalls);

            foreach(Node connectedNode in currentNode.NodeConnections)
            {
                float heldGCost = currentNode.GCost + Vector3.Distance(currentNode.transform.position,
                    connectedNode.transform.position);

                if(heldGCost < connectedNode.GCost)
                {
                    connectedNode.CameFrom = currentNode;
                    connectedNode.GCost = heldGCost;
                    connectedNode.HCost = Vector3.Distance(connectedNode.transform.position, end.transform.position);

                    if (!openSet.Contains(connectedNode))
                    {
                        openSet.Add(connectedNode);
                    }
                }
            }
        }

        Node closestNode = start;

        foreach(Node node in closedSet)
        {
            if (node.HCost < closestNode.HCost)
            {
                closestNode = node;
            }
        }

        //Give up if the closest node is the node we're currently sitting on
        if(closestNode == start)
        {
            return null;
        }

        List<Node> closestPath = new()
        {
            closestNode
        };

        //Reverse the path by getting the nodes we just came from
        while (closestNode != start)
        {
            closestNode = closestNode.CameFrom;
            closestPath.Add(closestNode);
        }

        closestPath.Reverse();

        return closestPath;
    }
}
