using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameControllerScript : MonoBehaviour {

    public MatrixScript[] matrixCollection = new MatrixScript[3];
    private int currentMatrix;

    public int level;
    public int maxDelay;
    public int maxControlDelay;
    public int startRows;
    private int controlDelay;
    private bool readyLeft = false;
    private bool readyRight = false;
    private bool readyChangeGameArea;

    static private int score;
    public Text scoreText;

    private void Awake()
    {
        Screen.SetResolution(900, 600, false);
    }

    // Use this for initialization
    void Start () {
        currentMatrix = 1;
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

            if (readyChangeGameArea)
            {
                int nextMatrix = currentMatrix < 2 ? currentMatrix + 1 : 0;
                matrixCollection[currentMatrix].ChangeGameArea(matrixCollection[nextMatrix]);
                currentMatrix = nextMatrix;
                readyChangeGameArea = false;
            }
        }
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
}
