using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

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

        if (interactPressed && inRange && !dialogue.isDialoguing && dialogue.dialogueFinished)
        {
            dialogue.ResetDialogue();
            
            interactPressed = false;
        }
    }


    private void CheckPlayerInRange()
    {
        Collider[] colliders = Physics.OverlapSphere(transform.position, interactionRadius);
        bool playerInRange = false;
        foreach (Collider collider in colliders)
        {
            if (collider.CompareTag("Player"))
            {
                playerInRange = true;
                break;
            }
        }

        inRange = playerInRange;
        pressE.enabled = inRange && !dialogue.isDialoguing;

        if (inRange && Input.GetButtonDown("Interact"))
        {
            interactPressed = true;
            Debug.Log("Player in range. Interaction button pressed.");
           
            if (dialogue.isDialoguing)
            {
                dialogue.ContinueDialogue();
                
            }
            else
            {
                dialogue.StartDialogue(npcDialogues);
            }
        }
        
    }

    


    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, interactionRadius);
    }
}
