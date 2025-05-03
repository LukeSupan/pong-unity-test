using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class MenuControl : MonoBehaviour
{
    private void Awake()
    {
        Time.timeScale = 1f; // This resets the timescale if the game was paused when we left to go to menu
    }

    public void Singleplayer()
    {
        // Set a variable to make the second player controllable
        GameSettings.CurrentMode = GameSettings.GameMode.Singleplayer;
        SceneManager.LoadSceneAsync("Pong");
    }

    public void LocalMultiplayer()
    {
        // Set a variable to make the second player AI
        GameSettings.CurrentMode = GameSettings.GameMode.LocalMultiplayer;
        SceneManager.LoadSceneAsync("Pong");
    }

    public void Quit()
    {
        Application.Quit();
    }


}
