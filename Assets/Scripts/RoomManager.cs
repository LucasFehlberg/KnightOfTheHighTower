/*****************************************************************************
// File Name : RoomManager.cs
// Author : Lucas Fehlberg
// Creation Date : April 3, 2025
// Last Updated : April 27, 2025
//
// Brief Description : Handles rooms
*****************************************************************************/

using System.Collections.Generic;
using UnityEngine;

public class RoomManager : MonoBehaviour
{
    public static RoomManager Instance;

    private static int floor = 0;
    private List<char> floorLayout;
    private Dictionary<int, List<List<char>>> possibleFloors = new();
    private Dictionary<string, List<char>> specialFloors = new();

    [SerializeField] private List<string> rewardTypes = new();

    [SerializeField] private string reward;

    private List<List<char>> exploredRooms = new();

    public static int Floor { get => floor; set => floor = value; }
    public List<char> FloorLayout { get => floorLayout; set => floorLayout = value; }
    public string Reward { get => reward; set => reward = value; }
    public List<List<char>> ExploredRooms { get => exploredRooms; set => exploredRooms = value; }

    /// <summary>
    /// Set up the singleton
    /// </summary>
    private void Start()
    {
        if(Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            Init();
            return;
        }

        Destroy(gameObject);
    }

    /// <summary>
    /// Initializes all possible room-layouts in the game
    /// Doesn't have prespawned enemies EXCEPT for in specific cases
    /// </summary>
    private void Init()
    {
        //Key:
        // Tiles
        // . - Normal Tile
        //   - Empty Tile
        // X - Walled Tile
        // V - Vector Plate
        // H - Hot Plate
        // S - Sentry Tower
        // 0 - Null Plate

        // Enemies
        // E - Generic Enemy Tile
        // n - Knight
        // r - Rook
        // p - Pawn
        // b - Bishop

        // Bosses
        // K - King Tile
        // Q - Queen Tile
        // N - Nightrider Tile
        // A - Amazon Tile
        // L - Lock

        //Something to note. Player always spawns on the bottom row, right 4

        //We'll initialize floor 1 first. This one is always the same. See below
        List<char> floor = new()
        {
            ' ', ' ', '.', '.', '.', '.', '.', '.',
            ' ', ' ', '.', '.', '.', '.', '.', '.',
            ' ', ' ', '.', '.', '.', 'L', '.', '.',
            ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ',
            ' ', ' ', '.', '.', ' ', ' ', ' ', ' ',
            ' ', 'X', 'X', 'X', 'X', ' ', ' ', ' ',
            ' ', 'X', '.', '.', 'X', ' ', ' ', ' ',
            ' ', 'X', '.', '.', 'X', ' ', ' ', ' '
            //              ^ Player Spawn
        };

        //Add this floor
        possibleFloors.Add(0, new List<List<char>> { floor });

        //Floors are semi-random per level. Game gets harder ideally with more complex pieces being added. Floor 1 adds
        //Pawns and Knights

        List<List<char>> floors = new();

        floor = new()
        {
            '.', 'p', 'p', '.', '.', 'p', 'p', '.',
            '.', '.', '.', '.', '.', '.', '.', '.',
            '.', '.', '.', '.', '.', '.', '.', '.',
            '.', '.', '.', '.', '.', '.', '.', '.',
            '.', '.', ' ', '.', '.', ' ', '.', '.',
            '.', '.', ' ', '.', '.', ' ', '.', '.',
            '.', 'n', ' ', '.', '.', ' ', 'n', '.',
            '.', '.', ' ', '.', '.', ' ', '.', '.'
        };

        floors.Add(floor);

        floor = new()
        {
            '.', 'n', '.', 'p', 'p', '.', 'n', '.',
            'p', '.', '.', '.', '.', '.', '.', 'p',
            '.', '.', '.', '.', '.', '.', '.', '.',
            ' ', ' ', '.', ' ', ' ', '.', '.', '.',
            '.', '.', '.', '.', ' ', '.', '.', '.',
            '.', '.', '.', '.', '.', '.', '.', '.',
            '.', '.', '.', '.', ' ', '.', '.', '.',
            '.', '.', '.', '.', ' ', '.', '.', '.'
        };

        floors.Add(floor);

        floor = new()
        {
            ' ', ' ', 'n', '.', '.', 'n', ' ', ' ',
            ' ', 'p', '.', '.', '.', '.', 'p', ' ',
            'p', '.', '.', ' ', ' ', '.', '.', 'p',
            '.', '.', ' ', 'X', 'X', ' ', '.', '.',
            '.', '.', ' ', 'X', 'X', ' ', '.', '.',
            '.', '.', '.', ' ', ' ', '.', '.', '.',
            ' ', '.', '.', '.', '.', '.', '.', ' ',
            ' ', ' ', '.', '.', '.', '.', ' ', ' '
        };

        floors.Add(floor);

        possibleFloors.Add(1, floors);

        //Start floor 2. Add 2 rooms and reuse the previous rooms as well
        //In the new rooms, we'll reinforce knights and pawns for now
        //These rooms will be slightly more complicated as well
        //We'll introduce bishops in floor 3
        floors = new();

        floor = new()
        {
            '.', 'n', '.', 'p', 'p', '.', 'n', '.',
            'n', 'X', 'X', '.', '.', 'X', 'X', 'n',
            '.', 'X', '.', '.', '.', '.', 'X', '.',
            '.', ' ', '.', '.', '.', '.', ' ', '.',
            '.', ' ', '.', '.', '.', '.', ' ', '.',
            '.', 'X', '.', '.', '.', '.', 'X', '.',
            '.', 'X', 'X', '.', '.', 'X', 'X', '.',
            '.', '.', '.', '.', '.', '.', '.', '.'
        };

        floors.Add(floor);

        //Having a pawn floor would be funny
        floor = new()
        {
            ' ', '.', '.', '.', '.', '.', '.', ' ',
            '.', 'p', 'p', '.', '.', 'p', 'p', '.',
            '.', 'p', 'p', '.', '.', 'p', 'p', '.',
            '.', '.', '.', 'X', 'X', '.', '.', '.',
            '.', '.', '.', 'X', 'X', '.', '.', '.',
            '.', '.', '.', '.', '.', '.', '.', '.',
            '.', '.', '.', '.', '.', '.', '.', '.',
            ' ', '.', '.', '.', '.', '.', '.', ' '
        };

        floors.Add(floor);

        possibleFloors.Add(2, floors);

        //Floor 3. Adds bishops

        floors = new();

        floor = new()
        {
            ' ', '.', '.', '.', '.', '.', '.', ' ',
            ' ', '.', '.', 'b', 'b', '.', '.', ' ',
            ' ', '.', '.', '.', '.', '.', '.', ' ',
            ' ', '.', 'X', '.', '.', 'X', '.', ' ',
            ' ', '.', 'X', '.', '.', 'X', '.', ' ',
            ' ', '.', '.', '.', '.', '.', '.', ' ',
            ' ', '.', '.', '.', '.', '.', '.', ' ',
            ' ', '.', '.', '.', '.', '.', '.', ' '
        };

        floors.Add(floor);

        floor = new()
        {
            '.', '.', '.', '.', '.', '.', '.', '.',
            '.', '.', '.', '.', '.', '.', '.', '.',
            '.', 'b', '.', '.', '.', '.', 'b', '.',
            '.', '.', ' ', '.', '.', ' ', '.', '.',
            '.', ' ', '.', '.', '.', '.', ' ', '.',
            '.', ' ', '.', '.', '.', '.', ' ', '.',
            '.', '.', '.', '.', '.', '.', '.', '.',
            '.', ' ', '.', '.', '.', '.', ' ', '.'
        };

        floors.Add(floor);

        possibleFloors.Add(3, floors);

        //Floor 4. Mix all previous enemies

        floors = new();

        floor = new()
        {
            ' ', 'b', 'n', '.', '.', 'n', 'b', ' ',
            '.', 'p', 'p', '.', '.', 'p', 'p', '.',
            '.', '.', '.', '.', '.', '.', '.', '.',
            '.', '.', '.', '.', '.', '.', '.', '.',
            '.', '.', '.', 'X', 'X', '.', '.', '.',
            '.', '.', '.', '.', '.', '.', '.', '.',
            '.', '.', '.', '.', '.', '.', '.', '.',
            ' ', '.', '.', '.', '.', '.', '.', ' '
        };

        floors.Add(floor);

        floor = new()
        {
            '.', 'n', '.', 'p', 'p', '.', 'n', '.',
            '.', '.', 'b', '.', '.', 'b', '.', '.',
            '.', '.', '.', '.', '.', '.', '.', '.',
            ' ', ' ', '.', '.', '.', '.', ' ', ' ',
            'X', 'X', '.', '.', '.', '.', 'X', 'X',
            '.', '.', '.', '.', '.', '.', '.', '.',
            '.', '.', '.', '.', '.', '.', '.', '.',
            '.', 'p', '.', '.', '.', '.', 'p', '.'
        };

        floors.Add(floor);

        floor = new()
        {
            'b', '.', '.', '.', '.', '.', '.', 'b',
            ' ', '.', '.', '.', '.', '.', '.', ' ',
            ' ', '.', '.', '.', '.', '.', '.', ' ',
            '.', 'p', 'p', ' ', ' ', 'p', 'p', '.',
            '.', '.', '.', ' ', ' ', '.', '.', '.',
            'n', '.', '.', '.', '.', '.', '.', 'n',
            '.', 'X', '.', '.', '.', '.', 'X', '.',
            '.', '.', '.', '.', '.', '.', '.', '.'
        };

        floors.Add(floor);

        possibleFloors.Add(4, floors);

        //Floor 5, add rooks

        floors = new();

        floor = new()
        {
            'r', 'X', '.', '.', '.', '.', 'X', 'r',
            '.', '.', '.', '.', '.', '.', '.', '.',
            '.', '.', '.', '.', '.', '.', '.', '.',
            '.', '.', 'X', '.', '.', 'X', '.', '.',
            '.', '.', 'X', '.', '.', 'X', '.', '.',
            '.', '.', '.', '.', '.', '.', '.', '.',
            '.', '.', '.', '.', '.', '.', '.', '.',
            'X', '.', '.', '.', '.', '.', '.', 'X'
        };

        floors.Add(floor);

        floor = new()
        {
            '.', 'r', '.', '.', '.', '.', 'r', '.',
            '.', 'X', '.', '.', '.', '.', 'X', '.',
            '.', '.', '.', '.', '.', '.', '.', '.',
            '.', '.', '.', 'X', 'X', '.', '.', '.',
            '.', '.', '.', '.', '.', '.', '.', '.',
            '.', '.', '.', '.', '.', '.', '.', '.',
            '.', '.', '.', '.', '.', '.', '.', '.',
            '.', 'X', '.', '.', '.', '.', 'X', '.'
        };

        floors.Add(floor);

        possibleFloors.Add(5, floors);

        //Floor 6, put it all together

        floors = new();

        floor = new()
        {
            '.', '.', '.', 'r', 'b', 'b', '.', '.',
            '.', '.', '.', '.', '.', '.', '.', '.',
            '.', '.', 'X', 'p', 'p', 'X', '.', '.',
            'X', '.', 'p', '.', '.', 'p', '.', 'X',
            '.', '.', '.', '.', '.', '.', '.', '.',
            '.', '.', '.', '.', '.', '.', '.', '.',
            ' ', '.', '.', '.', '.', '.', '.', ' ',
            ' ', '.', '.', '.', '.', '.', '.', ' '
        };

        floors.Add(floor);

        floor = new()
        {
            '.', '.', 'b', 'X', 'X', 'r', '.', '.',
            '.', '.', '.', '.', '.', '.', '.', '.',
            '.', '.', '.', '.', '.', '.', '.', '.',
            '.', '.', 'n', '.', '.', 'n', '.', '.',
            '.', 'X', 'X', '.', '.', 'X', 'X', '.',
            '.', 'p', '.', '.', '.', '.', 'p', '.',
            '.', '.', '.', '.', '.', '.', '.', '.',
            '.', '.', '.', '.', '.', '.', '.', '.'
        };

        floors.Add(floor);

        floor = new()
        {
            '.', 'r', '.', 'X', 'X', '.', 'r', '.',
            'X', 'n', '.', '.', '.', '.', 'n', 'X',
            'X', 'n', '.', '.', '.', '.', 'n', 'X',
            '.', '.', '.', '.', '.', '.', '.', '.',
            '.', '.', '.', '.', '.', '.', '.', '.',
            'X', '.', '.', '.', '.', '.', '.', 'X',
            'X', '.', '.', '.', '.', '.', '.', 'X',
            '.', '.', 'X', '.', '.', 'X', '.', '.'
        };

        floors.Add(floor);

        possibleFloors.Add(6, floors);

        //Rewards
        rewardTypes.Add("Starter");
        //rewardTypes.Add("Shop");
        rewardTypes.Add("Normal Selection");
        rewardTypes.Add("Terrain Selection");


        //Special Floors
        //King's Court
        floor = new()
        {
            'r', 'n', 'b', ' ', ' ', 'b', 'n', 'r',
            'p', 'p', 'p', 'p', 'p', 'p', 'p', 'p',
            '.', '.', '.', '.', '.', '.', '.', '.',
            '.', '.', '.', '.', '.', '.', '.', '.',
            '.', '.', '.', '.', '.', '.', '.', '.',
            '.', '.', '.', '.', '.', '.', '.', '.',
            '.', '.', '.', '.', '.', '.', '.', '.',
            '.', '.', '.', '.', '.', '.', '.', '.'
        };

        specialFloors.Add("KingsCourt", floor);

        floor = new()
        {
            '.', '.', '.', 'K', 'Q', '.', '.', '.',
            '.', '.', '.', '.', '.', '.', '.', '.',
            '.', '.', '.', '.', '.', '.', '.', '.',
            '.', '.', '.', '.', '.', '.', '.', '.',
            '.', '.', '.', '.', '.', '.', '.', '.',
            '.', '.', '.', '.', '.', '.', '.', '.',
            '.', '.', '.', '.', '.', '.', '.', '.',
            '.', '.', '.', '.', '.', '.', '.', '.'
        };

        specialFloors.Add("ThroneRoom", floor);
    }

