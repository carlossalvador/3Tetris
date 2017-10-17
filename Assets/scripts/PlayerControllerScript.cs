using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControllerScript : MonoBehaviour {

    //Game controller
    public GameControllerScript gameController;

    // Game area where falling piece are
    private int currentMatrix;
    
    // Variables for better experience in user controls
    public int maxControlDelay;
    private int controlDelay;
    private bool readyLeft;
    private bool readyRight;
    private bool readyChangeGameArea;
    private bool readyDown;
    private bool readyRotate;

    // Label for human player
    string owner = "player";

    // Use this for initialization
    void Start () {
        
    }
	
	// Update is called once per frame
	void Update () {
        if (Input.GetKey("left"))
            readyLeft = true;

        if (Input.GetKey("right"))
            readyRight = true;

        if (Input.GetKey("down"))
            readyDown = true;

        if (Input.GetKeyDown("space"))
            readyChangeGameArea = true;

        if (Input.GetKeyDown("r"))
            readyRotate = true;

        if (controlDelay < maxControlDelay)
            controlDelay++;
        else
        {
            controlDelay = 0;
            if (readyRight)
            {
                gameController.MoveToRight(currentMatrix, owner);
                readyRight = false;
            }

            if (readyLeft)
            {
                gameController.MoveToLeft(currentMatrix, owner);
                readyLeft = false;
            }

            if (readyDown)
            {
                gameController.MoveToDown(currentMatrix, owner);
                readyDown = false;
            }

            if (readyChangeGameArea)
            {
                int nextMatrix = currentMatrix < 2 ? currentMatrix + 1 : 0;
                if (gameController.ChangeGameArea(currentMatrix, nextMatrix, owner))
                    currentMatrix = nextMatrix;                
                
                readyChangeGameArea = false;
            }

            if (readyRotate)
            {
                gameController.RotateAreas();
                readyRotate = false;
            }
        }
    }

    // When a piece lands, middle area (spawn point) is set like active area
    public void ReturnSpawnArea()
    {
        currentMatrix = 1;
    }
}
