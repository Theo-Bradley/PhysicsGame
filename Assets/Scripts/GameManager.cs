using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public GameObject bombObject; //prefab of the bomb
    public GameObject logPlatform; //reference to the plank platform object
    public int maxBombs; //bombs remaining, set to max bombs in the inspector
    public int pigCount; //amount of pigs in the scene, set to inital in the inspector
    public Rigidbody2D[] rigidBodies; //collection of all physics bodies
    public int rigidBodiesCount = 0; //Current index in array (controlled by the script)
    public Canvas uiCanvas; //UI Canvas
    public TMP_Text scoreText; //Score text number
    public TMP_Text bombText; //Bomb text number

    private GameObject currentBomb; //reference to the bomb that is about to be spawned
    private int currentScore = 0; //current score
    private bool clickPong = false; //click pingpong
    private bool playingGame = true; //should the game be playing

    void Start()
    {
        rigidBodies = new Rigidbody2D[128]; //init
        bombText.text = maxBombs.ToString(); //set the bomb count's inital text
    }

    void Update()
    {
        Vector2 mousePosition = Input.mousePosition; //get mouse pos
        if (playingGame)
        {
            if (Input.GetKeyDown(KeyCode.Mouse0)) //if clicked
            {
                if (!clickPong && maxBombs > 0)
                {
                    clickPong = true; //prevent spawning until click is released
                    Vector3 bombSpawnPos = Camera.main.ScreenToWorldPoint(mousePosition); //worldspace pos from screenspace
                    bombSpawnPos = new Vector3(bombSpawnPos.x, bombSpawnPos.y); //remove z value
                    currentBomb = Instantiate(bombObject, //spawn a bomb
                        bombSpawnPos, //at world pos of click
                        Quaternion.identity); //with "no" rotation
                }
            }

            if (Input.GetKeyDown(KeyCode.Space))
            {
                SceneManager.LoadScene(1, LoadSceneMode.Additive);
            }

            if (Input.GetKeyUp(KeyCode.Mouse0)) //if not clicking
            {
                if (clickPong)
                {
                    clickPong = false; //allow spawning again when clicked
                    if (currentBomb != null)
                    {
                        currentBomb.GetComponent<Bomb>().Activate(); //tell bomb to explode
                        currentBomb = null; //remove dangling pointer
                        maxBombs--; //use up a bomb
                        AddScore(-5); //remove score
                        bombText.text = maxBombs.ToString(); //update bomb remaining text
                    }
                }
            }

            if (Input.GetKey(KeyCode.Mouse1)) //if rightclicking
            {
                Vector3 platformNewPos = Camera.main.ScreenToWorldPoint(mousePosition); //get the mouse pos in world
                platformNewPos = new Vector3(Mathf.Min(platformNewPos.x, 1f), //clamp pos to the left of the screen
                    platformNewPos.y, 0f); //remove the depth component
                logPlatform.transform.position = platformNewPos; //update log pos
            }

            if (currentBomb != null)
            {
                Vector3 bombNewPos = Camera.main.ScreenToWorldPoint(mousePosition); //worldspace pos from screenspace
                bombNewPos = new Vector3(bombNewPos.x, bombNewPos.y, -1f); //set z value to -1
                currentBomb.transform.position = bombNewPos; //update position
            }
        }
        else
        { 
            //load end scene into the current one
        }
    }

    public void Explode(Vector2 position)
    {
        Vector2 direction;
        const float Dmax = 2f; //max distance //both of these values will change the gradient
        const float Fmax = 200; //max force
        foreach (Rigidbody2D rb2d in rigidBodies) //loop over rigidbodies
        {
            if (rb2d != null)
            {
                direction = rb2d.position - position; //get (un-normalized) direction vector
                rb2d.AddForceAtPosition(direction.normalized * Mathf.Max((1f - Mathf.Pow(direction.magnitude/Dmax, 2f)) * Fmax, 0f),
                    position); //apply calulated (explosion) force at provided position
            }
        }
    }

    public void AddScore(int score)
    {
        currentScore += score; //increase the score
        scoreText.text = currentScore.ToString(); //update the UI text with the new score
    }

    public int AddRigidBody(Rigidbody2D newRB)
    {
        rigidBodies[rigidBodiesCount] = newRB;
        rigidBodiesCount++;
        return rigidBodiesCount - 1;
    }

    public void KillPig()
    {
        pigCount -= 1;
        if (pigCount <= 0)
        {
            EndLevel(); //end the level
        }
    }

    public int GetStarCount()
    {
        //TODO: replace with code to get the number of stars achieved in the level
        return 0;
    }

    private void EndLevel()
    {
        //carry all the code for ending the level, such as checking bomb remaining and giving extra score
        Debug.Log("EndLvl");
    }
}
