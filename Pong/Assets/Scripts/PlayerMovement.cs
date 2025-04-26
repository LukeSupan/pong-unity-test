using System;
using UnityEngine;
using UnityEngine.InputSystem;


// This is player 1 movement currently, I will make a player 2 script that just calls this given move2 instead of move1
public class PlayerControl : MonoBehaviour
{
    // Speed and vectors
    private Rigidbody2D _rb; // Rigidbody2D, we apply velocity to this to move
    [SerializeField] private float movespeed; // Movespeed can be changed in inspector
    Vector2 moveDirection = Vector2.zero;

    // Input
    public PlayerInputActions playerControls; // New input system, this is a script that we can use to do lots of controls, in this example I only use move, but any player input can be handled this way
    private InputAction move; // Input action for movement, more can be made

    // Initializes PlayerInputActions script
    private void Awake()
    {
        playerControls = new PlayerInputActions();
        move = playerControls.Player.Move1;
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        _rb = GetComponent<Rigidbody2D>();
    }

    // Activates player controls, required
    private void OnEnable()
    {
        move.Enable();
    }

    // Deactivates player controls, required
    private void OnDisable()
    {
        move.Disable();
    }

    // Fixed update to make movement smooth
    private void Update()
    {
        // Get the moveDirection from input system
        moveDirection = move.ReadValue<Vector2>();

        // Apply the y movement (which is all we get), multiply by movespeed
        _rb.linearVelocityY = moveDirection.y * movespeed;
    }
}
