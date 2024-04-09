using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.SocialPlatforms.Impl;

public class GameManager : MonoBehaviour
{
    public GameObject bombObject; //prefab of the bomb
    public Rigidbody2D[] rigidBodies; //collection of all physics bodies
    public int rigidBodiesCount = 0;
    public Canvas uiCanvas; //UI Canvas
    public TMP_Text scoreText; //Score text number

    private GameObject currentBomb;
    private int currentScore = 0;
    private bool clickPong = false; //click pingpong

    void Start()
    {
        //rigidBodies = new List<Rigidbody2D>(); //removed for testing.. later add to initalize list, then when a new
        //rb is spawned please add code to add it's rb to this list, and add code to prune it when/if they are destroyed
        rigidBodies = new Rigidbody2D[128];
    }

    void Update()
    {
        Vector2 mousePosition = Input.mousePosition; //get mouse pos
        if (Input.GetKeyDown(KeyCode.Mouse0)) //if clicked
        {
            if (!clickPong)
            {
                clickPong = true; //prevent spawning until click is released
                Vector3 bombSpawnPos = Camera.main.ScreenToWorldPoint(mousePosition); //worldspace pos from screenspace
                bombSpawnPos = new Vector3(bombSpawnPos.x, bombSpawnPos.y); //remove z value
                currentBomb = Instantiate(bombObject, //spawn a bomb
                    bombSpawnPos, //at world pos of click
                    Quaternion.identity); //with "no" rotation
            }
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
                    currentScore -= 5;
                }
            }
        }

        if (currentBomb != null)
        {
            Vector3 bombNewPos = Camera.main.ScreenToWorldPoint(mousePosition); //worldspace pos from screenspace
            bombNewPos = new Vector3(bombNewPos.x, bombNewPos.y); //remove z value
            currentBomb.transform.position = bombNewPos;
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
}
