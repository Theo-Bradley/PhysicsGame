using UnityEngine;

public class Bird : MonoBehaviour
{
    public GameManager gameManager; //scene's game manager
    public Rigidbody2D rigidBody; //rb of the bird
    public SpriteRenderer spriteRenderer; //sprite renderer on child
    private int gmIndex = -1; //index of rb on game manager

    void Update()
    {
        if (gmIndex == -1) //if not yet added to array
        {
            gmIndex = gameManager.AddRigidBody(rigidBody); //add to array on game manager
        }
    
        else
        {
            if (!spriteRenderer.isVisible) //if no longer on screen
            {
                rigidBody.velocity = Vector2.zero; //stop bird from moving
                transform.position = Vector3.zero; //move back onscreen
                gameManager.AddScore(-10); //remove some score

            }
        }
    }
}
