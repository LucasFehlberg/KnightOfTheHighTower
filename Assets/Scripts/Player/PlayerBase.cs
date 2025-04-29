/*****************************************************************************
// File Name : PlayerBase.cs
// Author : Lucas Fehlberg
// Creation Date : March 29, 2025
// Last Updated : April 29, 2025
//
// Brief Description : Controls player action map and other misc things for the player
*****************************************************************************/

using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class PlayerBase : MonoBehaviour
{
    private PlayerInput pInput;

    private int movementRemaining = 0;
    private int manipulationRemaining = 0;
    private int attackRemaining = 0;
    private int healthRemaining = 0;

    private int nextTurnMoveModifier = 0;
    private int nextTurnManipulateModifier = 0;
    private int nextTurnAttackModifier = 0;

    private bool canDoAction = true;

    public int MovementRemaining { get => movementRemaining; set => movementRemaining = value; }
    public int ManipulationRemaining { get => manipulationRemaining; set => manipulationRemaining = value; }
    public int AttackRemaining { get => attackRemaining; set => attackRemaining = value; }
    public int NextTurnMoveModifier { get => nextTurnMoveModifier; set => nextTurnMoveModifier = value; }
    public int NextTurnManipulateModifier { get => nextTurnManipulateModifier; set => nextTurnManipulateModifier = value; }
    public int NextTurnAttackModifier { get => nextTurnAttackModifier; set => nextTurnAttackModifier = value; }
    public int HealthRemaining { get => healthRemaining; set => healthRemaining = value; }
    public AttatchmentBase CurrentTile { get => currentTile; set => currentTile = value; }
    public bool CanDoAction { get => canDoAction; set => canDoAction = value; }

    [SerializeField] private TMP_Text movementText;
    [SerializeField] private TMP_Text attackText;
    [SerializeField] private TMP_Text manipulationText;
    [SerializeField] private TMP_Text lifeText;

    [SerializeField] private Texture NoMoreAbility;

    private InputAction restart;
    private InputAction quit;

    private AttatchmentBase currentTile;

    /// <summary>
    /// Sets up action map
    /// </summary>
    private void Awake()
    {
        pInput = GetComponent<PlayerInput>();
        pInput.currentActionMap.Enable();

        restart = pInput.currentActionMap.FindAction("Restart");
        quit = pInput.currentActionMap.FindAction("Quit");

        restart.started += Restart_started;
        quit.started += Quit_started;
    }

    /// <summary>
    /// Here for alpha, will be replaced with a pause menu later
    /// </summary>
    /// <param name="obj"></param>
    private void Quit_started(InputAction.CallbackContext obj)
    {
        Application.Quit();
    }

    /// <summary>
    /// Here for alpha, will be replaced with a pause menu
    /// </summary>
    /// <param name="obj"></param>
    private void Restart_started(InputAction.CallbackContext obj)
    {
        //Loads title since this is a roguelike
        SceneManager.LoadScene(0);
    }

    /// <summary>
    /// Avoid errors
    /// </summary>
    private void OnDestroy()
    {
        restart.started -= Restart_started;
        quit.started -= Quit_started;
    }

    /// <summary>
    /// Resets stats to base values
    /// </summary>
    private void ResetStats()
    {
        //Main Stats
        Stats.Health = 3;
        Stats.Movement = 1;
        Stats.Manipulation = 1;
        Stats.Attack = 1;
        Stats.TerrainRange = 2;


        //All the knight movements
        Stats.PossibleMovements.Clear();
        Stats.PossibleMovements = new List<Vector2>
            { new(1, 2), new(-1, 2), new(1, -2), new(-1, -2), new(2, 1), new(-2, 1), new(2, -1), new(-2, -1) };

        //Copy and pasted array
        Stats.PossibleAttacks.Clear();
        Stats.PossibleAttacks = new List<Vector2>
            { new(1, 2), new(-1, 2), new(1, -2), new(-1, -2), new(2, 1), new(-2, 1), new(2, -1), new(-2, -1) };

        //Randomness stats
        Stats.RandMaxAlter = 0;
        Stats.RandMinAlter = 0;
        Stats.Rerolls = 0;
    }

    /// <summary>
    /// Initializes and uses item effects
    /// </summary>
    private void Start()
    {
        ResetStats();

        //Stats.HeldItems.Add(new EscapePlan());
        //Stats.HeldItems.Add(new BigFreakingBoot());
        //Stats.HeldItems.Add(new PiercingArrow());
        //Stats.HeldItems.Add(new AdrenalineShot());
        //GameObject.FindGameObjectWithTag("ItemHolder").GetComponent<ItemHolder>().UpdateUI();

        foreach(Item item in Stats.HeldItems)
        {
            if(item.Player == null)
            {
                item.Player = this;
            }
        }

        //Stat effects
        foreach(Item item in Stats.HeldItems)
        {
            item.UpdateStats();
        }

        //Late stat effects
        foreach (Item item in Stats.HeldItems)
        {
            item.LateUpdateStats();
        }

        healthRemaining = Stats.Health;
    }

    /// <summary>
    /// Select current action
    /// </summary>
    /// <param name="action">Type of action</param>
    public void SetAction(int action) //Because apparently Unity events don't LIKE ENUMS
    {
        StartCoroutine(nameof(BufferAction), action);
    }

    private bool TutorialCheck(int action)
    {
        if (Stats.DoneTutorial)
        {
            return true;
        }

        if((TutorialScript.instance.TutorialState == 1 || TutorialScript.instance.TutorialState == 6) && action == 2)
        {
            return true;
        }

        if((TutorialScript.instance.TutorialState == 3 || TutorialScript.instance.TutorialState == 8) && action == 0)
        {
            return true;
        }

        if (TutorialScript.instance.TutorialState == 11 && action == 1)
        {
            return true;
        }

        if ((TutorialScript.instance.TutorialState == 5 || TutorialScript.instance.TutorialState == 12) 
            && action == -1)
        {
            return true;
        }

        return false;
    }

    /// <summary>
    /// Buffers to prevent bugs, but still feel smooth
    /// </summary>
    /// <param name="action">ActionID</param>
    /// <returns></returns>
    private IEnumerator BufferAction(int action)
    {
        if (!TutorialCheck(action))
        {
            yield break;
        }

        while (!canDoAction)
        {
            yield return null;
        }

        if(action == -1) //-1 is end turn
        {
            EndTurn();
            if (!Stats.DoneTutorial)
            {
                TutorialScript.instance.TutorialState++;
            }
            yield break;
        }

        //Kill all indicators first
        GetComponent<PlayerMovement>().KillIndicators();
        GetComponent<PlayerAttack>().KillIndicators();
        GetComponent<PlayerTerrain>().KillIndicators();

        GetComponent<PlayerMovement>().enabled = action == 0 && movementRemaining > 0; //0 is movement
        GetComponent<PlayerAttack>().enabled = action == 1 && attackRemaining > 0; //1 is attack
        GetComponent<PlayerTerrain>().enabled = action == 2 && manipulationRemaining > 0; //2 is terrain

        //Setup new indicators
        GetComponent<PlayerMovement>().ResetIndicators();

        GetComponent<PlayerAttack>().ResetIndicators();

        GetComponent<PlayerTerrain>().ResetIndicators();
    }

    /// <summary>
    /// Ends the turn
    /// </summary>
    private void EndTurn()
    {
        if (!canDoAction)
        {
            return;
        }

        if(currentTile != null)
        {
            currentTile.OnEndTurnActive(gameObject);
        }

        foreach (Item item in Stats.HeldItems)
        {
            item.OnEndTurn();
        }

        GetComponent<PlayerMovement>().KillIndicators();
        GetComponent<PlayerAttack>().KillIndicators();
        GetComponent<PlayerTerrain>().KillIndicators();

        GetComponent<PlayerMovement>().enabled = false;
        GetComponent<PlayerAttack>().enabled = false;
        GetComponent<PlayerTerrain>().enabled = false;

        foreach (Vector2 movement in Stats.AdditionalMovements)
        {
            if (Stats.PossibleMovements.Contains(movement))
            {
                Stats.PossibleMovements.Remove(movement);
            }
        }

        foreach (Vector2 attack in Stats.AdditionalAttacks)
        {
            if (Stats.PossibleAttacks.Contains(attack))
            {
                Stats.PossibleAttacks.Remove(attack);
            }
        }

        Stats.AdditionalAttacks.Clear();
        Stats.AdditionalMovements.Clear();

        GameObject.FindGameObjectWithTag("GameController").GetComponent<GameController>().EndTurn();
    }


    /// <summary>
    /// Does all OnStartTurn effects
    /// </summary>
    public void OnStartTurn()
    {
        movementRemaining = Stats.Movement;
        manipulationRemaining = Stats.Manipulation;
        attackRemaining = Stats.Attack;

        foreach (Item item in Stats.HeldItems)
        {
            item.OnStartTurn();
        }

        //Run modifier stuff
        foreach (GameObject enemy in GameObject.FindGameObjectsWithTag("Enemy"))
        {
            foreach (Modifier modifier in enemy.GetComponent<EnemyBase>().Modifiers)
            {
                modifier.OnPlayerStartTurn(this);
            }
        }

        SetAction(0);

        UpdateStats();
        UpdateMovementOptions();
    }

    /// <summary>
    /// Updates movement and attack options
    /// </summary>
    public void UpdateMovementOptions()
    {
        foreach (Vector2 movement in Stats.AdditionalMovements)
        {
            if (!Stats.PossibleMovements.Contains(movement))
            {
                Stats.PossibleMovements.Add(movement);
            }
        }

        foreach (Vector2 attack in Stats.AdditionalAttacks)
        {
            if (!Stats.PossibleAttacks.Contains(attack))
            {
                Stats.PossibleAttacks.Add(attack);
            }
        }
    }

    /// <summary>
    /// Update the visual stats
    /// </summary>
    public void UpdateStats()
    {
        movementText.text = ": " + MovementRemaining.ToString();
        attackText.text = ": " + AttackRemaining.ToString();
        manipulationText.text = ": " + ManipulationRemaining.ToString();
        lifeText.text = healthRemaining.ToString() + "/" + Stats.Health.ToString();
    }

    /// <summary>
    /// Deal damage to player. If damage makes health reach zero, lose the game
    /// </summary>
    /// <param name="damage">Amount of damage</param>
    public void TakeDamage(int damage)
    {
        StartCoroutine(nameof(TakeDamageCoroutine), damage);
    }

    /// <summary>
    /// The actual coroutine, as this allows methods to take place that can avoid things like death and damage
    /// </summary>
    /// <param name="damage">Damage</param>
    /// <returns></returns>
    private IEnumerator TakeDamageCoroutine(int damage)
    {
        healthRemaining -= damage;

        foreach (Item item in Stats.HeldItems)
        {
            item.OnTakeDamage(damage);
        }

        yield return new WaitForEndOfFrame();

        UpdateStats();

        if (healthRemaining <= 0)
        {
            foreach(Item item in Stats.HeldItems)
            {
                item.OnDeath();
            }

            healthRemaining = 0;

            yield return new WaitForEndOfFrame();

            //We'll check health again before killing the player
            if (healthRemaining <= 0)
            {
                GameObject.FindGameObjectWithTag("GameController").GetComponent<GameController>().EndGame();

                GetComponent<PlayerMovement>().enabled = false;
                GetComponent<PlayerAttack>().enabled = false;
                GetComponent<PlayerTerrain>().enabled = false;
            }
        }
    }

    /// <summary>
    /// The stupidest death the player can have
    /// Oh yeah, and nothing can save you from this funny death
    /// No items get run, and the player falls
    /// </summary>
    public void KillPlayerFunny()
    {
        GameObject.FindGameObjectWithTag("GameController").GetComponent<GameController>().EndGame();

        GetComponent<PlayerMovement>().enabled = false;
        GetComponent<PlayerAttack>().enabled = false;
        GetComponent<PlayerTerrain>().enabled = false;

        GetComponent<Rigidbody>().isKinematic = false;
        GetComponent<Rigidbody>().useGravity = true;

        GetComponent<Rigidbody>().angularVelocity = Vector3.forward;
    }
}
