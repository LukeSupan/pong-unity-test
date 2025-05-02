using System;
using UnityEngine;
using UnityEngine.EventSystems;

public class EnemyMovement : MonoBehaviour
{
    private GameManager gameManager;
    public Rigidbody2D ball; // Enemy needs to see where ball is

    // Speed and vectors
    private Rigidbody2D _rb; // Rigidbody2D, we apply velocity to this to move
    private float movespeed; // Movespeed, changed in gameManager
    public Vector2 moveDirection = Vector2.zero;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        // If we are playing LocalMultiplayer we don't need this script
        if (GameSettings.CurrentMode == GameSettings.GameMode.LocalMultiplayer)
        {
            this.enabled = false;
        } else
        {
            this.enabled = true;
        }

        gameManager = FindFirstObjectByType<GameManager>();
        _rb = GetComponent<Rigidbody2D>();

        // Enemy movespeed changes based on difficulty, behavior remains the same but appears different
        movespeed = gameManager.enemyMoveSpeed;
    }

    // Fixed update to make movement smooth
    private void FixedUpdate()
    {
        // Get the moveDirection by seeing if the ball is above or below
        // If ball is above, go up until it reaches ball, then wait within a margin of error
        // If ball is below, go down until it reaches ball, then wait within a margin of error

        if (ball.position.y > (_rb.position.y) + 0.01)
        {
            moveDirection.y = 1; // Ball is above, go up
        } else if (ball.position.y < (_rb.position.y) - 0.01)
        {
            moveDirection.y = -1; // Ball is below, go down
        } else
        {
            moveDirection.y = 0; // We are here, don't move
        }


        // Apply the y movement, if at top of screen and trying to go up, don't, if at bottom and going down, also don't
        if (_rb.position.y >= 4.5 && moveDirection.y > 0)
        {
            _rb.linearVelocityY = 0;
        }
        else if (_rb.position.y <= -4.5 && moveDirection.y < 0)
        {
            _rb.linearVelocityY = 0;
        }
        else
        {
            _rb.linearVelocityY = moveDirection.y * movespeed;
        }
    }
}
