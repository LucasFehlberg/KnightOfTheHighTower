/*****************************************************************************
// File Name : DoubleStep.cs
// Author : Lucas Fehlberg
// Creation Date : March 30, 2025
// Last Updated : March 30, 2025
//
// Brief Description : Allows any piece with this modifier to do a double-step on their first move
*****************************************************************************/

public class DoubleStep : Modifier
{
    /// <summary>
    /// Constructor
    /// </summary>
    public DoubleStep() { }

    private bool firstTurn = true;

    public override void SetDefaults()
    {
        modifierName = "DoubleStep";
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
