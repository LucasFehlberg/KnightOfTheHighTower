/*****************************************************************************
// File Name : EnemyToast.cs
// Author : Lucas Fehlberg
// Creation Date : April 16, 2025
// Last Updated : April 23, 2025
//
// Brief Description : Pop-Up for enemy information
*****************************************************************************/

using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class EnemyToast : MonoBehaviour
{
    [SerializeField] private GameObject toastBox;
    [SerializeField] private TMP_Text enemyName;
    [SerializeField] private TMP_Text enemyDescription;

    [SerializeField] private TMP_Text enemyHealth;
    [SerializeField] private TMP_Text enemyAttack;
    [SerializeField] private TMP_Text enemyMove;

    private List<GameObject> toastIndicators = new();

    [SerializeField] private GameObject indicatorPrefab;
    [SerializeField] private Material indicatorAttack;
    [SerializeField] private Material indicatorMovement;
    [SerializeField] private Material indicatorBoth;

    private bool active = false;

    private EnemyBase enemy;

    private LayerMask groundLayers;
    [SerializeField] private LayerMask obstacleLayers;
    private LayerMask enemyLayers;

    [SerializeField] private Canvas canvas;
    [SerializeField] private Sprite activeSprite;
    [SerializeField] private Sprite inactiveSprite;

    [SerializeField] private Image image;
    private RectTransform rectTransform;

    /// <summary>
    /// Calls a couroutine instead of doing an update for performance reasons
    /// Also set up various variables
    /// </summary>
    void Start()
    {
        rectTransform = GetComponent<RectTransform>();

        enemyLayers = 1 << LayerMask.NameToLayer("Enemy");
        groundLayers = 1 << LayerMask.NameToLayer("Ground");


        StartCoroutine(ToastController());
    }

    /// <summary>
    /// Moves the box
    /// </summary>
    private void FixedUpdate()
    {
        RectTransformUtility.ScreenPointToLocalPointInRectangle(canvas.transform as RectTransform, Input.mousePosition,
            canvas.worldCamera, out Vector2 newTransform);
        rectTransform.anchoredPosition = newTransform;
        if (rectTransform.anchoredPosition.x > 350)
        {
            toastBox.GetComponent<RectTransform>().anchoredPosition = new Vector3(-190, -10);
        }
        else
        {
            toastBox.GetComponent<RectTransform>().anchoredPosition = new Vector3(10, -10);
        }
    }

    /// <summary>
    /// Works similarly to FixedUpdate but slower so that way we're not running physics every 1/60 seconds
    /// </summary>
    /// <returns></returns>
    private IEnumerator ToastController()
    {
        while (true)
        {
            while (!active)
            {
                if (toastBox.activeSelf)
                {
                    toastBox.SetActive(false);
                    enemy = null;
                    KillIndicators();
                }
                yield return new WaitForFixedUpdate();
            }

            //First, raycast a ray to see if we hit an enemy
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            if(Physics.Raycast(ray, out RaycastHit hit, Mathf.Infinity, enemyLayers))
            {
                //Optimization, don't run if we're already touching an enemy and it's the same enemy
                if (toastBox.activeSelf && hit.collider.GetComponent<EnemyBase>() == enemy)
                {
                    yield return new WaitForFixedUpdate();
                    continue;
                }

                //Incase this is a new enemy
                KillIndicators();

                enemy = hit.collider.GetComponent<EnemyBase>();

                enemyName.text = enemy.Info.EnemyName;
                enemyDescription.text = enemy.Info.EnemyDescription;

                enemyHealth.text = enemy.HealthRemaining.ToString();
                enemyAttack.text = enemy.Attack.ToString();
                enemyMove.text = enemy.Movement.ToString();

                toastBox.SetActive(true);
                SetupIndicators();
            } 
            else
            {
                //Do nothing unless we have stuff active
                if (toastBox.activeSelf)
                {
                    toastBox.SetActive(false);
                    enemy = null;
                    KillIndicators();
                }
            }

            yield return new WaitForFixedUpdate();
        }
    }

    /// <summary>
    /// Kills all indicators
    /// </summary>
    private void KillIndicators()
    {
        while (toastIndicators.Count > 0)
        {
            Destroy(toastIndicators[0]);
            toastIndicators.RemoveAt(0);
        }
    }

    /// <summary>
    /// Sets up indicators
    /// </summary>
    private void SetupIndicators()
    {
        //Movement first, then attack
        List<Vector3> positions = new();
        List<Vector3> cleanup = new();

        foreach (Vector2 pos in enemy.PossibleInfiniteMovements)
        {
            positions.AddRange(CheckDirection(enemy.transform.position, 
                pos, enemy.CanJumpHoles, enemy.CanJumpWalls, false));
        }

        foreach(Vector2 pos in enemy.PossibleNormalMovements)
        {
            Vector3 currentPos = enemy.transform.position + new Vector3(pos.x, 0, pos.y);

            if (!Physics.CheckBox(currentPos + Vector3.down, Vector3.one * 0.45f, Quaternion.identity, groundLayers))
            {
                continue;
            }

            //Next, we check for obstacles
            if (Physics.CheckBox(currentPos, Vector3.one * 0.45f, Quaternion.identity, obstacleLayers))
            {
                continue;
            }

            positions.Add(currentPos);
        }

        //Cleanup
        foreach (Vector3 pos in positions)
        {
            if (!cleanup.Contains(pos))
            {
                cleanup.Add(pos);
            }
        }

        foreach(Vector3 pos in cleanup)
        {
            GameObject ind = Instantiate(indicatorPrefab, pos, Quaternion.identity);
            toastIndicators.Add(ind);

            ind.transform.GetChild(0).GetComponent<MeshRenderer>().material = indicatorMovement;
        }

        //attack
        List<Vector3> attacks = new();
        cleanup.Clear();

        foreach (Vector2 pos in enemy.PossibleInfiniteAttacks)
        {
            attacks.AddRange(CheckDirection(enemy.transform.position,
                pos, enemy.CanJumpHoles, enemy.CanJumpWalls, true));
        }

        foreach (Vector2 pos in enemy.PossibleNormalAttacks)
        {
            Vector3 currentPos = enemy.transform.position + new Vector3(pos.x, 0, pos.y);

            if (!Physics.CheckBox(currentPos + Vector3.down, Vector3.one * 0.45f, Quaternion.identity, groundLayers))
            {
                continue;
            }

            //Next, we check for obstacles
            if (Physics.CheckBox(currentPos, Vector3.one * 0.45f, Quaternion.identity, obstacleLayers))
            {
                if (Physics.OverlapBox(currentPos,
                    Vector3.one * 0.45f, Quaternion.identity, obstacleLayers)[0].CompareTag("Player"))
                {
                    attacks.Add(currentPos);
                }
                continue;
            }

            attacks.Add(currentPos);
        }

        //Cleanup
        foreach (Vector3 pos in attacks)
        {
            if (!cleanup.Contains(pos))
            {
                cleanup.Add(pos);
            }
        }

        foreach (Vector3 pos in cleanup)
        {
            bool exists = false;
            foreach(GameObject ob in toastIndicators)
            {
                //Condense amount of gameobjects by checking to see if one exists in that position first
                if(ob.transform.position == pos)
                {
                    ob.transform.GetChild(0).GetComponent<MeshRenderer>().material = indicatorBoth;
                    exists = true;
                    break;
                }
            }
            if (exists)
            {
                continue;
            }

            GameObject ind = Instantiate(indicatorPrefab, pos, Quaternion.identity);
            toastIndicators.Add(ind);

            ind.transform.GetChild(0).GetComponent<MeshRenderer>().material = indicatorAttack;
        }

    }

    /// <summary>
    /// Checks the direction of enemy movement
    /// </summary>
    /// <param name="origin">Origin of the movement</param>
    /// <param name="direction">Direction of the movement</param>
    /// <returns>List of all possible infinite directions</returns>
    private List<Vector3> CheckDirection(Vector3 origin, Vector2 direction, bool canJumpHoles, bool canJumpWalls, bool 
        attack)
    {
        List<Vector3> list = new();
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
                if (attack && Physics.OverlapBox(currentPos, 
                    Vector3.one * 0.45f, Quaternion.identity, obstacleLayers)[0].CompareTag("Player"))
                {
                    list.Add(currentPos);
                }
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

            list.Add(currentPos);
        }

        return list;
    }

    public void SwitchButtonSprite()
    {
        active = !active;

        if (active)
        {
            image.sprite = activeSprite;
        } 
        else
        {
            image.sprite = inactiveSprite;
        }
    }
}