    /// <summary>
    /// Generates the current floor
    /// </summary>
    public void GenerateFloor()
    {
        if(Floor == 0)
        {
            FloorLayout = possibleFloors[0][0];
            reward = rewardTypes[0];
        }

        if (Floor == 9)
        {
            FloorLayout = specialFloors["ThroneRoom"];
            reward = "Win";
        }

        exploredRooms.Add(FloorLayout);

        foreach (GameObject tile in GameObject.FindGameObjectsWithTag("Tile"))
        {
            Tile tileScript = tile.GetComponent<Tile>();

            tileScript.TileCreation(FloorLayout[tileScript.TileID]);
        }
    }

    public List<char> SelectFloor(out string reward)
    {
        reward = rewardTypes[Random.Range(1, rewardTypes.Count)];

        if(floor == 8)
        {
            return specialFloors["KingsCourt"];
        }

        if (floor == 9)
        {
            return specialFloors["ThroneRoom"];
        }

        List<List<char>> possibilities = new();

        for (int i = 0; i < 3; i++)
        {
            if(floor - i == 0)
            {
                break;
            }

            if(!possibleFloors.ContainsKey(floor - i))
            {
                continue;
            }

            foreach (List<char> room in possibleFloors[floor - i]) 
            {
                if (exploredRooms.Contains(room))
                {
                    continue;
                }

                possibilities.Add(room);
            }
        }

        return possibilities[Random.Range(0, possibilities.Count)];
    }
}
