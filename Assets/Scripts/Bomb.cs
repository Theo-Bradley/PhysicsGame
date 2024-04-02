using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bomb : MonoBehaviour
{
    private GameManager gameManager;

    void Start()
    {
        gameManager = FindObjectOfType<GameManager>(); //aquire the game manager
        Activate(); //for testing, explode immediately //later replace with explode when stopped clicking
    }

    public void Activate()
    {
        //explode
        gameManager.Explode(transform.position); //call func on game manager to apply explosion force in scene
        //play particle effect
        //when done, destroy
    }
}
