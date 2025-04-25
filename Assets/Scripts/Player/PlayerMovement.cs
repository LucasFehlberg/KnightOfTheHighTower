/*****************************************************************************
// File Name : PlayerMovement.cs
// Author : Lucas Fehlberg
// Creation Date : March 29, 2025
// Last Updated : April 24, 2025
//
// Brief Description : Moves the player around based on num movements left
*****************************************************************************/

using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private LayerMask groundLayers;
    [SerializeField] private LayerMask obstacleLayers;

    [SerializeField] private GameObject indicator;
    [SerializeField] private Material indicatorMaterial;

    private InputAction click;

    private Vector3 desiredPos = Vector3.zero;
    private Vector3 originalPos = Vector3.zero;
    private float percentage = 1f;

    private PlayerBase player;

    private bool awoken = false;

    private List<GameObject> triggeredTiles = new();
    private bool tileTrigger = false;

    public List<GameObject> TriggeredTiles { get => triggeredTiles; set => triggeredTiles = value; }
    public bool TileTrigger { get => tileTrigger; set => tileTrigger = value; }
    public float Percentage { get => percentage; set => percentage = value; }
    public Vector3 DesiredPos { get => desiredPos; set => desiredPos = value; }
    public Vector3 OriginalPos { get => originalPos; set => originalPos = value; }

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
    /// Move the player
    /// </summary>
    private void Click_started(InputAction.CallbackContext obj)
    {
        //I really shouldn't have to check this but apparently I do?
        //I'm not even kidding this actually prevents a stupid bug
        if (!isActiveAndEnabled)
        {
            return;
        }
        //If the player doesn't have any movement left, don't let them move
        if(player.MovementRemaining <= 0)
        {
            return;
        }
        //If the player is currently moving, don't run
        if(percentage < 1f)
        {
            return;
        }

        //Run physics
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out RaycastHit hit, Mathf.Infinity, groundLayers))
        {
            //Check if gameobject is in movement
            if (!ObjectInMovementPath(hit.collider.gameObject))
            {
                return;
            }

            Vector3 newPos = hit.collider.transform.position;

            desiredPos = new(newPos.x, transform.position.y, newPos.z);
            percentage = 0f;
            originalPos = transform.position;

            player.MovementRemaining--;
            player.UpdateStats();


            foreach (Item item in Stats.HeldItems)
            {
                item.OnMove(desiredPos);
            }

            KillIndicators();
        }
    }

    /// <summary>
    /// Check tiles to see if it is a valid option
    /// </summary>
    /// <returns></returns>
    private bool ObjectInMovementPath(GameObject go)
    {
        //If there is an obstacle, ignore
        if(Physics.CheckBox(go.transform.position + Vector3.up, Vector3.one * 0.45f, Quaternion.identity, 
            obstacleLayers))
        {
            return false;
        }

        Vector3 movement = go.transform.position - transform.position;
        Vector2 conversion = new(movement.x, movement.z);

        if(Stats.PossibleMovements.Contains(conversion))
        {
            return true;
        }

        return false;
    }

    /// <summary>
    /// Movement code
    /// </summary>
    private void FixedUpdate()
    {
        if (percentage < 1f)
        {
            player.CanDoAction = false;
            percentage += 0.05f;
            if(percentage > 1f)
            {
                percentage = 1f;
            }

            //Very specific curve fitting here
            float yValue = (-4.5f * (800f / 324f) * (percentage - 0.1f) * (percentage - 1f)) + 0.75f;
            if(yValue < 0.75f)
            {
                yValue = 0.75f;
            }
            if(desiredPos != originalPos)
            {
                Vector3 newPos = Vector3.Lerp(originalPos, desiredPos, percentage);
                transform.position = new(newPos.x, yValue, newPos.z);
            }

            if(percentage == 1f && !tileTrigger)
            {
                player.CanDoAction = true;
                if (player.MovementRemaining > 0)
                {
                    ResetIndicators();
                } 
                else
                {
                    enabled = false;
                }

                triggeredTiles.Clear();

                //If there is no ground, DON'T ignore
                if (!Physics.CheckBox(transform.position + Vector3.down, Vector3.one * 0.45f, Quaternion.identity, 
                    groundLayers))
                {
                    player.KillPlayerFunny();
                }
            }
        }
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

        foreach (Vector2 movement in Stats.PossibleMovements)
        {
            Vector3 testPos = transform.position + new Vector3(movement.x, 0, movement.y);

            //If there is no ground, ignore
            if (!Physics.CheckBox(testPos + Vector3.down, Vector3.one * 0.45f, Quaternion.identity, groundLayers))
            {
                continue;
            }

            //If there is an obstacle, ignore
            if (Physics.CheckBox(testPos, Vector3.one * 0.45f, Quaternion.identity, obstacleLayers))
            {
                continue;
            }
            
            GameObject newIndicator = Instantiate(indicator);
            newIndicator.transform.GetChild(0).GetComponent<MeshRenderer>().material = indicatorMaterial;
            newIndicator.transform.position = testPos;
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
