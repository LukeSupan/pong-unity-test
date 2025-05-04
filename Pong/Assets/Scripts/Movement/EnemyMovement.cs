using System;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class EnemyMovement : MonoBehaviour
{
    private GameManager gameManager;
    public Rigidbody2D ball; // Enemy needs to see where ball is

    public float targetPosition;

    // Speed and vectors
    private Rigidbody2D _rb; // Rigidbody2D, we apply velocity to this to move
    private float movespeed; // Movespeed, changed in gameManager
    public Vector2 moveDirection = Vector2.zero;
    private float errorMargin;

    // Walls for raycast
    public int wallLayer;

    // For updating the error each bounce
    private Vector2 lastBallVelocity = Vector2.zero;
    private float currentError = 0f;

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
        errorMargin = gameManager.enemyErrorMargin;
        
    }

    // Fixed update to make movement smooth
    private void FixedUpdate()
    {
        Vector2 ballDirection = ball.linearVelocity.normalized;

        // Only change the error if the ball velocity changes a lot, otherwise it has jitters
        if (Vector2.Dot(ballDirection, lastBallVelocity) < 0.99f)
        {
            currentError = UnityEngine.Random.Range(-errorMargin, errorMargin);
            lastBallVelocity = ballDirection;
        }


        // Make a ray using the ball, head to that rays location, depending on difficulty the margin of error will change
        // origin, direction, distance to check, layer to detect colliders on
        // origin being the ball, direction is where it travels, distance is somewhat abitrary

        Debug.DrawRay(ball.position, ball.linearVelocity.normalized * 100f, Color.red);

        // RaycastHit2D hit = Physics2D.GetRayIntersection(new Ray(ball.position, ball.linearVelocity.normalized), 1000f, wallLayer);
        RaycastHit2D hit = Physics2D.Raycast(ball.position, ball.linearVelocity.normalized, 2000f, LayerMask.GetMask("RayWalls"));

        // If we hit a collider, go to the hit location
        if (hit.collider != null)
        {
            Debug.Log("Ray hit: " + hit.collider.name);

            targetPosition = hit.point.y + currentError;


            float clampedTargetY = Mathf.Clamp(targetPosition, -4.5f, 4.5f);
            float newY = Mathf.MoveTowards(_rb.position.y, clampedTargetY, movespeed * Time.fixedDeltaTime);
            _rb.MovePosition(new Vector2(_rb.position.x, newY));

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
