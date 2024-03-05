using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class NPC : MonoBehaviour
{
    public string[] npcDialogues;
    public float interactionRadius = 3f; // Adjust the radius as needed
    public TMP_Text pressE;

    private bool inRange = false;
    public DialogueSystem dialogue;

    [HideInInspector] public bool farmerActive = false;
    [HideInInspector] public bool cafeActive = false;
    [HideInInspector] public bool phoneActive = false;
    [HideInInspector] public bool policeActive = false;

    private bool interactionTriggered = false;

    private void Start()
    {
        pressE.enabled = false;
    }
    private void Update()
    {
        // Check if the player is in range when the 'E' key is pressed
        CheckPlayerInRange();
        if (inRange)
        {
            pressE.enabled = true;
        }
        else
        {
            pressE.enabled = false;
        }
    }

    private void CheckPlayerInRange()
    {
        if (!interactionTriggered)
        {
            // Check if the player is in range using Physics.OverlapSphere
            Collider[] colliders = Physics.OverlapSphere(transform.position, interactionRadius);

            foreach (Collider collider in colliders)
            {
                if (collider.CompareTag("Player"))
                {
                    Debug.Log("Player is in range");
                    inRange = true;

                    if (Input.GetKeyDown(KeyCode.E))
                    {
                        // Player is in range, trigger the dialogue
                        dialogue.StartDialogue(npcDialogues);

                        // Set the appropriate NPC active flag
                        SetActiveFlag();

                        interactionTriggered = true; // Set to true to avoid repeating the interaction
                    }
                    return; // exit the loop if player is found
                }
            }

            // If player is not found in the loop, set inRange to false
            inRange = false;
        }
    }

    private void SetActiveFlag()
    {
        switch (gameObject.tag)
        {
            case "Phone":
                phoneActive = true;
                break;
            case "Farmer":
                farmerActive = true;
                break;
            case "Cafe":
                cafeActive = true;
                break;
            case "Police":
                policeActive = true;
                break;
        }
    }

    // Add this method to reset interactionTriggered
    public void ResetInteraction()
    {
        interactionTriggered = false;
    }

// Add this method to reset interactionTriggered

    // Draw the interaction radius gizmo for better visualization in the Scene view
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, interactionRadius);
    }
}
