using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pig : MonoBehaviour
{
    public Rigidbody2D rigidBody; //rb ob object
    public GameObject breakParticles; //prefab to spank when plank is broken
    public int hitPoints;
    private GameManager gameManager; //scene game manager
    bool shouldBreak = false; //should the plank be broken
    int hitCount = 0;
    int gmIndex = -1;

    void Start()
    {
        gameManager = FindObjectOfType<GameManager>(); //aquire the game manager
    }

    void Update()
    {
        if (gmIndex == -1) //if not yet added to array
        {
            gmIndex = gameManager.AddRigidBody(rigidBody); //add to array on game manager
        }
        else
        {
            if (rigidBody.velocity.magnitude >= 2.5f) //if moving sufficiently fast (simple force check)
                shouldBreak = true; //start breaking code
            if (shouldBreak)
            {
                Instantiate(breakParticles, transform.position, Quaternion.identity); //spawn particles where plank was
                gameManager.AddScore(50); //increase score when pig is broken
                gameManager.rigidBodies[gmIndex] = null; //remove rb from game manager array
                gameManager.KillPig();
                Destroy(gameObject); //remove pig
            }
        }
    }
    public void Hit(float velocity)
    {
        hitCount++; //increase hitcount
        if (hitCount >= hitPoints) //if hit enough
        {
            shouldBreak = true; //break
        }
    }

    public void OnCollisionEnter2D(Collision2D col)
    {
        Vector2 resultant = col.rigidbody.velocity + col.otherRigidbody.velocity; //get the difference in velocities
        if (Mathf.Abs(resultant.magnitude) >= 2f) //if it is greater than a value
        {
            Hit(Mathf.Abs(resultant.magnitude)); //get hit
        }
    }
}
