using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnScript : MonoBehaviour {

    //pieces are reference in Unity object inspector
    public GameObject[] pieces;
    private Color[] colors = new Color[3];

	// Use this for initialization
	void Start () {
        //Generate 3 new random colors
        for (int i = 0; i < 3; i++)
        {
            colors[i] = new Color(Random.Range(0.5f, 1.0f), Random.Range(0.5f, 1.0f), Random.Range(0.5f, 1.0f));
        }
        
        //check first random piece
        newPiece();
	}
	
    //generate a new random piece in spawn area
    void newPiece()
    {
        //select a random color and piece
        int rndPiece = Random.Range(0, pieces.Length);
        int rndColor = Random.Range(0, colors.Length);
        //new instance of piece, spawn point position is set in Unity inspector, Quaternion.identity is to not rotate sprite
        GameObject piece = Instantiate(pieces[rndPiece], transform.position, Quaternion.identity);
        //paint piece's blocks with random color
        foreach(SpriteRenderer block in piece.GetComponentsInChildren<SpriteRenderer>())
        {
            block.color = colors[rndColor];
        }
        
    }
}
