using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MatrixScript : MonoBehaviour {

    public static int rows = 17;
    public static int columns = 10;
    public float offsetX;
    public float offsetY;
    private float scale = 0.333f;
    private int delay = 0;
    public int blocksColor;

    public GameObject block;
    public SpawnScript spawn;
    public GameControllerScript gameController;

    public GameObject[,] cellsObjects = new GameObject[columns, rows];
    public Block[,] cells = new Block[columns, rows];
    
    // Use this for initialization
    void Start () {
        StartBlocks();
    }
	
	// Update is called once per frame
	void Update () {
        if (delay < gameController.maxDelay)
        {
            delay++;
        } else
        {
            delay = 0;
            DownActive();
        }
        DrawBlocks();
    }

    #region CONTROLLERS
    
    public void MoveLeft()
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
                        if (j - 1 == -1 || IsCellUse(j - 1, i))
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

    public void MoveRight()
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
                        if (j + 1 == columns || IsCellUse(j + 1, i))
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
    
    bool IsCellUse(int coorX, int coorY)
    {
        if (cells[coorX, coorY] == null)
            return false;
        else
            return (cells[coorX, coorY].value == 1 & cells[coorX, coorY].active == 0);
    }
    
    void MoveDownRows(int deletedRow)
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

    public void ChangeGameArea(MatrixScript targetMatrix)
    {
        for (int i = 0; i < rows; i++)
        {
            for (int j = 0; j < columns; j++)
            {
                if (cells[j, i] != null)
                {
                    if (cells[j, i].active == 1)
                    {
                        targetMatrix.CreateBlock(cells[j, i], j, i);
                        cells[j, i] = null;
                    }
                }
            }
        }
    }

    #endregion

    #region GAME

    public void DownActive()
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
                        if (IsCellUse(j, i - 1))
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
            LandPiece();
    }

    void LandPiece()
    {
        for (int i = 0; i < rows; i++)
        {
            for (int j = 0; j < columns; j++)
            {
                if (cells[j, i] != null)
                    cells[j, i].active = 0;
            }
        }

        CheckFullRows();
        spawn.GetPreviewPiece();
    }

    void StartBlocks()
    {
        for (int i = 0; i < gameController.levels; i++)
        {
            for (int j = 0; j < 5; j++)
            {
                Block block = new Block(1, 0, blocksColor);
                CreateBlock(block, UnityEngine.Random.Range(0, 10), i);
                //createBlock(block, j, i);
            }
        }
    }

    void CheckFullRows()
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
                MoveDownRows(i);
                gameController.updateScore(1);
                i--;
            }
        }
    }
    
    #endregion

    #region DRAWING

    public void CreateBlock(Block block, int coorX, int coorY)
    {
        if (cells[coorX, coorY] == null || cells[coorX, coorY].value == 0)
            cells[coorX, coorY] = block;
    }

    public void DrawBlocks()
    {
        for (int i = 0; i < rows; i++)
        {
            for (int j = 0; j < columns; j++)
            {
                if (cells[j, i] != null && cells[j, i].value == 1)
                {
                    if (cellsObjects[j, i] == null)
                    {
                        cellsObjects[j, i] = Instantiate(block, new Vector3(PositionX(j), PositionY(i), 0), Quaternion.identity);
                        ColorBlocks(cells[j, i], cellsObjects[j, i]);
                    }
                }
                else
                {
                    Destroy(cellsObjects[j, i]);
                }


            }
        }
    }

    void ColorBlocks(Block block, GameObject gObject)
    {
        Color c;
        switch (block.color)
        {
            case 1:
                c = new Color(.3f, .7f, .7f);
                break;
            case 2:
                c = new Color(.7f, .3f, .7f);
                break;
            default:
                c = new Color(.7f, .7f, .3f);
                break;
        }

        foreach (SpriteRenderer sprite in gObject.GetComponentsInChildren<SpriteRenderer>())
        {
            sprite.color = c;
        }
    }

    float PositionX(int coordinate)
    {
        return coordinate * scale + offsetX;
    }

    float PositionY(int coordinate)
    {
        return coordinate * scale + offsetY;
    }

    #endregion
}
