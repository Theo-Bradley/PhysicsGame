using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour
{
    public void PlayButton()
    {
        SceneManager.LoadScene(1, LoadSceneMode.Single); //load the next scene
    }

    public void QuitButton()
    {
        Application.Quit(); //quit out
    }
}
