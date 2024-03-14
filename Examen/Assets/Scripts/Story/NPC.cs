using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

//
//
// Created/Written by: Amy van Oosten
//
//

public class NPC : MonoBehaviour
{
    public string[] npcDialogues;
    public float interactionRadius = 3f;
    public TMP_Text pressE;
    public DialogueSystem dialogue;

    private bool inRange = false;
    private bool interactPressed = false;

    private void Start()
    {
        pressE.enabled = false; 
    }

    private void Update()
    {
        CheckPlayerInRange(); 

        if (interactPressed && inRange && !dialogue.isInDialogue && dialogue.dialogueFinished)
        {
            dialogue.ResetDialogue(); 
            interactPressed = false; 
        }
    }

    // Method to check if the player is in range
    private void CheckPlayerInRange()
    {
        // Use OverlapSphere to find colliders within the interaction radius
        Collider[] colliders = Physics.OverlapSphere(transform.position, interactionRadius);
        bool playerInRange = false;

        foreach (Collider collider in colliders)
        {
            if (collider.CompareTag("Player"))
            {
                playerInRange = true; 
                break; // Exit the loop since player is found
            }
        }

        // Update the inRange and enable/disable pressE accordingly
        inRange = playerInRange;
        pressE.enabled = inRange && !dialogue.isInDialogue;

        if (inRange && Input.GetButtonDown("Interact"))
        {
            interactPressed = true; 
            Debug.Log("Player in range. Interaction button pressed.");

            // If the dialogue system is in dialogue, continue the dialogue; otherwise, start a new dialogue
            if (dialogue.isInDialogue)
            {
                dialogue.ContinueDialogue(); 
            }
            else
            {
                dialogue.StartDialogue(npcDialogues);
            }
        }
    }

    // Method to draw a wire sphere representing the interaction radius in the scene view
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow; 
        Gizmos.DrawWireSphere(transform.position, interactionRadius); 
    }
}