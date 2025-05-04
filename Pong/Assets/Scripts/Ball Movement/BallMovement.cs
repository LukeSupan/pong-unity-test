using System;
using System.Collections;
using UnityEditor.Analytics;
using UnityEngine;

// Makes the ball bounce off of walls and paddles, gameManager calls to reset ball, alerts gameManager of score

public class BallMovement : MonoBehaviour
{
    private GameManager gameManager; // Reference for gameManager to do everything

    // Variables
    private Rigidbody2D _rb;
    private float angle;
    private bool goRight;
    private Vector2 direction;
    private float speed;

    // Game manager variables
    private float originalSpeed;
    private float speedMult;
    private float delay;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        _rb = GetComponent<Rigidbody2D>();
        gameManager = FindFirstObjectByType<GameManager>();

        // At the start, launch the ball in a random direction, within 45 degrees left or 45 degrees right
        goRight = UnityEngine.Random.value > 0.5f; // Randomly decide if going left or right

        originalSpeed = gameManager.ballOriginalSpeed;
        speedMult = gameManager.ballSpeedMult;
        delay = gameManager.ballDelay;

        // Speed needs to equal original speed at the start
        speed = originalSpeed;

        StartCoroutine(LaunchBall(delay));
    }



    // Coroutine that waits for the given time (in seconds) before launching the ball
    private IEnumerator LaunchBall(float delay)
    {
        // Wait for the specified delay time
        yield return new WaitForSeconds(delay);

        // If goRight, then shoot the ball in that angle, otherwise 
        if (goRight)
        {
            angle = UnityEngine.Random.Range(-45f, 45f);
        }
        else
        {
            angle = UnityEngine.Random.Range(135f, 225f);
        }

        // Calculate the final direction to send the ball in, the rest is just physics
        float angleRad = angle * Mathf.Deg2Rad;
        direction = new Vector2(Mathf.Cos(angleRad), Mathf.Sin(angleRad));

        _rb.linearVelocity = direction * originalSpeed;

        // If its after the first point, send to person that got point first
        // Once the ball hits a thing, award points and despawn
        // Script will restart upon point scored to this part
    }




    // This is called in gameManager
    public void ResetBall(bool endGame)
    {
        speed = originalSpeed;
        transform.position = Vector2.zero;
        _rb.linearVelocity = direction * 0;
        if(!endGame) StartCoroutine(LaunchBall(delay));
    }

    // If the ball hits the left or right wall, it enters a trigger
    private void OnTriggerEnter2D(Collider2D item)
    {

        // Check which wall was hit, assign score
        if (item.gameObject.name == "BoxColliderLeft") // Hits player 1's goal
        {
            gameManager.Player2Scored(); // Alert the game manager, who will handle this
            goRight = false; // Ball goes where it last was scored
        }
        else if (item.gameObject.name == "BoxColliderRight") // Hits player 2's goal
        {
            gameManager.Player1Scored();
            goRight = true; // Ball goes where it last was scored
        }
        else if(item.gameObject.layer == LayerMask.NameToLayer("Players")) // If it hits paddles
        {
            speed = speed * speedMult; // Scaling is fun, so multiply by speedMult

            // Get everything to find angle
            float paddleY = item.transform.position.y;
            float ballY = transform.position.y;
            float paddleHeight = item.bounds.size.y;

            // BallY - paddleY divided to find angle
            float hitFactor = (ballY - paddleY) / (paddleHeight / 2);

            // Use the angle to give ball new direction, if the ball was going left, make it go right, if going right make it go left
            Vector2 direction = new Vector2((_rb.linearVelocityX > 0 ? -1 : 1), hitFactor).normalized;

            // Set new ball velocity
            _rb.linearVelocity = direction * speed;
        } 
        else if(item.gameObject.name == "BoxColliderTop" || item.gameObject.name == "BoxColliderBottom") // If we hit a wall, go opposite way, this works better than physics for gameplay in my opinion
        {
            _rb.linearVelocityY = -_rb.linearVelocityY;
        }
    }

    // Need to fix the out of bounds ball somehow
    void LateUpdate()
    {
        Vector2 clampedPos = new Vector2(_rb.position.x, Mathf.Clamp(_rb.position.y, -5.1f, 5.1f));
        _rb.position = clampedPos;
    }


}
