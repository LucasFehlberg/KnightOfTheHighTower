/*****************************************************************************
// File Name : DoubleStep.cs
// Author : Lucas Fehlberg
// Creation Date : March 30, 2025
// Last Updated : March 17 2025
//
// Brief Description : Allows any piece with this modifier to do a double-step on their first move
*****************************************************************************/

public class DoubleStep : Modifier
{
    private bool firstTurn = true;

    public override void SetDefaults()
    {
        modifierName = "Double Step";
        modifierDescription = "Allows the enemy to move twice on the first turn";
        modifierDifficulty = 0;
    }

    /// <summary>
    /// If it is the first turn, disable first turn and add 1 movement
    /// </summary>
    public override void OnStartTurn()
    {
        if (firstTurn)
        {
            enemy.MovementRemaining += 1;
            firstTurn = false;
        }
    }
}
