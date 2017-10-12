using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class InstructionsScript : MonoBehaviour {

    private int page = 0;
    private int maxPages = 6;
    public Text pageCounter;
    public Image[] pages;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void GoTitle()
    {
        SceneManager.LoadScene("titleScene");
    }

    public void NextPage()
    {
        if (page < maxPages - 1)
        {
            page++;
        }
        else
        {
            page = 0;
        }
        UpdatePage();
    }

    public void PreviousPage()
    {
        if (page > 0)
        {
            page--;
        }
        else
        {
            page = maxPages - 1;
        }
        UpdatePage();
    }

    private void UpdatePage()
    {
        for (int i = 0; i < pages.Length; i++)
        {
            pages[i].gameObject.SetActive(false);
        }
        pages[page].gameObject.SetActive(true);
        pageCounter.text = (page + 1) + "/" + maxPages;
    }
}
