using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PauseMenu : MonoBehaviour
{
    public GameObject pauseMenuCanvas;

    [SerializeField] private AudioClip buttonClick;
    [SerializeField] private GameObject pauseFirstButton;

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
        SoundEffectPlayer.instance.PlaySoundClip(buttonClick, transform, 0.1f);
        // Either unpause or pause
        if (pauseMenuCanvas.activeSelf)
        {
            Time.timeScale = 1f;
            pauseMenuCanvas.SetActive(false);
        } else
        {
            // When the pause menu is opened, select the first button
            EventSystem.current.SetSelectedGameObject(null);
            EventSystem.current.SetSelectedGameObject(pauseFirstButton);
            Time.timeScale = 0f;
            pauseMenuCanvas.SetActive(true);
        }
    }

    // Continue button also toggles menu off
    public void Continue()
    {
        SoundEffectPlayer.instance.PlaySoundClip(buttonClick, transform, 0.1f);
        // Set a variable to make the second player controllable
        Time.timeScale = 1f; // This is always unpause so no need to check
        pauseMenuCanvas.SetActive(false);
    }

    // Quit to menu will go back to menu
    public void QuitToMenu()
    {
        SoundEffectPlayer.instance.PlaySoundClip(buttonClick, transform, 0.1f);
        SceneManager.LoadSceneAsync("Menu");
    }
}
