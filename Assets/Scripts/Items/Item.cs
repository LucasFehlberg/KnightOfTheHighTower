/*****************************************************************************
// File Name : Item.cs
// Author : Lucas Fehlberg
// Creation Date : March 29, 2025
// Last Updated : April 4, 2025
//
// Brief Description : Base class for items
*****************************************************************************/

using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Item
{
    private static List<Item> allItems = new();
    private static Dictionary<int, List<Item>> rarities = new();

    /// <summary>
    /// Creates a new item
    /// </summary>
    public Item()
    {
        SetDefaults();
    }

    protected string itemName;
    protected string itemNameDisplay = string.Empty;
    protected string itemDescription;
    protected int itemRarity;

    protected PlayerBase player;

    public string ItemName { get => itemName; }
    public string ItemDescription { get => itemDescription; }
    public string ItemNameDisplay { get => itemNameDisplay; set => itemNameDisplay = value; }
    public PlayerBase Player { get => player; set => player = value; }

    public Item Clone()
    {
        return (Item)MemberwiseClone();
    }

    /// <summary>
    /// Called when the item is loaded in
    /// </summary>
    public virtual void SetDefaults()
    {

    }

    /// <summary>
    /// When the turn is started
    /// </summary>
    public virtual void OnStartTurn()
    {

    }

    /// <summary>
    /// Updates the stats on a new floor, such as changing movement, or increasing stats
    /// </summary>
    public virtual void UpdateStats()
    {

    }

    /// <summary>
    /// Runs after UpdateStats(). Is meant for stats that get added late (such as evening out two stats after all item
    /// effects are applied). Random events should go here
    /// </summary>
    public virtual void LateUpdateStats()
    {

    }

    /// <summary>
    /// Runs when the player moves. Used for effects when the player moves, such as cosmetics or maybe a stun effect
    /// </summary>
    public virtual void OnMove(Vector3 position)
    {

    }

    /// <summary>
    /// Runs when the player does an attack, useful for splash damage or additional movement
    /// </summary>
    public virtual void OnAttack(EnemyBase enemy, int damage, Vector2 direction)
    {

    }

    /// <summary>
    /// Runs an effect when terrain is manipulated
    /// </summary>
    public virtual void OnTerrainManipulation(Vector3 position, string type)
    {

    }

    /// <summary>
    /// Runs when turn is ended
    /// </summary>
    public virtual void OnEndTurn()
    {

    }

    /// <summary>
    /// Runs when the player takes damage
    /// </summary>
    public virtual void OnTakeDamage(int damageTaken)
    {

    }

    /// <summary>
    /// Runs when the player dies
    /// </summary>
    public virtual void OnDeath()
    {

    }

    public virtual void OnKillEnemy(Vector3 position)
    {

    }

    /// <summary>
    /// Randomly creates an output between true or false. 
    /// </summary>
    /// <param name="chanceNumerator">X in "X our of Y times"</param>
    /// <param name="chanceDenominator">Y in "X out of Y times"</param>
    /// <returns></returns>
    public bool RandomEffect(int chanceNumerator, int chanceDenominator)
    {
        chanceNumerator += Stats.RandMinAlter;
        chanceDenominator += Stats.RandMaxAlter;

        if(chanceNumerator >= chanceDenominator)
        {
            return true;
        }

        List<int> randomList = new();
        for (int i = 0; i < chanceNumerator; i++)
        {
            randomList.Add(1);
        }

        while(randomList.Count < chanceDenominator)
        {
            randomList.Add(0);
        }
        
        for (int i = 0; i < Stats.Rerolls + 1; i++)
        {
            if(randomList[UnityEngine.Random.Range(0, randomList.Count)] == 1)
            {
                return true;
            }
        }

        return false;
    }

    /// <summary>
    /// Loads all item classes
    /// </summary>
    public static void LoadAllItemInformation()
    {
        foreach(Item item in GetAllItems())
        {
            allItems.Add(item);

            if (!rarities.ContainsKey(item.itemRarity))
            {
                rarities[item.itemRarity] = new List<Item>();
            }

            rarities[item.itemRarity].Add(item);
        }
    }
    
    /// <summary>
    /// Returns a list of the starting items
    /// </summary>
    /// <returns>The Starting items</returns>
    public static List<Item> StartingItems()
    {
        List<Item> list = new();

        foreach(Item item in rarities[0])
        {
            list.Add(item.Clone());
        }

        return list;
    }

    /// <summary>
    /// Creates an instance of every item in the game
    /// </summary>
    /// <returns></returns>
    private static IEnumerable<Item> GetAllItems()
    {
        return AppDomain.CurrentDomain.GetAssemblies().SelectMany(assembly => assembly.GetTypes())
            .Where(type => type.IsSubclassOf(typeof(Item))).Select(type => Activator.CreateInstance(type) as Item);
    }

    /// <summary>
    /// Spawns a random item
    /// </summary>
    /// <returns>The random item. Returns null if no valid items are left in the pool</returns>
    public static Item SpawnItem(int baseRarity, bool garunteed)
    {
        if (garunteed)
        {
            return rarities[baseRarity][UnityEngine.Random.Range(0, rarities[baseRarity].Count)].Clone();
        }

        int rng = UnityEngine.Random.Range(0, 100);

        int highestRarity = 0;

        foreach (int rarityTier in rarities.Keys)
        {
            if (rarityTier > highestRarity)
            {
                highestRarity = rarityTier;
            }
        }

        //Split it off into 3 tiers at least, being common, uncommon and rare
        //5%
        if(rng > 94)
        {
            baseRarity += 2;
            if (baseRarity > highestRarity)
            {
                baseRarity = highestRarity;
            }

            return rarities[baseRarity][UnityEngine.Random.Range(0, rarities[baseRarity].Count)].Clone();
        }

        //20%
        if (rng > 74)
        {
            baseRarity += 1;
            if (baseRarity > highestRarity)
            {
                baseRarity = highestRarity;
            }

            return rarities[baseRarity][UnityEngine.Random.Range(0, rarities[baseRarity].Count)].Clone();
        }

        //75%
        return rarities[baseRarity][UnityEngine.Random.Range(0, rarities[baseRarity].Count)].Clone();
    }
}
