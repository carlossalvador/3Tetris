using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockControllerScript : MonoBehaviour {

    // Game area where block is hold
    MatrixScript matrixHolder;

    // Block coordinates
    int x;
    int y;

    // Handle click when mouse is over this block.
    // Notify matrixHolder to delete block in coordinates given
    private void OnMouseOver()
    {
        if (Input.GetMouseButton(0))
        {
            matrixHolder.DeleteBlockOnClick(x, y);
        }
    }
    
    // Setter for matrixHolder
    public void SetMatrixHolder(MatrixScript matrix)
    {
        matrixHolder = matrix;
    }

    // Setter for x coordinate
    public void SetXAxis(int x)
    {
        this.x = x;
    }

    // Setter for y coordinate
    public void SetYAxis(int y)
    {
        this.y = y;
    }
}
