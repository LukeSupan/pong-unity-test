using UnityEngine;
using UnityEngine.SceneManagement;

// Controls gameflow, sets score
// Holds variables for other things, makes it easy to change
public class GameManager : MonoBehaviour
{
    // Ball variables
    public float ballOriginalSpeed;
    public float ballDelay;
    public float ballSpeedMult;

    // Player variables
    public float playerMoveSpeed;
    public float enemyMoveSpeed;

    // Score variables
    public int player1Score = 0;
    public int player2Score = 0;

    public BallMovement ball;

    // Player 1 dealings
    public void Player1Scored()
    {
        player1Score++;
        Debug.Log($"Player 1 Score: {player1Score}");
        ball.ResetBall();

        // Go to menu if someone wins
        if (ScoreCheck())
        {
            SceneManager.LoadSceneAsync("Menu");
        }
    }

    // Player 2 dealings
    public void Player2Scored()
    {
        player2Score++;
        Debug.Log($"Player 2 Score: {player2Score}");
        ball.ResetBall();

        // Go to menu if someone wins
        if (ScoreCheck())
        {
            SceneManager.LoadSceneAsync("Menu");
        }
    }

    // Returns true if game is over, otherwise false
    private bool ScoreCheck()
    {
        if ((player1Score >= 11) && (player2Score <= (player1Score - 2)))
        {
            Debug.Log("Player 1 wins oh yeah");
            return true;

        } else if ((player2Score >= 11) && (player1Score <= (player2Score - 2)))
        {
            Debug.Log("Player 2 wins oh yeah");
            return true;
        }

        Debug.Log("no one wins right now");

        return false;
    }
}