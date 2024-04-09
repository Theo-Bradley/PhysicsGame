using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bird : MonoBehaviour
{
    public Rigidbody2D rigidBody;
    private GameManager gameManager;
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
            //fill with standard running code
            //when should destroy
            //gameManager.rigidBodies[gmIndex] = null;
        }
    }
}
