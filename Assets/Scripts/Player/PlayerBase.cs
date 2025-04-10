/*****************************************************************************
// File Name : PlayerBase.cs
// Author : Lucas Fehlberg
// Creation Date : March 29, 2025
// Last Updated : April 9 2025
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

    public int MovementRemaining { get => movementRemaining; set => movementRemaining = value; }
    public int ManipulationRemaining { get => manipulationRemaining; set => manipulationRemaining = value; }
    public int AttackRemaining { get => attackRemaining; set => attackRemaining = value; }
    public int NextTurnMoveModifier { get => nextTurnMoveModifier; set => nextTurnMoveModifier = value; }
    public int NextTurnManipulateModifier { get => nextTurnManipulateModifier; set => nextTurnManipulateModifier = value; }
    public int NextTurnAttackModifier { get => nextTurnAttackModifier; set => nextTurnAttackModifier = value; }
    public int HealthRemaining { get => healthRemaining; set => healthRemaining = value; }

    [SerializeField] private TMP_Text movementText;
    [SerializeField] private TMP_Text attackText;
    [SerializeField] private TMP_Text manipulationText;
    [SerializeField] private TMP_Text lifeText;

    private InputAction restart;
    private InputAction quit;

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
        //Kill all indicators first
        GetComponent<PlayerMovement>().KillIndicators();
        GetComponent<PlayerAttack>().KillIndicators();
        GetComponent<PlayerTerrain>().KillIndicators();

        GetComponent<PlayerMovement>().enabled = action == 0; //0 is movement
        GetComponent<PlayerAttack>().enabled = action == 1; //1 is attack
        GetComponent<PlayerTerrain>().enabled = action == 2; //2 is terrain

        //Setup new indicators
        if (movementRemaining > 0)
        {
            GetComponent<PlayerMovement>().ResetIndicators();
        }

        if (attackRemaining > 0)
        {
            GetComponent<PlayerAttack>().ResetIndicators();
        }

        if (manipulationRemaining > 0)
        {
            GetComponent<PlayerTerrain>().ResetIndicators();
        }
    }

    /// <summary>
    /// Ends the turn
    /// </summary>
    public void EndTurn()
    {
        foreach(Item item in Stats.HeldItems)
        {
            item.OnEndTurn();
        }

        GetComponent<PlayerMovement>().KillIndicators();
        GetComponent<PlayerAttack>().KillIndicators();
        GetComponent<PlayerTerrain>().KillIndicators();

        GetComponent<PlayerMovement>().enabled = false;
        GetComponent<PlayerAttack>().enabled = false;
        GetComponent<PlayerTerrain>().enabled = false;

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

        SetAction(0);

        UpdateStats();
    }

    /// <summary>
    /// Update the visual stats
    /// </summary>
    public void UpdateStats()
    {
        movementText.text = "Move: " + MovementRemaining.ToString();
        attackText.text = "Attack: " + AttackRemaining.ToString();
        manipulationText.text = "Manipulation: " + ManipulationRemaining.ToString();
        lifeText.text = "Life: " + healthRemaining.ToString();
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
    }
}
