/*****************************************************************************
// File Name : BigFreakingBoot.cs
// Author : Lucas Fehlberg
// Creation Date : April 2, 2025
// Last Updated : April 4, 2025
//
// Brief Description : An item that stuns enemies on movement
*****************************************************************************/

using UnityEngine;

public class BigFreakingBoot : Item
{
    private LayerMask enemyLayers;

    /// <summary>
    /// Stun nearby enemies
    /// </summary>
    /// <param name="position">Position of stun</param>
    public override void OnMove(Vector3 position)
    {
        for (int x = -1; x < 2; x++)
        {
            for (int z = -1; z < 2; z++)
            {
                if(x == 0 && z == 0)
                {
                    continue;
                }

                Vector3 testPos = position + new Vector3(x, 0, z);

                if (!Physics.CheckBox(testPos, Vector3.one * 0.45f, Quaternion.identity, enemyLayers))
                {
                    continue;
                }

                Physics.OverlapBox(testPos, Vector3.one * 0.45f, Quaternion.identity, enemyLayers)[0]
                    .GetComponent<EnemyBase>().Modifiers.Add(new Stunned(1));
            }
        }
    }

    /// <summary>
    /// Set itemName and itemDescription
    /// </summary>
    public override void SetDefaults()
    {
        itemName = "BigFreakingBoot";
        itemNameDisplay = "Big Freaking Boot";
        itemDescription = "Enemies within 1 tile (both diagonally and orthagonally) will be stunned after move";
        itemRarity = 1;

        enemyLayers =  1 << LayerMask.NameToLayer("Enemy");
    }
}
