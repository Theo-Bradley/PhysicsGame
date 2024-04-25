using UnityEngine;

public class Bomb : MonoBehaviour
{
    public GameObject explosionParticles; //refernce to the explosion particle prefab
    private GameManager gameManager;
    private GameObject bombSpriteObj;
    private bool playAnim = false; //used for animation
    private float animStartTime = 0f; //..
    private Material material; //used to change the white level

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
                Instantiate(explosionParticles, transform.position, Quaternion.identity); //spawn explosion particles
                gameManager.UseBombAsync(); //decrease bomb counter
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