using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class TitleController : MonoBehaviour {

    private int level = 0;
    public Text levelText;

    // Set window resolution and set no fullscreen
    private void Awake()
    {
        Screen.SetResolution(900, 600, false);
    }
    
    // Go to game screen
    public void StartGame()
    {
        SceneManager.LoadScene("mainScene");
    }

    // Go to instructions screen
    public void LoadInstructions()
    {
        SceneManager.LoadScene("instructionsScene");
    }

    // Up difficulty level
    public void UpLevel()
    {
        if (level < 2)
        {
            level++;
        }
        else
        {
            level = 0;
        }

        UpdateLevel();
    }

    // Down difficulty level
    public void DownLevel()
    {
        if (level > 0)
        {
            level--;
        }
        else
        {
            level = 2;
        }

        UpdateLevel();
    }

    // Update difficult level in game controller and show in title message
    private void UpdateLevel()
    {
        GameControllerScript.level = level;
        switch (level)
        {
            case 2:
                levelText.text = "HARD";
                break;
            case 1:
                levelText.text = "NORMAL";
                break;
            default:
                levelText.text = "EASY";
                break;
        }        
    }
}
