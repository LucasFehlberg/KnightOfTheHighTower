/*****************************************************************************
// File Name : GameController.cs
// Author : Lucas Fehlberg
// Creation Date : March 30, 2025
// Last Updated : April 22, 2025
//
// Brief Description : Controls the game flow
*****************************************************************************/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameController : MonoBehaviour
{
    [SerializeField] private List<GameObject> turnOrder;
    [SerializeField] private GameObject UI;
    [SerializeField] private GameObject Loss;

    [SerializeField] private GameObject CameraPoint; 
    [SerializeField] private GameObject EndCameraPoint;

    [SerializeField] private GameObject MainUI;

    [SerializeField] private GameObject starterSelectionUI;
    [SerializeField] private GameObject itemSelectionUI;
    [SerializeField] private GameObject roomSelectionUI;
    [SerializeField] private GameObject terrainSelectionUI;

    private bool lost = false;

    private int currentIndex = -1;

    private int turnNumber = 1;

    /// <summary>
    /// The current turn
    /// </summary>
    public int TurnNumber { get => turnNumber; set => turnNumber = value; }

    /// <summary>
    /// Sets up turn order
    /// </summary>
    private void Start()
    {
        HideMostUI();
        RoomManager.Instance.GenerateFloor();

        Loss.SetActive(false);
        MainUI.SetActive(true);

        turnOrder.Add(GameObject.FindGameObjectWithTag("Player"));

        foreach(GameObject enemy in GameObject.FindGameObjectsWithTag("Enemy"))
        {
            turnOrder.Add(enemy);
        }

        GameObject.FindGameObjectWithTag("ItemHolder").GetComponent<ItemHolder>().UpdateUI();

        StartCoroutine(nameof(StartRoom));
    }

    /// <summary>
    /// Moves camera down and other beginning of turn stuff
    /// </summary>
    /// <returns></returns>
    private IEnumerator StartRoom()
    {
        yield return new WaitForEndOfFrame();
        CameraPoint.SetActive(true);

        EndTurn();
    }

    /// <summary>
    /// Ends the turn
    /// </summary>
    public void EndTurn()
    {
        if (lost)
        {
            return;
        }
        currentIndex++;

        if(currentIndex >= turnOrder.Count)
        {
            currentIndex = 0;
            turnNumber++;
        }

        if(currentIndex < 0)
        {
            currentIndex = 0;
        }

        //Remove dead enemies from turn order
        while (turnOrder[currentIndex].gameObject == null || (!turnOrder[currentIndex].gameObject.CompareTag("Player")
            && turnOrder[currentIndex].GetComponent<EnemyBase>().Dying))
        {
            turnOrder.RemoveAt(currentIndex);
            if(currentIndex >= turnOrder.Count)
            {
                currentIndex = 0;
            }
        }

        //Win the game if the player kills all enemies
        if(turnOrder.Count == 1)
        {
            //WinScreen.SetActive(true);
            //UI.SetActive(false);
            MainUI.SetActive(false);
            EndCameraPoint.SetActive(true);
            CameraPoint.SetActive(false);
            NextAction();
            return;
        }

        //Player turn
        if (turnOrder[currentIndex].CompareTag("Player"))
        {
            turnOrder[currentIndex].GetComponent<PlayerBase>().OnStartTurn();
            UI.SetActive(true);
            return;
        }

        if (turnOrder[currentIndex].CompareTag("Enemy"))
        {
            turnOrder[currentIndex].GetComponent<EnemyBase>().OnStartTurn();
            UI.SetActive(false);
            return;
        }
    }

    /// <summary>
    /// Clears all win screen
    /// </summary>
    private void HideMostUI()
    {
        starterSelectionUI.SetActive(false);
        roomSelectionUI.SetActive(false);
        itemSelectionUI.SetActive(false);
        terrainSelectionUI.SetActive(false);
    }

    /// <summary>
    /// Sets up the next action
    /// </summary>
    private void NextAction()
    {
        if (!Stats.DoneTutorial)
        {
            Stats.DoneTutorial = true;
        }
       switch(RoomManager.Instance.Reward)
       {
            case ("Starter"):
                starterSelectionUI.SetActive(true);

                List<Item> startingItems = Item.StartingItems();

                foreach (GameObject gameObject in GameObject.FindGameObjectsWithTag("StarterButton"))
                {
                    gameObject.GetComponent<ItemButton>().SetItem(startingItems[0]);
                    //Lazy way of doing this honestly. Running a proper for loop would be better
                    if (startingItems.Count > 1)
                    {
                        startingItems.RemoveAt(0);
                    }
                }
                break;

            case ("Normal Selection"):
                itemSelectionUI.SetActive(true);

                Item[] items = new Item[2];
                items[0] = Item.SpawnItem(1, false);

                //No dupes
                do
                {
                    items[1] = Item.SpawnItem(1, false);
                } while (items[1].ItemName == items[0].ItemName);

                foreach (GameObject button in GameObject.FindGameObjectsWithTag("SelectionButton"))
                {
                    button.GetComponent<ItemButton>().SetItem(items[items[0] == null ? 1 : 0]);
                    items[0] = null;
                }
                break;

            case ("Terrain Selection"):
                terrainSelectionUI.SetActive(true);

                List<string> tiles = new();
                List<string> allTiles = new();
                allTiles.AddRange(Attatchment.AttatchmentDescriptions.Keys);

                while (tiles.Count < 2)
                {
                    string tile = allTiles[Random.Range(0, allTiles.Count)];
                    allTiles.Remove(tile);

                    //if (Stats.HeldTiles.Contains(tile))
                    //{
                    //    continue;
                    //}

                    tiles.Add(tile);
                }

                foreach (GameObject button in GameObject.FindGameObjectsWithTag("SelectionButton"))
                {
                    button.GetComponent<ItemButton>().SetTile(tiles[0]);
                    tiles.RemoveAt(0);
                }

                int index = 0;

                foreach(GameObject button in GameObject.FindGameObjectsWithTag("InventoryTag"))
                {
                    button.GetComponent<ItemButton>().SetTile(Stats.HeldTiles[index]);
                    index++;
                }

                break;
            case "Win":
                SceneManager.LoadScene(2); //Send them to the credits screen after beating the king and queen
                break;
        }
    }

    /// <summary>
    /// Sets up next floor
    /// </summary>
    public void SetupNextFloor()
    {
        HideMostUI();
        RoomManager.Floor++;

        roomSelectionUI.SetActive(true);

        foreach(GameObject button in GameObject.FindGameObjectsWithTag("SelectionButton"))
        {
            List<char> floor = RoomManager.Instance.SelectFloor(out string reward);

            button.GetComponent<FloorButton>().SetFloor(reward, floor);
        }
    }

    /// <summary>
    /// Goes to next floor
    /// </summary>
    public void NextFloor()
    {
        SceneManager.LoadScene(1);
    }

    /// <summary>
    /// Ends the game in a loss
    /// </summary>
    public void EndGame()
    {
        RoomManager.Floor = 0;
        UI.SetActive(false);
        Loss.SetActive(true);
        lost = true;
    }
}
