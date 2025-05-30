using System;
using UnityEngine;
using UnityEngine.InputSystem;


// Moves both player 1 and player 2 currently. We don't need to split this in two even with AI as the controllers are different.

public class PlayerControl : MonoBehaviour
{
    private GameManager gameManager;


    // Speed and vectors
    private Rigidbody2D _rb; // Rigidbody2D, we apply velocity to this to move
    private float movespeed; // Movespeed, changed in gameManager
    Vector2 moveDirection = Vector2.zero;

    // Input
    public PlayerInputActions playerControls; // New input system, this is a script that we can use to do lots of controls, in this example I only use move, but any player input can be handled this way
    private InputAction move; // Input action for movement, more can be made

    // Initializes PlayerInputActions script
    private void Awake()
    {
        // If player1, use W and S, if player 2, use arrow keys, we can use both at once for multiplayer
        playerControls = new PlayerInputActions();
        if (CompareTag("Player1"))
        {
            move = playerControls.Player.Move1;
        }
        else if (CompareTag("Player2") && (GameSettings.CurrentMode == GameSettings.GameMode.LocalMultiplayer)) // Only activate player 2 if we are in LocalMultiplayer, otherwise, AI script runs
        {
            this.enabled = true;
            move = playerControls.Player.Move2;
        }
        else
        {
            // This will occur when the GameMode is singleplayer, this script is not needed on player 2 in that case, and is deactivated
            this.enabled = false;
            return;
        }

    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        gameManager = FindFirstObjectByType<GameManager>();
        _rb = GetComponent<Rigidbody2D>();

        movespeed = gameManager.playerMoveSpeed;

    }

    // Activates player controls, required
    private void OnEnable()
    {
        move?.Enable();
    }

    // Deactivates player controls, required
    private void OnDisable()
    {
        move?.Disable();
    }

    // Fixed update to make movement smooth
    private void Update()
    {
        // Get the moveDirection from input system
        moveDirection = move.ReadValue<Vector2>();

        // Apply the y movement, if at top of screen and trying to go up, don't, if at bottom and going down, also don't
        if(_rb.position.y >= 4.5 && moveDirection.y > 0)
        {
            _rb.linearVelocityY = 0;
        } else if (_rb.position.y <= -4.5 && moveDirection.y < 0)
        {
            _rb.linearVelocityY = 0;
        } else
        {
            _rb.linearVelocityY = moveDirection.y * movespeed;
        }
    }
}
