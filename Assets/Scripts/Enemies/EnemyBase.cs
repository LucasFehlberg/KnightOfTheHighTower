/*****************************************************************************
// File Name : EnemyBase.cs
// Author : Lucas Fehlberg
// Creation Date : March 30, 2025
// Last Updated : April 25, 2025
//
// Brief Description : Controls enemy stats and other general things
*****************************************************************************/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBase : MonoBehaviour
{
    [SerializeField] private LayerMask playerLayer;
    [SerializeField] private LayerMask obstacleLayers;
    [SerializeField] private LayerMask groundLayers;

    [SerializeField] private int health = 1;
    [SerializeField] private int movement = 1;
    [SerializeField] private int attack = 1;

    [SerializeField] private bool canJumpHoles = false;
    [SerializeField] private bool canJumpWalls = false;

    private int healthRemaining = 1;
    private int movementRemaining = 1;
    private int attackRemaining = 1;

    [SerializeField] private List<Vector2> possibleNormalMovements = new();
    [SerializeField] private List<Vector2> possibleInfiniteMovements = new();

    [SerializeField] private List<Vector2> possibleNormalAttacks = new();
    [SerializeField] private List<Vector2> possibleInfiniteAttacks = new();

    [SerializeField] private List<Modifier> modifiers = new();

    [SerializeField] private EnemyInformation info;
    [SerializeField] private GameObject deathParticles;

    [SerializeField] private Animator animator;

    private Node target;

    private bool dying = false;

    private AttatchmentBase currentTile;

    private List<Vector2> additionalAttackOptions = new();
    private List<Vector2> additionalMovementOptions = new();

    public int HealthRemaining { get => healthRemaining; set => healthRemaining = value; }
    public int MovementRemaining { get => movementRemaining; set => movementRemaining = value; }
    public int AttackRemaining { get => attackRemaining; set => attackRemaining = value; }
    public List<Modifier> Modifiers { get => modifiers; set => modifiers = value; }
    public bool CanJumpHoles { get => canJumpHoles; set => canJumpHoles = value; }
    public bool CanJumpWalls { get => canJumpWalls; set => canJumpWalls = value; }
    public List<Vector2> PossibleNormalMovements { get => possibleNormalMovements; set => 
            possibleNormalMovements = value; }
    public List<Vector2> PossibleInfiniteMovements { get => possibleInfiniteMovements; set => 
            possibleInfiniteMovements = value; }
    public List<Vector2> PossibleNormalAttacks { get => possibleNormalAttacks; set => possibleNormalAttacks = value; }
    public List<Vector2> PossibleInfiniteAttacks { get => 
            possibleInfiniteAttacks; set => possibleInfiniteAttacks = value; }
    public Node Target { get => target; set => target = value; }
    public bool Dying { get => dying; set => dying = value; }
    public AttatchmentBase CurrentTile { get => currentTile; set => currentTile = value; }
    public List<Vector2> AdditionalAttackOptions 
    { get => additionalAttackOptions; set => additionalAttackOptions = value; }
    public List<Vector2> AdditionalMovementOptions 
    { get => additionalMovementOptions; set => additionalMovementOptions = value; }
    public EnemyInformation Info { get => info; set => info = value; }
    public int Attack { get => attack; set => attack = value; }
    public int Movement { get => movement; set => movement = value; }

    /// <summary>
    /// When the enemy comes into existance, do some initialization
    /// </summary>
    private void Start()
    {
        HealthRemaining = health;

        foreach(Modifier modifiers in modifiers)
        {
            modifiers.Enemy = this;
        }
    }

    /// <summary>
    /// Updates movement and attack options
    /// </summary>
    public void UpdateMovementOptions()
    {
        foreach (Vector2 movement in additionalMovementOptions)
        {
            if (!PossibleNormalMovements.Contains(movement))
            {
                possibleNormalMovements.Add(movement);
            }
        }

        foreach (Vector2 attack in additionalAttackOptions)
        {
            if (!PossibleNormalAttacks.Contains(attack))
            {
                possibleNormalAttacks.Add(attack);
            }
        }
    }

    /// <summary>
    /// When the enemy starts their turn
    /// </summary>
    public void OnStartTurn()
    {
        target = null;
        MovementRemaining = movement;
        attackRemaining = attack;

        //Start turn
        foreach (Modifier modifiers in modifiers)
        {
            //Side effect of jank, will redo this later to be more streamlined
            modifiers.Enemy = this;

            modifiers.OnStartTurn();
        }

        //Latestart turn
        foreach (Modifier modifiers in modifiers)
        {
            modifiers.OnLateStartTurn();
        }

        StartCoroutine(nameof(EnemyTurn));
    }

    /// <summary>
    /// When enemy takes damage
    /// </summary>
    /// <param name="damage">Recieving Damage</param>
    public void TakeDamage(int damage, bool fall = false)
    {
        HealthRemaining -= damage;
        if (healthRemaining <= 0)
        {
            foreach(Item item in Stats.HeldItems)
            {
                item.OnKillEnemy(transform.position);
            }

            foreach(Modifier modifier in modifiers)
            {
                modifier.OnKill();
            }

            if (!fall)
            {
                if(animator != null)
                {
                    animator.SetBool("Dead", true);

                    dying = true;
                } else
                {
                    Instantiate(deathParticles, transform.position + Vector3.up * 0.5f, Quaternion.Euler(-90, 0, 0));
                    Destroy(gameObject);
                }

            }
        }
    }
    
    public void KillPiece()
    {
        Instantiate(deathParticles, transform.position, Quaternion.Euler(-90, 0, 0));

        Destroy(gameObject);
    }

    //On their turn, enemies move and do special things in that order, attacking only when they have an opportunity to

    /// <summary>
    /// What the enemy does on this turn
    /// </summary>
    /// <returns></returns>
    private IEnumerator EnemyTurn()
    {
        while (movementRemaining > 0)
        {
            //We check for attack before we move
            if (!GetComponent<EnemyMovement>().Moving && !GetComponent<EnemyMovement>().TileTrigger)
            {
                CheckForAttack();
                GetComponent<EnemyMovement>().MoveEnemy();

                while(GetComponent<EnemyMovement>().Moving || GetComponent<EnemyMovement>().TileTrigger)
                {
                    yield return null;
                }
            }
            yield return null;
        }

        yield return null;

        CheckForAttack();

        if (currentTile != null)
        {
            currentTile.OnEndTurnActive(gameObject);
        }

        ClearAdditionals();

        //Use up the rest of attack
        while (AttackRemaining > 0)
        {
            if (!CheckForAttack())
            {
                AttackRemaining = 0;
            }
        }

        GameObject.FindGameObjectWithTag("GameController").GetComponent<GameController>().EndTurn();
    }

    /// <summary>
    /// Clears any additional movement
    /// </summary>
    private void ClearAdditionals()
    {
        foreach (Vector2 movement in additionalMovementOptions)
        {
            if (possibleNormalMovements.Contains(movement))
            {
                possibleNormalMovements.Remove(movement);
            }
        }

        foreach (Vector2 attack in additionalAttackOptions)
        {
            if (possibleNormalAttacks.Contains(attack))
            {
                possibleNormalAttacks.Remove(attack);
            }
        }

        additionalMovementOptions.Clear();
        additionalAttackOptions.Clear();
    }

    /// <summary>
    /// Enemy Attack
    /// </summary>
    private bool CheckForAttack()
    {
        if (attackRemaining <= 0)
        {
            return false;
        }

        //Infinites first
        foreach(Vector2 vector in PossibleInfiniteAttacks)
        {
            PlayerBase player = CheckDirection(transform.position, vector);
            if(player != null)
            {
                attackRemaining -= 1;
                player.TakeDamage(1);
                return true;
            }
        }

        foreach(Vector2 vector in possibleNormalAttacks)
        {
            Vector3 testPos = transform.position + new Vector3(vector.x, 0, vector.y);

            if (Physics.CheckBox(testPos, Vector3.one * 0.45f, Quaternion.identity, playerLayer))
            {
                attackRemaining -= 1;
                Physics.OverlapBox(testPos, Vector3.one * 0.45f, Quaternion.identity, playerLayer)
                    [0].GetComponent<PlayerBase>().TakeDamage(1);
                return true;
            }
        }

        return false;
    }

    /// <summary>
    /// Checks the direction of enemy attack
    /// </summary>
    /// <param name="origin">Origin of the attack</param>
    /// <param name="direction">Direction of the attack</param>
    /// <returns>If the enemy attacked the player</returns>
    private PlayerBase CheckDirection(Vector3 origin, Vector2 direction)
    {
        PlayerBase player = null;
        Vector3 currentPos = origin;
        bool stillRunning = true;

        //I don't need this but I'm stupid so I'm keeping it in
        if (direction == Vector2.zero)
        {
            Debug.Log("You have a movement direction set to 0, 0 somewhere. Fix it");
            stillRunning = false;
        }

        while (stillRunning)
        {
            currentPos += new Vector3(direction.x, 0, direction.y);

            //Check for floor
            if (!Physics.CheckBox(currentPos + Vector3.down, Vector3.one * 0.45f, Quaternion.identity, groundLayers))
            {
                if (!canJumpHoles)
                {
                    stillRunning = false;
                }
                continue;
            }

            //Next, we check for obstacles
            if (Physics.CheckBox(currentPos, Vector3.one * 0.45f, Quaternion.identity, obstacleLayers))
            {
                if (!canJumpWalls)
                {
                    stillRunning = false;
                }
                continue;
            }

            //Check if the position is still inside the board
            if (currentPos.x < -3.5f || currentPos.x > 3.5f || currentPos.z < 0f || currentPos.z > 7f)
            {
                stillRunning = false;
            }

            if (Physics.CheckBox(currentPos, Vector3.one * 0.45f, Quaternion.identity, playerLayer))
            {
                attackRemaining -= 1;
                player = Physics.OverlapBox(currentPos, Vector3.one * 0.45f, Quaternion.identity, playerLayer)
                    [0].GetComponent<PlayerBase>();
                stillRunning = false;
            }

        }

        return player;
    }

    /// <summary>
    /// The stupid death, enemy edition
    /// </summary>
    public void KillEnemyFunny()
    {
        dying = true;
        TakeDamage(10000000, true);

        GetComponent<Rigidbody>().isKinematic = false;
        GetComponent<Rigidbody>().useGravity = true;

        GetComponent<Rigidbody>().angularVelocity = Vector3.forward;

        StartCoroutine(nameof(Death));
        StopCoroutine(nameof(EnemyTurn));
        GameObject.FindGameObjectWithTag("GameController").GetComponent<GameController>().EndTurn();

    }

    /// <summary>
    /// Kill the enemy from falling
    /// </summary>
    /// <returns></returns>
    private IEnumerator Death()
    {
        yield return new WaitForSeconds(10);
        Destroy(gameObject);
    }
}
