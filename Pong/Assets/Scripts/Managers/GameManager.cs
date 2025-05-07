using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using UnityEngine.UI;
using UnityEngine.EventSystems;

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
    public float enemyErrorMargin;
    public float enemyReactionError;
    public float enemyErrorMultiplier;

    // Score variables
    public int player1Score = 0;
    public int player2Score = 0;

    // Score text variables
    public TextMeshProUGUI Player1ScoreText;
    public TextMeshProUGUI Player2ScoreText;

    // Ball to call functions
    public BallMovement ball;

    // Win Screen canvas and text
    public GameObject Winscreen;
    public TextMeshProUGUI WinnerText;

    // The first button on the winScreen
    [SerializeField] private GameObject winFirstButton;

    // Player 1 dealings
    public void Player1Scored()
    {
        player1Score++;
        Player1ScoreText.text = player1Score.ToString(); // Update scoretext based on score

        // Display if someone wins
        if (ScoreCheck())
        {
            ball.ResetBall(true);
        }
        else
        {
            ball.ResetBall(false);
        }
    }

    // Player 2 dealings
    public void Player2Scored()
    {
        player2Score++;
        Player2ScoreText.text = player2Score.ToString(); // Update scoretext based on score

        // Display if someone wins
        if(ScoreCheck())
        {
            ball.ResetBall(true);
        } else
        {
            ball.ResetBall(false);
        }
    }

    // Returns true if game is over, otherwise false
    private bool ScoreCheck()
    {
        if ((player1Score >= 6) && (player2Score <= (player1Score - 2)))
        {
            // Edit text to say this and display win screen
            WinnerText.SetText("PLAYER 1 WINS!");
            Winscreen.SetActive(true);
            EventSystem.current.SetSelectedGameObject(null);
            EventSystem.current.SetSelectedGameObject(winFirstButton);

            return true;

        } else if ((player2Score >= 6) && (player1Score <= (player2Score - 2)))
        {
            // Edit text to say this and display win screen
            WinnerText.SetText("PLAYER 2 WINS!");
            Winscreen.SetActive(true);
            EventSystem.current.SetSelectedGameObject(null);
            EventSystem.current.SetSelectedGameObject(winFirstButton);


            return true;
        }
        return false;
    }
}