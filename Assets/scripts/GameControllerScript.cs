using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameControllerScript : MonoBehaviour {

    public MatrixScript[] matrixCollection = new MatrixScript[3];
    private int currentMatrix;
    public Text gameOver;
    public Button goBack;
    public Button restart;

    public static int level;
    public int maxDelay;
    public int maxControlDelay;
    public int startRows;
    private int controlDelay;
    private int gameOverTimer;
    private bool readyLeft;
    private bool readyRight;
    private bool readyChangeGameArea;
    private bool readyDown;

    static private int score;
    public Text scoreText;    

    // Use this for initialization
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
    }
	
	// Update is called once per frame
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
        }

        CheckGameOver();
    }

    public void returnSpawnArea()
    {
        currentMatrix = 1;
    }

    public void updateScore(int addScore)
    {
        score += addScore;
        scoreText.text = score.ToString();
    }

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

    void GameOver()
    {
        gameOver.gameObject.SetActive(true);
        restart.gameObject.SetActive(true);
        goBack.gameObject.transform.SetPositionAndRotation(new Vector3(385, 235), Quaternion.identity);
    }
}
