using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneScript : MonoBehaviour {

	// Reload game screen
    public void RestartGame()
    {
        SceneManager.LoadScene("mainScene");
    }

    // Go to title screen
    public void GoBack()
    {
        SceneManager.LoadScene("titleScene");
    }
}
