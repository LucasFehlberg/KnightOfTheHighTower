/*****************************************************************************
// File Name : Ball.cs
// Author : Lucas Fehlberg
// Creation Date : May 18, 2025
// Last Updated : May 18, 2025
//
// Brief Description : Ball moves in a direction until it hits an enemy
*****************************************************************************/

using System.Collections;
using UnityEngine;

public class Ball : EnemyBase
{
    private Tile tileOn;
    private Rigidbody rb;
    private Vector3 originalPosition;

    private bool isBallRolling = false;

    private bool isGrounded = true;

    private Vector3 velo = Vector3.zero;

    public bool IsBallRolling { get => isBallRolling; set => isBallRolling = value; }

    /// <summary>
    /// Get the tile this ball is on
    /// </summary>
    private void Start()
    {
        tileOn = transform.parent.parent.GetComponent<Tile>();
        rb = GetComponent<Rigidbody>();
    }

    /// <summary>
    /// Roll the ball onHit
    /// </summary>
    /// <param name="damage"></param>
    /// <param name="cameFrom"></param>
    /// <param name="fall"></param>
    public override void TakeDamage(int damage, Vector3 cameFrom, bool fall = false)
    {
        StartCoroutine(StartRolling(damage, cameFrom));
    }

    private IEnumerator StartRolling(int damage, Vector3 cameFrom)
    {
        yield return new WaitForEndOfFrame();
        IsBallRolling = true;

        originalPosition = transform.position;

        gameObject.layer = LayerMask.NameToLayer("Default");
        tileOn.BuiltUpon = false;
        if (cameFrom == Vector3.zero)
        {
            Destroy(gameObject);
        }

        Vector2 position = new(transform.position.x, transform.position.z);
        Vector2 cameFromPos = new(cameFrom.x, cameFrom.z);

        velo = new Vector3(position.x - cameFromPos.x, 0, position.y - cameFromPos.y).normalized * 4;

        if (velo == Vector3.zero)
        {
            Destroy(gameObject);
        }

        rb.isKinematic = false;
        rb.useGravity = true;
        rb.velocity = velo;
    }

    /// <summary>
    /// Allows it to roll at a consistent speed
    /// </summary>
    private void Update()
    {
        if (rb.velocity.x != velo.x || rb.velocity.z != velo.z)
        {
            rb.velocity = new(velo.x, rb.velocity.y, velo.z);
        }

        if(transform.position.y < -50)
        {
            Destroy(gameObject);
        }
    }

    /// <summary>
    /// Apply physics
    /// </summary>
    private void FixedUpdate()
    {
        if(isGrounded && !Physics.CheckBox(new Vector3(transform.position.x, 0, transform.position.z), 
            new Vector3(0.2f, 0.1f, 0.2f), Quaternion.identity,
            groundLayers))
        {
            isGrounded = false;
            isBallRolling = false;
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawCube(new Vector3(transform.position.x, 0, transform.position.z), new Vector3(0.2f, 0.1f, 0.2f));
    }

    private void OnCollisionEnter(Collision collision)
    {
        if(velo.magnitude <= 0f)
        {
            return;
        }
        if(!isGrounded)
        {
            Destroy(gameObject);
        }

        if (collision.collider.CompareTag("Wall"))
        {
            Destroy(gameObject);
        }

        if (collision.collider.CompareTag("Player"))
        {
            collision.gameObject.GetComponent<PlayerBase>().TakeDamage(2);
            Destroy(gameObject);
        }

        if (collision.collider.CompareTag("Enemy") || collision.collider.CompareTag("Ball"))
        {
            collision.gameObject.GetComponent<EnemyBase>().TakeDamage(2, originalPosition);
            Destroy(gameObject);
        }
    }
}
