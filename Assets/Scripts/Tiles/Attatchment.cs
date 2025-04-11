/*****************************************************************************
// File Name : Attatchment.cs
// Author : Lucas Fehlberg
// Creation Date : April 10, 2025
// Last Updated : April 10, 2025
//
// Brief Description : Class for attatchment information
*****************************************************************************/

using System.Collections.Generic;
using UnityEngine;
using System.IO;

[CreateAssetMenu(fileName = "Attatchment", menuName = "Attatchment Info")]
public class Attatchment : ScriptableObject
{
    private static Dictionary<char, string> attatchmentValues = new();
    private static Dictionary<string, string> attatchmentDescriptions = new();
    private static Dictionary<string, Sprite> attatchmentSprites = new();

    [SerializeField] protected string attatchmentDescription;
    [SerializeField] protected char attatchmentValue;
    [SerializeField] protected string attatchmentName;
    [SerializeField] private Sprite attatchmentSprite;

    public static Dictionary<char, string> AttatchmentValues 
    { get => attatchmentValues; set => attatchmentValues = value; }
    public static Dictionary<string, string> AttatchmentDescriptions 
    { get => attatchmentDescriptions; set => attatchmentDescriptions = value; }
    public static Dictionary<string, Sprite> AttatchmentSprites 
    { get => attatchmentSprites; set => attatchmentSprites = value; }

    /// <summary>
    /// Loads all attatchment classes
    /// </summary>
    public static void LoadAllAttatchmentInformation()
    {
        LoadAllTiles();
    }

    /// <summary>
    /// loads all tiles in the game
    /// </summary>
    private static void LoadAllTiles()
    {
        string path = "Assets/Scripts/Tiles/Resources/TileInfo";
        //Search folder
        string[] files = Directory.GetFiles(path, "*.asset", SearchOption.TopDirectoryOnly);

        foreach (string file in files)
        {
            string fileName = Path.GetFileNameWithoutExtension(file);

            Attatchment attatchment = Resources.Load<Attatchment>("TileInfo/" + fileName);
            attatchmentValues.Add(attatchment.attatchmentValue, attatchment.attatchmentName);
            attatchmentDescriptions.Add(attatchment.attatchmentName, attatchment.attatchmentDescription);
            attatchmentSprites.Add(attatchment.attatchmentName, attatchment.attatchmentSprite);
        }
    }
}
