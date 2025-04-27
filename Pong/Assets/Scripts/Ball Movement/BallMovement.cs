using System.Collections;
using UnityEngine;

public class BallMovement : MonoBehaviour
{

    // Variables
    private Rigidbody2D _rb;
    float angle;
    [SerializeField] float speed = 5f;
    bool goRight;
    Vector2 direction;
    [SerializeField] float ballDelay = 2f;



    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        _rb = GetComponent<Rigidbody2D>();

        // At the start, launch the ball in a random direction, within 45 degrees left or 45 degrees right
        goRight = Random.value > 0.5f; // Randomly decide if going left or right

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
            angle = Random.Range(-45f, 45f);
        }
        else
        {
            angle = Random.Range(135f, 225f);
        }

        // Calculate the final direction to send the ball in, the rest is just physics
        float angleRad = angle * Mathf.Deg2Rad;
        direction = new Vector2(Mathf.Cos(angleRad), Mathf.Sin(angleRad));

        _rb.linearVelocity = direction * speed;

        // If its after the first point, send to person that got point first
        // Once the ball hits a thing, award points and despawn
        // Script will restart upon point scored to this part
    }



    private void ResetBall()
    {
        transform.position = Vector2.zero;
        _rb.linearVelocity = direction * 0;

        // Optionally, relaunch the ball in a random direction
        Start(); // Re-call the Start() method, I don't like this and im changing it, but if this works for now, thats cool
    }



    // If the ball hits the left or right wall, it enters a trigger
    private void OnTriggerEnter2D(Collider2D wall)
    {

        // Check which wall was hit, assign score
        if (wall.gameObject.name == "EdgeColliderLeft")
        {
            Debug.Log("Player 2 scores!");
        }
        else if (wall.gameObject.name == "EdgeColliderLeft")
        {
            Debug.Log("Player 1 scores!");
        }

        // Any collider it hits will be one of these, so we can just despawn it here
        ResetBall();
    }

}
