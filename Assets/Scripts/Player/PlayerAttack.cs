/*****************************************************************************
// File Name : PlayerAttack.cs
// Author : Lucas Fehlberg
// Creation Date : March 30, 2025
// Last Updated : April 24, 2025
//
// Brief Description : Allows the player to deal damage
*****************************************************************************/

using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerAttack : MonoBehaviour
{
    [SerializeField] private LayerMask enemyLayers;

    [SerializeField] private GameObject indicator;
    [SerializeField] private Material indicatorActiveMaterial;
    [SerializeField] private Material indicatorInactiveMaterial;

    private InputAction click;

    private PlayerBase player;

    private bool awoken = false;

    //For now. We'll make static variables for this at some point
    public LayerMask EnemyLayers { get => enemyLayers; set => enemyLayers = value; }

    /// <summary>
    /// Sets up input actions
    /// </summary>
    private void Start()
    {
        awoken = true;
        player = GetComponent<PlayerBase>();

        //Setup actions
        click = GetComponent<PlayerInput>().currentActionMap.FindAction("MainAction");
        click.started += Click_started;
    }

    /// <summary>
    /// Deletes clickstarted
    /// </summary>
    private void OnDestroy()
    {
        if (!awoken)
        {
            return;
        }

        click.started -= Click_started;
    }

    /// <summary>
    /// Attacks an enemy
    /// </summary>
    /// <param name="obj"></param>
    private void Click_started(InputAction.CallbackContext obj)
    {
        //I really shouldn't have to check this but apparently I do?
        //I'm not even kidding this actually prevents a stupid bug
        if (!isActiveAndEnabled)
        {
            return;
        }
        //If the player doesn't have any attack left
        if(player.AttackRemaining <= 0)
        {
            return;
        }

        //Run physics
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, Mathf.Infinity, enemyLayers))
        {
            //Check if gameobject is in movement
            if (!ObjectInMovementPath(hit.collider.gameObject, out Vector2 direction))
            {
                return;
            }

            player.AttackRemaining--;
            player.UpdateStats();
            
            hit.collider.GetComponent<EnemyBase>().TakeDamage(Stats.Damage);
            foreach (Item item in Stats.HeldItems)
            {
                item.OnAttack(hit.collider.GetComponent<EnemyBase>(), Stats.Damage, direction);
            }

            KillIndicators();
            if(player.AttackRemaining > 0)
            {
                StartCoroutine(nameof(WaitUpdate));
            } 
            else
            {
                enabled = false;
            }
        }
    }

    /// <summary>
    /// Waits before resetting indicators
    /// </summary>
    /// <returns></returns>
    private IEnumerator WaitUpdate()
    {
        yield return new WaitForEndOfFrame();

        ResetIndicators();
    }

    /// <summary>
    /// Check tiles to see if it is a valid option
    /// </summary>
    /// <returns></returns>
    private bool ObjectInMovementPath(GameObject go, out Vector2 direction)
    {
        Vector3 movement = go.transform.position - transform.position;
        Vector2 conversion = new(movement.x, movement.z);

        if(Stats.PossibleAttacks.Contains(conversion))
        {
            direction = conversion;
            return true;
        }

        direction = Vector3.zero;
        return false;
    }

    /// <summary>
    /// Sets up all indicators
    /// </summary>
    public void ResetIndicators()
    {
        //Putting this in here for more code efficiency
        if (!isActiveAndEnabled)
        {
            return;
        }

        foreach (Vector2 movement in Stats.PossibleAttacks)
        {
            Vector3 testPos = transform.position + new Vector3(movement.x, 0, movement.y);

            if(testPos.x < -3.5f || testPos.x > 3.5f)
            {
                continue;
            }

            if(testPos.z < 0 || testPos.z > 7.5f)
            {
                continue;
            }

            GameObject newIndicator = Instantiate(indicator);
            newIndicator.transform.position = testPos;

            //If there is no enemy, set up a different material
            if (!Physics.CheckBox(testPos, Vector3.one * 0.45f, Quaternion.identity, enemyLayers))
            {
                newIndicator.transform.GetChild(0)
                    .GetComponent<MeshRenderer>().material = indicatorInactiveMaterial;
                continue;
            }

            newIndicator.transform.GetChild(0).GetComponent<MeshRenderer>().material = indicatorActiveMaterial;
        }
    }

    /// <summary>
    /// Kills all indicators
    /// </summary>
    public void KillIndicators()
    {
        foreach(GameObject indicatorA in GameObject.FindGameObjectsWithTag("Indicator"))
        {
            Destroy(indicatorA);
        }
    }
}
