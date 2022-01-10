using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

//This is set as a monobehaviour inside a prefab because Unity doesnt let me use static methods in UI :'(
public  class ButtonFunctions : MonoBehaviour
{
    public void GotoGame()
    {
        SceneManager.LoadScene("Game");
    }

    public void GotoOptions()
    {
        SceneManager.LoadScene("Options");
    }

    public void GotoMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }

    public void GotoScore()
    {
        SceneManager.LoadScene("Scores");
    }

    public void ExitGame()
    {
        Debug.Log("Quitting");
        Application.Quit();
    }
}
