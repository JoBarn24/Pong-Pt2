using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ScoreManager : MonoBehaviour
{
    public TextMeshProUGUI rightText;
    public TextMeshProUGUI leftText;
    public TextMeshProUGUI winText;
    public GameObject ball;
    public static int leftScore = 0;
    public static int rightScore = 0;
    
    private bool gameOver = false;
    
    private void OnTriggerEnter(Collider other)
    {
        if (gameOver)
        {
            return;
        }
        
        BallScript ballScript = ball.GetComponent<BallScript>();
        
        if (other.CompareTag("Ball") && gameOver == false)
        {
            if (gameObject.name == "LeftGoal")
            {
                AddPointToRight();
                Debug.Log("Right player scored! Score: " + rightScore);
                ballScript.ResetBall("right");

                if (gameOver)
                {
                    ballScript.StopBall();
                }
            }
            else if (gameObject.name == "RightGoal")
            {
                AddPointToLeft();
                Debug.Log("Left player scored! Score: " + leftScore);
                ballScript.ResetBall("left");
                if (gameOver)
                {
                    ballScript.StopBall();
                }
            }
        }
    }
    private void AddPointToLeft()
    {
        leftScore++;
        leftText.text = leftScore.ToString();
        CheckGameOver();
    }

    private void AddPointToRight()
    {
        rightScore++;
        rightText.text = rightScore.ToString();
        CheckGameOver();
    }

    private void CheckGameOver()
    {
        if (leftScore >= 11)
        {
            gameOver = true;
            
            BallScript ballScript = ball.GetComponent<BallScript>();
            ballScript.StopBall();
            
            Debug.Log("Left Paddle Wins!");
            winText.text = "Left Paddle Wins!";
            
            Invoke("ResetGame", 10);
        }
        else if (rightScore >= 11)
        {
            gameOver = true;
            
            BallScript ballScript = ball.GetComponent<BallScript>();
            ballScript.StopBall();
            
            Debug.Log("Right Paddle Wins!");
            winText.text = "Right Paddle Wins!";
            
            Invoke("ResetGame", 10);
        }
    }

    private void ResetGame()
    {
        leftScore = 0;
        rightScore = 0;
        
        leftText.text = leftScore.ToString();
        rightText.text = rightScore.ToString();
        winText.text = "";
        
        BallScript ballScript = ball.GetComponent<BallScript>();
        ballScript.ResetBall("left");
        
        Debug.Log("Game reset.");
        gameOver = false;
    }
}
