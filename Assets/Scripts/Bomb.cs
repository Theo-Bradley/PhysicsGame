using UnityEngine;

public class Bomb : MonoBehaviour
{
    private GameManager gameManager;
    private GameObject bombSpriteObj;
    private bool playAnim = false;
    private float animStartTime = 0f;
    private Material material;

    void Start()
    {
        gameManager = FindObjectOfType<GameManager>(); //aquire the game manager
        bombSpriteObj = transform.GetChild(0).gameObject; //get the child object which is used for rendering
        material = bombSpriteObj.GetComponent<Renderer>().material; //get the material on the child
    }

    void Update()
    {
        if (playAnim)
        {
            float fac = Mathf.Lerp(0.0f, 1.0f, (Time.time - animStartTime) / 0.8f); //lerp from 0 to 1 in 0.8s
            material.SetFloat("_WhiteValue", fac); //update the WhiteValue (luminance override) of the bomb
            if (fac >= 1.0f)
            {
                gameManager.Explode(transform.position); //call func on game manager to apply explosion force in scene
                //TO DO: add code to instantiate a bomb explosion particle system
                Destroy(gameObject); //remove me from scene
            }
        }
    }

    public void Activate()
    {
        //play animation, and stard explosion and destruction countdown
        animStartTime = Time.time; //set start time
        playAnim = true; //allow animation loop to execute
        bombSpriteObj.GetComponent<Animator>().SetBool("Activated", true); //start animation playing
    }
}