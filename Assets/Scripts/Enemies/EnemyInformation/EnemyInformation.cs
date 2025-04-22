/*****************************************************************************
// File Name : EnemyInformation.cs
// Author : Lucas Fehlberg
// Creation Date : April 16, 2025
// Last Updated : April 16, 2025
//
// Brief Description : Class for enemy information
*****************************************************************************/

using UnityEngine;

[CreateAssetMenu(fileName = "EnemyInfo", menuName = "Enemy Info")]
public class EnemyInformation : ScriptableObject
{
    [SerializeField] private string enemyName;
    [SerializeField] private string enemyDescription;

    public string EnemyName { get => enemyName; set => enemyName = value; }
    public string EnemyDescription { get => enemyDescription; set => enemyDescription = value; }
}
