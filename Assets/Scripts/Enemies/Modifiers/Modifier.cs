/*****************************************************************************
// File Name : Modifier.cs
// Author : Lucas Fehlberg
// Creation Date : March 30, 2025
// Last Updated : April 17, 2025
//
// Brief Description : Base class for all enemy modifiers
*****************************************************************************/

using System.Collections.Generic;
using System;
using System.Linq;

public class Modifier
{
    private static List<Modifier> allModifiers = new();

    protected int modifierDifficulty;

    /// <summary>
    /// 0 is a starter one, -1 is a player infliction
    /// </summary>
    protected string modifierName;

    protected string modifierDescription;

    private static Dictionary<int, List<Modifier>> modifiersByDifficulty = new();

    protected EnemyBase enemy;

    /// <summary>
    /// Constructor
    /// </summary>
    public Modifier()
    {
        SetDefaults();
    }

    /// <summary>
    /// Sets name and description and stuff
    /// </summary>
    public virtual void SetDefaults()
    {

    }

    public EnemyBase Enemy { get => enemy; set => enemy = value; }
    public static List<Modifier> AllModifiers { get => allModifiers; set => allModifiers = value; }
    public int ModifierDifficulty { get => modifierDifficulty; set => modifierDifficulty = value; }
    public string ModifierName { get => modifierName; set => modifierName = value; }
    public string ModifierDescription { get => modifierDescription; set => modifierDescription = value; }

    /// <summary>
    /// When the enemy starts their turn
    /// </summary>
    public virtual void OnStartTurn()
    {

    }
    
    /// <summary>
    /// Runs after start turn
    /// </summary>
    public virtual void OnLateStartTurn()
    {

    }

    /// <summary>
    /// Runs when it is the player's turn
    /// </summary>
    public virtual void OnPlayerStartTurn(PlayerBase player)
    {

    }


    /// <summary>
    /// Runs when the turn is over
    /// </summary>
    public virtual void OnEndTurn()
    {

    }

    /// <summary>
    /// Runs when the enemy dies
    /// </summary>

    public virtual void OnKill()
    {

    }

    /// <summary>
    /// Clones the current modifier
    /// </summary>
    /// <returns>Cloned modifier</returns>
    public Modifier Clone()
    {
        return (Modifier)MemberwiseClone();
    }

    /// <summary>
    /// Loads all modifier classes
    /// </summary>
    public static void LoadAllModifierInformation()
    {
        foreach (Modifier modifier in GetAllModifiers())
        {
            allModifiers.Add(modifier);

            if (!modifiersByDifficulty.ContainsKey(modifier.modifierDifficulty))
            {
                modifiersByDifficulty[modifier.modifierDifficulty] = new List<Modifier>();
            }

            modifiersByDifficulty[modifier.modifierDifficulty].Add(modifier);
        }
    }

    /// <summary>
    /// Creates an instance of every item in the game
    /// </summary>
    /// <returns></returns>
    private static IEnumerable<Modifier> GetAllModifiers()
    {
        return AppDomain.CurrentDomain.GetAssemblies().SelectMany(assembly => assembly.GetTypes())
            .Where(type => type.IsSubclassOf(typeof(Modifier)))
            .Select(type => Activator.CreateInstance(type) as Modifier);
    }
}
