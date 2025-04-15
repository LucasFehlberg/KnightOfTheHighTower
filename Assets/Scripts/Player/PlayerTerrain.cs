/*****************************************************************************
// File Name : PlayerTerrain.cs
// Author : Lucas Fehlberg
// Creation Date : March 30, 2025
// Last Updated : April 13, 2025
//
// Brief Description : Controls the player's terrain manipulation
*****************************************************************************/

using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerTerrain : MonoBehaviour
{
    //This one will likely be the most annoying one
    //There will be a Menu on the right that allows the player to select what they want to do
    //The two main actions is removing terrain and adding it

    [SerializeField] private LayerMask tileLayers;
    [SerializeField] private LayerMask enemyLayers;
    [SerializeField] private LayerMask playerLayer;
    [SerializeField] private LayerMask wallLayers;
    [SerializeField] private LayerMask groundLayers;

    [SerializeField] private GameObject indicator;
    [SerializeField] private Material indicatorMaterial;

    private InputAction click;

    private PlayerBase player;

    private bool awoken = false;
    private bool add = false;
    private string type = "Wall";

    [SerializeField] private GameObject UI;
    [SerializeField] private List<GameObject> slots = new();

    public bool Add { get => add; set => add = value; }

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
    /// Enable the UI
    /// </summary>
    private void OnEnable()
    {
        UI.SetActive(true);
        add = false;
        type = "Wall";
        int index = 0;
        foreach(GameObject slot in slots)
        {
            if (Stats.HeldTiles[index] == null)
            {
                slot.transform.parent.gameObject.SetActive(false);
                continue;
            }

            slot.transform.parent.gameObject.SetActive(true);

            string description = Attatchment.AttatchmentDescriptions[Stats.HeldTiles[index]];
            Sprite sprite = Attatchment.AttatchmentSprites[Stats.HeldTiles[index]];

            slot.GetComponent<TileBox>().ButtonUI(description, Stats.HeldTiles[index], sprite);
            index++;
        }
    }

    /// <summary>
    /// Disable the UI
    /// </summary>
    private void OnDisable()
    {
        if(UI != null)
        {
            UI.SetActive(false);
        }
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
    /// Do terrain action
    /// </summary>
    /// <param name="obj"></param>
    private void Click_started(InputAction.CallbackContext obj)
    {
        if (!isActiveAndEnabled)
        {
            return;
        }

        if (player.ManipulationRemaining <= 0)
        {
            return;
        }

        //Run physics
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out RaycastHit hit, Mathf.Infinity, tileLayers))
        {
            Vector2 playerPos = new(transform.position.x, transform.position.z);
            Vector2 tilePos = new(hit.collider.transform.position.x, hit.collider.transform.position.z);

            if (Vector2.Distance(playerPos, tilePos) > Stats.TerrainRange + 0.5f)
            {
                return;
            }

            if (Physics.CheckBox(hit.collider.transform.position, Vector3.one * 0.45f, Quaternion.identity,
                enemyLayers))
            {
                return;
            }

            if (!hit.collider.TryGetComponent<Tile>(out var tile))
            {
                tile = hit.transform.parent.parent.GetComponent<Tile>();
            }

            if (add)
            {
                //We don't want to make a box on the player here
                if (Physics.CheckBox(hit.collider.transform.position, Vector3.one * 0.45f, Quaternion.identity,
                    playerLayer))
                {
                    return;
                }

                //If it's a wall, check to see if the tile exists
                if(type != "Wall" && !tile.GetComponent<Tile>().HasTile)
                {
                    return;
                }

                if (!tile.GetComponent<Tile>().Build(type))
                {
                    return;
                }
            }
            else
            {
                //We won't check for player here because it would be funny if the player fell to their death
                if (!tile.GetComponent<Tile>().Destroy())
                {
                    return;
                }

                //If there is no ground, DON'T ignore
                if (!Physics.CheckBox(transform.position + Vector3.down, Vector3.one * 0.45f, Quaternion.identity,
                    groundLayers))
                {
                    player.KillPlayerFunny();
                }
            }

            player.ManipulationRemaining--;
            player.UpdateStats();

            foreach(Item item in Stats.HeldItems)
            {
                item.OnTerrainManipulation(hit.collider.transform.position, type);
            }

            KillIndicators();
            if (player.ManipulationRemaining > 0)
            {
                ResetIndicators();
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

        foreach (GameObject tile in GameObject.FindGameObjectsWithTag("Tile"))
        {
            Vector2 playerPos = new Vector2(transform.position.x, transform.position.z);
            Vector2 tilePos = new Vector2(tile.transform.position.x, tile.transform.position.z);

            if(Vector2.Distance(playerPos, tilePos) > Stats.TerrainRange + 0.5f)
            {
                continue;
            }

            if (add && !tile.GetComponent<Tile>().BuiltUpon)
            {
                if (Physics.CheckBox(tile.transform.position, Vector3.one * 0.45f, Quaternion.identity,
                    enemyLayers))
                {
                    continue;
                }

                if (Physics.CheckBox(tile.transform.position, Vector3.one * 0.45f, Quaternion.identity,
                    playerLayer))
                {
                    continue;
                }

                //If it's a wall, check to see if the tile exists
                if (type != "Wall" && !tile.GetComponent<Tile>().HasTile)
                {
                    continue;
                }

                GameObject newUI = Instantiate(indicator);
                newUI.transform.GetChild(0).GetComponent<MeshRenderer>().material = indicatorMaterial;
                newUI.transform.position = tile.transform.position + (Vector3.up * 0.75f);
            }

            if (!add && tile.GetComponent<Tile>().HasTile)
            {
                if (Physics.CheckBox(tile.transform.position, Vector3.one * 0.45f, Quaternion.identity,
                    enemyLayers))
                {
                    continue;
                }

                GameObject newUI = Instantiate(indicator);
                newUI.transform.GetChild(0).GetComponent<MeshRenderer>().material = indicatorMaterial;
                newUI.transform.position = tile.transform.position + (Vector3.up * 0.75f);

                if (Physics.CheckBox(tile.transform.position + Vector3.up, Vector3.one * 0.45f, Quaternion.identity,
                    wallLayers))
                {
                    newUI.transform.position += Vector3.up;
                }
            }
        }
    }

    /// <summary>
    /// Kills all indicators
    /// </summary>
    public void KillIndicators()
    {
        foreach (GameObject indicatorA in GameObject.FindGameObjectsWithTag("Indicator"))
        {
            Destroy(indicatorA);
        }
    }

    /// <summary>
    /// Selects the remove terrain feature
    /// </summary>
    public void RemoveTerrain()
    {
        add = false;
        KillIndicators();

        if(player.ManipulationRemaining > 0)
        {
            ResetIndicators();
        }
    }

    /// <summary>
    /// Selects the add terrain feature
    /// </summary>
    /// <param name="type">Type of terrain</param>
    public void AddTerrain(string type)
    {
        add = true;
        this.type = type;
        KillIndicators();
        if(player.ManipulationRemaining > 0)
        {
            ResetIndicators();
        }
    }

    /// <summary>
    /// Same as above, but by Stats.HeldTiles
    /// </summary>
    /// <param name="id">ID number of held terrain</param>
    public void AddTerrainByInt(int id)
    {
        AddTerrain(Stats.HeldTiles[id]);
    }
}
