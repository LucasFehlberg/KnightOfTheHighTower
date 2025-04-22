/*****************************************************************************
// File Name : Credits.cs
// Author : Lucas Fehlberg
// Creation Date : April 18, 2025
// Last Updated : April 18, 2025
//
// Brief Description : Scroll the credits
*****************************************************************************/

using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Credits : MonoBehaviour
{
    [SerializeField] PieceEnding[] kingAndQueen = new PieceEnding[2];
    [SerializeField] private float creditSpeed = 1.0f;

    /// <summary>
    /// Invokes the repeating, and starts the scroll coroutine
    /// </summary>
    private void Start()
    {
        GetComponent<RectTransform>().anchoredPosition = new Vector2(0, -1080);
        InvokeRepeating(nameof(DropPiece), 5f, 4f);
        StartCoroutine(ScrollCredits());
    }

    /// <summary>
    /// Drops a random piece still in the pool
    /// </summary>
    private void DropPiece()
    {
        List<GameObject> pieces = GameObject.FindGameObjectsWithTag("Enemy").ToList<GameObject>();

        if(pieces.Count == 0)
        {
            foreach (var piece in kingAndQueen)
            {
                piece.SetFree();
            }
            CancelInvoke(nameof(DropPiece));
            return;
        }

        GameObject enemy = null;

        while(enemy == null)
        {
            if(pieces.Count == 0)
            {
                break;
            }

            enemy = pieces[Random.Range(0, pieces.Count)];

            if (enemy.GetComponent<PieceEnding>().Fall)
            {
                pieces.Remove(enemy);
                enemy = null;
                continue;
            }

            enemy.GetComponent<PieceEnding>().SetFree();
        }
    }

    /// <summary>
    /// Scrolls the credits
    /// </summary>
    /// <returns></returns>
    private IEnumerator ScrollCredits()
    {
        yield return new WaitForSecondsRealtime(5f);

        while(GetComponent<RectTransform>().anchoredPosition.y < -1080 + GetComponent<RectTransform>().sizeDelta.y)
        {
            GetComponent<RectTransform>().anchoredPosition += Vector2.up * creditSpeed;
            yield return null;
        }

        yield return new WaitForSecondsRealtime(5f);
        //Back to title
        SceneManager.LoadScene(0);
    }
}
