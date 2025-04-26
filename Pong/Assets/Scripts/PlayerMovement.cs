using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerControl : MonoBehaviour
{

    // Rigidbody2D used for movement
    private Rigidbody2D _rb;
    // Top speed for pong, serialize field so I can change it easily, this i set in inspector
    [SerializeField] private float movespeed;
    // New input system
    public InputAction playerControls;
    // It always up or down, we can simplify from this but this is good for learning
    Vector2 moveDirection = Vector2.zero;



    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        _rb = GetComponent<Rigidbody2D>();

    }

    // Activates player controls, required
    private void OnEnable()
    {
        playerControls.Enable();
    }

    // Deactivates player controls, required
    private void OnDisable()
    {
        playerControls.Disable();
    }

    // Fixed update to make movement smooth
    private void FixedUpdate()
    {
        // Get the moveDirection from input system
        moveDirection = playerControls.ReadValue<Vector2>();

        // Apply the y movement (which is all we get), multiply by movespeed
        _rb.linearVelocityY = moveDirection.y * movespeed;
    }
}
