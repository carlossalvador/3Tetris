using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameControllerScript : MonoBehaviour {

    // Array with the 3 game areas
    public MatrixScript[] matrixCollection = new MatrixScript[3];
    // Game area where falling piece are
    private int currentMatrix;
    // Objects in game over message
    public Text gameOver;
    public Button goBack;
    public Button restart;

    // Difficulty variables
    public static int level;
    // Delay to drop falling piece one row
    public int maxDelay;
    // Variables for better experience in user controls
    public int maxControlDelay;
    private int controlDelay;
    private bool readyLeft;
    private bool readyRight;
    private bool readyChangeGameArea;
    private bool readyDown;
    private bool readyRotate;
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
        currentMatrix = 1;
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
        if (Input.GetKey("left"))
        {
            readyLeft = true;
        }

        if (Input.GetKey("right"))
        {
            readyRight = true;
        }

        if (Input.GetKey("down"))
        {
            readyDown = true;
        }

        if (Input.GetKeyDown("space"))
        {
            readyChangeGameArea = true;
        }

        if (Input.GetKeyDown("r"))
        {
            readyRotate = true;
        }
        
        if (controlDelay < maxControlDelay)
            controlDelay++;
        else
        {
            controlDelay = 0;
            if (readyRight)
            {
                matrixCollection[currentMatrix].MoveRight();
                readyRight = false;
            }

            if (readyLeft)
            {
                matrixCollection[currentMatrix].MoveLeft();
                readyLeft = false;
            }

            if (readyDown)
            {
                matrixCollection[currentMatrix].DownActive();
                readyDown = false;
            }

            if (readyChangeGameArea)
            {
                int nextMatrix = currentMatrix < 2 ? currentMatrix + 1 : 0;
                matrixCollection[currentMatrix].ChangeGameArea(matrixCollection[nextMatrix]);
                currentMatrix = nextMatrix;
                readyChangeGameArea = false;
            }

            if (readyRotate)
            {
                foreach (var matrix in matrixCollection)
                {
                    matrix.RotateGameArea();
                }
                readyRotate = false;
            }
        }
        
        CheckGameOver();
    }

    // When a piece lands, middle area (spawn point) is set like active area
    public void ReturnSpawnArea()
    {
        currentMatrix = 1;
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
