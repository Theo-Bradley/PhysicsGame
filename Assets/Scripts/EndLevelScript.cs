using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class EndLevelScript : MonoBehaviour
{
    const int maxSceneIndex = 3;

    public Canvas MenuCanvas; //end level menu canvas
    public RawImage starsImage; //stars UI element
    public Texture stars0; //image with no stars
    public Texture stars1; //image with 1 star
    public Texture stars2; //image with 2 stars
    public Texture stars3; //image with 3 stars

    
    private GameManager gameManager; //reference to the game manager of the scene we are added to

    void Start()
    {
        MenuCanvas.worldCamera = Camera.main; //get camera of the scene we are added to
        gameManager = FindObjectOfType<GameManager>(); //get the game manager of the scene we are added to
        
        int numStars = gameManager.GetStarCount(); //get number of stars achieved
        switch (numStars)
        {
            case 0: //if no stars
                starsImage.texture = stars0; //use image with no stars
                break;
            case 1: //..
                starsImage.texture = stars1; //..
                break;
            case 2: //..
                starsImage.texture = stars2; //..
                break;
            case 3: //..
                starsImage.texture = stars3; //..
                break;
            default: //catch case
                starsImage.texture = stars0; //use image with no stars
                break;
        }
    }

    public void NextLevel()
    {
        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex; //get current scene index (build settings)
        if (currentSceneIndex < maxSceneIndex) //if not maxed (there are more scenes to load)
            SceneManager.LoadScene(currentSceneIndex + 1, LoadSceneMode.Single); //increment index and load next scene
        else //if there are no more scenes to load
            SceneManager.LoadScene(0, LoadSceneMode.Single); //load main menu TODO: add victory screen
    }

    public void EndLevel()
    {
        SceneManager.LoadScene(0, LoadSceneMode.Single); //load main menu
    }
}
