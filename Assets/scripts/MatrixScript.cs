using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MatrixScript : MonoBehaviour {

    // Matrix dimensions
    public static int rows = 17;
    public static int columns = 10;

    // Offsets for draw in screen
    public float offsetX;
    public float offsetY;
    private float scale = 0.333f;

    // Delay to move falling piece
    private int delay = 0;

    // Color of game area
    public int areaColor;

    // Sprite generated in unity for blocks
    public GameObject blockPrefab;

    // Reference to spawn controller, set here to notify when a new block is need
    public SpawnScript spawn;

    // Reference to get difficulty parameters
    public GameControllerScript gameController;

    // References to other matrixs or areas, will be use in penalties
    public MatrixScript[] otherMatrixs = new MatrixScript[2];

    // Matrix of Unity GameObject to draw
    public GameObject[,] cellsObjects = new GameObject[columns, rows];

    // Matrix of Block class where logic is
    public Block[,] cells = new Block[columns, rows];
    	
	// Draw blocks in frame updating and set a delay to move down falling piece
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
    
    // Move falling piece at left, check first if some blocks are there
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

    // Move falling piece at right, check first if some blocks are there
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

    // coorX: cordinate for X axis or columns
    // coorY: cordinate for Y axis or rows
    // Check if a cell is ocupied by a block
    bool IsCellUse(int coorX, int coorY)
    {
        if (cells[coorX, coorY] == null)
            return false;
        else
            return (cells[coorX, coorY].value == 1 & cells[coorX, coorY].active == 0);
    }
    
    // deletedRow: number of row completed
    // When a row is completed, the rows above will fall down one block
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

    // targetMatrix: Matrix reference to switch falling piece
    // Switch piece to another matrix or game area.
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

    // Down the blocks of falling piece
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

    // penaltiesBlocks: blocks of piece that fall in wrong area
    // Remove 2 blocks from penalty piece
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

    // Add one block to each other matrix
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
                    Block penaltyBlocks = new Block(1, 0, areaColor);
                    otherMatrixs[m].CreateBlock(penaltyBlocks, randomColumn, j + 1);
                    break;
                } else
                {
                    if (j - 1 == -1)
                    {
                        Block penaltyBlocks = new Block(1, 0, areaColor);
                        otherMatrixs[m].CreateBlock(penaltyBlocks, randomColumn, j);
                    }
                }
            }
        }
    }

    // When a row is completed in the other matrix one penalty block will be remove
    void RemovePenaltyBlocks()
    {
        for (int m = 0; m < otherMatrixs.Length; m++)
        {
            for (int i = rows - 1; i >= 0; i--)
            {
                for (int j = 0; j < columns; j++)
                {
                    Block blockToRemove = otherMatrixs[m].cells[j, i];
                    if (blockToRemove!= null && blockToRemove.color != otherMatrixs[m].areaColor)
                    {
                        otherMatrixs[m].cells[j, i] = null;
                        break;
                    }
                }
            }
        }
    }

    // When a piece is landing the blocks are marked as inactive or divide if it's not of color area
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
                        if (cells[j, i].color != areaColor)
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

    // Random blocks are generate in columns, number of rows depends on difficulty
    public void StartBlocks()
    {
        for (int i = 0; i < gameController.startRows; i++)
        {
            for (int j = 0; j < 5; j++)
            {
                Block block = new Block(1, 0, areaColor);
                CreateBlock(block, UnityEngine.Random.Range(0, 10), i);
            }
        }
    }

    // Check if a row is complete or full with pieces of area color
    void CheckFullRows()
    {
        for (int i = 0; i < rows; i++)
        {
            bool isFull = true;
            for (int j = 0; j < columns; j++)
            {
                if (cells[j, i] == null || cells[j, i].value == 0 || cells[j, i].color != areaColor)
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

    // block: instance of block which be add to matrix
    // coorX: X axis coordinate or columns
    // coorY: Y axis coordinate or rows
    // Create a block in coordinates gived
    public void CreateBlock(Block block, int coorX, int coorY)
    {
        if (cells[coorX, coorY] == null || cells[coorX, coorY].value == 0)
            cells[coorX, coorY] = block;
    }

    // Draw the blocks and delete blocks with zero value
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

    // block: block which color will be get
    // gObject: Unity game object which color will be change
    // Color blocks in drawing
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

    // coordinate: X axis in screen
    // position in X axis in sreen add scale of sprites and offset
    float PositionX(int coordinate)
    {
        return coordinate * scale + offsetX;
    }

    // coordinate: Y axis in screen
    // position in Y axis in sreen add scale of sprites and offset
    float PositionY(int coordinate)
    {
        return coordinate * scale + offsetY;
    }

    #endregion

    // x: X axis of block
    // y: Y axis of block
    // Print coordinates of any block. Only for debug porpuses.
    void PrintCoords(int x, int y)
    {
        print("block coords x = " + x + " y = " + y);
    }
}
