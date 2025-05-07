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
    private float reactionError;
    private float errorMultiplier;

    // Walls for raycast
    public int wallLayer;

    // For updating the error each bounce
    private Vector2 lastBallVelocity = Vector2.zero;
    private float currentError;
    private float currentReactionError;

    // Reaction time timer
    private float reactionTimer = 0f;
    private bool waitingForReaction = false;


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

        // Enemy stats can be changed easily in gameManager, I could also use this to implement different difficulty levels, but currently that isn't planned
        movespeed = gameManager.enemyMoveSpeed;
        errorMargin = gameManager.enemyErrorMargin;
        reactionError = gameManager.enemyReactionError;
        errorMultiplier = gameManager.enemyErrorMultiplier;


    }

    // Fixed update to make movement smooth
    private void FixedUpdate()
    {
        Vector2 ballDirection = ball.linearVelocity.normalized;
        float ballSpeed = ball.linearVelocity.magnitude;

        if (ball.linearVelocityX < 0)
        {
            // If the ball is heading away from the enemy, reset to the middle
            float middleY = Mathf.MoveTowards(_rb.position.y, 0, movespeed * Time.fixedDeltaTime);
            _rb.MovePosition(new Vector2(_rb.position.x, middleY));
            return; // Enemy doesn't need to do anything, so skip the rest
        }

        // Only change the error if the ball velocity changes a lot, otherwise it has jitters
        if ((ballDirection.magnitude != 0) && Vector2.Dot(ballDirection, lastBallVelocity) < 0.99f)
        {
            // Get the current error, the faster the ball is going the wider the margin of error for the enemy, this simulates humans reacting worse to quicker balls in an easy way
            currentError = UnityEngine.Random.Range(-errorMargin - (ballSpeed * errorMultiplier), errorMargin + (ballSpeed * errorMultiplier));
            currentReactionError = UnityEngine.Random.Range(0.08f, reactionError);
            lastBallVelocity = ballDirection;

            // Start the reacion delay
            reactionTimer = 0f;
            waitingForReaction = true;
        }

        // If we should keep waiting, end this fixed update, otherwise go ahead
        if (waitingForReaction)
        {
            reactionTimer += Time.fixedDeltaTime;
            if (reactionTimer < currentReactionError)
            {
                return;
            }
            else
            {
                waitingForReaction = false; // Movement is allowed
            }
        }

        // If you'd like to see the raycast, uncomment this, turn gizmos on while in Game mode and you will see the ray
        // Debug.DrawRay(ball.position, ball.linearVelocity.normalized * 100f, Color.red);


        // Make a ray using the ball, head to that rays location, depending on difficulty the margin of error will change
        // origin, direction, distance to check, layer to detect colliders on
        // origin being the ball, direction is where it travels, distance is somewhat abitrary
        RaycastHit2D hit = Physics2D.Raycast(ball.position, ball.linearVelocity.normalized, 2000f, LayerMask.GetMask("RayWalls"));

        // If we hit a collider, go to the hit location
        if (hit.collider != null)
        {
            targetPosition = hit.point.y + currentError;

            // Only allow the paddle to go between -4.5 and 4.5 on the y axis
            float clampedTargetY = Mathf.Clamp(targetPosition, -4.5f, 4.5f);

            // Move towards for a nice clean movement
            // Delay the time until the move
            float newY = Mathf.MoveTowards(_rb.position.y, clampedTargetY, movespeed * Time.fixedDeltaTime);
            _rb.MovePosition(new Vector2(_rb.position.x, newY));

        }
    }
}
