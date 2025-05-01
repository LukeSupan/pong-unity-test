using UnityEngine;

public static class GameSettings
{
    /*
     This class is used to alter the behavior of the paddle on the right side of the screen
     Single player will result in it being controlled by AI 
     Multiplayer will result in it being controller by the arrow keys or right stick on a controller
    */
    public enum GameMode { Singleplayer, LocalMultiplayer }
    public static GameMode CurrentMode;
}
