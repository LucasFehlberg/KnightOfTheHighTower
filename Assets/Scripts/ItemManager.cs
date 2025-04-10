/*****************************************************************************
// File Name : ItemManager.cs
// Author : Lucas Fehlberg
// Creation Date : April 4, 2025
// Last Updated : April 5, 2025
//
// Brief Description : Manages items in a run inside of a gameObject
                       Also handles modifiers because they run very similarly
*****************************************************************************/

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

            return;
        }

        Destroy(gameObject);
    }
}
