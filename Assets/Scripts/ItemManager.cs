/*****************************************************************************
// File Name : ItemManager.cs
// Author : Lucas Fehlberg
// Creation Date : April 4, 2025
// Last Updated : April 10, 2025
//
// Brief Description : Manages items in a run inside of a gameObject
//                     Also handles modifiers because they run very similarly
//                     Pretty much loads everything that needs to be in the game into the game
*****************************************************************************/

using System.IO;
using UnityEngine;

public class ItemManager : MonoBehaviour
{
    public ItemManager Instance;

    /// <summary>
    /// Loads all items in the game
    /// </summary>
    private void Start()
    {
        if(Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            Item.LoadAllItemInformation();
            Modifier.LoadAllModifierInformation();
            LoadAllTiles();

            return;
        }

        Destroy(gameObject);
    }

    /// <summary>
    /// 
    /// </summary>
    private void LoadAllTiles()
    {
        string path = "Assets/Prefabs/Resources/TileAttatchments";
        //Searches the directory
        string[] files = Directory.GetFiles(path, "*.prefab", SearchOption.TopDirectoryOnly);

        foreach (string file in files)
        {
            Tile.Attatchments.Add(Path.GetFileNameWithoutExtension(file));
        }
    }
}
