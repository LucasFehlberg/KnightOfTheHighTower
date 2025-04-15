/*****************************************************************************
// File Name : PlayerBase.cs
// Author : Lucas Fehlberg
// Creation Date : March 29, 2025
// Last Updated : April 10, 2025
//
// Brief Description : Tile class. Basically, instead of instantiating and deleting everyting we have it all attatched
//                     to the tile prefab. Stupid? Maybe, but allows for better control of what's active and inactive
//                     This is mostly for level design and player control
*****************************************************************************/

using System.Collections.Generic;
using UnityEngine;

public class Tile : MonoBehaviour
{
    [SerializeField] private Material tileMaterial;

    [SerializeField] private GameObject tile;
    [SerializeField] private GameObject wall;

    [SerializeField] private List<GameObject> enemyPrefabs;

    private bool builtUpon = false;
    private bool hasTile = true;
    private GameObject enemyPrefab;

    [SerializeField] private GameObject attatchmentHolder;

    [SerializeField] private GameObject EnemySpanwer;

    //Not entirely helpful yet, but will be in the future. This will be used for dungeon layout stuff
    [SerializeField] private int tileID;

    [SerializeField] private GameObject pathfindingNode;

    public bool BuiltUpon { get => builtUpon; set => builtUpon = value; }
    public bool HasTile { get => hasTile; set => hasTile = value; }
    public int TileID { get => tileID; set => tileID = value; }
    /// <summary>
    /// Handles material setting and eventually level design
    /// </summary>
    private void Start()
    {
        //This might become redundant when I do actual art
        tile.GetComponent<Renderer>().material = tileMaterial;
        wall.GetComponent<Renderer>().material = tileMaterial;
    }

    public bool Build(string building)
    {
        if(building == "Wall")
        {
            if (!tile.activeSelf)
            {
                tile.SetActive(true);
                hasTile = true;
                CheckPathfinding();
                return true;
            }

            if (!wall.activeSelf)
            {
                wall.SetActive(true);
                builtUpon = true;
                CheckPathfinding();
                return true;
            }

            return false;
        }

        if (builtUpon)
        {
            return false;
        }

        AddAttatchment(building);
        builtUpon = true;

        return true;
    }

    /// <summary>
    /// Destroy tiles on top first, then the tile itself
    /// </summary>
    /// <returns></returns>
    public bool Destroy()
    {
        if (wall.activeSelf)
        {
            wall.SetActive(false);
            builtUpon = false;
            CheckPathfinding();
            return true;
        }

        if (builtUpon)
        {
            builtUpon = false;
            Destroy(attatchmentHolder.transform.GetChild(0).gameObject);
            CheckPathfinding();
            return true;
        }

        if (tile.activeSelf)
        {
            tile.SetActive(false);
            hasTile = false;
            CheckPathfinding();
            return true;
        }

        return false;
    }

    /// <summary>
    /// Checks to see if the pathfinding node should be active.
    /// This means the tile doesn't have a wall and the tile itself is active. The wall is the only part of a tile
    /// that can block off other tiles
    /// </summary>
    private void CheckPathfinding()
    {
        if(hasTile && !wall.activeSelf)
        {
            pathfindingNode.SetActive(true);
            return;
        }

        pathfindingNode.SetActive(false);
    }

    /// <summary>
    /// Sets up the board
    /// </summary>
    /// <param name="type">Type of tile</param>
    public void TileCreation(char type)
    {
        if (Attatchment.AttatchmentValues.ContainsKey(type))
        {
            AddAttatchment(Attatchment.AttatchmentValues[type]);
            return;
        }

        switch (type)
        {
            case (' '):
                hasTile = false;
                builtUpon = false;
                tile.SetActive(false);
                break;
            case ('X'):
                hasTile = true;
                builtUpon = true;
                wall.SetActive(true);
                break;
            case ('L'):
                enemyPrefab = enemyPrefabs[0];
                hasTile = true;
                builtUpon = false;
                SpawnEnemy();
                break;
            case ('p'):
                enemyPrefab = enemyPrefabs[1];
                hasTile = true;
                builtUpon = false;
                SpawnEnemy();
                break;
            case ('n'):
                enemyPrefab = enemyPrefabs[2];
                hasTile = true;
                builtUpon = false;
                SpawnEnemy();
                break;
            case ('b'):
                enemyPrefab = enemyPrefabs[3];
                hasTile = true;
                builtUpon = false;
                SpawnEnemy();
                break;
            case ('r'):
                enemyPrefab = enemyPrefabs[4];
                hasTile = true;
                builtUpon = false;
                SpawnEnemy();
                break;
            case ('K'):
                enemyPrefab = enemyPrefabs[5];
                hasTile = true;
                builtUpon = false;
                SpawnEnemy();
                break;
            case ('Q'):
                enemyPrefab = enemyPrefabs[6];
                hasTile = true;
                builtUpon = false;
                SpawnEnemy();
                break;
        }

        CheckPathfinding();
    }

    /// <summary>
    /// Spawns an enemy. Uses enemyPrefab to do such
    /// </summary>
    private void SpawnEnemy()
    {
        GameObject enemy = Instantiate(enemyPrefab);
        enemy.transform.position = EnemySpanwer.transform.position;
    }

    /// <summary>
    /// Adds an attatchment to the tile
    /// </summary>
    private void AddAttatchment(string attatchment)
    {
        Instantiate(Resources.Load("TileAttatchments/" + attatchment), attatchmentHolder.transform);
        CheckPathfinding();
    }
}
