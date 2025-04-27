using UnityEngine;

public class GameManager : MonoBehaviour
{
    public int player1Score = 0;
    public int player2Score = 0;

    public BallMovement ball;

    public void Player1Scored()
    {
        player1Score++;
        Debug.Log($"Player 1 Score: {player1Score}");
        ball.ResetBall();
    }

    public void Player2Scored()
    {
        player2Score++;
        Debug.Log($"Player 2 Score: {player2Score}");
        ball.ResetBall();
    }
}
