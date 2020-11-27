using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public enum BTNType
{ 
    Start,
    Next,
    Option,
    Sound,
    Back,
    Quit
}

public class MainUI : MonoBehaviour
{
    public void LoadGameScene()
    {
        LoadingScene.LoadScene("Map");
    }
    public void LoadMainScene()
    {
        SceneManager.LoadScene("Main");
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
