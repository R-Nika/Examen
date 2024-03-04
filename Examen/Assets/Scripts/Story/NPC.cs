using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPC : MonoBehaviour
{
    public string[] npcDialogues;
    public float interactionRadius = 3f; // Adjust the radius as needed

    private bool inRange = false;
    public DialogueSystem dialogue;

    private void Update()
    {
        // Check if the player is in range when the 'E' key is pressed
        if (Input.GetKeyDown(KeyCode.E))
        {
            CheckPlayerInRange();
        }
    }

    private void CheckPlayerInRange()
    {
        // Check if the player is in range using Physics.CheckSphere
        Collider[] colliders = Physics.OverlapSphere(transform.position, interactionRadius);

        foreach (Collider collider in colliders)
        {
            if (collider.CompareTag("Player"))
            {
                Debug.Log("Player is in range");
                // Player is in range, trigger the dialogue
                dialogue.StartDialogue(npcDialogues);
            }
        }
    }

    // Draw the interaction radius gizmo for better visualization in the Scene view
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, interactionRadius);
    }
}

