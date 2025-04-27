using UnityEngine;

public class BallMovement : MonoBehaviour
{
    private Rigidbody2D _rb;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        _rb = GetComponent<Rigidbody2D>();

        // At the start, launch the ball in a random direction, within 45 degrees left or 45 degrees right
        // If its after the first point, send to person that got point first
        // Once the ball hits a thing, award points and despawn
        // Script will restart upon point scored to this part

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
