using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public int maxBombs; //bombs remaining, set to max bombs in the inspector
    public int pigCount; //amount of pigs in the scene, set to inital in the inspector
    public int maxScore; //set to the score required for 3 stars in the inspector
    public GameObject bombObject; //prefab of the bomb
    public GameObject logPlatform; //reference to the plank platform object
    public int rigidBodiesCount = 0; //Current index in array (controlled by the script)
    public Canvas uiCanvas; //UI Canvas
    public TMP_Text scoreText; //Score text number
    public TMP_Text bombText; //Bomb text number
    public AudioSource musicSource; //AudioSource for the music
    public Image muteUIImage; //UI button for muting/unmuting the music
    public Sprite unmutedSprite; //sprite for the mute button (note icon)
    public Sprite mutedSprite; //sprite for the unmute button (struck through note icon)
    public Rigidbody2D[] rigidBodies; //collection of all physics bodies
    public int sceneIndex; //scene index of the current level

    private GameObject currentBomb; //reference to the bomb that is about to be spawned
    private int currentScore = 0; //current score
    private bool clickPong = false; //click pingpong
    private bool playingGame = true; //should the game be playing
    private bool menuLatch = true; //can I spawn the end level menu?
    private bool muted = false; //is the music muted?
    private float muteButtonPosition; //the leftmost X position of the mute button

    void Start()
    {
        rigidBodies = new Rigidbody2D[128]; //init
        sceneIndex = SceneManager.GetActiveScene().buildIndex; //get the scene index of the currently loaded scene
        bombText.text = maxBombs.ToString(); //set the bomb count's inital text
        Vector3[] corners = new Vector3[4]; //initalize array to hold the mute button corners
        muteUIImage.rectTransform.GetWorldCorners(corners); //calculate corners and populate the array
        muteButtonPosition = corners[0].x; //get the x pos of the bottom left corner for the button position
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
                    Vector3 bombSpawnPos = Camera.main.ScreenToWorldPoint(mousePosition); //screenspace to worldspace pos
                    if (bombSpawnPos.x <= muteButtonPosition) //mouse is not over the mute button
                    {
                        bombSpawnPos = new Vector3(bombSpawnPos.x, bombSpawnPos.y); //remove z value
                        currentBomb = Instantiate(bombObject, //spawn a bomb
                            bombSpawnPos, //at world pos of click
                            Quaternion.identity); //with "no" rotation
                        clickPong = true; //prevent spawning until click is released
                    }
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
            if (menuLatch) //if hasn't spawned a menu yet
            {
                SceneManager.LoadScene(1, LoadSceneMode.Additive); //load menu scene into this scene
                menuLatch = false; //stop it from spawning again
            }
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
        if ((float) currentScore >= maxScore) //if we got the "max score"
        {
            return 3; //return 3 stars
        }
        if ((float) currentScore >= 0.66f * maxScore) //if we got most of the max score
        {
            return 2; //.. 2 stars
        }
        if ((float) currentScore >= 0.3f * maxScore) //if we only got some of the max score
        {
            return 1; //.. 1 ..
        }

        return 0; //if nothing else was returned return 0 stars
    }

    public void MuteButtonPressed()
    {
        if (muted) //if was muted
        {
            muteUIImage.sprite = unmutedSprite; //set the image to be unmuted
            musicSource.mute = false; //unmute the audio
            muted = false; //allow muting
            return;
        }
        if (!muted) //if was unmuted
        {
            muteUIImage.sprite = mutedSprite; //set the image to muted
            musicSource.mute = true; //mute the audio
            muted = true; //allow unmuting
            return;
        }
    }

    private void EndLevel()
    {
        //carry all the code for ending the level, such as checking bomb remaining and giving extra score
        AddScore(maxBombs * 10); //calculate score from bombs remaining
        playingGame = false; //exit the main loop and display the end level menu
    }
}