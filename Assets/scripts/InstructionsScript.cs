using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class InstructionsScript : MonoBehaviour {

    // Count visible page
    private int page = 0;
    // Number of pages
    private int maxPages = 6;
    // Text to show actual page
    public Text pageCounter;
    // The images are set in Unity inspector
    public Image[] pages;
    
    // Go to title screen
    public void GoTitle()
    {
        SceneManager.LoadScene("titleScene");
    }

    // Move to next page
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

    // Move to previous page
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

    // Update page message and show instructions image
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
