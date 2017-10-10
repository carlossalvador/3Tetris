using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NextPieceScript : MatrixScript
{

	// Use this for initialization
	void Start () {

    }
	
	// Update is called once per frame
	void Update () {
        DrawBlocks();
    }
    
    public void DeletePiece()
    {
        for (int i = 0; i < rows; i++)
        {
            for (int j = 0; j < columns; j++)
            {
                if (cells[j, i] != null)
                    cells[j, i] = null;
            }
        }
        DrawBlocks();
    }
}
