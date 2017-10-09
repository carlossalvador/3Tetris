using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MatrixScript : MonoBehaviour {

    public static int rows = 17;
    public static int columns = 10;
    public float offsetX = -8f;
    public float offsetY = 1f;
    private float scale = 0.333f;
    private int delay = 0;
    public int maxDelay = 60;
    public int levels;

    public GameObject block;
    public SpawnScript spawn;

    public static GameObject[,] cellsObjects = new GameObject[columns, rows];
    public static Block[,] cells = new Block[columns, rows];

    public int maxControlDelay;
    private int controlDelay;
    private bool readyLeft = false;
    private bool readyRight = false;

    // Use this for initialization
    void Start () {
        startBlocks();
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

        if (controlDelay < maxControlDelay)
            controlDelay++;
        else
        {
            controlDelay = 0;
            if (readyRight)
            {
                moveRight();
                readyRight = false;
            }

            if (readyLeft)
            {
                moveLeft();
                readyLeft = false;
            }
        }


        if (delay < maxDelay)
        {
            delay++;
        } else
        {
            delay = 0;
            downActive();
        }
        drawBlocks();
    }

    

    public void drawBlocks()
    {
        for (int i = 0; i < rows; i++)
        {
            for (int j = 0; j < columns; j++)
            {
                if (cells[j, i] != null && cells[j, i].value == 1 )
                {
                    if (cellsObjects[j, i] == null)
                    {
                        cellsObjects[j, i] = Instantiate(block, new Vector3(positionX(j), positionY(i), 0), Quaternion.identity);
                    }
                } else
                {
                    Destroy(cellsObjects[j, i]);
                }
                    
                    
            }
        }
    }

    public void downActive()
    {
        bool isLanding = false;

        for (int i = 0; i < rows; i++)
        {
            for (int j = 0; j < columns; j++)
            {
                if (cells[j, i] != null)
                {
                    if (cells[j, i].active == 1)
                    {
                        print("revisando x=" + j + " y=" + i);
                        if (isCellUse(j, i - 1))
                        {
                            isLanding = true;
                            break;
                        }
                    }
                }   
            }
        }

        if (!isLanding)
        {
            for (int i = 0; i < rows; i++)
            {
                for (int j = 0; j < columns; j++)
                {
                    if (cells[j, i] != null & (i - 1 >= 0))
                    {
                        if (cells[j, i].active == 1)
                        {
                            cells[j, i - 1] = cells[j, i];
                            cells[j, i] = null;
                            if (i - 1 == 0)
                            {
                                isLanding = true;
                            }
                        }
                    }
                }
            }
        }

        if (isLanding)
            landPiece();
    }

    private void moveLeft()
    {
        bool isBlocked = false;

        for (int i = 0; i < rows; i++)
        {
            for (int j = 0; j < columns; j++)
            {
                if (cells[j, i] != null)
                {
                    if (cells[j, i].active == 1)
                    {
                        if (j - 1 == -1 || isCellUse(j - 1, i))
                        {
                            isBlocked = true;
                            break;
                        }

                    }
                }
            }
        }

        if (!isBlocked)
        {
            for (int i = 0; i < rows; i++)
            {
                for (int j = 0; j < columns; j++)
                {
                    if (cells[j, i] != null && cells[j, i].active == 1)
                    {
                        cells[j - 1, i] = cells[j, i];
                        cells[j, i] = null;
                    }
                }
            }
        }
    }

    private void moveRight()
    {
        bool isBlocked = false;

        for (int i = 0; i < rows; i++)
        {
            for (int j = columns - 1; j > -1; j--)
            {
                if (cells[j, i] != null)
                {
                    if (cells[j, i].active == 1)
                    {
                        if (j + 1 == columns || isCellUse(j + 1, i))
                        {
                            isBlocked = true;
                            break;
                        }

                    }
                }
            }
        }

        if (!isBlocked)
        {
            for (int i = 0; i < rows; i++)
            {
                for (int j = columns - 1; j > -1; j--)
                {
                    if (cells[j, i] != null && cells[j, i].active == 1)
                    {
                        cells[j + 1, i] = cells[j, i];
                        cells[j, i] = null;
                    }
                }
            }
        }
    }

    float positionX(int coordinate)
    {
        return coordinate * scale + offsetX;
    }

    float positionY(int coordinate)
    {
        return coordinate * scale + offsetY;
    }
    
    void startBlocks()
    {
        for (int i = 0; i < levels; i++)
        {
            for (int j = 0; j < 3; j++)
            {
                createBlock(UnityEngine.Random.Range(0,9), i, 1, 0);
            }
        }
        
    }

    bool isCellUse(int coorX, int coorY)
    {
        if (cells[coorX, coorY] == null)
            return false;
        else
            return (cells[coorX, coorY].value == 1 & cells[coorX, coorY].active == 0);
    }

    void landPiece()
    {
        for (int i = 0; i < rows; i++)
        {
            for (int j = 0; j < columns; j++)
            {
                if (cells[j, i] != null)
                    cells[j, i].active = 0;
            }
        }

        checkFullRows();
        spawn.newPiece();
    }

    public void createBlock(int coorX, int coorY, int value, int active)
    {
        if (cells[coorX, coorY] == null || cells[coorX, coorY].value == 0)
            cells[coorX, coorY] = new Block(value, active);
    }

    void checkFullRows()
    {
        for (int i = 0; i < rows; i++)
        {
            bool isFull = true;
            for (int j = 0; j < columns; j++)
            {
                if (cells[j, i] == null || cells[j, i].value == 0)
                    isFull = false;
            }
            if (isFull)
            {
                for (int j = 0; j < columns; j++)
                {
                    cells[j, i] = null;
                }
                moveDownRows(i);
            }
        }
    }

    void moveDownRows(int deletedRow)
    {
        for (int i = deletedRow; i < rows; i++)
        {
            for (int j = 0; j < columns; j++)
            {
                if (cells[j, i] != null & (i - 1 >= 0))
                {
                    if (cells[j, i].value == 1)
                    {
                        cells[j, i - 1] = cells[j, i];
                        cells[j, i] = null;
                    }
                }
            }
        }
    }
}
