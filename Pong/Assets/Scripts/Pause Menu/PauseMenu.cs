using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    public GameObject pauseMenuCanvas;

    // Input
    public PlayerInputActions menuControls; // New input system, this is a script that we can use to do lots of controls, in this example I only use move, but any player input can be handled this way
    private InputAction menuOpen; // Input action for movement, more can be made


    // Initializes PlayerInputActions script
    private void Awake()
    {
        // Menu control setup
        menuControls = new PlayerInputActions();
        menuOpen = new InputAction();

        menuOpen = menuControls.UI.MenuOpen; // Menu toggle on input
    }

    // If we pressed the button to open menu, open/close the menu
    private void Update()
    {
        if(menuOpen.WasPressedThisFrame())
        {
            ToggleMenu();
        }
    }

    // Activates menu controls, required
    private void OnEnable()
    {
        menuOpen.Enable();
    }

    // Deactivates menu controls, required
    private void OnDisable()
    {
        menuOpen.Disable();
    }

    // On select/escape press, toggle menu on and off
    public void ToggleMenu()
    {
        // Either pause or unpause
        if (pauseMenuCanvas.activeSelf)
        {
            Time.timeScale = 1f;
            pauseMenuCanvas.SetActive(false);
        } else
        {
            Time.timeScale = 0f;
            pauseMenuCanvas.SetActive(true);
        }
    }

    // Continue button also toggles menu off
    public void Continue()
    {
        // Set a variable to make the second player controllable
        Time.timeScale = 1f; // This is always unpause so no need to check
        pauseMenuCanvas.SetActive(false);
    }

    // Quit to menu will go back to menu
    public void QuitToMenu()
    {
        Time.timeScale = 1f; // timeScale would remain 0 if we didn't do this
        SceneManager.LoadSceneAsync("Menu");
    }
}
