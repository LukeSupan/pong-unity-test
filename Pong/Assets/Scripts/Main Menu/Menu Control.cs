using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MenuControl : MonoBehaviour
{
    [SerializeField] private AudioClip buttonClick;
    [SerializeField] private Button firstButton;

    private void Awake()
    {
        Time.timeScale = 1f; // This resets the timescale if the game was paused when we left to go to menu
    }

    public void Singleplayer()
    {
        SoundEffectPlayer.instance.PlaySoundClip(buttonClick, transform, 0.1f);
        
        // Set a variable to make the second player controllable
        GameSettings.CurrentMode = GameSettings.GameMode.Singleplayer;
        SceneManager.LoadSceneAsync("Pong");

    }

public void LocalMultiplayer()
    {
        SoundEffectPlayer.instance.PlaySoundClip(buttonClick, transform, 0.1f);

        // Set a variable to make the second player AI
        GameSettings.CurrentMode = GameSettings.GameMode.LocalMultiplayer;
        SceneManager.LoadSceneAsync("Pong");

    }

    public void Quit()
    {
        SoundEffectPlayer.instance.PlaySoundClip(buttonClick, transform, 0.1f);

        Application.Quit();
    }
}
