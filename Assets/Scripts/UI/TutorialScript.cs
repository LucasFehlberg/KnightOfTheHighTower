/*****************************************************************************
// File Name : TutorialScript.cs
// Author : Lucas Fehlberg
// Creation Date : April 29, 2025
// Last Updated : May 1, 2025
//
// Brief Description : Tutorial Script
*****************************************************************************/

using System.Collections;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class TutorialScript : MonoBehaviour
{
    public static TutorialScript instance;
    
    const int FADE_TIMER = 60;
    //Buttons
    [SerializeField] private GameObject terrainButton;
    [SerializeField] private GameObject attackButton;
    [SerializeField] private GameObject movementButton;
    [SerializeField] private GameObject endTurnButton;
    [SerializeField] private GameObject inspectButton;
    [SerializeField] private GameObject addTerrainButton;

    //Player stuff
    [SerializeField] private PlayerInput pInput;
    [SerializeField] private PlayerTerrain pTerrain;
    [SerializeField] private PlayerMovement pMovement;
    [SerializeField] private PlayerAttack pAttack;
    [SerializeField] private PlayerBase pBase;

    //Tutorial Glyphs
    [SerializeField] private GameObject cameraControl;
    [SerializeField] private RectTransform arrowRectTransform;

    [SerializeField] private GameObject tile1;
    [SerializeField] private GameObject tile2;
    [SerializeField] private GameObject tile3;

    [SerializeField] private CanvasScaler scaler;

    private bool[] doneCamera = new bool[4] {false, false, false, false};

    private InputAction cameraControlsLR;
    private InputAction cameraControlsUD;

    private int tutorialState = 0;

    private Vector2 originalArrowPosition;

    public int TutorialState { get => tutorialState; set => tutorialState = value; }
    public GameObject Tile1 { get => tile1; set => tile1 = value; }
    public GameObject Tile2 { get => tile2; set => tile2 = value; }

    /// <summary>
    /// Destroys this script if the tutorial is done
    /// </summary>
    private void Start()
    {
        if (Stats.DoneTutorial)
        {
            Destroy(gameObject);
            return;
        }

        instance = this;

        terrainButton.SetActive(false);
        attackButton.SetActive(false);
        movementButton.SetActive(false);
        endTurnButton.SetActive(false);
        inspectButton.SetActive(false);
        cameraControl.SetActive(false);
        arrowRectTransform.anchoredPosition = Vector2.zero;
        arrowRectTransform.gameObject.SetActive(false);

        StartCoroutine(nameof(Tutorial));

        cameraControlsUD = pInput.currentActionMap.FindAction("CameraUpDown");
        cameraControlsLR = pInput.currentActionMap.FindAction("CameraLeftRight");

        cameraControlsLR.started += CameraControlsLR_started;
        cameraControlsUD.started += CameraControlsUD_started;
    }

    private void CameraControlsUD_started(InputAction.CallbackContext obj)
    {
        if (obj.ReadValue<float>() > 0 && !doneCamera[0])
        {
            doneCamera[0] = true;
            cameraControl.transform.GetChild(0).GetComponent<TMP_Text>().color = Color.green;
        }

        if (obj.ReadValue<float>() < 0 && !doneCamera[1])
        {
            doneCamera[1] = true;
            cameraControl.transform.GetChild(2).GetComponent<TMP_Text>().color = Color.green;
        }
    }

    /// <summary>
    /// Does tutorial stuff
    /// </summary>
    /// <param name="obj"></param>
    private void CameraControlsLR_started(InputAction.CallbackContext obj)
    {
        if(obj.ReadValue<float>() < 0 && !doneCamera[2])
        {
            doneCamera[2] = true;
            cameraControl.transform.GetChild(3).GetComponent<TMP_Text>().color = Color.green;
        }

        if (obj.ReadValue<float>() > 0 && !doneCamera[3])
        {
            doneCamera[3] = true;
            cameraControl.transform.GetChild(1).GetComponent<TMP_Text>().color = Color.green;
        }
    }

    /// <summary>
    /// No Unity Lag
    /// </summary>
    private void OnDisable()
    {
        if (!Stats.DoneTutorial)
        {
            cameraControlsLR.started -= CameraControlsLR_started;
            cameraControlsUD.started -= CameraControlsUD_started;
        }
    }

    /// <summary>
    /// Runs the entire tutorial
    /// </summary>
    /// <returns></returns>
    private IEnumerator Tutorial()
    {
        Coroutine arrowCoroutine = null;
        yield return new WaitForSeconds(1f);

        cameraControl.SetActive(true);
        //Enter camera
        for (int i = 0; i < FADE_TIMER; i++)
        {
            cameraControl.GetComponent<Image>().color = new Color(1, 1, 1, (float)(i + 1f)/(float)FADE_TIMER);
            foreach(Transform text in cameraControl.transform.GetComponentInChildren<Transform>())
            {
                Color textColor = text.GetComponent<TMP_Text>().color;
                text.GetComponent<TMP_Text>().color = new Color(textColor.r, textColor.g, textColor.b, 
                    cameraControl.GetComponent<Image>().color.a);
            }
            yield return new WaitForEndOfFrame();
        }

        bool cameraDone = false;
        while (!cameraDone)
        {
            cameraDone = true;
            foreach (bool cameraDirection in doneCamera)
            {
                if (!cameraDirection)
                {
                    cameraDone = false;
                    break;
                }
            }

            yield return null;
        }

        //Exit camera
        for (int i = FADE_TIMER; i > 0; i--)
        {
            cameraControl.GetComponent<Image>().color = new Color(1, 1, 1, (float)(i + 1f) / (float)FADE_TIMER);
            foreach (Transform text in cameraControl.transform.GetComponentInChildren<Transform>())
            {
                Color textColor = text.GetComponent<TMP_Text>().color;
                text.GetComponent<TMP_Text>().color = new Color(textColor.r, textColor.g, textColor.b,
                    cameraControl.GetComponent<Image>().color.a);
            }
            yield return new WaitForEndOfFrame();
        }

        cameraControl.SetActive(false);

        yield return new WaitForSeconds(1);
        tutorialState++;
        //Terrain
        terrainButton.GetComponent<RectTransform>().anchoredPosition = new Vector2(150, -100);
        terrainButton.SetActive(true);

        while (terrainButton.GetComponent<RectTransform>().anchoredPosition.y < -15)
        {
            terrainButton.GetComponent<RectTransform>().anchoredPosition = Vector2.MoveTowards
                (terrainButton.GetComponent<RectTransform>().anchoredPosition, new Vector2(150, -15), 1f);
            yield return new WaitForEndOfFrame();
        }

        arrowRectTransform.gameObject.SetActive(true);

        terrainButton.GetComponent<RectTransform>().anchoredPosition = new Vector2(150, -15);

        arrowCoroutine = StartCoroutine(MoveArrow(terrainButton.GetComponent<RectTransform>().position +
            Vector3.up * 50f, Quaternion.identity, 2));

        //Move the arrow seperately

        while (!pTerrain.isActiveAndEnabled)
        {
            yield return null;
        }
        StopCoroutine(arrowCoroutine);

        tutorialState++;

        arrowCoroutine = StartCoroutine(FollowPoint(tile1.transform.position + (Vector3.up * 2f), 3));

        while(tutorialState != 3)
        {
            yield return null;
        }

        arrowRectTransform.gameObject.SetActive(false);
        StopCoroutine(arrowCoroutine);

        yield return new WaitForSeconds(1);

        //Movement
        movementButton.GetComponent<RectTransform>().anchoredPosition = new Vector2(-150, -100);
        movementButton.SetActive(true);

        while (movementButton.GetComponent<RectTransform>().anchoredPosition.y < -15)
        {
            movementButton.GetComponent<RectTransform>().anchoredPosition = Vector2.MoveTowards
                (movementButton.GetComponent<RectTransform>().anchoredPosition, new Vector2(-150, -15), 1f);
            yield return new WaitForEndOfFrame();
        }

        movementButton.GetComponent<RectTransform>().anchoredPosition = new Vector2(-150, -15);

        arrowRectTransform.gameObject.SetActive(true);
        arrowCoroutine = StartCoroutine(MoveArrow(movementButton.GetComponent<RectTransform>().position +
            Vector3.up * 50f, Quaternion.identity, 4));

        while (!pMovement.isActiveAndEnabled)
        {
            yield return null;
        }
        StopCoroutine(arrowCoroutine);

        tutorialState++;
        arrowCoroutine = StartCoroutine(FollowPoint(tile1.transform.position + (Vector3.up * 2f), 5));

        while (tutorialState != 5)
        {
            yield return null;
        }

        StopCoroutine(arrowCoroutine);
        arrowRectTransform.gameObject.SetActive(false);

        //End turn
        yield return new WaitForSeconds(1);

        endTurnButton.GetComponent<RectTransform>().anchoredPosition = new Vector2(480, -100);
        endTurnButton.SetActive(true);

        while (endTurnButton.GetComponent<RectTransform>().anchoredPosition.y < 0)
        {
            endTurnButton.GetComponent<RectTransform>().anchoredPosition = Vector2.MoveTowards
                (endTurnButton.GetComponent<RectTransform>().anchoredPosition, new Vector2(480, 0), 1f);
            yield return new WaitForEndOfFrame();
        }

        endTurnButton.GetComponent<RectTransform>().anchoredPosition = new Vector2(480, 0);

        arrowRectTransform.gameObject.SetActive(true);
        arrowCoroutine = StartCoroutine(MoveArrow(endTurnButton.GetComponent<RectTransform>().position +
            Vector3.up * 75f, Quaternion.identity, 6));

        while (tutorialState != 6)
        {
            yield return null;
        }

        //Terrain Again
        arrowCoroutine = StartCoroutine(MoveArrow(terrainButton.GetComponent<RectTransform>().position +
            Vector3.up * 50f, Quaternion.identity, 7));

        while (!pTerrain.isActiveAndEnabled)
        {
            yield return null;
        }

        StopCoroutine(arrowCoroutine);
        arrowCoroutine = StartCoroutine(MoveArrow(addTerrainButton.GetComponent<RectTransform>().position +
            Vector3.left * 75f, Quaternion.Euler(0, 0, 90), 7));

        while (tutorialState != 7)
        {
            yield return null;
        }

        arrowCoroutine = StartCoroutine(FollowPoint(tile2.transform.position + (Vector3.up * 2f), 8));

        while (tutorialState != 8)
        {
            yield return null;
        }

        arrowCoroutine = StartCoroutine(MoveArrow(movementButton.GetComponent<RectTransform>().position +
            Vector3.up * 50f, Quaternion.identity, 9));

        while (!pMovement.isActiveAndEnabled)
        {
            yield return null;
        }

        StopCoroutine(arrowCoroutine);
        arrowCoroutine = StartCoroutine(FollowPoint(tile2.transform.position + (Vector3.up * 2f), 9));

        while (tutorialState != 9)
        {
            yield return null;
        }

        arrowRectTransform.gameObject.SetActive(false);

        //Inspection
        yield return new WaitForSeconds(1);

        inspectButton.GetComponent<RectTransform>().anchoredPosition = new Vector2(50, -100);
        inspectButton.SetActive(true);

        while (inspectButton.GetComponent<RectTransform>().anchoredPosition.y < 50)
        {
            inspectButton.GetComponent<RectTransform>().anchoredPosition = Vector2.MoveTowards
                (inspectButton.GetComponent<RectTransform>().anchoredPosition, new Vector2(50, 50), 1f);
            yield return new WaitForEndOfFrame();
        }

        inspectButton.GetComponent<RectTransform>().anchoredPosition = new Vector2(50, 50);

        arrowRectTransform.gameObject.SetActive(true);

        arrowCoroutine = StartCoroutine(MoveArrow(inspectButton.GetComponent<RectTransform>().position +
            Vector3.up * 75f, Quaternion.identity, 10));

        while (tutorialState != 10)
        {
            yield return null;
        }

        arrowCoroutine = StartCoroutine(FollowPoint(tile3.transform.position + (Vector3.up * 2f), 11));

        while (tutorialState != 11)
        {
            yield return null;
        }

        arrowRectTransform.gameObject.SetActive(false);

        yield return new WaitForSeconds(1);

        //Attack
        attackButton.GetComponent<RectTransform>().anchoredPosition = new Vector2(0, -100);
        attackButton.SetActive(true);

        while (attackButton.GetComponent<RectTransform>().anchoredPosition.y < -15)
        {
            attackButton.GetComponent<RectTransform>().anchoredPosition = Vector2.MoveTowards
                (attackButton.GetComponent<RectTransform>().anchoredPosition, new Vector2(0, -15), 1f);
            yield return new WaitForEndOfFrame();
        }

        attackButton.GetComponent<RectTransform>().anchoredPosition = new Vector2(0, -15);

        arrowRectTransform.gameObject.SetActive(true);

        arrowCoroutine = StartCoroutine(MoveArrow(attackButton.GetComponent<RectTransform>().position +
            Vector3.up * 75f, Quaternion.identity, 12));

        while (!pAttack.isActiveAndEnabled)
        {
            yield return null;
        }

        StopCoroutine(arrowCoroutine);
        arrowCoroutine = StartCoroutine(FollowPoint(tile3.transform.position + (Vector3.up * 2f), 12));

        while (GameObject.FindGameObjectsWithTag("Enemy").Length > 0)
        {
            yield return null;
        }

        arrowRectTransform.gameObject.SetActive(false);
    }

    /// <summary>
    /// Moves the tutorial arrow
    /// </summary>
    /// <param name="endPosition"></param>
    /// <param name="endRotation"></param>
    /// <returns></returns>
    private IEnumerator MoveArrow(Vector2 endPosition, Quaternion endRotation, int condition)
    {
        float time = 0f;
        Vector2 originalPos = arrowRectTransform.position;
        Quaternion originalRot = arrowRectTransform.rotation;
        while (tutorialState < condition)
        {
            arrowRectTransform.position = Vector2.Lerp(originalPos, endPosition / scaler.scaleFactor, time);
            arrowRectTransform.rotation = Quaternion.Lerp(originalRot, endRotation, time);
            time += 0.1f;
            if(time > 1f)
            {
                time = 1f;
            }
            yield return new WaitForFixedUpdate();
        }
    }

    /// <summary>
    /// Makes the arrow follow a world point
    /// </summary>
    /// <param name="worldPosition"></param>
    /// <param name="condition"></param>
    /// <returns></returns>
    private IEnumerator FollowPoint(Vector3 worldPosition, int condition)
    {
        float time = 0f;
        Vector2 originalPos = arrowRectTransform.position;
        Quaternion originalRot = arrowRectTransform.rotation;
        while(tutorialState < condition)
        {
            Vector3 point = Camera.main.WorldToScreenPoint(worldPosition) / scaler.scaleFactor;
            
            arrowRectTransform.position = Vector2.Lerp(originalPos, point, time);
            arrowRectTransform.rotation = Quaternion.Lerp(originalRot, Quaternion.identity, time);


            time += 0.1f;
            if(time > 1f)
            {
                time = 1f;
            }

            yield return new WaitForFixedUpdate();
        }
    }
}
