using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Inheritance from game area matrix
public class NextPieceScript : MatrixScript
{
    
	// In the updates the piece is draw
	void Update () {
        DrawBlocks();
    }
    
    // When a new piece will be added the last is erase
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
