/*****************************************************************************
// File Name : Stats.cs
// Author : Lucas Fehlberg
// Creation Date : March 29, 2025
// Last Updated : April 8, 2025
//
// Brief Description : Statistic data
*****************************************************************************/

using System.Collections.Generic;
using UnityEngine;

public static class Stats
{
    //Main Stats
    private static int movement = 1;
    private static int manipulation = 1;
    private static int attack = 1;
    private static int health = 3;

    //Stats related to the main stats but not directly them
    private static int damage = 1;
   private static int terrainRange = 2; //Consider if it's too OP
                                        //It's too OP

    //Misc. Stats
    private static int rerolls = 0;
    private static int randMinAlter = 0;  //Luck
    private static int randMaxAlter = 0; //Randomness

    //List Stats
    private static List<Vector2> possibleMovements = new();
    private static List<Vector2> additionalMovements = new();
    private static List<Vector2> possibleAttacks = new();
    private static List<Vector2> additionalAttacks = new();

    private static List<Item> heldItems = new();
    private static List<Modifier> currentModifiers = new();

    private static List<string> heldTiles = new() { null, null, null };

    private static string currentlyHeldTile = null;



    /// <summary>
    /// The amount of movement the player can do on a given turn
    /// </summary>
    public static int Movement { get => movement; set => movement = value; }

    /// <summary>
    /// The amount of terrain manipulation (adding or removing) the player can do on a turn
    /// </summary>
    public static int Manipulation { get => manipulation; set => manipulation = value; }

    /// <summary>
    /// The movements the player is able to make
    /// </summary>
    public static List<Vector2> PossibleMovements { get => possibleMovements; set => possibleMovements = value; }
    /// <summary>
    /// The possible attacks a player can perform
    /// </summary>
    public static List<Vector2> PossibleAttacks { get => possibleAttacks; set => possibleAttacks = value; }
    /// <summary>
    /// The currently held items by the player
    /// </summary>
    public static List<Item> HeldItems { get => heldItems; set => heldItems = value; }
    /// <summary>
    /// The amount of times a player can attack in a given turn
    /// </summary>
    public static int Attack { get => attack; set => attack = value; }
    /// <summary>
    /// Max health
    /// </summary>
    public static int Health { get => health; set => health = value; }
    /// <summary>
    /// Amount of damage dealt in a turn
    /// </summary>
    public static int Damage { get => damage; set => damage = value; }

    /// <summary>
    /// Range of which the player can manipulate terrain
    /// </summary>
    public static int TerrainRange { get => terrainRange; set => terrainRange = value; }
    /// <summary>
    /// Number of random Rerolls the player has. Rerolls reroll random chances on items for a more favorable event
    /// </summary>
    public static int Rerolls { get => rerolls; set => rerolls = value; }

    /// <summary>
    /// Additions to this stat increase the chance of succeding a random roll. 
    /// Can also be negative for decreased chances
    /// Referred to ingame as "Luck"
    /// </summary>
    public static int RandMinAlter { get => randMinAlter; set => randMinAlter = value; }
    /// <summary>
    /// Additions to this stat decrease the chance of succeeding a random roll.
    /// Can also be negative for increased chances
    /// Referred to ingame as "Randomness"
    /// </summary>
    public static int RandMaxAlter { get => randMaxAlter; set => randMaxAlter = value; }
    /// <summary>
    /// Modifiers that are attatched to all enemies
    /// </summary>
    public static List<Modifier> CurrentModifiers { get => currentModifiers; set => currentModifiers = value; }
    public static List<Vector2> AdditionalMovements { get => additionalMovements; set => additionalMovements = value; }
    public static List<Vector2> AdditionalAttacks { get => additionalAttacks; set => additionalAttacks = value; }
    public static List<string> HeldTiles { get => heldTiles; set => heldTiles = value; }
    public static string CurrentlyHeldTile { get => currentlyHeldTile; set => currentlyHeldTile = value; }

    /// <summary>
    /// Properly resets all stats that need to be reset
    /// </summary>
    public static void ResetStats()
    {
        heldTiles = new()
        {
            null, null, null
        };

        heldItems.Clear();
    }
}
