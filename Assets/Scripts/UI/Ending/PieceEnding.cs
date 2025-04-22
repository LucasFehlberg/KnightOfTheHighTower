/*****************************************************************************
// File Name : PieceEnding.cs
// Author : Lucas Fehlberg
// Creation Date : April 17, 2025
// Last Updated : April 18, 2025
//
// Brief Description : The piece falls forever
*****************************************************************************/

using UnityEngine;

public class PieceEnding : MonoBehaviour
{
    private bool fall = false;

    [SerializeField] private Vector3 rotationVelo;
    [SerializeField] private Vector3 velocityVelo;

    public bool Fall { get => fall; set => fall = value; }

    /// <summary>
    /// Rotates the piece with angular velocity, akin to funny death
    /// </summary>
    private void Update()
    {
        GetComponent<Rigidbody>().angularVelocity = rotationVelo;

        if (fall)
        {
            GetComponent<Rigidbody>().velocity = velocityVelo;
        }

        //We don't want to hit any limits and crash unity
        if(transform.position.y < -15)
        {
            Destroy(gameObject);
        }
    }

    /// <summary>
    /// Unfreezes position so they fall
    /// </summary>
    public void SetFree()
    {
        GetComponent<Rigidbody>().velocity = Vector3.zero;
        GetComponent<Rigidbody>().constraints = RigidbodyConstraints.None;

        fall = true;
    }
}
