using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public GameObject bombObject; //prefab of the bomb
    public List<Rigidbody2D> rigidBodies; //collection of all physics bodies

    private bool clickPong = false; //click pingpong

    void Start()
    {
        //rigidBodies = new List<Rigidbody2D>(); //removed for testing.. later add to initalize list, then when a new
        //rb is spawned please add code to add it's rb to this list, and add code to prune it when/if they are destroyed
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Mouse0)) //if clicked
        {
            if (!clickPong)
            {
                Vector2 mousePosition = Input.mousePosition; //get mouse pos
                clickPong = true; //prevent spawning until click is released
                Instantiate(bombObject, //spawn a bomb
                    Camera.main.ScreenToWorldPoint(mousePosition), //at world pos of click
                    Quaternion.identity); //with "no" rotation
            }
        }

        if (Input.GetKeyUp(KeyCode.Mouse0)) //if not clicking
        {
            if (clickPong)
                clickPong = false; //allow spawning again when clicked
        }
    }

    public void Explode(Vector2 position)
    {
        Vector2 direction;
        const float Dmax = 5f; //max distance //both of these values will change the gradient
        const float Fmax = 200; //max force
        foreach (Rigidbody2D rb2d in rigidBodies) //loop over rigidbodies
        {
            direction = rb2d.position - position; //get (un-normalized) direction vector
            rb2d.AddForceAtPosition(direction.normalized * Mathf.Max((1f - Mathf.Pow(direction.magnitude/Dmax, 2f)) * Fmax, 0f),
                position); //apply calulated (explosion) force at provided position
        }
    }
}
