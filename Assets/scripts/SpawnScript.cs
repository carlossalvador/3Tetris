using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnScript : MonoBehaviour {

    // Array with pieces patrons
    private int[,,] intPieces;

    // Matrix with spawn point
    public MatrixScript mainMatrix;

    // Matrix where next piece will be show
    public NextPieceScript previewMatrix;

    // This reference is use to reset the active matrix
    public GameControllerScript gameController;

    // Store the piece generated in next piece matrix
    private int randomPiece;
    private int randomColor;

    // Set pieces patrons and generate first piece in preview and spawn point
    void Start () {
        //Generate pieces
        intPieces = new int[,,] {
            {// L
                {1, 0, 0, 0},
                {1, 0, 0, 0},
                {1, 1, 0, 0},
                {0, 0, 0, 0},
            },
            {// O
                {1, 1, 0, 0},
                {1, 1, 0, 0},
                {0, 0, 0, 0},
                {0, 0, 0, 0},
            },
            {// J
                {0, 1, 0, 0},
                {0, 1, 0, 0},
                {1, 1, 0, 0},
                {0, 0, 0, 0},
            },
            {// Z
                {1, 1, 0, 0},
                {0, 1, 1, 0},
                {0, 0, 0, 0},
                {0, 0, 0, 0},
            },
            {// S
                {0, 1, 1, 0},
                {1, 1, 0, 0},
                {0, 0, 0, 0},
                {0, 0, 0, 0},
            },
            {// T
                {1, 1, 1, 0},
                {0, 1, 0, 0},
                {0, 0, 0, 0},
                {0, 0, 0, 0},
            },
            {// I
                {1, 0, 0, 0},
                {1, 0, 0, 0},
                {1, 0, 0, 0},
                {1, 0, 0, 0},
            }
        };

        NewPiece();
        GetPreviewPiece();
    }

    // Get next piece and send to spawn point
    public void GetPreviewPiece()
    {
        for (int i = 0; i < 4; i++)
        {
            for (int j = 0; j < 4; j++)
            {
                if (intPieces[randomPiece, i, j] == 1)
                {
                    Block block = new Block(1, 1, randomColor);
                    mainMatrix.CreateBlock(block, j + 4, 16 - i);
                }
            }
        }

        gameController.ReturnSpawnArea();
        previewMatrix.DeletePiece();
        NewPiece();
    }
	
    // Generate a new random piece in next piece area
    public void NewPiece()
    {
        //select a random color and piece
        randomColor = Random.Range(0, 3);
        randomPiece = Random.Range(0, intPieces.GetLength(0));
        for (int i = 0; i < 4; i++)
        {
            for (int j = 0; j < 4; j++)
            {
                if (intPieces[randomPiece, i, j] == 1)
                {
                    Block block = new Block(1, 1, randomColor);
                    previewMatrix.CreateBlock(block, j, 4 - i);
                }
            }
        }
    }
}
