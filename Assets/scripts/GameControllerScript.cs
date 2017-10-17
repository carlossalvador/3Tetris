using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameControllerScript : MonoBehaviour {

    // Array with the 3 game areas
    public MatrixScript[] matrixCollection = new MatrixScript[3];
    // Objects in game over message
    public Text gameOver;
    public Button goBack;
    public Button restart;

    // Delay to drop falling piece one row
    public int maxDelay;

    // Difficulty variables
    public static int level;

    // Number of rows with random pieces on game start
    public int startRows;
    
    // Time to set game over message
    private int gameOverTimer;    

    // Score counter
    static private int score;

    // To show score message
    public Text scoreText;

    // In initialization middle area is set as falling piece holder and difficult variables are set too.
    // With less delay pieces will fall faster and make game harder.
    // Start the matrix with some blocks if difficult is high.
    void Start () {
        startRows = level;
        switch (level)
        {
            case 2:
                maxDelay = 20;
                break;
            case 1:
                maxDelay = 35;
                break;
            default:
                maxDelay = 50;
                break;
        }
        for (int i = 0; i < matrixCollection.Length; i++)
        {
            matrixCollection[i].StartBlocks();
        }
    }
	
	// In every frame user controls are check and after a little delay actions are execute
	void Update () {
        CheckGameOver();
    }

    // matrix: matrix where is falling piece
    // owner: player who will move piece to left
    // Move active piece to left
    public void MoveToLeft(int matrix, string owner)
    {
        matrixCollection[matrix].MoveLeft(owner);
    }

    // matrix: matrix where is falling piece
    // owner: player who will move piece to right
    // Move active piece to right
    public void MoveToRight(int matrix, string owner)
    {
        matrixCollection[matrix].MoveRight(owner);
    }

    // matrix: matrix where is falling piece
    // owner: player who will move piece down
    // Move active piece down
    public void MoveToDown(int matrix, string owner)
    {
        matrixCollection[matrix].DownActive(owner);
    }

    // matrix: matrix where is falling piece
    // nextMatrix: matrix where piece will go
    // owner: player who will change piece
    // Move a piece to another game area
    public bool ChangeGameArea(int matrix, int nextMatrix, string owner)
    {
        return matrixCollection[matrix].ChangeGameArea(matrixCollection[nextMatrix], owner);
    }

    //Rotate game areas feature
    public void RotateAreas()
    {
        foreach (var matrix in matrixCollection)
        {
            matrix.RotateGameArea();
        }
    }

    // addScore: points to be added
    // Add score and update message in UI
    public void UpdateScore(int addScore)
    {
        score += addScore;
        scoreText.text = score.ToString();
    }

    // Check if blocks are stacked in top row, there is a delay to not set game over on spawn piece
    void CheckGameOver()
    {
        bool isStuck = false;
        for (int i = 0; i < 10; i++)
        {
            if (matrixCollection[1].cells[i, 16] != null && matrixCollection[1].cells[i, 16].value == 1)
            {
                isStuck = true;
            }
        }

        if (isStuck)
        {
            if (gameOverTimer < 80)
                gameOverTimer++;
            else
                GameOver();
        }
        else
            gameOverTimer = 0;
    }

    // Show game over message
    void GameOver()
    {
        gameOver.gameObject.SetActive(true);
        restart.gameObject.SetActive(true);
        goBack.gameObject.transform.SetPositionAndRotation(new Vector3(385, 235), Quaternion.identity);
    }
}
