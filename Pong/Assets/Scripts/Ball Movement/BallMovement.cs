using System;
using System.Collections;
using UnityEngine;

// Makes the ball bounce off of walls and paddles, resets the ball when it hits the side

public class BallMovement : MonoBehaviour
{

    // Variables
    private Rigidbody2D _rb;
    float angle;
    [SerializeField] float originalSpeed = 5f;
    float speed = 5f;
    float speedMult = 1.05f;
    bool goRight;
    Vector2 direction;
    [SerializeField] float ballDelay = 2f;

    private GameManager gameManager; // Reference for gameManager to do everything
    



    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        _rb = GetComponent<Rigidbody2D>();
        gameManager = FindFirstObjectByType<GameManager>();

        // At the start, launch the ball in a random direction, within 45 degrees left or 45 degrees right
        goRight = UnityEngine.Random.value > 0.5f; // Randomly decide if going left or right

        StartCoroutine(LaunchBall(ballDelay));
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
    public void ResetBall()
    {
        speed = originalSpeed;
        transform.position = Vector2.zero;
        _rb.linearVelocity = direction * 0;

        // Optionally, relaunch the ball in a random direction
        Start(); // Re-call the Start() method, I don't like this and im changing it, but if this works for now, thats cool
    }

    // If the ball hits the left or right wall, it enters a trigger
    private void OnTriggerEnter2D(Collider2D item)
    {

        // Check which wall was hit, assign score
        if (item.gameObject.name == "EdgeColliderLeft")
        {
            gameManager.Player2Scored(); // Alert the game manager, who will handle this
        }
        else if (item.gameObject.name == "EdgeColliderRight")
        {
            gameManager.Player1Scored();
        }
        else if(item.gameObject.layer == LayerMask.NameToLayer("Players"))
        {
            speed = speed * speedMult;

            // Get everything to find angle
            float paddleY = item.transform.position.y;
            float ballY = transform.position.y;
            float paddleHeight = item.bounds.size.y;

            // BallY - paddleY divided to find angle
            float hitFactor = (ballY - paddleY) / (paddleHeight / 2);

            // Use the angle to give ball new direction, if the ball was going left, make it go right, if going right make it go left
            Vector2 direction = new Vector2((_rb.linearVelocityX > 0 ? -1 : 1), hitFactor).normalized;

            _rb.linearVelocity = direction * speed;
        } 
        else if(item.gameObject.name == "EdgeColliderTop" || item.gameObject.name == "EdgeColliderBottom") // If we hit a wall, go opposite way, this is works better than physics
        {
            _rb.linearVelocityY = -_rb.linearVelocityY;
        }

    }

}
