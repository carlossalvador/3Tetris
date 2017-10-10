using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnScript : MonoBehaviour {

    //matrix is referenced in Unity object inspector
    private int[,,] intPieces;
    public MatrixScript mainMatrix;
    public NextPieceScript previewMatrix;
    public GameControllerScript gameController;
    private int randomPiece;
    private int randomColor;

    // Use this for initialization
    void Start () {
        //Generate pieces
        intPieces = new int[,,] {
            {//L
                {1, 0, 0, 0},
                {1, 0, 0, 0},
                {1, 1, 0, 0},
                {0, 0, 0, 0},
            },
            {//O
                {1, 1, 0, 0},
                {1, 1, 0, 0},
                {0, 0, 0, 0},
                {0, 0, 0, 0},
            },
            {//J
                {0, 1, 0, 0},
                {0, 1, 0, 0},
                {1, 1, 0, 0},
                {0, 0, 0, 0},
            },
            {//Z
                {1, 1, 0, 0},
                {0, 1, 1, 0},
                {0, 0, 0, 0},
                {0, 0, 0, 0},
            },
            {//S
                {0, 1, 1, 0},
                {1, 1, 0, 0},
                {0, 0, 0, 0},
                {0, 0, 0, 0},
            },
            {//T
                {1, 1, 1, 0},
                {0, 1, 0, 0},
                {0, 0, 0, 0},
                {0, 0, 0, 0},
            },
            {//I
                {1, 0, 0, 0},
                {1, 0, 0, 0},
                {1, 0, 0, 0},
                {1, 0, 0, 0},
            }
        };
                
        //check first random piece
        NewPiece();
        GetPreviewPiece();

    }

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

        gameController.returnSpawnArea();
        previewMatrix.DeletePiece();
        NewPiece();
    }
	
    //generate a new random piece in spawn area
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
