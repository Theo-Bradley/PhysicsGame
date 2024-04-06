using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Piece : MonoBehaviour
{
    public Rigidbody2D rigidBody; //rb ob object
    public GameObject breakParticles; //prefab to spank when plank is broken
    private GameManager gameManager; //scene game manager
    bool shouldBreak = false; //should the plank be broken

    void Start()
    {
        gameManager = FindObjectOfType<GameManager>(); //aquire the game manager
    }

    void Update()
    {
        if (rigidBody.velocity.magnitude >= 2.5f) //if moving sufficiently fast (simple force check)
            shouldBreak = true; //start breaking code
        if (shouldBreak)
        {
            Instantiate(breakParticles, transform.position, Quaternion.identity); //spawn particles where plank was
            gameManager.AddScore(10); //increase score when plank is broken
            Destroy(gameObject); //remove plank
        }
    }
}
