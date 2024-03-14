using System.Collections;
using System.Collections.Generic;
using UnityEngine;





//Created by Davido with help from Chat GTP4





public class AnimalAnimationManager : MonoBehaviour
{
    //Calls the animator from the animals
    public Animator animalAnimation;
    //Bool which is in the animator that can be true or false
    private bool animationPlaying = false;

    // Start is called before the first frame update
    void Start()
    {
        // Calls upon the animator from the object to check
        animalAnimation = GetComponent<Animator>();
    }

    void OnTriggerEnter(Collider other)
    {
        // Check if the collider belongs to the player
        if (other.CompareTag("Player"))
        {
            // Sets the bool to true and plays the animation
            animationPlaying = true;
            Debug.Log("Player is in.");


        }
    }



    void Update()
    {
        // Check if the boolean variable is true
        if (animationPlaying)
        {
            // Trigger the animation
            animalAnimation.SetBool("animationPlaying", true);

          
        }
    }



}
