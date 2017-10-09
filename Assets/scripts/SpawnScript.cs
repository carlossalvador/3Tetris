using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnScript : MonoBehaviour {

    //pieces are reference in Unity object inspector
    public GameObject[] pieces;
    private int[,,] intPieces;
    private Color[] colors = new Color[3];
    public MatrixScript matrix;

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

        //Generate 3 new random colors
        for (int i = 0; i < 3; i++)
        {
            colors[i] = new Color(Random.Range(0.5f, 1.0f), Random.Range(0.5f, 1.0f), Random.Range(0.5f, 1.0f));
        }
        
        //check first random piece
        newPiece();
	}
	
    //generate a new random piece in spawn area
    public void newPiece()
    {
        //select a random color and piece
        int rndPiece = Random.Range(0, pieces.Length);
        int rndColor = Random.Range(0, colors.Length);
        ////new instance of piece, spawn point position is set in Unity inspector, Quaternion.identity is to not rotate sprite
        //GameObject piece = Instantiate(pieces[rndPiece], transform.position, Quaternion.identity);
        ////paint piece's blocks with random color
        //foreach(SpriteRenderer block in piece.GetComponentsInChildren<SpriteRenderer>())
        //{
        //    block.color = colors[rndColor];
        //}

        int randomPiece = Random.Range(0, intPieces.GetLength(0));
        print(randomPiece);
        for (int i = 0; i < 4; i++)
        {
            for (int j = 0; j < 4; j++)
            {
                matrix.createBlock(j + 4, 16 - i, intPieces[randomPiece, i, j], intPieces[randomPiece, i, j]);
            }
        }

    }
}
