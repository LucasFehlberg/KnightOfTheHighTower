/*****************************************************************************
// File Name : ModifierHandler.cs
// Author : Lucas Fehlberg
// Creation Date : April 1, 2025
// Last Updated : April 1, 2025
//
// Brief Description : Handles Modifiers
*****************************************************************************/

using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ModifierHandler : MonoBehaviour
{
    public enum Modifiers
    {
        DOUBLE_STEP = 1
    }

    [SerializeField] private List<Modifiers> modifiers = new List<Modifiers>();
    [SerializeField] private EnemyBase enemy;
    
    /// <summary>
    /// Sets starting modifiers
    /// </summary>
    private void Start()
    {
        foreach (Modifiers modifier in modifiers)
        {
            switch (modifier)
            {
                case Modifiers.DOUBLE_STEP:
                    enemy.Modifiers.Add(new DoubleStep());
                    break;
            }
        }

        foreach(Modifier modifier in Stats.CurrentModifiers)
        {
            enemy.Modifiers.Add(modifier.Clone());
        }
    }
}
