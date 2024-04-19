using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngineInternal;

public class Piece : MonoBehaviour
{
    public Rigidbody2D rigidBody; //rb ob object
    public GameObject breakParticles; //prefab to spank when plank is broken
    public float breakThresh; //required "force" to break
    private GameManager gameManager; //scene game manager
    bool shouldBreak = false; //should the plank be broken
    int gmIndex = -1; //my index in the rigidbodies array on game manager

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
            if (rigidBody.velocity.magnitude >= breakThresh) //if moving sufficiently fast (simple force check)
                shouldBreak = true; //start breaking code
            if (shouldBreak)
            {
                Instantiate(breakParticles, transform.position, Quaternion.identity); //spawn particles where plank was
                gameManager.AddScore(10); //increase score when plank is broken
                gameManager.rigidBodies[gmIndex] = null; //remove rigidbody from the array on game manager
                Destroy(gameObject); //remove plank
            }
        }
    }
}
