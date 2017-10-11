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

    public GameObject blockPrefab;
    public SpawnScript spawn;
    public GameControllerScript gameController;
    public MatrixScript[] otherMatrixs = new MatrixScript[2];

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

    private void DividePenaltyPiece(Block[] penaltiesBlocks)
    {
        int piecesRemoved = 0;
        for (int i = rows - 1; i > 0; i--)
        {
            for (int j = 0; j < columns; j++)
            {
                for (int x = penaltiesBlocks.Length - 1; x > 0; x--)
                {
                    if (piecesRemoved < 2 && cells[j, i] == penaltiesBlocks[x])
                    {
                        cells[j, i] = null;
                        piecesRemoved++;
                    }
                }
            }
        }
    }

    void AddPenaltyBlocks()
    {
        for (int m = 0; m < otherMatrixs.Length; m++)
        {
            int randomColumn = UnityEngine.Random.Range(0, 9);
            for (int j = rows - 2; j >= 0; j--)
            {
                Block checkBlock = otherMatrixs[m].cells[randomColumn, j];
                if (checkBlock != null && checkBlock.value == 1)
                {
                    Block penaltyBlocks = new Block(1, 0, blocksColor);
                    otherMatrixs[m].CreateBlock(penaltyBlocks, randomColumn, j + 1);
                    break;
                } else
                {
                    if (j - 1 == -1)
                    {
                        Block penaltyBlocks = new Block(1, 0, blocksColor);
                        otherMatrixs[m].CreateBlock(penaltyBlocks, randomColumn, j);
                    }
                }
            }
        }
    }

    void RemovePenaltyBlocks()
    {
        for (int m = 0; m < otherMatrixs.Length; m++)
        {
            for (int i = rows - 1; i >= 0; i--)
            {
                for (int j = 0; j < columns; j++)
                {
                    Block blockToRemove = otherMatrixs[m].cells[j, i];
                    if (blockToRemove!= null && blockToRemove.color != otherMatrixs[m].blocksColor)
                    {
                        otherMatrixs[m].cells[j, i] = null;
                        break;
                    }
                }
            }
        }
    }

    void LandPiece()
    {
        bool isPenalty = false;
        int penalties = 0;
        Block[] penaltiesBlocks = new Block[4];

        for (int i = 0; i < rows; i++)
        {
            for (int j = 0; j < columns; j++)
            {
                if (cells[j, i] != null)
                {
                    if (cells[j, i].active == 1)
                    {
                        if (cells[j, i].color != blocksColor)
                        {
                            isPenalty = true;
                            if (penalties < 4)
                            {
                                penaltiesBlocks[penalties] = cells[j, i];
                                penalties++;
                            }
                        }
                        cells[j, i].active = 0;
                    }
                }
            }
        }

        if (isPenalty)
        {
            DividePenaltyPiece(penaltiesBlocks);
            AddPenaltyBlocks();
        }
        else
            CheckFullRows();

        spawn.GetPreviewPiece();
    }

    void StartBlocks()
    {
        for (int i = 0; i < gameController.startRows; i++)
        {
            for (int j = 0; j < 5; j++)
            {
                Block block = new Block(1, 0, blocksColor);
                CreateBlock(block, UnityEngine.Random.Range(0, 10), i);
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
                if (cells[j, i] == null || cells[j, i].value == 0 || cells[j, i].color != blocksColor)
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
                RemovePenaltyBlocks();
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
                        cellsObjects[j, i] = Instantiate(blockPrefab, new Vector3(PositionX(j), PositionY(i), 0), Quaternion.identity);
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

    void PrintCoords(int x, int y)
    {
        print("block coords x = " + x + " y = " + y);
    }
}
