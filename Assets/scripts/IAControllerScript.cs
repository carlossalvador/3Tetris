using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IAControllerScript : MonoBehaviour {

    //game controller
    public GameControllerScript gameController;

    // Game area where falling piece are
    private int currentMatrix;

    // Delay for IA choices
    public int maxControlDelay;
    private int controlDelay;

    // Label for IA player
    string owner = "IA";
    
	// Set a delay to IA for choose an action
	void Update () {
        if (controlDelay < maxControlDelay)
            controlDelay++;
        else
        {
            controlDelay = 0;
            switch (Random.Range(0, 4))
            {
                case 1:
                    gameController.MoveToLeft(currentMatrix, owner);
                    break;
                case 2:
                    gameController.MoveToRight(currentMatrix, owner);
                    break;
                case 3:
                    int nextMatrix = currentMatrix < 2 ? currentMatrix + 1 : 0;
                    if (gameController.ChangeGameArea(currentMatrix, nextMatrix, owner))
                        currentMatrix = nextMatrix;
                    break;
                default:
                    break;
            }
            
        }
	}
}
