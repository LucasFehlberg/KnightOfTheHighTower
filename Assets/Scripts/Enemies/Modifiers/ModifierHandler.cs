/*****************************************************************************
// File Name : ModifierHandler.cs
// Author : Lucas Fehlberg
// Creation Date : April 1, 2025
// Last Updated : April 13, 2025
//
// Brief Description : Handles Modifiers
*****************************************************************************/

using System.Collections.Generic;
using UnityEngine;

public class ModifierHandler : MonoBehaviour
{
    [SerializeField] private List<string> modifiers = new();
    [SerializeField] private EnemyBase enemy;
    
    /// <summary>
    /// Sets starting modifiers
    /// </summary>
    private void Start()
    {
        foreach (Modifier modifier in Modifier.AllModifiers)
        {
            if (modifiers.Contains(modifier.ModifierName))
            {
                enemy.Modifiers.Add(modifier.Clone());
            }
        }

        foreach(Modifier modifier in Stats.CurrentModifiers)
        {
            enemy.Modifiers.Add(modifier.Clone());
        }
    }
}
