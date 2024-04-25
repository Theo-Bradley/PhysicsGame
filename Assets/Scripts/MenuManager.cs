using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour
{
    public AnimationClip bgWiggle; //reference to the bg wiggle animation clip

    public void Start()
    {
        bgWiggle.wrapMode = WrapMode.Loop; //change wrap mode (looping state) for animation to loop
        //this has to be done in code as there is no way to do this in the inspector for my unity version
    }

    public void PlayButton()
    {
        SceneManager.LoadScene(3, LoadSceneMode.Single); //load the next scene
    }

    public void QuitButton()
    {
        Application.Quit(); //quit out
    }
}
