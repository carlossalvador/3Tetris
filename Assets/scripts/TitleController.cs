using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class TitleController : MonoBehaviour {

    private int level = 0;
    public Text levelText;

    private void Awake()
    {
        Screen.SetResolution(900, 600, false);
    }

    // Use this for initialization
    void Start () {
        
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    public void StartGame()
    {
        SceneManager.LoadScene("mainScene");
    }

    public void LoadInstructions()
    {
        SceneManager.LoadScene("instructionsScene");
    }

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
